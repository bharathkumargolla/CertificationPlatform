using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using Certification.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Questions.Commands.CreateQuestion;

public sealed class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateQuestionCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var certificationDefinition = await _dbContext.Certifications
            .FirstOrDefaultAsync(existing => existing.Id == request.CertificationDefinitionId && !existing.IsDeleted, cancellationToken);

        if (certificationDefinition is null)
        {
            return Result.Failure<Guid>($"Certification with id '{request.CertificationDefinitionId}' was not found.");
        }

        if (!certificationDefinition.IsActive)
        {
            return Result.Failure<Guid>($"Certification '{certificationDefinition.Code}' is not active.");
        }

        var question = new Question
        {
            CertificationDefinitionId = request.CertificationDefinitionId,
            QuestionText = request.QuestionText,
            Explanation = request.Explanation,
            QuestionType = request.QuestionType,
            DifficultyLevel = request.DifficultyLevel,
            Points = request.Points,
            DisplayOrder = request.DisplayOrder,
            CreatedAtUtc = DateTime.UtcNow,
        };

        foreach (var option in request.Options)
        {
            question.QuestionOptions.Add(new QuestionOption
            {
                OptionText = option.OptionText,
                IsCorrect = option.IsCorrect,
                DisplayOrder = option.DisplayOrder,
            });
        }

        _dbContext.Questions.Add(question);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Id);
    }
}
