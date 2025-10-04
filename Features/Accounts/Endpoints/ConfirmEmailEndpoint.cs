using MediatR;
using OnlineExam.Features.Accounts.Commands;
using OnlineExam.Features.Accounts.Dtos;
using OnlineExam.Features.Accounts.Orchestrators;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Accounts.Endpoints
{
    public static class ConfirmEmailEndpoint
    {
        public static void MapConfirmEmailEndpoint(this WebApplication app)
        {
            app.MapPost("/api/accounts/confirm-email", async (ConfirmEmailWithCodeDto request, IMediator mediator) =>
            {
                var result = await mediator.Send(new EmailConfirmationOrchestrator(request));
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("ConfirmEmail")
            .WithTags("Accounts");
        }
    }
}