using Certification.Application.Common.Interfaces;
using Certification.Application.Common.Results;
using Certification.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Modules.Commands.CreateModule;

public sealed class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateModuleCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
    {
        var codeExists = await _dbContext.Modules
            .AnyAsync(module => module.Code == request.Code, cancellationToken);

        if (codeExists)
        {
            return Result.Failure<Guid>($"A module with code '{request.Code}' already exists.");
        }

        var module = new Module
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            DisplayOrder = request.DisplayOrder,
            CreatedAtUtc = DateTime.UtcNow,
        };

        _dbContext.Modules.Add(module);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(module.Id);
    }
}
