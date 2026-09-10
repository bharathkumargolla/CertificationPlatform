using Certification.Application.Common.Commands;
using Certification.Application.Common.Results;

namespace Certification.Application.Certifications.Commands.UpdateCertification;

public sealed class UpdateCertificationCommand : ICommand<Result>
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
