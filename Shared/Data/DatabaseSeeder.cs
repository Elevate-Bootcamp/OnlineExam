using Microsoft.AspNetCore.Identity;
using OnlineExam.Domain;
using OnlineExam.Infrastructure.ApplicationDBContext;

namespace OnlineExam.Shared.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider sp)
        {
            // 0) Roles & Admin
            await SeedRolesAndAdminAsync(sp);

            // DbContext
            var ctx = sp.GetRequiredService<ApplicationDbContext>();

            // 1) Seed Categories
            await SeedCategoriesAsync(ctx);

            // 2) Seed Exams
            await SeedExamsAsync(ctx);
        }

        // ====== 0) Identity (Roles + Admin) ======
        private static async Task SeedRolesAndAdminAsync(IServiceProvider sp)
        {
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            var adminEmail = "admin@onlineexam.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "online exam Admin",
                    LastName = "",
                    ImageUrl = "tst",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        // ====== 1) Seed Categories ======
        private static async Task SeedCategoriesAsync(ApplicationDbContext context)
        {
            // Check if categories already exist
            if (context.Categories.Any())
            {
                return; // Database has been seeded
            }

            var categories = new List<Category>
            {
                new Category
                {
                    Title = "Mathematics",
                    IconUrl = "/uploads/math-icon.png",
                    Description = "Algebra, Geometry, Calculus, and more"
                },
                new Category
                {
                    Title = "Science",
                    IconUrl = "/uploads/science-icon.png",
                    Description = "Physics, Chemistry, Biology, and Environmental Science"
                },
                new Category
                {
                    Title = "History",
                    IconUrl = "/uploads/history-icon.png",
                    Description = "World History, Ancient Civilizations, and Modern Events"
                },
                new Category
                {
                    Title = "Geography",
                    IconUrl = "/uploads/geography-icon.png",
                    Description = "Countries, Capitals, Landforms, and Maps"
                },
                new Category
                {
                    Title = "English Language",
                    IconUrl = "/uploads/english-icon.png",
                    Description = "Grammar, Literature, and Writing Skills"
                },
                new Category
                {
                    Title = "Computer Science",
                    IconUrl = "/uploads/computer-icon.png",
                    Description = "Programming, Algorithms, and Data Structures"
                },
                new Category
                {
                    Title = "Business",
                    IconUrl = "/uploads/business-icon.png",
                    Description = "Economics, Management, and Entrepreneurship"
                },
                new Category
                {
                    Title = "Arts & Music",
                    IconUrl = "/uploads/arts-icon.png",
                    Description = "Visual Arts, Music Theory, and Art History"
                }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        // ====== 2) Seed Exams ======
        private static async Task SeedExamsAsync(ApplicationDbContext context)
        {
            // Check if exams already exist
            if (context.Exams.Any())
            {
                return; // Database has been seeded
            }

            var categories = context.Categories.ToList();
            var exams = new List<Exam>();

            foreach (var category in categories)
            {
                // Add 2-3 exams for each category
                var categoryExams = GetExamsForCategory(category.Id, category.Title);
                exams.AddRange(categoryExams);
            }

            await context.Exams.AddRangeAsync(exams);
            await context.SaveChangesAsync();
        }

        private static List<Exam> GetExamsForCategory(int categoryId, string categoryTitle)
        {
            var now = DateTime.UtcNow;

            return categoryTitle switch
            {
                "Mathematics" => new List<Exam>
                {
                    new Exam
                    {
                        Title = "Algebra Fundamentals",
                        IconUrl = "/uploads/algebra-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-4),
                        EndDate = now.AddDays(31),
                        Duration = 60,
                        IsActive = true,
                        Description = "Test your knowledge of basic algebraic concepts and equations"
                    },
                    new Exam
                    {
                        Title = "Geometry Mastery",
                        IconUrl = "/uploads/geometry-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-2),
                        EndDate = now.AddDays(32),
                        Duration = 75,
                        IsActive = true,
                        Description = "Advanced geometry concepts including theorems and proofs"
                    },
                    new Exam
                    {
                        Title = "Calculus Basics",
                        IconUrl = "/uploads/calculus-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-3),
                        EndDate = now.AddDays(30),
                        Duration = 90,
                        IsActive = true,
                        Description = "Introduction to differential and integral calculus"
                    }
                },
                "Science" => new List<Exam>
                {
                    new Exam
                    {
                        Title = "Physics Principles",
                        IconUrl = "/uploads/physics-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-1),
                        EndDate = now.AddDays(28),
                        Duration = 60,
                        IsActive = true,
                        Description = "Fundamental physics concepts and laws"
                    },
                    new Exam
                    {
                        Title = "Chemistry Essentials",
                        IconUrl = "/uploads/chemistry-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-2),
                        EndDate = now.AddDays(35),
                        Duration = 70,
                        IsActive = true,
                        Description = "Periodic table, chemical reactions, and laboratory safety"
                    }
                },
                "History" => new List<Exam>
                {
                    new Exam
                    {
                        Title = "World Wars History",
                        IconUrl = "/uploads/history-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-1),
                        EndDate = now.AddDays(40),
                        Duration = 50,
                        IsActive = true,
                        Description = "Comprehensive overview of World War I and II"
                    },
                    new Exam
                    {
                        Title = "Ancient Civilizations",
                        IconUrl = "/uploads/ancient-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-3),
                        EndDate = now.AddDays(45),
                        Duration = 65,
                        IsActive = true,
                        Description = "Egypt, Greece, Rome, and Mesopotamia"
                    }
                },
                "Geography" => new List<Exam>
                {
                    new Exam
                    {
                        Title = "World Capitals Challenge",
                        IconUrl = "/uploads/geography-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-1),
                        EndDate = now.AddDays(25),
                        Duration = 45,
                        IsActive = true,
                        Description = "Test your knowledge of world capitals and countries"
                    }
                },
                "English Language" => new List<Exam>
                {
                    new Exam
                    {
                        Title = "Grammar Proficiency",
                        IconUrl = "/uploads/grammar-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-2),
                        EndDate = now.AddDays(30),
                        Duration = 55,
                        IsActive = true,
                        Description = "Advanced English grammar and syntax"
                    },
                    new Exam
                    {
                        Title = "Literature Analysis",
                        IconUrl = "/uploads/literature-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-4),
                        EndDate = now.AddDays(38),
                        Duration = 80,
                        IsActive = true,
                        Description = "Critical analysis of classic and modern literature"
                    }
                },
                "Computer Science" => new List<Exam>
                {
                    new Exam
                    {
                        Title = "Programming Fundamentals",
                        IconUrl = "/uploads/programming-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-1),
                        EndDate = now.AddDays(50),
                        Duration = 120,
                        IsActive = true,
                        Description = "Basic programming concepts and problem solving"
                    },
                    new Exam
                    {
                        Title = "Data Structures",
                        IconUrl = "/uploads/datastructures-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-5),
                        EndDate = now.AddDays(60),
                        Duration = 100,
                        IsActive = true,
                        Description = "Arrays, linked lists, trees, and algorithms"
                    },
                    new Exam
                    {
                        Title = "Web Development Basics",
                        IconUrl = "/uploads/webdev-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-7),
                        EndDate = now.AddDays(42),
                        Duration = 90,
                        IsActive = true,
                        Description = "HTML, CSS, JavaScript fundamentals"
                    }
                },
                "Business" => new List<Exam>
                {
                    new Exam
                    {
                        Title = "Economics Principles",
                        IconUrl = "/uploads/economics-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(2),
                        EndDate = now.AddDays(33),
                        Duration = 70,
                        IsActive = true,
                        Description = "Micro and macro economics fundamentals"
                    },
                    new Exam
                    {
                        Title = "Business Management",
                        IconUrl = "/uploads/management-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-4),
                        EndDate = now.AddDays(36),
                        Duration = 65,
                        IsActive = true,
                        Description = "Leadership, organization, and business strategy"
                    }
                },
                "Arts & Music" => new List<Exam>
                {
                    new Exam
                    {
                        Title = "Art History Survey",
                        IconUrl = "/uploads/arthistory-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-3),
                        EndDate = now.AddDays(40),
                        Duration = 75,
                        IsActive = true,
                        Description = "Renaissance to modern art movements"
                    },
                    new Exam
                    {
                        Title = "Music Theory Basics",
                        IconUrl = "/uploads/music-exam.png",
                        CategoryId = categoryId,
                        StartDate = now.AddDays(-6),
                        EndDate = now.AddDays(44),
                        Duration = 60,
                        IsActive = true,
                        Description = "Notes, scales, chords, and musical notation"
                    }
                },
                _ => new List<Exam>() // Default empty list for any unexpected categories
            };
        }
    }
}