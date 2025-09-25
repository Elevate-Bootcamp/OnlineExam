using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Infrastructure.ApplicationDBContext;
using System.Linq.Expressions;

namespace OnlineExam.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task AddAsync(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<Category> entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<Category, bool>>? criteria = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(Category entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<Category> entities)
        {
            throw new NotImplementedException();
        }

        public Task<Category> FirstOrDefaultAsync(Expression<Func<Category, bool>> criteria, params Expression<Func<Category, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetAllAsync(params Expression<Func<Category, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetByIdAsync(int id, params Expression<Func<Category, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public void Update(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
