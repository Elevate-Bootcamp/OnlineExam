using MediatR;
using OnlineExam.Features.Categories.Dtos;

namespace OnlineExam.Features.Categories.Commands
{
    public record CreateCategoryCommand(createCategoryDTo CreateCategoryDTo) : IRequest<int>;
}
