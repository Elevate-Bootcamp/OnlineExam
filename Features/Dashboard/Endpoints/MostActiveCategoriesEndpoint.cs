using MediatR;
using OnlineExam.Features.Dashboard.Dtos;
using OnlineExam.Features.Dashboard.Queries;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Dashboard.Endpoints
{
    public static class MostActiveCategoriesEndpoint
    {
        public static void MapMostActiveCategoriesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/admin/dashboard")
                .WithTags("Admin Dashboard")
                .RequireAuthorization();

            // GET /api/admin/dashboard/most-active-categories - Data for graph of most active categories
            group.MapGet("/most-active-categories", async (IMediator mediator) =>
            {
                var query = new GetMostActiveCategoriesQuery();
                var result = await mediator.Send(query);
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("GetMostActiveCategories")
            .Produces<ServiceResponse<MostActiveCategoriesDto>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<MostActiveCategoriesDto>>(StatusCodes.Status403Forbidden)
            .Produces<ServiceResponse<MostActiveCategoriesDto>>(StatusCodes.Status500InternalServerError);
        }
    }
}