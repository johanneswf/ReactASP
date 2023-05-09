using Bogus;
using ReactASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ReactASP.Data
{
    public class SeedData
    {
        private static ApplicationDbContext db = default!;
        private static UserManager<ApplicationUser> userManager;
        private static RoleManager<IdentityRole> roleManager;

        private static string password;

        private const string employeeRole = "Employee";
        private const string adminRole = "Admin";

        public static async Task InitAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            if (serviceProvider is null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            db = context ?? throw new ArgumentNullException(nameof(context));

            if (await db.Users.AnyAsync()) return;


            userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>() ??
            throw new ArgumentNullException(nameof(userManager));

            roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>() ??
            throw new ArgumentNullException(nameof(userManager));

            password = "BraLösenord123!";

            await CreateRolesAsync(new[] { employeeRole, adminRole });

            await GenerateUsersAsync(10);

            await db.SaveChangesAsync();
        }

        private static async Task CreateRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task GenerateUsersAsync(int nrOfUsers)
        {
            var faker = new Faker<ApplicationUser>("sv").Rules((f, e) =>
            {
                e.Email = f.Person.Email;
                e.UserName = f.Person.Email;
                e.EmailConfirmed = true;
            });

            var users = faker.Generate(nrOfUsers);

            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
                await AddToRoleeASync(user, employeeRole);
            }
        }

        private static async Task AddToRoleeASync(ApplicationUser employee, string role)
        {
            if (await userManager.IsInRoleAsync(employee, role)) return;
            var result = await userManager.AddToRoleAsync(employee, role);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }
}
