namespace Certification.Domain.Entities;

public sealed class QuestionOption
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid QuestionId { get; set; }

    public string OptionText { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }

    public int DisplayOrder { get; set; }

    public Question? Question { get; set; }
}
