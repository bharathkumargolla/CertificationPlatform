using Certification.Application.Certifications.Commands.DeleteCertification;
using Certification.Domain.Entities;
using Certification.UnitTests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Certification.UnitTests.Certifications;

public sealed class DeleteCertificationCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingCertification_SoftDeletesAndReturnsSuccess()
    {
        await using var dbContext = TestApplicationDbContext.Create();

        var module = new Module { Code = "MOD-DEL", Name = "Module" };
        dbContext.Modules.Add(module);

        var certification = new CertificationDefinition
        {
            ModuleId = module.Id,
            Code = "CERT-404",
            Name = "Certification To Delete",
            DurationInMinutes = 60,
            PassingScore = 70,
        };
        dbContext.Certifications.Add(certification);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteCertificationCommandHandler(dbContext);

        var result = await handler.Handle(new DeleteCertificationCommand { Id = certification.Id }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var persisted = await dbContext.Certifications.SingleAsync(existing => existing.Id == certification.Id);
        persisted.IsDeleted.Should().BeTrue();
        persisted.DeletedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithMissingCertification_ReturnsFailure()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var handler = new DeleteCertificationCommandHandler(dbContext);

        var result = await handler.Handle(new DeleteCertificationCommand { Id = Guid.NewGuid() }, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }
}
