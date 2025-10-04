using OnlineExam.Domain;
using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Enums;

namespace OnlineExam.Features.Questions.Dtos
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ExamId { get; set; }
        public QuestionType Type { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
