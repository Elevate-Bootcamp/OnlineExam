using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using OnlineExam.Features.Accounts.Commands;
using OnlineExam.Features.Accounts.Dtos;
using OnlineExam.Features.Accounts.Orchestrators;

namespace OnlineExam.Features.Accounts.Endpoints
{
    public static class RegisterEndpoint
    {
        public static void MapRegisterEndpoint(this WebApplication app)
        {
            app.MapPost("/api/accounts/register", async (RegisterDto request, IMediator mediator) =>
            {
                var result = await mediator.Send(new OrchestrateRegistrationCommand(request));
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .WithName("Register")
            .WithTags("Accounts");
        }

    }
}
