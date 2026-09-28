using Certification.Application.Common.Commands;
using Certification.Application.Common.Results;
using Certification.Application.Questions.DTOs;
using Certification.Domain.Enums;

namespace Certification.Application.Questions.Commands.CreateQuestion;

public sealed class CreateQuestionCommand : ICommand<Result<Guid>>
{
    public Guid CertificationDefinitionId { get; init; }

    public string QuestionText { get; init; } = string.Empty;

    public string? Explanation { get; init; }

    public QuestionType QuestionType { get; init; }

    public DifficultyLevel DifficultyLevel { get; init; }

    public int Points { get; init; }

    public int DisplayOrder { get; init; }

    public IReadOnlyList<QuestionOptionDto> Options { get; init; } = [];
}
