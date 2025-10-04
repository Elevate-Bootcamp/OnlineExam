using MediatR;
using OnlineExam.Features.Categories.Dtos;

namespace OnlineExam.Features.Categories.Commands
{
    public record UpdateCategoryCommand(int Id, UpdateCategoryDTo UpdateCategoryDTo) : IRequest<int>;

}
