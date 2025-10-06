using MediatR;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Queries
{
    public record GetUserCategoriesQuery(int PageNumber = 1, int PageSize = 20)
        : IRequest<ServiceResponse<PagedResult<UserCategoryDto>>>;
}