using MediatR;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Plugins;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Commands;

namespace OnlineExam.Features.Categories.Handlers
{
    public class CreateCategoryCommandHandler(IGenericRepository<Category> _categoryRepository , IUnitOfWork _unitOfWork) : IRequestHandler<CreateCategoryCommand, int>
    {
       



        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {


            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot" , "uploads");
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
            return category.Id;

        }
    }
}
