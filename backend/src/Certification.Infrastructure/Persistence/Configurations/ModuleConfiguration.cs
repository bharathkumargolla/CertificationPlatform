using Certification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Certification.Infrastructure.Persistence.Configurations;

public sealed class ModuleConfiguration : EntityConfigurationBase<Module>
{
    public override void Configure(EntityTypeBuilder<Module> builder)
    {
        base.Configure(builder);

        builder.ToTable("Modules");

        builder.Property(module => module.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(module => module.Code)
            .IsUnique();

        builder.Property(module => module.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(module => module.Description)
            .HasMaxLength(1000);

        builder.Property(module => module.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(module => module.IsActive)
            .HasDefaultValue(true);
    }
}
