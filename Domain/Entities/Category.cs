using System;

namespace OnlineExam.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string IconUrl { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string? Description { get; set; }
    }
}
