using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Domain;
using OnlineExam.Domain.Entities;
using OnlineExam.Infrastructure.EntityConfigurations;
using StackExchange.Redis;
using TechZone.Core.Entities;

namespace OnlineExam.Infrastructure.ApplicationDBContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }   
        public DbSet<UserAnswer> userAnswers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<UserExamAttempt> UserExamAttempts { get; set; }
        public DbSet<EmailQueue> emailQueues { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<UserSelectedChoice> UserSelectedChoices { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new ExamConfiguration());
            modelBuilder.ApplyConfiguration(new UserAnswerConfiguration());
            modelBuilder.ApplyConfiguration(new UserExamAttemptConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}