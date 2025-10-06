using MediatR;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Features.Exams.Queries;
using OnlineExam.Shared.Responses;
using System.Security.Claims;

namespace OnlineExam.Features.Exams.Endpoints
{
    public static class GetAdminExamsEndpoint
    {
        public static void MapAdminExamEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/admin/exams")
                .WithTags("Exams");
                

            // GET /api/admin/exams - List all exams with filters
            group.MapGet("/", async (
                IMediator mediator,
                HttpContext context,
                [AsParameters] GetExamsForAdminQuery query) =>
            {
                // Manual admin check
                var user = context.User;
                if (!user.IsInRole("Admin"))
                {
                    var forbiddenResponse = ServiceResponse<PagedResult<AdminExamDto>>.ForbiddenResponse();
                    return Results.Json(forbiddenResponse, statusCode: forbiddenResponse.StatusCode);
                }

                var result = await mediator.Send(query);
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("GetExamsForAdmin")
            .Produces<ServiceResponse<PagedResult<AdminExamDto>>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<PagedResult<AdminExamDto>>>(StatusCodes.Status403Forbidden);
        }
    }
}