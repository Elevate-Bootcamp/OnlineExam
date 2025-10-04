using MediatR;
using OnlineExam.Features.Categories.Dtos;

namespace OnlineExam.Features.Categories.Queries
{
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDetailsDto>;

}
