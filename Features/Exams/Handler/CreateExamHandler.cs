using MediatR;
using OnlineExam.Domain;
using OnlineExam.Features.Exams.Commands;
using OnlineExam.Infrastructure.ApplicationDBContext;
using System;

namespace OnlineExam.Features.Exams.Handler
{
    public sealed class CreateExamHandler:IRequestHandler<CreateExamCommand,int>
    {
        private readonly ApplicationDbContext _db;
        private readonly IPublisher _publisher;

        public CreateExamHandler(ApplicationDbContext db) { _db = db; }

        public async Task<int> Handle(CreateExamCommand cmd,CancellationToken ct)
        {
            var exam = new Exam
            {
                Title = cmd.Title.Trim(),
                IconUrl = cmd.IconUrl,
                CategoryId = cmd.CategoryId.Value,
                StartDate = cmd.StartDate,
                EndDate = cmd.EndDate,
                Duration = cmd.DurationMinutes,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Description = string.IsNullOrWhiteSpace(cmd.Description) ? null : cmd.Description!.Trim()
            };

            _db.Exams.Add(exam);
            await _db.SaveChangesAsync(ct);

            return exam.Id;
        }
    }
}