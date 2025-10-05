using OnlineExam.Domain.Entities;
using System;

namespace OnlineExam.Domain
{
    public class Exam : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public int CategoryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Description { get; set; }

        // navigation property
        public Category? Category { get; set; }

    }
}
