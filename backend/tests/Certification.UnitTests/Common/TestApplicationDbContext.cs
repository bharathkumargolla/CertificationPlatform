using Certification.Application.Common.Interfaces;
using Certification.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Certification.UnitTests.Common;

internal sealed class TestApplicationDbContext : DbContext, IApplicationDbContext
{
    public TestApplicationDbContext(DbContextOptions<TestApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Module> Modules => Set<Module>();

    public DbSet<CertificationDefinition> Certifications => Set<CertificationDefinition>();

    public static TestApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestApplicationDbContext(options);
    }
}
