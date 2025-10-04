using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Questions.Dtos;

namespace OnlineExam.Features.Questions.Commands
{
    public record CreateQuestionCommand(QuestionDTO questionDTO) :IRequest<string>;
    public class CreateQuestionHandler : IRequestHandler <CreateQuestionCommand,string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Question> _questionRepository;


        public CreateQuestionHandler(IUnitOfWork unitOfWork, IGenericRepository<Question> questionRepository)
        {
            _unitOfWork = unitOfWork;
            _questionRepository = questionRepository;
        }
        public async Task<string> Handle(CreateQuestionCommand request,CancellationToken cancellationToken)
        {
            try
            {
                var question = new Question
                {
                    Title = request.questionDTO.Title,
                    ExamId = request.questionDTO.ExamId,
                    Type = request.questionDTO.Type,
                    CreationDate = DateTime.UtcNow,
                };
                await _questionRepository.AddAsync(question);
                await _unitOfWork.SaveChangesAsync();
                return "Question created successfully";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

        }

    }

}
