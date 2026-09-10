using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Certifications.Commands.DeleteCertification;

public sealed class DeleteCertificationCommandHandler : IRequestHandler<DeleteCertificationCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteCertificationCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteCertificationCommand request, CancellationToken cancellationToken)
    {
        var certification = await _dbContext.Certifications
            .FirstOrDefaultAsync(existing => existing.Id == request.Id && !existing.IsDeleted, cancellationToken);

        if (certification is null)
        {
            return Result.Failure($"Certification with id '{request.Id}' was not found.");
        }

        certification.IsDeleted = true;
        certification.DeletedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
