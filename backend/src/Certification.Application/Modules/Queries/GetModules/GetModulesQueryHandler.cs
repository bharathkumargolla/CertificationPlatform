using Certification.Application.Common.Extensions;
using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Models;
using Certification.Application.Modules.DTOs;
using Certification.Contracts.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Modules.Queries.GetModules;

public sealed class GetModulesQueryHandler : IRequestHandler<GetModulesQuery, ApiResponse<PagedResult<ModuleDto>>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapperService _mapperService;

    public GetModulesQueryHandler(IApplicationDbContext dbContext, IMapperService mapperService)
    {
        _dbContext = dbContext;
        _mapperService = mapperService;
    }

    public async Task<ApiResponse<PagedResult<ModuleDto>>> Handle(GetModulesQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Modules.Where(module => !module.IsDeleted);

        var search = request.SearchRequest.Search;
        if (search is not null)
        {
            query = query.Where(module => module.Code.Contains(search) || module.Name.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var modules = await query
            .OrderBy(module => module.DisplayOrder)
            .ApplyPaging(request.PagedRequest)
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<ModuleDto>
        {
            Items = modules.Select(module => _mapperService.Map<ModuleDto>(module)).ToList(),
            TotalCount = totalCount,
            PageNumber = request.PagedRequest.PageNumber,
            PageSize = request.PagedRequest.PageSize,
        };

        return PagedResponse.Create(pagedResult);
    }
}
