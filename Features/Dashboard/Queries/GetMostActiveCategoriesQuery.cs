using MediatR;
using OnlineExam.Features.Dashboard.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Dashboard.Queries
{
    public record GetMostActiveCategoriesQuery : IRequest<ServiceResponse<MostActiveCategoriesDto>>;
}