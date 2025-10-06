using MediatR;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Exams.Commands
{
    public record SubmitExamCommand(int ExamId, SubmitExamDto SubmitExamDto) : IRequest<ServiceResponse<ExamResultDto>>;
}