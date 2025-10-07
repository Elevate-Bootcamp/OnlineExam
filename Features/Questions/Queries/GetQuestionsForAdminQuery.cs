// GetQuestionsForAdminQuery.cs
using MediatR;
using OnlineExam.Features.Questions.Dtos;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Questions.Queries
{
    public record GetQuestionsForAdminQuery(QuestionQueryParameters Parameters)
        : IRequest<ServiceResponse<PagedResult<AdminQuestionDto>>>;
}