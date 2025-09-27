//using OnlineExam.Features.Exams.Commands;
//using System.Text.RegularExpressions;
//using FluentValidation;
//using Microsoft.EntityFrameworkCore;

//namespace OnlineExam.Features.Exams.Validators
//{
//    public sealed class CreateExamValidator:AbstractValidator<CreateExamCommand>
//    {
//        private static readonly Regex TitleRegex = new(@"^[A-Za-z\s]{3,20}$",RegexOptions.Compiled);

//        public CreateExamValidator(AppDbContext db)
//        {
//            RuleFor(x => x.Title)
//                .NotEmpty()
//                .Must(t => TitleRegex.IsMatch(t))
//                .WithMessage("Title must be 3–20 alphabetic characters (letters and spaces).");

//            RuleFor(x => x.IconUrl)
//                .NotEmpty().WithMessage("Icon is required.");

//            RuleFor(x => x.CategoryId)
//                .GreaterThan(0)
//                .MustAsync(async (id,ct) => await db.Categories.AsNoTracking().AnyAsync(c => c.Id == id,ct))
//                .WithMessage("Category not found.");

//            RuleFor(x => x.StartDateUtc)
//                .Must(sd => sd > DateTime.UtcNow).WithMessage("Start date must be after current date (UTC).");

//            RuleFor(x => x.EndDateUtc)
//                .Must((cmd,ed) => ed >= cmd.StartDateUtc).WithMessage("End date must be >= start date.");

//            RuleFor(x => x.DurationMinutes)
//                .InclusiveBetween(20,180).WithMessage("Duration must be between 20 and 180 minutes.");

//            // (اختياري) منع تداخل الامتحانات داخل نفس التصنيف
//            RuleFor(x => x)
//                .MustAsync(async (cmd,ct) =>
//                    !await db.Exams.AsNoTracking().AnyAsync(e =>
//                        e.CategoryId == cmd.CategoryId &&
//                        e.StartDate < cmd.EndDateUtc &&
//                        cmd.StartDateUtc < e.EndDate,ct))
//                .WithMessage("Overlapping exam exists in the same category.");
//        }
//    }
//}
