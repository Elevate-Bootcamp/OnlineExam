using MediatR;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Queries
{
    public record GetCategoriesQueryForAdmin(
        int PageNumber = 1,
        int PageSize = 10,
        string? Search = null,
        string? SortBy = null
    ) : IRequest<ServiceResponse<PagedResult<AdminCategoryDto>>>;
}