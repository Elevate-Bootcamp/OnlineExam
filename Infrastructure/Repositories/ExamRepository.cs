using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Infrastructure.ApplicationDBContext;
using System.Linq.Expressions;

namespace OnlineExam.Infrastructure.Repositories
{
    public class ExamRepository: IExamRepository
    {
        private ApplicationDbContext context;

        public ExamRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task AddAsync(Exam entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<Exam> entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<Exam, bool>>? criteria = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(Exam entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<Exam> entities)
        {
            throw new NotImplementedException();
        }

        public Task<Exam> FirstOrDefaultAsync(Expression<Func<Exam, bool>> criteria, params Expression<Func<Exam, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Exam>> GetAllAsync(params Expression<Func<Exam, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<Exam> GetByIdAsync(int id, params Expression<Func<Exam, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public void Update(Exam entity)
        {
            throw new NotImplementedException();
        }
    }
}
