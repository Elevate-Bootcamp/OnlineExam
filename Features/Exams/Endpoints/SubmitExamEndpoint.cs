using MediatR;
using OnlineExam.Features.Exams.Commands;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Exams.Endpoints
{
    public static class SubmitExamEndpoint
    {
        public static void MapSubmitExamEndpoint(this WebApplication app)
        {
            app.MapPost("/api/exams/{id}/submit", async (int id, SubmitExamDto submitExamDto, IMediator mediator) =>
            {
                var result = await mediator.Send(new SubmitExamCommand(id, submitExamDto));
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("SubmitExam")
            .WithTags("Exams")
            .Produces<ServiceResponse<ExamResultDto>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<ExamResultDto>>(StatusCodes.Status400BadRequest)
            .Produces<ServiceResponse<ExamResultDto>>(StatusCodes.Status404NotFound);
        }
    }
}