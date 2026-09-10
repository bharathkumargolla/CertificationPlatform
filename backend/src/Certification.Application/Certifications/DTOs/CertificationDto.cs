namespace Certification.Application.Certifications.DTOs;

public sealed class CertificationDto
{
    public Guid Id { get; init; }

    public Guid ModuleId { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string? Version { get; init; }

    public string? Vendor { get; init; }

    public string? Description { get; init; }

    public int DurationInMinutes { get; init; }

    public int PassingScore { get; init; }

    public int CertificateValidityMonths { get; init; }

    public int DisplayOrder { get; init; }

    public bool IsActive { get; init; }
}
