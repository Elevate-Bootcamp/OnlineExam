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
                    FullName = "online exam Admin",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}