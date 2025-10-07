using MediatR;
using OnlineExam.Features.Dashboard.Dtos;
using OnlineExam.Features.Dashboard.Queries;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Dashboard.Endpoints
{
    public static class MostActiveExamsEndpoint
    {
        public static void MapMostActiveExamsEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/admin/dashboard")
                .WithTags("Admin Dashboard")
                .RequireAuthorization();

            // GET /api/admin/dashboard/most-active-exams - Data for graph of most active exams
            group.MapGet("/most-active-exams", async (IMediator mediator) =>
            {
                var query = new GetMostActiveExamsQuery();
                var result = await mediator.Send(query);
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("GetMostActiveExams")
            .Produces<ServiceResponse<MostActiveExamsDto>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<MostActiveExamsDto>>(StatusCodes.Status403Forbidden)
            .Produces<ServiceResponse<MostActiveExamsDto>>(StatusCodes.Status500InternalServerError);
        }
    }
}