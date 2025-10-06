using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Commands;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;
using System.Security.Claims;

namespace OnlineExam.Features.Categories.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ServiceResponse<bool>>
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        public DeleteCategoryCommandHandler(IGenericRepository<Category> categoryRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user is authenticated
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                {
                    return ServiceResponse<bool>.UnauthorizedResponse(
                        "Authentication required",
                        "مطلوب مصادقة"
                    );
                }
                // get all the roles of the user
                var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

                // Check if user is in Admin role
                if (!user.IsInRole("Admin"))
                {
                    return ServiceResponse<bool>.ForbiddenResponse(
                        "Access forbidden. Admin role required.",
                        "الوصول ممنوع. مطلوب دور المسؤول."
                    );
                }
                var category = await _categoryRepository.GetByIdAsync(request.Id);
                if (category == null || category.IsDeleted)
                {
                    return ServiceResponse<bool>.NotFoundResponse(
                        "Category not found",
                        "الفئة غير موجودة"
                    );
                }
                if (category.Exams.Count > 0)
                {
                    return ServiceResponse<bool>.ConflictResponse(
                        "Category cannot be deleted because it has associated exams",
                        "لا يمكن حذف الفئة لأنها تحتوي على امتحانات مرتبطة"
                    );
                }

                //// Delete the icon file if it exists
                //if (!string.IsNullOrEmpty(category.IconUrl))
                //{
                //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", category.IconUrl.TrimStart('/'));
                //    if (File.Exists(filePath))
                //    {
                //        File.Delete(filePath);
                //    }
                //}

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