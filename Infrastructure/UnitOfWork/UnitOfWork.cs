using Microsoft.EntityFrameworkCore.Storage;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Infrastructure.ApplicationDBContext;
using OnlineExam.Infrastructure.Repositories;

namespace OnlineExam.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // Repository instances
        public IAnswerRepository Answers { get; }
        public ICategoryRepository Categories { get; }
        public IQuestionRepository Questions { get; }
        public IExamRepository Exams { get; }

        public IQuestionRepository Question => throw new NotImplementedException();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            // Initialize repositories
            Answers = new AnswerRepository(_context);
            Categories = new CategoryRepository(_context);
            Questions = new QuestionRepository(_context);
            Exams = new ExamRepository(_context);

        }

        // Transaction methods
        public Task<IDbContextTransaction> BeginTransactionAsync()
            => _context.Database.BeginTransactionAsync();

        public int Complete() => _context.SaveChanges();

        public Task<int> CompleteAsync() => _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
