using MediatR;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Commands
{
    public record UpdateCategoryCommand(int Id, UpdateCategoryDTo UpdateCategoryDTo) : IRequest<ServiceResponse<int>>;
}