using Microsoft.EntityFrameworkCore;
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

        public async Task AddAsync(Question entity)
        {
            await context.Questions.AddAsync(entity);
        }

        public  async Task AddRangeAsync(IEnumerable<Question> entities)
        {
            await context.Questions.AddRangeAsync(entities);
        }

        public async Task<int> CountAsync(Expression<Func<Question, bool>>? criteria = null)
        {
            var count = await context.Questions.CountAsync();
            return count;
        }

        public void Delete(Question entity)
        {
                var question = context.Questions.Find(entity.Id);
                if (question != null)
                {
                    context.Questions.Remove(question);
                }
        }

        public void DeleteRange(IEnumerable<Question> entities)
        {
            foreach(var question in entities)
            {
                var item = context.Questions.Find(question.Id);
                if(item!=null)
                {
                    context.Questions.Remove(item);
                }
            }
        }

        public Task<Question> FirstOrDefaultAsync(Expression<Func<Question, bool>> criteria, params Expression<Func<Question, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Question>> GetAllAsync(params Expression<Func<Question, object>>[] includes)
        {
            return await context.Questions.Include(q=>q.Exam).ToListAsync();
          
        }

        public async Task<Question> GetByIdAsync(int id, params Expression<Func<Question, object>>[] includes)
        {
            //return await  context.Questions.Where(a => a.Id == id).FirstOrDefaultAsync();
            IQueryable<Question> query = context.Questions;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(q => q.Id == id);
        }

        public void Update(Question entity)
        {
            var question = context.Questions.Find(entity.Id);
            if (question!=null)
            {
                context.Questions.Update(question);

            }

        }
    }
}
