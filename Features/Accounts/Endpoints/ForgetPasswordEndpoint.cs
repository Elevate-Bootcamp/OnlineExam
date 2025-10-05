using MediatR;
using OnlineExam.Features.Accounts.Commands;
using OnlineExam.Features.Accounts.Dtos;
using OnlineExam.Features.Accounts.Orchestrators;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Accounts.Endpoints
{
    public static class ForgotPasswordEndpoint
    {
        public static void MapForgotPasswordEndpoint(this WebApplication app)
        {
            app.MapPost("/api/accounts/forgot-password", async (ForgotPasswordDto request, IMediator mediator) =>
            {
                var result = await mediator.Send(new OrchestrateForgetPasswordCommand(request));
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("ForgotPassword")
            .WithTags("Accounts");
        }
    }
}