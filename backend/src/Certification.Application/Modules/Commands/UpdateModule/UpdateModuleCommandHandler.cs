using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Modules.Commands.UpdateModule;

public sealed class UpdateModuleCommandHandler : IRequestHandler<UpdateModuleCommand, Result>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateModuleCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
    {
        var module = await _dbContext.Modules
            .FirstOrDefaultAsync(existing => existing.Id == request.Id && !existing.IsDeleted, cancellationToken);

        if (module is null)
        {
            return Result.Failure($"Module with id '{request.Id}' was not found.");
        }

        module.Name = request.Name;
        module.Description = request.Description;
        module.DisplayOrder = request.DisplayOrder;
        module.IsActive = request.IsActive;
        module.ModifiedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
