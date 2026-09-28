using Certification.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Certification.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Module> Modules { get; }

    DbSet<CertificationDefinition> Certifications { get; }

    DbSet<Question> Questions { get; }

    DbSet<QuestionOption> QuestionOptions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
