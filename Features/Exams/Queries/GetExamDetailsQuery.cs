using MediatR;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Exams.Queries
{
    public record GetExamDetailsQuery(int Id) : IRequest<ServiceResponse<UserExamDetailsDto>>;
}