using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Modules.Commands.DeleteModule;

public sealed class DeleteModuleCommandHandler : IRequestHandler<DeleteModuleCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteModuleCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {
        var module = await _dbContext.Modules
            .FirstOrDefaultAsync(existing => existing.Id == request.Id && !existing.IsDeleted, cancellationToken);

        if (module is null)
        {
            return Result.Failure($"Module with id '{request.Id}' was not found.");
        }

        module.IsDeleted = true;
        module.DeletedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
