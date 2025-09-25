using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExam.Domain;

namespace OnlineExam.Infrastructure.EntityConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Title");

            builder.Property(c => c.IconUrl)
                .IsRequired()
                .HasMaxLength(2048)
                .HasColumnName("IconUrl");

            builder.Property(c => c.CreationDate)
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(c => c.Description)
                .HasMaxLength(1000)
                .HasColumnName("Description");
        }
    }
}