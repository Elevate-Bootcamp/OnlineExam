using OnlineExam.Domain.Entities;
using System;

namespace OnlineExam.Domain
{
    public class Category : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public string? Description { get; set; }
    }
}
