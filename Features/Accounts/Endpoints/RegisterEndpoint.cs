using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineExam.Features.Accounts.Dtos;
using OnlineExam.Features.Accounts.Commands;

namespace OnlineExam.Features.Accounts.Endpoints
{
    public static class RegisterEndpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapPost("/accounts/register", async (UserDto model, IMediator mediator) =>
            {
                // Validate input (basic test case)
                if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                {
                    return Results.BadRequest(new { Error = "Username, email, and password are required." });
                }

                // Test case: Simulate a successful registration
                var command = new RegisterCommand(model.UserName, model.Email, model.Password);
                var result = await mediator.Send(command);

                // Return success or failure based on MediatR result
                return result.IsSuccess
                    ? Results.Created($"/accounts/register/{result.Data?.Id}", new { Message = "Registration successful", UserId = result.Data?.Id })
                    : Results.BadRequest(new { Error = result.Error });
            }).Produces(201).Produces(400);
        }
    }
}