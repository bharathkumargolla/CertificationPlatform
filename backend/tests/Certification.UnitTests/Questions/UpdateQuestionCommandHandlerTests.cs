using Certification.Application.Questions.Commands.UpdateQuestion;
using Certification.Application.Questions.DTOs;
using Certification.Domain.Entities;
using Certification.Domain.Enums;
using Certification.UnitTests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Certification.UnitTests.Questions;

public sealed class UpdateQuestionCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingQuestion_ReplacesOptionsAndReturnsSuccess()
    {
        await using var dbContext = TestApplicationDbContext.Create();

        var module = new Module { Code = "MOD-UQ", Name = "Module" };
        dbContext.Modules.Add(module);

        var certification = new CertificationDefinition
        {
            ModuleId = module.Id,
            Code = "CERT-UQ",
            Name = "Certification",
            DurationInMinutes = 60,
            PassingScore = 70,
            IsActive = true,
        };
        dbContext.Certifications.Add(certification);

        var question = new Question
        {
            CertificationDefinitionId = certification.Id,
            QuestionText = "Original text",
            QuestionType = QuestionType.SingleChoice,
            Points = 5,
        };
        question.QuestionOptions.Add(new QuestionOption { OptionText = "Old A", IsCorrect = true });
        question.QuestionOptions.Add(new QuestionOption { OptionText = "Old B", IsCorrect = false });
        dbContext.Questions.Add(question);
        await dbContext.SaveChangesAsync();

        var handler = new UpdateQuestionCommandHandler(dbContext);
        var command = new UpdateQuestionCommand
        {
            Id = question.Id,
            QuestionText = "Updated text",
            QuestionType = QuestionType.MultipleChoice,
            Points = 10,
            IsActive = true,
            Options =
            [
                new QuestionOptionDto { OptionText = "New A", IsCorrect = true },
                new QuestionOptionDto { OptionText = "New B", IsCorrect = true },
                new QuestionOptionDto { OptionText = "New C", IsCorrect = false },
            ],
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var persisted = await dbContext.Questions
            .Include(existing => existing.QuestionOptions)
            .SingleAsync(existing => existing.Id == question.Id);

        persisted.QuestionText.Should().Be("Updated text");
        persisted.QuestionOptions.Should().HaveCount(3);
        persisted.QuestionOptions.Should().OnlyContain(option => option.OptionText.StartsWith("New"));

        var totalOptionsInDatabase = await dbContext.QuestionOptions.CountAsync();
        totalOptionsInDatabase.Should().Be(3);
    }

    [Fact]
    public async Task Handle_WithMissingQuestion_ReturnsFailure()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var handler = new UpdateQuestionCommandHandler(dbContext);

        var command = new UpdateQuestionCommand
        {
            Id = Guid.NewGuid(),
            QuestionText = "Does not exist",
            QuestionType = QuestionType.SingleChoice,
            Points = 5,
            Options =
            [
                new QuestionOptionDto { OptionText = "A", IsCorrect = true },
                new QuestionOptionDto { OptionText = "B", IsCorrect = false },
            ],
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }
}
