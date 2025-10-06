using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.Features.Categories.Commands;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Endpoints
{
    public static class CreateCategoryEndpoint
    {
        public static void MapCreateCategoryEndpoint(this WebApplication app)
        {
            app.MapPost("/api/categories", async ([FromForm] createCategoryDTo createCategoryDTo, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateCategoryCommand(createCategoryDTo));
                return result;
            })
            .WithName("CreateCategory")
            .WithTags("Categories")
            .Produces<ServiceResponse<int>>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}