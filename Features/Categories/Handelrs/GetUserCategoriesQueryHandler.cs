using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Features.Categories.Queries;

namespace OnlineExam.Features.Categories.Handlers
{
    public class GetUserCategoriesQueryHandler(IGenericRepository<Category> _categoryRepository) : IRequestHandler<GetUserCategoriesQuery, List<UserCategoryDto>>
    {
       
        public async Task<List<UserCategoryDto>> Handle(GetUserCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new UserCategoryDto
                {
                    Title = c.Title,
                    IconUrl = c.IconUrl
                }).ToList();
        }
    }
    }
