using MediatR;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Features.Exams.Queries;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Exams.Endpoints
{
    public static class GetExamByIDEndpoint
    {
        public static void MapGetExamByIDEndpoint(this WebApplication app)
        {
            var group = app.MapGroup("/api/exams")
                .WithTags("Exams");

            // GET /api/exams/{id} - Get exam details
            group.MapGet("/{id}", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetExamDetailsQuery(id));
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("GetExamDetails")
            .Produces<ServiceResponse<UserExamDetailsDto>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<UserExamDetailsDto>>(StatusCodes.Status404NotFound);
        }

    }
}
