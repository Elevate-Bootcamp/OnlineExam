using MediatR;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Features.Categories.Queries;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Endpoints
{
    public static class GetCategoryByIdEndpoint
    {
        public static void MapGetCategoryByIdEndpoint(this WebApplication app)
        {
            app.MapGet("/api/categories/{id}", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoryByIdQuery(id));
                return result;
            })
            //.RequireAuthorization()
            .WithName("GetCategoryById")
            .WithTags("Categories")
            .Produces<ServiceResponse<object>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<object>>(StatusCodes.Status401Unauthorized)
            .Produces<ServiceResponse<object>>(StatusCodes.Status404NotFound);
        }
    }
}