using Certification.Application.Certifications.DTOs;
using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Certifications.Queries.GetCertification;

public sealed class GetCertificationQueryHandler : IRequestHandler<GetCertificationQuery, Result<CertificationDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapperService _mapperService;

    public GetCertificationQueryHandler(IApplicationDbContext dbContext, IMapperService mapperService)
    {
        _dbContext = dbContext;
        _mapperService = mapperService;
    }

    public async Task<Result<CertificationDto>> Handle(GetCertificationQuery request, CancellationToken cancellationToken)
    {
        var certification = await _dbContext.Certifications
            .FirstOrDefaultAsync(existing => existing.Id == request.Id && !existing.IsDeleted, cancellationToken);

        if (certification is null)
        {
            return Result.Failure<CertificationDto>($"Certification with id '{request.Id}' was not found.");
        }

        return Result.Success(_mapperService.Map<CertificationDto>(certification));
    }
}
