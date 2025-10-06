using MediatR;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Queries
{
    public record GetCategoryByIdQuery(int Id) : IRequest<ServiceResponse<CategoryDetailsDto>>;
}