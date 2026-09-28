using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using Certification.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Questions.Commands.UpdateQuestion;

public sealed class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateQuestionCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _dbContext.Questions
            .Include(existing => existing.QuestionOptions)
            .FirstOrDefaultAsync(existing => existing.Id == request.Id && !existing.IsDeleted, cancellationToken);

        if (question is null)
        {
            return Result.Failure($"Question with id '{request.Id}' was not found.");
        }

        question.QuestionText = request.QuestionText;
        question.Explanation = request.Explanation;
        question.QuestionType = request.QuestionType;
        question.DifficultyLevel = request.DifficultyLevel;
        question.Points = request.Points;
        question.DisplayOrder = request.DisplayOrder;
        question.IsActive = request.IsActive;
        question.ModifiedAtUtc = DateTime.UtcNow;

        // Replace the option set wholesale rather than trying to reconcile individual options -
        // callers always resend the full list. Removing via the DbSet and adding the replacements
        // as new entities (instead of also clearing the loaded navigation collection) avoids the
        // change tracker processing the same rows as both an explicit delete and an orphan removal.
        _dbContext.QuestionOptions.RemoveRange(question.QuestionOptions);

        foreach (var option in request.Options)
        {
            _dbContext.QuestionOptions.Add(new QuestionOption
            {
                QuestionId = question.Id,
                OptionText = option.OptionText,
                IsCorrect = option.IsCorrect,
                DisplayOrder = option.DisplayOrder,
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
