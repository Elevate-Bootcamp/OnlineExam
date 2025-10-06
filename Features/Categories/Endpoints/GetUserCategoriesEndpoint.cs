using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.Features.Categories.Queries;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Endpoints
{
    public static class GetUserCategoriesEndpoint
    {
        public static void MapGetUserCategoriesEndpoint(this WebApplication app)
        {
            app.MapGet("/api/categories/user", async (
                IMediator mediator,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 20
                ) =>
            {
                var result = await mediator.Send(new GetUserCategoriesQuery(pageNumber, pageSize));
                return result;
            })
            //.RequireAuthorization()
            .WithName("GetUserCategories")
            .WithTags("Categories")
            .Produces<ServiceResponse<object>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<object>>(StatusCodes.Status401Unauthorized)
            .Produces<ServiceResponse<object>>(StatusCodes.Status404NotFound);

        }
    }
}