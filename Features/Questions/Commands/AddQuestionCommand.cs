using MediatR;
using OnlineExam.Features.Questions.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Questions.Commands
{
    public record AddQuestionCommand(AddQuestionDto QuestionDto) : IRequest<ServiceResponse<QuestiondataDto>>;
}