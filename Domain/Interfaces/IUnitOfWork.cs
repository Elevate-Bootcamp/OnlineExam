using Microsoft.EntityFrameworkCore.Storage;

namespace OnlineExam.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Transaction Methods
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}