using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Commands;

namespace OnlineExam.Features.Categories.Handlers
{
    public class UpdateCategoryCommandHandler(IGenericRepository<Category> _categoryRepository , IUnitOfWork _unitOfWork) : IRequestHandler<UpdateCategoryCommand, int>
    {
        public async Task<int> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null) throw new Exception("Category not found");
            category.Title = request.UpdateCategoryDTo.Title;

            if (request.UpdateCategoryDTo.Icon != null)
            {
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

             _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return category.Id;
        }
    }
}
