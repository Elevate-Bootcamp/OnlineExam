using MediatR;
using OnlineExam.Features.Accounts.Queries;

namespace OnlineExam.Features.Profile.Endpoints
{
    public static class GetProfileEndpoint
    {
        public static void MapProfileEndpoint(this WebApplication app)
        {
            app.MapGet("/api/profile", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new Queries.GetProfileQuery());
                return result;
            })
                .RequireAuthorization()
                .WithName("get")
                .WithTags("Profile");
        }
    }
}
