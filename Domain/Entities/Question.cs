﻿using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExam.Domain
{
    public class Question : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        [ForeignKey(nameof(Exam))]
        public int ExamId { get; set; }

        // Navigation property 
        public Exam? Exam { get; set; }
        public ICollection<Choice> Choices { get; set; } = [];
        public ICollection<UserAnswer> UserAnswers { get; set; } = [];

    }
}
