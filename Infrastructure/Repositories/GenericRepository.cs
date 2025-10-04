using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Infrastructure.ApplicationDBContext;
using System.Linq.Expressions;

namespace OnlineExam.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        // Add a single entity to the database
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Add multiple entities to the database
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        // Count entities with optional criteria
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? criteria = null)
        {
            return criteria == null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(criteria);
        }

        // Delete a single entity
        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        // Delete multiple entities
        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        // Get the first entity matching criteria (with optional includes)
        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> criteria,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply includes if provided
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(criteria);
        }

        // Get all entities (with optional includes)
        public async Task<IEnumerable<TEntity>> GetAllAsync(
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply includes if provided
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        // Get an entity by its primary key (with optional includes)
        public async Task<TEntity?> GetByIdAsync(
            int id,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply includes if provided
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        // Update an entity
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
