﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineExam.Domain;
using OnlineExam.Domain.Enums;

namespace OnlineExam.Infrastructure.EntityConfigurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exams");

            // BaseEntity properties
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .HasColumnName("CreatedAt");

            builder.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedAt");

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            // Specific properties
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Title");

            builder.Property(e => e.IconUrl)
                .IsRequired()
                .HasMaxLength(2048)
                .HasColumnName("IconUrl");

            builder.Property(e => e.CategoryId)
                .IsRequired()
                .HasColumnName("CategoryId");

            builder.Property(e => e.StartDate)
                .IsRequired()
                .HasColumnType("datetime")
                .HasColumnName("StartDate");

            builder.Property(e => e.EndDate)
                .IsRequired()
                .HasColumnType("datetime")
                .HasColumnName("EndDate");

            builder.Property(e => e.Duration)
                .IsRequired()
                .HasColumnName("Duration");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("IsActive");

            builder.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("Description");

            builder.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}