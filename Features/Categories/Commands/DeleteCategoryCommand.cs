using MediatR;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Commands
{
    public record DeleteCategoryCommand(int Id) : IRequest<ServiceResponse<bool>>;
}