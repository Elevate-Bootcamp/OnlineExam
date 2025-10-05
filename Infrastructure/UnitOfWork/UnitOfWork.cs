using Microsoft.EntityFrameworkCore.Storage;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Infrastructure.ApplicationDBContext;
using OnlineExam.Infrastructure.Repositories;

namespace OnlineExam.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

        }

     
        // Transaction methods
        public Task<IDbContextTransaction> BeginTransactionAsync()
            => _context.Database.BeginTransactionAsync();

        public void Dispose() => _context.Dispose();


        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
