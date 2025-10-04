using MediatR;

namespace OnlineExam.Features.Categories.Commands
{
    public record DeleteCategoryCommand(int Id) : IRequest<bool>;

}
