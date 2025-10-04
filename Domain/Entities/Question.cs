using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Enums;

namespace OnlineExam.Domain
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
       // public string Type { get; set; } = string.Empty;
        public int ExamId { get; set; }
        public QuestionType Type { get; set; }
        public DateTime CreationDate { get; set; }
        // Navigation property to Exam
        public Exam? Exam { get; set; }

    }
}
