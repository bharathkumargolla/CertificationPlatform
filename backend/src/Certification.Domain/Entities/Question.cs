using Certification.Domain.Common;
using Certification.Domain.Enums;

namespace Certification.Domain.Entities;

public sealed class Question : BaseEntity
{
    public Guid CertificationDefinitionId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public string? Explanation { get; set; }

    public QuestionType QuestionType { get; set; }

    public DifficultyLevel DifficultyLevel { get; set; }

    public int Points { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public CertificationDefinition? CertificationDefinition { get; set; }

    public ICollection<QuestionOption> QuestionOptions { get; set; } = new List<QuestionOption>();
}
