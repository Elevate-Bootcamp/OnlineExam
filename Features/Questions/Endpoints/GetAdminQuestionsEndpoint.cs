using MediatR;
using OnlineExam.Features.Questions.Dtos;
using OnlineExam.Features.Questions.Queries;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Questions.Endpoints
{
    public static class GetAdminQuestionsEndpoint
    {
        public static void MapAdminQuestionEndpoint(this WebApplication app)
        {
            var group = app.MapGroup("/api/questions")
                .WithTags("Questions");

            // GET /api/admin/questions - List all questions with filters
            group.MapGet("/", async (
                IMediator mediator,
                HttpContext context,
                [AsParameters] QuestionQueryParameters parameters) =>
            {
                // Manual admin check
                var user = context.User;
                if (!user.IsInRole("Admin"))
                {
                    var forbiddenResponse = ServiceResponse<PagedResult<AdminQuestionDto>>.ForbiddenResponse();
                    return Results.Json(forbiddenResponse, statusCode: forbiddenResponse.StatusCode);
                }

                var query = new GetQuestionsForAdminQuery(parameters);
                var result = await mediator.Send(query);
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("GetQuestionsForAdmin")
            .Produces<ServiceResponse<PagedResult<AdminQuestionDto>>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<PagedResult<AdminQuestionDto>>>(StatusCodes.Status403Forbidden);
        }
    }
}