using MediatR;

namespace OnlineExam.Features.Exams.Commands
{
    public sealed record CreateExamCommand(
     string Title,
     string IconUrl,
     int CategoryId,
     DateTime StartDateUtc,
     DateTime EndDateUtc,
     int DurationMinutes,
     string? Description
 ):IRequest<int>;

}



