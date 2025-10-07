using MediatR;
using OnlineExam.Features.Questions.Commands;
using OnlineExam.Features.Questions.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Questions.Endpoints
{
    public static class UpdateQuestionEndpoint
    {
        public static void MapUpdateQuestionEndpoints(this WebApplication app)
        {                
            // PUT /api/questions/{id} - Update a question and its choices
            app.MapPut("/api/questions/{id}", async (
                IMediator mediator,
                int id,
                UpdateQuestionDto? questionDto) =>
            {
                var command = new UpdateQuestionCommand(id, questionDto);
                var result = await mediator.Send(command);

                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithTags("Questions")
            .WithName("UpdateQuestion")
            .Produces<ServiceResponse<QuestiondataDto>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<QuestiondataDto>>(StatusCodes.Status400BadRequest)
            .Produces<ServiceResponse<QuestiondataDto>>(StatusCodes.Status403Forbidden)
            .Produces<ServiceResponse<QuestiondataDto>>(StatusCodes.Status404NotFound)
            .Produces<ServiceResponse<QuestiondataDto>>(StatusCodes.Status500InternalServerError);
        }
    }
}