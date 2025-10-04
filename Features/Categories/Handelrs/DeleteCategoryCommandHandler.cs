using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Commands;

namespace OnlineExam.Features.Categories.Handelrs
{
    public class DeleteCategoryCommandHandler(IGenericRepository<Category> _categoryRepository , IUnitOfWork _unitOfWork) : IRequestHandler<DeleteCategoryCommand, bool>
    {
        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                return false;

            if (!string.IsNullOrEmpty(category.IconUrl))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", category.IconUrl.TrimStart('/'));
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            _categoryRepository.Delete(category);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
