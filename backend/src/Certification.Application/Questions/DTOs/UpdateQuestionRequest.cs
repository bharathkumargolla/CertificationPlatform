using Certification.Domain.Enums;

namespace Certification.Application.Questions.DTOs;

public sealed class UpdateQuestionRequest
{
    public string QuestionText { get; init; } = string.Empty;

    public string? Explanation { get; init; }

    public QuestionType QuestionType { get; init; }

    public DifficultyLevel DifficultyLevel { get; init; }

    public int Points { get; init; }

    public int DisplayOrder { get; init; }

    public bool IsActive { get; init; }

    public List<QuestionOptionDto> Options { get; init; } = [];
}
