using Certification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Certification.Infrastructure.Persistence.Configurations;

public sealed class QuestionConfiguration : EntityConfigurationBase<Question>
{
    public override void Configure(EntityTypeBuilder<Question> builder)
    {
        base.Configure(builder);

        builder.ToTable("Questions");

        builder.Property(question => question.QuestionText)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(question => question.Explanation)
            .HasMaxLength(4000);

        builder.Property(question => question.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(question => question.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(question => question.CertificationDefinitionId);

        builder.HasIndex(question => question.DisplayOrder);

        builder.HasOne(question => question.CertificationDefinition)
            .WithMany()
            .HasForeignKey(question => question.CertificationDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
