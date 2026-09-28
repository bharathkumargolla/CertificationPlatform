namespace Certification.Application.Questions.DTOs;

public sealed class QuestionOptionDto
{
    public Guid Id { get; init; }

    public string OptionText { get; init; } = string.Empty;

    public bool IsCorrect { get; init; }

    public int DisplayOrder { get; init; }
}
