﻿using MediatR;
using OnlineExam.Features.Accounts.Commands;
using OnlineExam.Shared.Responses;
using System.Security.Claims;

namespace OnlineExam.Features.Accounts.Endpoints
{
    public static class LogoutEndpoint
    {
        public static void MapLogoutEndpoint(this WebApplication app)
        {
            app.MapPost("/api/accounts/logout", async (ClaimsPrincipal user, IMediator mediator) =>
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
                if (string.IsNullOrEmpty(userId))
                    return Results.Unauthorized();

                var result = await mediator.Send(new LogoutCommand(userId));
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .RequireAuthorization()
            .WithName("Logout")
            .WithTags("Accounts");
        }
    }
}