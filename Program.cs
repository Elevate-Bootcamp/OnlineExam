using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineExam.Domain;
using OnlineExam.Domain.Interfaces;
using OnlineExam.Features.Accounts.Commands;
using OnlineExam.Features.Accounts.Endpoints;
using OnlineExam.Infrastructure.ApplicationDBContext;
using OnlineExam.Infrastructure.Repositories;
using OnlineExam.Infrastructure.UnitOfWork;
using Serilog;
using Serilog.Events;

namespace OnlineExam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Serilog setup (unchanged)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "OnlineExam.Api")
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} | {CorrelationId} | {Message:lj}{NewLine}{Exception}", theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Literate)
                .WriteTo.File("logs/OnlineExam-.log", rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {SourceContext} | {CorrelationId} | {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Starting OnlineExam API application");

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            // Services (unchanged)
            builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IExamRepository, ExamRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => { /* unchanged */ })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),sqlOptions => { /* unchanged */ });
                options.EnableSensitiveDataLogging(false);
                options.EnableServiceProviderCaching();
                options.EnableDetailedErrors(builder.Environment.IsDevelopment());
                options.LogTo(message => Log.Debug("[EF] {Message}",message),LogLevel.Warning);
            });
            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            //builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            //    opt.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMediatR(typeof(Program).Assembly);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Trust the dev certificate on startup
            if (app.Environment.IsDevelopment())
            {
                var cert = new HttpClient().GetAsync("https://localhost:5001").ContinueWith(task => { });
                app.UseDeveloperExceptionPage(); // Optional: for detailed errors
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapGet("/", () => "OnlineExam API is running...");
            app.MapRegisterEndpoint(); // Map the register endpoint

            app.Use(async (ctx,next) =>
            {
                try { await next(); }
                catch(FluentValidation.ValidationException ex)
                {
                    var dict = ex.Errors
                        .GroupBy(e => e.PropertyName ?? "")
                        .ToDictionary(g => g.Key,g => g.Select(e => e.ErrorMessage).ToArray());
                    await Results.ValidationProblem(dict).ExecuteAsync(ctx);
                }
            });


            app.Run();
        }
    }
}