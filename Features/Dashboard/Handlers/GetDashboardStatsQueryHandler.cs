using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Dashboard.Dtos;
using OnlineExam.Features.Dashboard.Queries;
using OnlineExam.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace OnlineExam.Features.Dashboard.Handlers
{
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, ServiceResponse<DashboardStatsDto>>
    {
        private readonly IGenericRepository<Exam> _examRepository;
        private readonly IGenericRepository<Question> _questionRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<UserExamAttempt> _userExamAttemptRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetDashboardStatsQueryHandler(
            IGenericRepository<Exam> examRepository,
            IGenericRepository<Question> questionRepository,
            IGenericRepository<Category> categoryRepository,
            IGenericRepository<UserExamAttempt> userExamAttemptRepository,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _userExamAttemptRepository = userExamAttemptRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user is authenticated and is Admin
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true || !user.IsInRole("Admin"))
                {
                    return ServiceResponse<DashboardStatsDto>.ForbiddenResponse(
                        "Access forbidden. Admin role required.",
                        "الوصول ممنوع. مطلوب دور المسؤول."
                    );
                }

                // Run queries sequentially to avoid DbContext threading issues
                var totalExams = await _examRepository.GetAll().CountAsync(e => !e.IsDeleted, cancellationToken);
                var activeExams = await _examRepository.GetAll().CountAsync(e => e.IsActive && !e.IsDeleted, cancellationToken);
                var inactiveExams = await _examRepository.GetAll().CountAsync(e => !e.IsActive && !e.IsDeleted, cancellationToken);
                var totalQuestions = await _questionRepository.GetAll().CountAsync(q => !q.IsDeleted, cancellationToken);
                var totalCategories = await _categoryRepository.GetAll().CountAsync(c => !c.IsDeleted, cancellationToken);
                var totalUsers = await _userManager.Users.CountAsync(u => u.EmailConfirmed, cancellationToken);
                var totalExamAttempts = await _userExamAttemptRepository.GetAll().CountAsync(ua => !ua.IsDeleted, cancellationToken);

                // Calculate average score
                decimal averageScore = 0;
                var completedAttempts = _userExamAttemptRepository.GetAll()
                    .Where(ua => !ua.IsDeleted && ua.FinishedAt.HasValue && ua.TotalQuestions > 0);

                if (await completedAttempts.AnyAsync(cancellationToken))
                {
                    averageScore = await completedAttempts
                        .AverageAsync(ua => (decimal)ua.Score / ua.TotalQuestions * 100, cancellationToken);
                }

                var stats = new DashboardStatsDto
                {
                    TotalExams = totalExams,
                    ActiveExams = activeExams,
                    InactiveExams = inactiveExams,
                    TotalQuestions = totalQuestions,
                    TotalCategories = totalCategories,
                    TotalUsers = totalUsers,
                    TotalExamAttempts = totalExamAttempts,
                    AverageScore = Math.Round(averageScore, 2)
                };

                return ServiceResponse<DashboardStatsDto>.SuccessResponse(
                    stats,
                    "Dashboard stats retrieved successfully",
                    "تم استرجاع إحصائيات لوحة التحكم بنجاح"
                );
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"ERROR retrieving dashboard stats: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                return ServiceResponse<DashboardStatsDto>.InternalServerErrorResponse(
                    $"An error occurred while retrieving dashboard stats: {ex.Message}",
                    $"حدث خطأ أثناء استرجاع إحصائيات لوحة التحكم: {ex.Message}"
                );
            }
        }
    }
}