using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Commands;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;
using System.Security.Claims;
using System.Text;

namespace OnlineExam.Features.Categories.Handlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ServiceResponse<int>>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCategoryCommandHandler(
            IGenericRepository<Category> categoryRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor; // Remove the manual new() assignment
        }

        public async Task<ServiceResponse<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user is authenticated
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                {
                    return ServiceResponse<int>.UnauthorizedResponse(
                        "Authentication required",
                        "مطلوب مصادقة"
                    );
                }

                // Check if user is in Admin role
                if (!user.IsInRole("Admin"))
                {
                    return ServiceResponse<int>.ForbiddenResponse(
                        "Access forbidden. Admin role required.",
                        "الوصول ممنوع. مطلوب دور المسؤول."
                    );
                }

                // Validate that request and DTO are not null
                if (request?.CreateCategoryDTo == null)
                {
                    return ServiceResponse<int>.ErrorResponse(
                        "Invalid request data",
                        "بيانات الطلب غير صالحة",
                        400
                    );
                }

                // Validate Title
                if (string.IsNullOrWhiteSpace(request.CreateCategoryDTo.Title))
                {
                    return ServiceResponse<int>.ErrorResponse(
                        "Title is required",
                        "العنوان مطلوب",
                        400
                    );
                }

                // Validate file
                if (request.CreateCategoryDTo.Icon == null || request.CreateCategoryDTo.Icon.Length == 0)
                {
                    return ServiceResponse<int>.ErrorResponse(
                        "Icon file is required",
                        "ملف الأيقونة مطلوب",
                        400
                    );
                }

                // Validate file size (e.g., 5MB max)
                if (request.CreateCategoryDTo.Icon.Length > 5 * 1024 * 1024)
                {
                    return ServiceResponse<int>.ErrorResponse(
                        "Icon file size must be less than 5MB",
                        "يجب أن يكون حجم ملف الأيقونة أقل من 5 ميجابايت",
                        400
                    );
                }

                // Validate file extension
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg" };
                var fileExtension = Path.GetExtension(request.CreateCategoryDTo.Icon.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return ServiceResponse<int>.ErrorResponse(
                        "Only image files are allowed (jpg, jpeg, png, gif, svg)",
                        "يُسمح فقط بملfiles الصور (jpg, jpeg, png, gif, svg)",
                        400
                    );
                }

                // Validate title is unique
                var existingCategory = await _categoryRepository.FirstOrDefaultAsync(c => c.Title == request.CreateCategoryDTo.Title);
                if (existingCategory != null)
                {
                    return ServiceResponse<int>.ConflictResponse(
                        "Category title must be unique",
                        "يجب أن يكون عنوان الفئة فريدًا"
                    );
                }

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + request.CreateCategoryDTo.Icon.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.CreateCategoryDTo.Icon.CopyToAsync(fileStream, cancellationToken);
                }

                var category = new Category
                {
                    Title = request.CreateCategoryDTo.Title,
                    IconUrl = "/uploads/" + uniqueFileName,
                    CreatedAt = DateTime.UtcNow
                };

                await _categoryRepository.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResponse<int>.SuccessResponse(
                    category.Id,
                    "Category created successfully",
                    "تم إنشاء الفئة بنجاح"
                );
            }
            catch (Exception ex)
            {
                // Log the actual exception for debugging
                // _logger.LogError(ex, "Error creating category");

                // Safe error handling - don't access request properties in catch block
                return ServiceResponse<int>.InternalServerErrorResponse(
                    "An error occurred while creating category",
                    "حدث خطأ أثناء إنشاء الفئة"
                );
            }
        }
    }
}