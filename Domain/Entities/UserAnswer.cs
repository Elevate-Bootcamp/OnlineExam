namespace OnlineExam.Domain.Entities
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        public string SelectedChoiceIds { get; set; } // Comma-separated selected choice IDs (for multi-choice)
        public bool IsCorrect { get; set; }
        // Navigation properties
        public UserExamAttempt Attempt { get; set; }

    }
}
