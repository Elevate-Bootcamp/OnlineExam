using MediatR;
using OnlineExam.Features.Questions.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Questions.Queries
{
    public record GetQuestionDetailsQuery(int QuestionId) : IRequest<ServiceResponse<QuestiondetailsDto>>;
}