using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Infrastructure.EntityConfigurations
{
    public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
    {
        public void Configure(EntityTypeBuilder<UserAnswer> builder)
        {
            builder.ToTable("UserAnswers");

            // BaseEntity properties
            builder.HasKey(ua => ua.Id);
            builder.Property(ua => ua.Id).ValueGeneratedOnAdd();

            builder.Property(ua => ua.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .HasColumnName("CreatedAt");

            builder.Property(ua => ua.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedAt");

            builder.Property(ua => ua.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            // Specific properties
            builder.Property(ua => ua.AttemptId)
                .IsRequired()
                .HasColumnName("AttemptId");

            builder.Property(ua => ua.QuestionId)
                .IsRequired()
                .HasColumnName("QuestionId");

            builder.Property(ua => ua.SelectedChoiceIds)
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnName("SelectedChoiceIds");

            builder.Property(ua => ua.IsCorrect)
                .IsRequired()
                .HasColumnName("IsCorrect");

            builder.HasOne(ua => ua.Attempt)
                .WithMany(a => a.UserAnswers)
                .HasForeignKey(ua => ua.AttemptId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}