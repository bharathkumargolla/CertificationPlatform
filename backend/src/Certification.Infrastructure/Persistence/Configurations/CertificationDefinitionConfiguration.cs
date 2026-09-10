using Certification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Certification.Infrastructure.Persistence.Configurations;

public sealed class CertificationDefinitionConfiguration : EntityConfigurationBase<CertificationDefinition>
{
    public override void Configure(EntityTypeBuilder<CertificationDefinition> builder)
    {
        base.Configure(builder);

        builder.ToTable("Certifications");

        builder.Property(certification => certification.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(certification => certification.Code)
            .IsUnique();

        builder.Property(certification => certification.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(certification => certification.Vendor)
            .HasMaxLength(100);

        builder.Property(certification => certification.Version)
            .HasMaxLength(50);

        builder.Property(certification => certification.Description)
            .HasMaxLength(1000);

        builder.Property(certification => certification.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(certification => certification.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(certification => certification.ModuleId);

        builder.HasOne(certification => certification.Module)
            .WithMany()
            .HasForeignKey(certification => certification.ModuleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
