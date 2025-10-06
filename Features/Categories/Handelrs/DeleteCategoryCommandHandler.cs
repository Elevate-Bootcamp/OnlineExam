using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Commands;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ServiceResponse<bool>>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IGenericRepository<Category> categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(request.Id);
                if (category == null)
                {
                    return ServiceResponse<bool>.NotFoundResponse(
                        "Category not found",
                        "الفئة غير موجودة"
                    );
                }

                // Delete the icon file if it exists
                if (!string.IsNullOrEmpty(category.IconUrl))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", category.IconUrl.TrimStart('/'));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                _categoryRepository.Delete(category);
                await _unitOfWork.SaveChangesAsync();

                return ServiceResponse<bool>.SuccessResponse(
                    true,
                    "Category deleted successfully",
                    "تم حذف الفئة بنجاح"
                );
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.InternalServerErrorResponse(
                    "An error occurred while deleting category",
                    "حدث خطأ أثناء حذف الفئة"
                );
            }
        }
    }
}