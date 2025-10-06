using MediatR;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Exams.Dtos;
using OnlineExam.Features.Exams.Queries;
using OnlineExam.Shared.Responses;

namespace OnlineExam.Features.Exams.Handlers
{
    public class GetExamsForAdminQueryHandler : IRequestHandler<GetExamsForAdminQuery, ServiceResponse<PagedResult<AdminExamDto>>>
    {
        private readonly IGenericRepository<Exam> _examRepository;

        public GetExamsForAdminQueryHandler(IGenericRepository<Exam> examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ServiceResponse<PagedResult<AdminExamDto>>> Handle(GetExamsForAdminQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _examRepository.GetAll().Where(e => !e.IsDeleted);

                // Apply search filter
                if (!string.IsNullOrEmpty(request.Search))
                {
                    query = query.Where(e => e.Title.Contains(request.Search));
                }

                // Apply category filter
                if (request.CategoryId.HasValue)
                {
                    query = query.Where(e => e.CategoryId == request.CategoryId.Value);
                }

                // Apply active filter
                if (request.IsActive.HasValue)
                {
                    query = query.Where(e => e.IsActive == request.IsActive.Value);
                }

                // Apply sorting
                query = request.SortBy?.ToLower() switch
                {
                    "title" => query.OrderBy(e => e.Title),
                    "titledesc" => query.OrderByDescending(e => e.Title),
                    "startdate" => query.OrderBy(e => e.StartDate),
                    "startdatedesc" => query.OrderByDescending(e => e.StartDate),
                    "creationdate" => query.OrderBy(e => e.CreatedAt),
                    "creationdatedesc" => query.OrderByDescending(e => e.CreatedAt),
                    _ => query.OrderBy(e => e.Id)
                };

                var totalCount = query.Count();

                var exams = query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(e => new AdminExamDto
                    {
                        Id = e.Id,
                        Title = e.Title,
                        IconUrl = e.IconUrl,
                        CategoryName = e.Category!.Title,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Duration = e.Duration,
                        IsActive = e.IsActive,
                        CreationDate = e.CreatedAt,
                        Description = e.Description
                    })
                    .ToList();

                var pagedResult = new PagedResult<AdminExamDto>(exams, totalCount, request.PageNumber, request.PageSize);

                return ServiceResponse<PagedResult<AdminExamDto>>.SuccessResponse(
                    pagedResult,
                    "Exams retrieved successfully",
                    "تم استرجاع الامتحانات بنجاح"
                );
            }
            catch (Exception ex)
            {
                return ServiceResponse<PagedResult<AdminExamDto>>.InternalServerErrorResponse(
                    "An error occurred while retrieving exams",
                    "حدث خطأ أثناء استرجاع الامتحانات"
                );
            }
        }
    }
}