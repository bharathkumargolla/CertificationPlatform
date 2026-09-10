using Certification.Application.Certifications.Commands.CreateCertification;
using Certification.Domain.Entities;
using Certification.UnitTests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Certification.UnitTests.Certifications;

public sealed class CreateCertificationCommandHandlerTests
{
    private static async Task<Module> SeedActiveModuleAsync(TestApplicationDbContext dbContext)
    {
        var module = new Module
        {
            Code = "MOD-CERT",
            Name = "Certification Host Module",
            IsActive = true,
        };
        dbContext.Modules.Add(module);
        await dbContext.SaveChangesAsync();

        return module;
    }

    [Fact]
    public async Task Handle_WithUniqueCodeAndActiveModule_CreatesCertificationAndReturnsSuccess()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var module = await SeedActiveModuleAsync(dbContext);

        var handler = new CreateCertificationCommandHandler(dbContext);
        var command = new CreateCertificationCommand
        {
            ModuleId = module.Id,
            Code = "CERT-101",
            Name = "Certified Testing Professional",
            DurationInMinutes = 90,
            PassingScore = 70,
            CertificateValidityMonths = 24,
            DisplayOrder = 1,
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var persisted = await dbContext.Certifications.SingleAsync();
        persisted.Code.Should().Be(command.Code);
        persisted.ModuleId.Should().Be(module.Id);
    }

    [Fact]
    public async Task Handle_WithDuplicateCode_ReturnsFailure()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var module = await SeedActiveModuleAsync(dbContext);

        dbContext.Certifications.Add(new CertificationDefinition
        {
            ModuleId = module.Id,
            Code = "CERT-101",
            Name = "Existing Certification",
            DurationInMinutes = 60,
            PassingScore = 70,
        });
        await dbContext.SaveChangesAsync();

        var handler = new CreateCertificationCommandHandler(dbContext);
        var command = new CreateCertificationCommand
        {
            ModuleId = module.Id,
            Code = "CERT-101",
            Name = "Duplicate Certification",
            DurationInMinutes = 90,
            PassingScore = 70,
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("CERT-101");

        var count = await dbContext.Certifications.CountAsync();
        count.Should().Be(1);
    }

    [Fact]
    public async Task Handle_WithNonExistentModule_ReturnsFailure()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var handler = new CreateCertificationCommandHandler(dbContext);

        var command = new CreateCertificationCommand
        {
            ModuleId = Guid.NewGuid(),
            Code = "CERT-202",
            Name = "Orphan Certification",
            DurationInMinutes = 60,
            PassingScore = 70,
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();

        var count = await dbContext.Certifications.CountAsync();
        count.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WithInactiveModule_ReturnsFailure()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var module = new Module
        {
            Code = "MOD-INACTIVE",
            Name = "Retired Module",
            IsActive = false,
        };
        dbContext.Modules.Add(module);
        await dbContext.SaveChangesAsync();

        var handler = new CreateCertificationCommandHandler(dbContext);
        var command = new CreateCertificationCommand
        {
            ModuleId = module.Id,
            Code = "CERT-303",
            Name = "Certification For Inactive Module",
            DurationInMinutes = 60,
            PassingScore = 70,
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();

        var count = await dbContext.Certifications.CountAsync();
        count.Should().Be(0);
    }
}
