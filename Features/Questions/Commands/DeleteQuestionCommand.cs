using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;

namespace OnlineExam.Features.Questions.Commands
{
    public record DeleteQuestionCommand(int id) : IRequest<bool>;
    public class DeleteQuestionHandler : IRequestHandler<DeleteQuestionCommand, bool>
    {
        private readonly IGenericRepository<Question> _questionRepository;
        public DeleteQuestionHandler(IGenericRepository<Question> questionRepository)
        {
            _questionRepository = questionRepository;
        }
        public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.id);
            if (question == null) return false;
            _questionRepository.Delete(question);
            return true;
        }
    }

}
