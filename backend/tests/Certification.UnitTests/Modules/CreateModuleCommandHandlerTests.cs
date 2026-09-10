using Certification.Application.Modules.Commands.CreateModule;
using Certification.Domain.Entities;
using Certification.UnitTests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Certification.UnitTests.Modules;

public sealed class CreateModuleCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithUniqueCode_CreatesModuleAndReturnsSuccess()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var handler = new CreateModuleCommandHandler(dbContext);

        var command = new CreateModuleCommand
        {
            Code = "MOD-101",
            Name = "Introduction to Testing",
            Description = "Covers the basics.",
            DisplayOrder = 1,
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var persisted = await dbContext.Modules.SingleAsync();
        persisted.Code.Should().Be(command.Code);
        persisted.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Handle_WithDuplicateCode_ReturnsFailure()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        dbContext.Modules.Add(new Module
        {
            Code = "MOD-101",
            Name = "Existing Module",
        });
        await dbContext.SaveChangesAsync();

        var handler = new CreateModuleCommandHandler(dbContext);
        var command = new CreateModuleCommand
        {
            Code = "MOD-101",
            Name = "Duplicate Module",
            DisplayOrder = 2,
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("MOD-101");

        var count = await dbContext.Modules.CountAsync();
        count.Should().Be(1);
    }
}
