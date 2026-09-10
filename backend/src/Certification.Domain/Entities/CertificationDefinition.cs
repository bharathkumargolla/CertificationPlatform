using Certification.Domain.Common;

namespace Certification.Domain.Entities;

public sealed class CertificationDefinition : BaseEntity
{
    public Guid ModuleId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Version { get; set; }

    public string? Vendor { get; set; }

    public string? Description { get; set; }

    public int DurationInMinutes { get; set; }

    public int PassingScore { get; set; }

    public int CertificateValidityMonths { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public Module? Module { get; set; }
}
