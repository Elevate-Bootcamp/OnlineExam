using MediatR;
using OnlineExam.Features.UserAnswers.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.UserAnswers.Queries
{
    public record GetUserAnswerDetailQuery(int AttemptId) : IRequest<ServiceResponse<UserAnswerDetailDto>>;
}