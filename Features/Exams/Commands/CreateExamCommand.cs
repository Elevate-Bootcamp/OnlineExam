using MediatR;

namespace OnlineExam.Features.Exams.Commands
{
    public sealed record CreateExamCommand(
     string Title,
     string IconUrl,
     int? CategoryId,
     DateTime StartDate,
     DateTime EndDate,
     int DurationMinutes,
     string? Description
                               ):IRequest<int>;

}



