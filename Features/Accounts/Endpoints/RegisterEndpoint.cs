using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using OnlineExam.Features.Accounts.Commands;
using OnlineExam.Features.Accounts.Dtos;

namespace OnlineExam.Features.Accounts.Endpoints
{
    public static class RegisterEndpoint
    {
        public static void MapRegisterEndpoint(this WebApplication app)
        {
            app.MapPost("/api/accounts/register", async (UserDto request, IMediator mediator) =>
            {
                var accountService = await mediator.Send(new RegisterCommand(
                    request.UserName ?? string.Empty,
                    request.Email ?? string.Empty,
                    request.FullName ?? string.Empty,
                    request.Password ?? string.Empty
                ));
                if (accountService is null)
                {
                    return Results.BadRequest(accountService);
                }
                return Results.Ok("User registered successfully.");
            })
            .WithName("Register")
            .WithTags("Accounts");
        }
    }
}
