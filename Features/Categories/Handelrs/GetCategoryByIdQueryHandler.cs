using MediatR;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Features.Categories.Queries;

namespace OnlineExam.Features.Categories.Handelrs
{
    public class GetCategoryByIdQueryHandler(ICategoryRepository _categoryRepository) : IRequestHandler<GetCategoryByIdQuery, CategoryDetailsDto>
    {
      
        public async Task<CategoryDetailsDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                return null;

            return new CategoryDetailsDto
            {
                Id = category.Id,
                Title = category.Title,
                IconUrl = category.IconUrl,
                CreationDate = category.CreationDate
            };
        }
    }

}
