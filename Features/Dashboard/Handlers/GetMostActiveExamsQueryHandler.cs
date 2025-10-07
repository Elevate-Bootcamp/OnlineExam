using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Dashboard.Dtos;
using OnlineExam.Features.Dashboard.Queries;
using OnlineExam.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace OnlineExam.Features.Dashboard.Handlers
{
    public class GetMostActiveExamsQueryHandler : IRequestHandler<GetMostActiveExamsQuery, ServiceResponse<MostActiveExamsDto>>
    {
        private readonly IGenericRepository<UserExamAttempt> _userExamAttemptRepository;
        private readonly IGenericRepository<Exam> _examRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMostActiveExamsQueryHandler(
            IGenericRepository<UserExamAttempt> userExamAttemptRepository,
            IGenericRepository<Exam> examRepository,
            IGenericRepository<Category> categoryRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userExamAttemptRepository = userExamAttemptRepository;
            _examRepository = examRepository;
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<MostActiveExamsDto>> Handle(GetMostActiveExamsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user is authenticated and is Admin
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true || !user.IsInRole("Admin"))
                {
                    return ServiceResponse<MostActiveExamsDto>.ForbiddenResponse(
                        "Access forbidden. Admin role required.",
                        "الوصول ممنوع. مطلوب دور المسؤول."
                    );
                }

                // Get exam attempts grouped by exam
                var examAttempts = await _userExamAttemptRepository.GetAll()
                    .Where(ua => !ua.IsDeleted)
                    .GroupBy(ua => ua.ExamId)
                    .Select(g => new
                    {
                        ExamId = g.Key,
                        AttemptCount = g.Count(),
                        TotalParticipants = g.Select(ua => ua.UserId).Distinct().Count(),
                        AverageScore = g.Where(ua => ua.FinishedAt.HasValue && ua.TotalQuestions > 0)
                                       .Average(ua => (decimal)ua.Score / ua.TotalQuestions * 100)
                    })
                    .OrderByDescending(x => x.AttemptCount)
                    .Take(10) // Top 10 most active exams
                    .ToListAsync(cancellationToken);

                if (!examAttempts.Any())
                {
                    return ServiceResponse<MostActiveExamsDto>.SuccessResponse(
                        new MostActiveExamsDto { Exams = new List<ExamActivityDto>() },
                        "No exam activity data found",
                        "لم يتم العثور على بيانات نشاط الامتحانات"
                    );
                }

                // Get exam details
                var examIds = examAttempts.Select(e => e.ExamId).ToList();
                var exams = await _examRepository.GetAll()
                    .Where(e => examIds.Contains(e.Id) && !e.IsDeleted)
                    .Select(e => new { e.Id, e.Title, e.CategoryId, e.IsActive })
                    .ToListAsync(cancellationToken);

                // Get category details
                var categoryIds = exams.Select(e => e.CategoryId).Distinct().ToList();
                var categories = await _categoryRepository.GetAll()
                    .Where(c => categoryIds.Contains(c.Id) && !c.IsDeleted)
                    .Select(c => new { c.Id, c.Title })
                    .ToListAsync(cancellationToken);

                // Create lookups
                var examLookup = exams.ToDictionary(e => e.Id);
                var categoryLookup = categories.ToDictionary(c => c.Id);

                // Map to DTOs
                var examActivities = examAttempts.Select(ea =>
                {
                    var exam = examLookup.GetValueOrDefault(ea.ExamId);
                    var category = exam != null ? categoryLookup.GetValueOrDefault(exam.CategoryId) : null;

                    return new ExamActivityDto
                    {
                        ExamId = ea.ExamId,
                        ExamTitle = exam?.Title ?? "Unknown Exam",
                        CategoryName = category?.Title ?? "Unknown Category",
                        AttemptCount = ea.AttemptCount,
                        TotalParticipants = ea.TotalParticipants,
                        AverageScore = Math.Round(ea.AverageScore, 2),
                        IsActive = exam?.IsActive ?? false
                    };
                }).ToList();

                var result = new MostActiveExamsDto
                {
                    Exams = examActivities
                };

                return ServiceResponse<MostActiveExamsDto>.SuccessResponse(
                    result,
                    "Most active exams data retrieved successfully",
                    "تم استرجاع بيانات أكثر الامتحانات نشاطًا بنجاح"
                );
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"ERROR retrieving most active exams: {ex.Message}");
                Console.WriteLine($"STACK TRACE: {ex.StackTrace}");

                return ServiceResponse<MostActiveExamsDto>.InternalServerErrorResponse(
                    "An error occurred while retrieving most active exams data",
                    "حدث خطأ أثناء استرجاع بيانات أكثر الامتحانات نشاطًا"
                );
            }
        }
    }
}