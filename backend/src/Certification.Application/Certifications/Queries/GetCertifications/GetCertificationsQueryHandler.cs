using Certification.Application.Certifications.DTOs;
using Certification.Application.Common.Extensions;
using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Models;
using Certification.Contracts.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Certifications.Queries.GetCertifications;

public sealed class GetCertificationsQueryHandler : IRequestHandler<GetCertificationsQuery, ApiResponse<PagedResult<CertificationDto>>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapperService _mapperService;

    public GetCertificationsQueryHandler(IApplicationDbContext dbContext, IMapperService mapperService)
    {
        _dbContext = dbContext;
        _mapperService = mapperService;
    }

    public async Task<ApiResponse<PagedResult<CertificationDto>>> Handle(GetCertificationsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Certifications.Where(certification => !certification.IsDeleted);

        var search = request.SearchRequest.Search;
        if (search is not null)
        {
            query = query.Where(certification => certification.Code.Contains(search) || certification.Name.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var certifications = await query
            .OrderBy(certification => certification.DisplayOrder)
            .ThenBy(certification => certification.Name)
            .ApplyPaging(request.PagedRequest)
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<CertificationDto>
        {
            Items = certifications.Select(certification => _mapperService.Map<CertificationDto>(certification)).ToList(),
            TotalCount = totalCount,
            PageNumber = request.PagedRequest.PageNumber,
            PageSize = request.PagedRequest.PageSize,
        };

        return PagedResponse.Create(pagedResult);
    }
}
