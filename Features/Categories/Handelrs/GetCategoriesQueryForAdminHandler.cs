using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Features.Categories.Queries;

namespace OnlineExam.Features.Categories.Handlers
{
    public class GetCategoriesQueryForAdminHandler(IGenericRepository<Category> _categoryRepository) : IRequestHandler<GetCategoriesQueryForAdmin, List<AdminCategoryDto>>
    {
        public async Task<List<AdminCategoryDto>> Handle(GetCategoriesQueryForAdmin request, CancellationToken cancellationToken)
        {
            var query = _categoryRepository.GetAll();

            if (!string.IsNullOrEmpty(request.Search))
                query = query.Where(c => c.Title.Contains(request.Search));

            query = request.SortBy switch
            {
                "name" => query.OrderBy(c => c.Title),
                "creationDate" => query.OrderBy(c => c.CreatedAt),
                _ => query
            };

            return query.Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .Select(c => new AdminCategoryDto
                        {
                            Id = c.Id,
                            Title = c.Title,
                            IconUrl = c.IconUrl,
                            CreationDate = c.CreatedAt
                        }).ToList();
        }
    
    }
}
