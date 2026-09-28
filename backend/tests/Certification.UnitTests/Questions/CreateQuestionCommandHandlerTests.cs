using Certification.Application.Questions.Commands.CreateQuestion;
using Certification.Application.Questions.DTOs;
using Certification.Domain.Entities;
using Certification.Domain.Enums;
using Certification.UnitTests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Certification.UnitTests.Questions;

public sealed class CreateQuestionCommandHandlerTests
{
    private static async Task<(Module Module, CertificationDefinition Certification)> SeedActiveCertificationAsync(TestApplicationDbContext dbContext)
    {
        var module = new Module { Code = "MOD-Q", Name = "Question Host Module", IsActive = true };
        dbContext.Modules.Add(module);

        var certification = new CertificationDefinition
        {
            ModuleId = module.Id,
            Code = "CERT-Q",
            Name = "Question Host Certification",
            DurationInMinutes = 60,
            PassingScore = 70,
            IsActive = true,
        };
        dbContext.Certifications.Add(certification);
        await dbContext.SaveChangesAsync();

        return (module, certification);
    }

    private static List<QuestionOptionDto> ValidSingleChoiceOptions() =>
    [
        new QuestionOptionDto { OptionText = "Option A", IsCorrect = true },
        new QuestionOptionDto { OptionText = "Option B", IsCorrect = false },
    ];

    [Fact]
    public async Task Handle_WithValidRequest_CreatesQuestionWithOptionsAndReturnsSuccess()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var (_, certification) = await SeedActiveCertificationAsync(dbContext);

        var handler = new CreateQuestionCommandHandler(dbContext);
        var command = new CreateQuestionCommand
        {
            CertificationDefinitionId = certification.Id,
            QuestionText = "What is 2 + 2?",
            QuestionType = QuestionType.SingleChoice,
            DifficultyLevel = DifficultyLevel.Easy,
            Points = 10,
            Options = ValidSingleChoiceOptions(),
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var persisted = await dbContext.Questions
            .Include(question => question.QuestionOptions)
            .SingleAsync();
        persisted.QuestionText.Should().Be(command.QuestionText);
        persisted.QuestionOptions.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WithNonExistentCertification_ReturnsFailure()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var handler = new CreateQuestionCommandHandler(dbContext);

        var command = new CreateQuestionCommand
        {
            CertificationDefinitionId = Guid.NewGuid(),
            QuestionText = "Orphan question",
            QuestionType = QuestionType.SingleChoice,
            Points = 10,
            Options = ValidSingleChoiceOptions(),
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();

        var count = await dbContext.Questions.CountAsync();
        count.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WithInactiveCertification_ReturnsFailure()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var module = new Module { Code = "MOD-INACTIVE-Q", Name = "Module" };
        dbContext.Modules.Add(module);

        var certification = new CertificationDefinition
        {
            ModuleId = module.Id,
            Code = "CERT-INACTIVE-Q",
            Name = "Inactive Certification",
            DurationInMinutes = 60,
            PassingScore = 70,
            IsActive = false,
        };
        dbContext.Certifications.Add(certification);
        await dbContext.SaveChangesAsync();

        var handler = new CreateQuestionCommandHandler(dbContext);
        var command = new CreateQuestionCommand
        {
            CertificationDefinitionId = certification.Id,
            QuestionText = "Question for inactive certification",
            QuestionType = QuestionType.SingleChoice,
            Points = 10,
            Options = ValidSingleChoiceOptions(),
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();

        var count = await dbContext.Questions.CountAsync();
        count.Should().Be(0);
    }
}
