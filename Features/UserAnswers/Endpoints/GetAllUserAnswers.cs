using MediatR;
using OnlineExam.Features.UserAnswers.Dtos;
using OnlineExam.Features.UserAnswers.Queries;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.UserAnswers.Endpoints
{
    public static class GetAllUserAnswers
    {
        public static void MapGetAllUserAnswersEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/my-answers")
                .WithTags("User Answers");

            // GET /api/my-answers - List user's past exam attempts (paginated)
            group.MapGet("/", async (IMediator mediator, [AsParameters] GetUserAnswerHistoryQuery query) =>
            {
                var result = await mediator.Send(query);
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("GetUserAnswersHistory")
            .Produces<ServiceResponse<PagedUserAnswerHistoryDto>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<PagedUserAnswerHistoryDto>>(StatusCodes.Status401Unauthorized)
            .Produces<ServiceResponse<PagedUserAnswerHistoryDto>>(StatusCodes.Status500InternalServerError);

         
        }
    }
}