using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using Certification.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Certifications.Commands.CreateCertification;

public sealed class CreateCertificationCommandHandler : IRequestHandler<CreateCertificationCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateCertificationCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Handle(CreateCertificationCommand request, CancellationToken cancellationToken)
    {
        var codeExists = await _dbContext.Certifications
            .AnyAsync(certification => certification.Code == request.Code, cancellationToken);

        if (codeExists)
        {
            return Result.Failure<Guid>($"A certification with code '{request.Code}' already exists.");
        }

        var module = await _dbContext.Modules
            .FirstOrDefaultAsync(existing => existing.Id == request.ModuleId && !existing.IsDeleted, cancellationToken);

        if (module is null)
        {
            return Result.Failure<Guid>($"Module with id '{request.ModuleId}' was not found.");
        }

        if (!module.IsActive)
        {
            return Result.Failure<Guid>($"Module '{module.Code}' is not active.");
        }

        var certification = new CertificationDefinition
        {
            ModuleId = request.ModuleId,
            Code = request.Code,
            Name = request.Name,
            Version = request.Version,
            Vendor = request.Vendor,
            Description = request.Description,
            DurationInMinutes = request.DurationInMinutes,
            PassingScore = request.PassingScore,
            CertificateValidityMonths = request.CertificateValidityMonths,
            DisplayOrder = request.DisplayOrder,
            CreatedAtUtc = DateTime.UtcNow,
        };

        _dbContext.Certifications.Add(certification);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(certification.Id);
    }
}
