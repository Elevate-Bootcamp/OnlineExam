using OnlineExam.Domain;
using OnlineExam.Domain.Entities;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Infrastructure.ApplicationDBContext;
using System.Linq.Expressions;

namespace OnlineExam.Infrastructure.Repositories
{
    public class AnswerRepository: IAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public AnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(UserAnswer entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<UserAnswer> entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<UserAnswer, bool>>? criteria = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserAnswer entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<UserAnswer> entities)
        {
            throw new NotImplementedException();
        }

        public Task<UserAnswer> FirstOrDefaultAsync(Expression<Func<UserAnswer, bool>> criteria, params Expression<Func<UserAnswer, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserAnswer>> GetAllAsync(params Expression<Func<UserAnswer, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<UserAnswer> GetByIdAsync(int id, params Expression<Func<UserAnswer, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public void Update(UserAnswer entity)
        {
            throw new NotImplementedException();
        }
    }
}
