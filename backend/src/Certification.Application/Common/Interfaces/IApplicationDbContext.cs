using Certification.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Module> Modules { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
