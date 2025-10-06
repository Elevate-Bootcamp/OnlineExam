using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Commands;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ServiceResponse<int>>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IGenericRepository<Category> categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<int>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(request.Id);
                if (category == null)
                {
                    return ServiceResponse<int>.NotFoundResponse(
                        "Category not found",
                        "الفئة غير موجودة"
                    );
                }

                // Validate file if provided
                if (request.UpdateCategoryDTo.Icon != null)
                {
                    if (request.UpdateCategoryDTo.Icon.Length == 0)
                    {
                        return ServiceResponse<int>.ErrorResponse(
                            "Icon file is empty",
                            "ملف الأيقونة فارغ",
                            400
                        );
                    }

                    // Validate file size (e.g., 5MB max)
                    if (request.UpdateCategoryDTo.Icon.Length > 5 * 1024 * 1024)
                    {
                        return ServiceResponse<int>.ErrorResponse(
                            "Icon file size must be less than 5MB",
                            "يجب أن يكون حجم ملف الأيقونة أقل من 5 ميجابايت",
                            400
                        );
                    }

                    // Validate file extension
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg" };
                    var fileExtension = Path.GetExtension(request.UpdateCategoryDTo.Icon.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return ServiceResponse<int>.ErrorResponse(
                            "Only image files are allowed (jpg, jpeg, png, gif, svg)",
                            "يُسمح فقط بملفات الصور (jpg, jpeg, png, gif, svg)",
                            400
                        );
                    }

                    // Delete old icon file if it exists
                    if (!string.IsNullOrEmpty(category.IconUrl))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", category.IconUrl.TrimStart('/'));
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }

                    // Upload new icon file
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + request.UpdateCategoryDTo.Icon.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.UpdateCategoryDTo.Icon.CopyToAsync(fileStream, cancellationToken);
                    }

                    category.IconUrl = "/uploads/" + uniqueFileName;
                }

                // Update title
                category.Title = request.UpdateCategoryDTo.Title;
                category.UpdatedAt = DateTime.UtcNow; // Assuming you have this property

                _categoryRepository.Update(category);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResponse<int>.SuccessResponse(
                    category.Id,
                    "Category updated successfully",
                    "تم تحديث الفئة بنجاح"
                );
            }
            catch (Exception ex)
            {
                return ServiceResponse<int>.InternalServerErrorResponse(
                    "An error occurred while updating category",
                    "حدث خطأ أثناء تحديث الفئة"
                );
            }
        }
    }
}