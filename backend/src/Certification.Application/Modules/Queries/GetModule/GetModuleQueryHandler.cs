using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using Certification.Application.Modules.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Modules.Queries.GetModule;

public sealed class GetModuleQueryHandler : IRequestHandler<GetModuleQuery, Result<ModuleDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapperService _mapperService;

    public GetModuleQueryHandler(IApplicationDbContext dbContext, IMapperService mapperService)
    {
        _dbContext = dbContext;
        _mapperService = mapperService;
    }

    public async Task<Result<ModuleDto>> Handle(GetModuleQuery request, CancellationToken cancellationToken)
    {
        var module = await _dbContext.Modules
            .FirstOrDefaultAsync(existing => existing.Id == request.Id && !existing.IsDeleted, cancellationToken);

        if (module is null)
        {
            return Result.Failure<ModuleDto>($"Module with id '{request.Id}' was not found.");
        }

        return Result.Success(_mapperService.Map<ModuleDto>(module));
    }
}
