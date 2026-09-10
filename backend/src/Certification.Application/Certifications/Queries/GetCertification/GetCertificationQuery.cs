using Certification.Application.Certifications.DTOs;
using Certification.Application.Common.Queries;
using Certification.Application.Common.Results;

namespace Certification.Application.Certifications.Queries.GetCertification;

public sealed class GetCertificationQuery : IQuery<Result<CertificationDto>>
{
    public Guid Id { get; init; }
}
