using Certification.Application.Modules.Commands.DeleteModule;
using Certification.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Certification.UnitTests.Modules;

public sealed class DeleteModuleCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingModule_SoftDeletesAndReturnsSuccess()
    {
        await using var dbContext = TestApplicationDbContext.Create();

        var module = new Module
        {
            Code = "MOD-202",
            Name = "Module To Delete",
        };
        dbContext.Modules.Add(module);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteModuleCommandHandler(dbContext);

        var result = await handler.Handle(new DeleteModuleCommand { Id = module.Id }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var persisted = await dbContext.Modules.SingleAsync(existing => existing.Id == module.Id);
        persisted.IsDeleted.Should().BeTrue();
        persisted.DeletedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithMissingModule_ReturnsFailure()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var handler = new DeleteModuleCommandHandler(dbContext);

        var result = await handler.Handle(new DeleteModuleCommand { Id = Guid.NewGuid() }, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }
}
