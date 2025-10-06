using MediatR;
using OnlineExam.Features.Exams.Commands;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Exams.Endpoints
{
    public static class StartExamAttemptEndpoint
    {
        public static void MapStartExamAttemptEndpoint(this WebApplication app)
        {
            app.MapPost("/api/exams/{id}/attempt", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new StartExamAttemptCommand(id));
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("StartExamAttempt")
            .WithTags("Exams")
            .Produces<ServiceResponse<List<QuestionDto>>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<List<QuestionDto>>>(StatusCodes.Status400BadRequest)
            .Produces<ServiceResponse<List<QuestionDto>>>(StatusCodes.Status404NotFound);
        }
    }
}