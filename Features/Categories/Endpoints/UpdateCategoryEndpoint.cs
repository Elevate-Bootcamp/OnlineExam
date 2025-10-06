using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.Features.Categories.Commands;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Endpoints
{
    public static class UpdateCategoryEndpoint
    {
        public static void MapUpdateCategoryEndpoint(this WebApplication app)
        {
            app.MapPut("/api/categories/{id}", async (int id, [FromForm] UpdateCategoryDTo editCategoryDTo, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateCategoryCommand(id, editCategoryDTo));
                return result;
            })
            .WithName("UpdateCategory")
            .WithTags("Categories")
            .Produces<ServiceResponse<int>>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}