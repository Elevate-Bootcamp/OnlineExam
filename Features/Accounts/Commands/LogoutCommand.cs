using MediatR;

namespace OnlineExam.Features.Accounts.Commands
{
    public record LogoutCommand(string str) : IRequest<string>;

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, string>
    {
        public async Task<string> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(request.str);
        }
    }
}
