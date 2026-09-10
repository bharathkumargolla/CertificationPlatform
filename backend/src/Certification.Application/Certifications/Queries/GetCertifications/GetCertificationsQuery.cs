using Certification.Application.Certifications.DTOs;
using Certification.Application.Common.Models;
using Certification.Application.Common.Queries;
using Certification.Contracts.Common;

namespace Certification.Application.Certifications.Queries.GetCertifications;

public sealed class GetCertificationsQuery : IQuery<ApiResponse<PagedResult<CertificationDto>>>
{
    public PagedRequest PagedRequest { get; init; } = new();

    public SearchRequest SearchRequest { get; init; } = new();

    public SortRequest SortRequest { get; init; } = new();
}
