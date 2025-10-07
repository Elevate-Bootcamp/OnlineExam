using MediatR;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Exams.Commands
{
    public record CreateExamCommand(CreateExamDto CreateExamDto) : IRequest<ServiceResponse<int>>;

}


