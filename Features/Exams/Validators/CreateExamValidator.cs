using OnlineExam.Features.Exams.Commands;
using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Infrastructure.ApplicationDBContext;

namespace OnlineExam.Features.Exams.Validators
{
    public sealed class CreateExamValidator:AbstractValidator<CreateExamCommand>
    {
        private static readonly Regex TitleRegex = new(@"^[A-Za-z\s]{3,20}$",RegexOptions.Compiled);

        public CreateExamValidator(ApplicationDbContext db)
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Must(t => TitleRegex.IsMatch(t))
                .WithMessage("Title must be 3–20 alphabetic characters (letters and spaces).");

            RuleFor(x => x.IconUrl)
                .NotEmpty().WithMessage("Image is required.");

            RuleFor(x => x.CategoryId)
        .NotEmpty().WithMessage("Please select a category.")
       .MustAsync(async (id,ct) => await db.Categories.AsNoTracking().AnyAsync(c => c.Id == id,ct))
        .WithMessage("Category not found."); ;

            RuleFor(x => x.StartDate)
            .Must(sd => sd > DateTime.Now)
            .WithMessage("Start date must be after the current date/time.");
            
            RuleFor(x => x.EndDate)
                .Must((cmd,ed) => ed >= cmd.StartDate)
                .WithMessage("End date must be equal to or after the start date.");

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(20,180)
                .WithMessage("Duration must be between 20 and 180 minutes.");
        }
    }
}
