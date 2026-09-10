using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Certifications.Commands.UpdateCertification;

public sealed class UpdateCertificationCommandHandler : IRequestHandler<UpdateCertificationCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateCertificationCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(UpdateCertificationCommand request, CancellationToken cancellationToken)
    {
        var certification = await _dbContext.Certifications
            .FirstOrDefaultAsync(existing => existing.Id == request.Id && !existing.IsDeleted, cancellationToken);

        if (certification is null)
        {
            return Result.Failure($"Certification with id '{request.Id}' was not found.");
        }

        certification.ModuleId = request.ModuleId;
        certification.Code = request.Code;
        certification.Name = request.Name;
        certification.Version = request.Version;
        certification.Vendor = request.Vendor;
        certification.Description = request.Description;
        certification.DurationInMinutes = request.DurationInMinutes;
        certification.PassingScore = request.PassingScore;
        certification.CertificateValidityMonths = request.CertificateValidityMonths;
        certification.DisplayOrder = request.DisplayOrder;
        certification.IsActive = request.IsActive;
        certification.ModifiedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
