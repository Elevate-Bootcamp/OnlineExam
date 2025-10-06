using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineExam.Features.Categories.Dtos;
using OnlineExam.Features.Categories.Queries;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Categories.Endpoints
{
    public static class GetCategoriesForAdminEndpoint
    {
        public static void MapGetCategoriesForAdminEndpoint(this WebApplication app)
        {
            app.MapGet("/api/admin/categories", async (
                IMediator mediator,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10,
                [FromQuery] string? search = null,
                [FromQuery] string? sortBy = null
                ) =>
            {
                var result = await mediator.Send(new GetCategoriesQueryForAdmin(pageNumber, pageSize, search, sortBy));
                return result;
            })
            .RequireAuthorization()
            .WithName("GetCategoriesForAdmin")
            .WithTags("Categories")
            .Produces<ServiceResponse<PagedResult<AdminCategoryDto>>>(StatusCodes.Status200OK)
            .Produces<ServiceResponse<PagedResult<AdminCategoryDto>>>(StatusCodes.Status401Unauthorized)
            .Produces<ServiceResponse<PagedResult<AdminCategoryDto>>>(StatusCodes.Status403Forbidden);
        }
    }
}