using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.Features.Accounts.Dtos;

namespace OnlineExam.Features.Accounts.Endpoints
{
    public static class LoginEndpoint
    {
        public static void MapLoginEndpoint(this WebApplication app)
        {
            app.MapPost("/api/accounts/login", async (LoginReqDTO request, IMediator mediator) =>
            {
                IResult result = await mediator.Send(new Commands.LoginCommand(request));

                return result;
            })
            .WithName("Login")
            .WithTags("Accounts");
        }

    }
}
