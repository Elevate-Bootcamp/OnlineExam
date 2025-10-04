using MediatR;
using OnlineExam.Features.Categories.Dtos;

namespace OnlineExam.Features.Categories.Queries
{
    public record GetCategoriesQueryForAdmin(int PageNumber = 1, int PageSize = 10, string? Search = null, string? SortBy = null) : IRequest<List<AdminCategoryDto>>;

}
