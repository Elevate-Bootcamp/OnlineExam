using MediatR;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Features.Exams.Queries;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Exams.Endpoints
{
    public static class GetUserExamsEndpoint
    {
        public static void MapUserExamEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/exams")
                .WithTags("Exams");
                

            // GET /api/exams - List exams (with optional category filter)
            group.MapGet("/", async (
                IMediator mediator,
                [AsParameters] GetExamsQuery query) =>
            {
                var result = await mediator.Send(query);
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("GetExams")
            .Produces<ServiceResponse<PagedResult<UserExamDto>>>(StatusCodes.Status200OK);

            
        }
    }
}