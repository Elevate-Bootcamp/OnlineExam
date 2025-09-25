using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Infrastructure.EntityConfigurations
{
    public class UserExamAttemptConfiguration : IEntityTypeConfiguration<UserExamAttempt>
    {
        public void Configure(EntityTypeBuilder<UserExamAttempt> builder)
        {
            builder.ToTable("UserExamAttempts");

            builder.HasKey(ua => ua.Id);
            builder.Property(ua => ua.Id).ValueGeneratedOnAdd();

            builder.Property(ua => ua.UserId)
                .IsRequired()
                .HasMaxLength(450)
                .HasColumnName("UserId");

            builder.Property(ua => ua.ExamId)
                .IsRequired()
                .HasColumnName("ExamId");

            builder.Property(ua => ua.Score)
                .IsRequired()
                .HasColumnName("Score");

            builder.Property(ua => ua.TotalQuestions)
                .IsRequired()
                .HasColumnName("TotalQuestions");

            builder.Property(ua => ua.AttemptDate)
                .IsRequired()
                .HasColumnType("datetime")
                .HasColumnName("AttemptDate");

            builder.Property(ua => ua.IsHighestScore)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsHighestScore");

            // Note: ApplicationUser is not provided; assuming it's an AspNetUsers entity with string Id
            builder.HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ua => ua.Exam)
                .WithMany()
                .HasForeignKey(ua => ua.ExamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}