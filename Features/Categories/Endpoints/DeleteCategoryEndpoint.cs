using MediatR;
using OnlineExam.Features.Categories.Commands;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Endpoints
{
    public static class DeleteCategoryEndpoint
    {
        public static void MapDeleteCategoryEndpoint(this WebApplication app)
        {
            app.MapDelete("/api/categories/{id}", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteCategoryCommand(id));
                return result;
            })
            .WithName("DeleteCategory")
            .WithTags("Categories")
            .Produces<ServiceResponse<bool>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<bool>>(StatusCodes.Status404NotFound);
        }
    }
}