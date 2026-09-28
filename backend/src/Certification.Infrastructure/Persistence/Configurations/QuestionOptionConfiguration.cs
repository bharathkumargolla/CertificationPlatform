using Certification.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Certification.Infrastructure.Persistence.Configurations;

public sealed class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
{
    public void Configure(EntityTypeBuilder<QuestionOption> builder)
    {
        builder.ToTable("QuestionOptions");

        builder.HasKey(option => option.Id);

        builder.Property(option => option.OptionText)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(option => option.DisplayOrder)
            .HasDefaultValue(0);

        builder.HasOne(option => option.Question)
            .WithMany(question => question.QuestionOptions)
            .HasForeignKey(option => option.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
