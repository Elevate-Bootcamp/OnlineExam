using MediatR;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Questions.Dtos;

namespace OnlineExam.Features.Questions.Commands
{
    public record UpdateQuestionCommand(QuestionDTO Question):IRequest<bool>;
    public class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateQuestionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _unitOfWork.Questions.GetByIdAsync(request.Question.Id);
            if (question == null) return false;

            question.Title = request.Question.Title;
            question.ExamId = request.Question.ExamId;  
            question.Type = request.Question.Type;
            _unitOfWork.Questions.Update(question);
            return true;
        }
    }


}
