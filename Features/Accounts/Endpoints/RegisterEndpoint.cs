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
            app.MapPost("/api/accounts/register", async (RegisterDto request, IMediator mediator) =>
            {
                var result = await mediator.Send(new RegisterCommand(request));
                return result;
            })
            .WithName("Register")
            .WithTags("Accounts");
        }
    }
}
