using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Questions.Dtos;

namespace OnlineExam.Features.Questions.Commands
{
    public record UpdateQuestionCommand(QuestionDTO Question):IRequest<bool>;
    public class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Question> _questionRepository;
        public UpdateQuestionHandler(IUnitOfWork unitOfWork , IGenericRepository<Question> questionRepository)
        {
            _unitOfWork = unitOfWork;
            _questionRepository = questionRepository;
        }
        public async Task<bool> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.Question.Id);
            if (question == null) return false;

            question.Title = request.Question.Title;
            question.ExamId = request.Question.ExamId;  
            question.Type = request.Question.Type;
            _questionRepository.Update(question);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }


}
