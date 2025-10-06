using MediatR;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Exams.Queries
{
    public record GetExamsQuery(
        int PageNumber = 1,
        int PageSize = 20,
        int? CategoryId = null
    ) : IRequest<ServiceResponse<PagedResult<UserExamDto>>>;
}