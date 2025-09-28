using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Infrastructure.ApplicationDBContext;
using System.Linq.Expressions;

namespace OnlineExam.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category entity)
        {
          await  _context.Categories.AddAsync(entity);

            
        }

        public async Task AddRangeAsync(IEnumerable<Category> entities)
        {
            await _context.Categories.AddRangeAsync(entities);    
        }

        public async Task<int> CountAsync(Expression<Func<Category, bool>>? criteria = null)
        {
            if (criteria != null)
                return await _context.Categories.CountAsync(criteria);
            return await _context.Categories.CountAsync();
        }

        public void Delete(Category entity)
        {
            _context.Categories.Remove(entity);
        }

        public void DeleteRange(IEnumerable<Category> entities)
        {
            _context.Categories.RemoveRange(entities);
        }

        public async Task<Category> FirstOrDefaultAsync(Expression<Func<Category, bool>> criteria, params Expression<Func<Category, object>>[] includes)
        {
            IQueryable<Category> query = _context.Categories;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(criteria);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(params Expression<Func<Category, object>>[] includes)
        {
            IQueryable<Category> query = _context.Categories;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id, params Expression<Func<Category, object>>[] includes)
        {
            IQueryable<Category> query = _context.Categories;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Update(Category entity)
        {
            _context.Categories.Update(entity);
        }
    }
}
