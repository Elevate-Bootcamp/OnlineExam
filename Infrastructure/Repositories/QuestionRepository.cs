using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Infrastructure.ApplicationDBContext;
using System.Linq.Expressions;

namespace OnlineExam.Infrastructure.Repositories
{
    public class QuestionRepository: IQuestionRepository
    {
        private ApplicationDbContext context;

        public QuestionRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task AddAsync(Question entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<Question> entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<Question, bool>>? criteria = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(Question entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<Question> entities)
        {
            throw new NotImplementedException();
        }

        public Task<Question> FirstOrDefaultAsync(Expression<Func<Question, bool>> criteria, params Expression<Func<Question, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Question>> GetAllAsync(params Expression<Func<Question, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<Question> GetByIdAsync(int id, params Expression<Func<Question, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public void Update(Question entity)
        {
            throw new NotImplementedException();
        }
    }
}
