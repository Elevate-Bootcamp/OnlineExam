using MediatR;
using OnlineExam.Features.Categories.Dtos;

namespace OnlineExam.Features.Categories.Queries
{
    public record GetUserCategoriesQuery(int PageNumber = 1, int PageSize = 20) : IRequest<List<UserCategoryDto>>;

}
