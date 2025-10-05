using MediatR;
using OnlineExam.Features.Accounts.Commands;
using OnlineExam.Features.Accounts.Dtos;
using OnlineExam.Features.Accounts.Orchestrators;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Accounts.Endpoints
{
    public static class ResetPasswordEndpoint
    {
        public static void MapResetPasswordEndpoint(this WebApplication app)
        {
            app.MapPost("/api/accounts/reset-password", async (ResetPasswordDto request, IMediator mediator) =>
            {
                var result = await mediator.Send(new ResetPasswordOrchestrator(request));
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("ResetPassword")
            .WithTags("Accounts");
        }
    }
}