using Certification.Application.Questions.Commands.DeleteQuestion;
using Certification.Domain.Entities;
using Certification.Domain.Enums;
using Certification.UnitTests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Certification.UnitTests.Questions;

public sealed class DeleteQuestionCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingQuestion_SoftDeletesAndReturnsSuccess()
    {
        await using var dbContext = TestApplicationDbContext.Create();

        var module = new Module { Code = "MOD-DQ", Name = "Module" };
        dbContext.Modules.Add(module);

        var certification = new CertificationDefinition
        {
            ModuleId = module.Id,
            Code = "CERT-DQ",
            Name = "Certification",
            DurationInMinutes = 60,
            PassingScore = 70,
            IsActive = true,
        };
        dbContext.Certifications.Add(certification);

        var question = new Question
        {
            CertificationDefinitionId = certification.Id,
            QuestionText = "Question to delete",
            QuestionType = QuestionType.SingleChoice,
            Points = 5,
        };
        dbContext.Questions.Add(question);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteQuestionCommandHandler(dbContext);

        var result = await handler.Handle(new DeleteQuestionCommand { Id = question.Id }, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var persisted = await dbContext.Questions.SingleAsync(existing => existing.Id == question.Id);
        persisted.IsDeleted.Should().BeTrue();
        persisted.DeletedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithMissingQuestion_ReturnsFailure()
    {
        await using var dbContext = TestApplicationDbContext.Create();
        var handler = new DeleteQuestionCommandHandler(dbContext);

        var result = await handler.Handle(new DeleteQuestionCommand { Id = Guid.NewGuid() }, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }
}
