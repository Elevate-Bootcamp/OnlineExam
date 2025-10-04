using MediatR;
using OnlineExam.Domain.Interfaces;

namespace OnlineExam.Features.Questions.Commands
{
    public record DeleteQuestionCommand(int id) : IRequest<bool>;
    public class DeleteQuestionHandler : IRequestHandler<DeleteQuestionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteQuestionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _unitOfWork.Question.GetByIdAsync(request.id);
            if (question == null) return false;
            _unitOfWork.Question.Delete(question);
            return true;
        }
    }

}
