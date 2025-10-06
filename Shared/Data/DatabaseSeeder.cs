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
    }
}