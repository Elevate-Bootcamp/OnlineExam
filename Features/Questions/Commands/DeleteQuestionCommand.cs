using MediatR;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Questions.Commands
{
    public record DeleteQuestionCommand(int QuestionId) : IRequest<ServiceResponse<bool>>;
}