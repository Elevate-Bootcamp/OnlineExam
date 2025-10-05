using MediatR;
using OnlineExam.Features.Profile.Commands;
using OnlineExam.Features.Profile.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Profile.Endpoints
{
    public static class UpdateProfileEndpoint
    {
        public static void MapUpdateProfileEndpoint(this WebApplication app)
        {
            app.MapPut("/api/profile", async (UpdateProfileDto request, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateProfileCommand(request));
                return Results.Json(result, statusCode: result.StatusCode);
            })
            .RequireAuthorization()
            .WithName("UpdateProfile")
            .WithTags("Profile");
        }
    }
}