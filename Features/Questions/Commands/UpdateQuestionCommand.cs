using MediatR;
using OnlineExam.Features.Questions.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Questions.Commands
{
    public record UpdateQuestionCommand(int QuestionId, UpdateQuestionDto QuestionDto)
        : IRequest<ServiceResponse<QuestiondataDto>>;
}