using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExam.Domain;

namespace OnlineExam.Infrastructure.EntityConfigurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(q => q.Id);
            builder.Property(q => q.Id).ValueGeneratedOnAdd();

            builder.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("Title");

            builder.Property(q => q.Type)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Type");

            builder.Property(q => q.ExamId)
                .IsRequired()
                .HasColumnName("ExamId");

            builder.Property(q => q.CreationDate)
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(q => q.Exam)
                .WithMany()
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}