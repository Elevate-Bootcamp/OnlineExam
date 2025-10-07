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
    public class GetMostActiveCategoriesQueryHandler : IRequestHandler<GetMostActiveCategoriesQuery, ServiceResponse<MostActiveCategoriesDto>>
    {
        private readonly IGenericRepository<UserExamAttempt> _userExamAttemptRepository;
        private readonly IGenericRepository<Exam> _examRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMostActiveCategoriesQueryHandler(
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

        public async Task<ServiceResponse<MostActiveCategoriesDto>> Handle(GetMostActiveCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user is authenticated and is Admin
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true || !user.IsInRole("Admin"))
                {
                    return ServiceResponse<MostActiveCategoriesDto>.ForbiddenResponse(
                        "Access forbidden. Admin role required.",
                        "الوصول ممنوع. مطلوب دور المسؤول."
                    );
                }

                // Get all exams with their categories
                var exams = await _examRepository.GetAll()
                    .Where(e => !e.IsDeleted)
                    .Select(e => new { e.Id, e.CategoryId })
                    .ToListAsync(cancellationToken);

                // Get all attempts with their exam IDs
                var attempts = await _userExamAttemptRepository.GetAll()
                    .Where(ua => !ua.IsDeleted)
                    .Select(ua => new { ua.Id, ua.ExamId, ua.UserId, ua.Score, ua.TotalQuestions, ua.FinishedAt })
                    .ToListAsync(cancellationToken);

                // Get all categories
                var categories = await _categoryRepository.GetAll()
                    .Where(c => !c.IsDeleted)
                    .Select(c => new { c.Id, c.Title })
                    .ToListAsync(cancellationToken);

                // Group exams by category
                var examsByCategory = exams
                    .GroupBy(e => e.CategoryId)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.Id).ToList());

                // Calculate category statistics
                var categoryActivities = categories.Select(category =>
                {
                    var categoryExamIds = examsByCategory.GetValueOrDefault(category.Id, new List<int>());
                    var categoryAttempts = attempts.Where(a => categoryExamIds.Contains(a.ExamId)).ToList();

                    var completedAttempts = categoryAttempts.Where(a => a.FinishedAt.HasValue && a.TotalQuestions > 0).ToList();
                    var averageScore = completedAttempts.Any()
                        ? completedAttempts.Average(a => (decimal)a.Score / a.TotalQuestions * 100)
                        : 0;

                    return new CategoryActivityDto
                    {
                        CategoryId = category.Id,
                        CategoryName = category.Title,
                        ExamCount = categoryExamIds.Count,
                        TotalAttempts = categoryAttempts.Count,
                        TotalParticipants = categoryAttempts.Select(a => a.UserId).Distinct().Count(),
                        AverageScore = Math.Round(averageScore, 2)
                    };
                })
                .OrderByDescending(c => c.TotalAttempts)
                .Take(10) // Top 10 most active categories
                .ToList();

                var result = new MostActiveCategoriesDto
                {
                    Categories = categoryActivities
                };

                return ServiceResponse<MostActiveCategoriesDto>.SuccessResponse(
                    result,
                    "Most active categories data retrieved successfully",
                    "تم استرجاع بيانات أكثر التصنيفات نشاطًا بنجاح"
                );
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"ERROR retrieving most active categories: {ex.Message}");
                Console.WriteLine($"STACK TRACE: {ex.StackTrace}");

                return ServiceResponse<MostActiveCategoriesDto>.InternalServerErrorResponse(
                    "An error occurred while retrieving most active categories data",
                    "حدث خطأ أثناء استرجاع بيانات أكثر التصنيفات نشاطًا"
                );
            }
        }
    }
}