using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Commands;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Handlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ServiceResponse<int>>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(IGenericRepository<Category> categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
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
                        "يُسمح فقط بملفات الصور (jpg, jpeg, png, gif, svg)",
                        400
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
                // Log the exception
                return ServiceResponse<int>.InternalServerErrorResponse(
                    "An error occurred while creating category",
                    "حدث خطأ أثناء إنشاء الفئة"
                );
            }
        }
    }
}