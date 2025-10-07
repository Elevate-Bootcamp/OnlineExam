using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.Features.Exams.Commands;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Exams.Endpoints
{
    public static class DeleteExamEndpoint
    {
        public static void MapDeleteExamEndpoint(this WebApplication app)
        {
            app.MapDelete("api/exams/", async (
                [FromBody] DeleteExamDto dto,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteExamCommand(dto));
                return Results.Ok(result);
            })
            .WithTags("Exams")
            .WithSummary("Soft delete an exam and its related questions")
            .Produces<ServiceResponse<bool>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<bool>>(StatusCodes.Status404NotFound);
        }
    }
}
