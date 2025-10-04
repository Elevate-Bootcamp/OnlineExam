using MediatR;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Questions.Dtos;

namespace OnlineExam.Features.Questions.Queries
{
    public record GetAllQuestionsQuery : IRequest<List<QuestionDTO>>;
    public class  QuestionHandler:IRequestHandler<GetAllQuestionsQuery,List<QuestionDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<QuestionDTO>> Handle(GetAllQuestionsQuery request,CancellationToken cancellationToken)
        {
            try
            {
                var questions = await _unitOfWork.Questions.GetAllAsync();
                var questionDTOs = questions.Select(q => new QuestionDTO
                {
                    Id = q.Id,
                    Title = q.Title,
                    ExamId = q.ExamId,
                    Type = q.Type,
                    CreationDate = q.CreationDate,
                }).ToList();
                return questionDTOs;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}
