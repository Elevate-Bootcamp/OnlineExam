using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace OnlineExam.Domain.Entities
{
    public class UserExamAttempt
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ExamId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime AttemptDate { get; set; }
        public bool IsHighestScore { get; set; } = false;
        // Navigation properties
        public ApplicationUser User { get; set; }
        public Exam Exam { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }

    }
}
