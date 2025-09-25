using Microsoft.EntityFrameworkCore.Storage;

namespace OnlineExam.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository Properties
        IAnswerRepository Answers { get; }
        ICategoryRepository Categories { get; }
        IQuestionRepository Questions { get; }
        IExamRepository Exams { get; }
        IQuestionRepository Question { get; }

        // Transaction Methods
        Task<int> CompleteAsync();
        int Complete();
        Task<int> SaveAsync();
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}

