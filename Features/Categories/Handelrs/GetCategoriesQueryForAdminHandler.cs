using MediatR;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Features.Categories.Queries;

namespace OnlineExam.Features.Categories.Handelrs
{
    public class GetCategoriesQueryForAdminHandler(ICategoryRepository _categoryRepository) : IRequestHandler<GetCategoriesQueryForAdmin, List<AdminCategoryDto>>
    {
        public async Task<List<AdminCategoryDto>> Handle(GetCategoriesQueryForAdmin request, CancellationToken cancellationToken)
        {
            var query = await _categoryRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(request.Search))
                query = query.Where(c => c.Title.Contains(request.Search)).ToList();

            query = request.SortBy switch
            {
                "name" => query.OrderBy(c => c.Title).ToList(),
                "creationDate" => query.OrderBy(c => c.CreationDate).ToList(),
                _ => query
            };

            return query.Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .Select(c => new AdminCategoryDto
                        {
                            Id = c.Id,
                            Title = c.Title,
                            IconUrl = c.IconUrl,
                            CreationDate = c.CreationDate
                        }).ToList();
        }
    
    }
}
