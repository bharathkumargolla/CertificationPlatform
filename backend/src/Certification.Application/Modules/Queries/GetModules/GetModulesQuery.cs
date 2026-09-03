using Certification.Application.Common.Models;
using Certification.Application.Common.Queries;
using Certification.Application.Modules.DTOs;
using Certification.Contracts.Common;

namespace Certification.Application.Modules.Queries.GetModules;

public sealed class GetModulesQuery : IQuery<ApiResponse<PagedResult<ModuleDto>>>
{
    public PagedRequest PagedRequest { get; init; } = new();

    public SearchRequest SearchRequest { get; init; } = new();

    public SortRequest SortRequest { get; init; } = new();
}
