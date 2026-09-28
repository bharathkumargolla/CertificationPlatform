using Certification.Domain.Enums;

namespace Certification.Application.Questions.DTOs;

public sealed class QuestionDto
{
    public Guid Id { get; init; }

    public Guid CertificationDefinitionId { get; init; }

    public string QuestionText { get; init; } = string.Empty;

    public string? Explanation { get; init; }

    public QuestionType QuestionType { get; init; }

    public DifficultyLevel DifficultyLevel { get; init; }

    public int Points { get; init; }

    public int DisplayOrder { get; init; }

    public bool IsActive { get; init; }

    public IReadOnlyList<QuestionOptionDto> Options { get; init; } = [];
}
