using MediatR;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Commands
{
    public record CreateCategoryCommand(createCategoryDTo CreateCategoryDTo) : IRequest<ServiceResponse<int>>;
}