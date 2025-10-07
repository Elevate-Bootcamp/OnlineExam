using MediatR;
using OnlineExam.Shared.Responses;
using OnlineExam.Features.Exams.Dtos;

namespace OnlineExam.Features.Exams.Commands
{
    public record DeleteExamCommand(DeleteExamDto DeleteExamDto) : IRequest<ServiceResponse<bool>>;
}
