using AEL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEL.Infrastructure.Persistence.Seeders
{
    public static class IdentitySeeder
    {
        public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var roles = new List<string> { "Admin" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new Role { Name = roleName };
                    await roleManager.CreateAsync(role);
                }
            }

            // Create de user, if not exists
            var adminEmail = "alecalo60@hotmail.com";
            var existingUser = await userManager.FindByEmailAsync(adminEmail);

            if (existingUser == null)
            {
                var user = new User
                {
                    UserName = "AlexCaro",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "SoyUnPass123.*");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

    }
}
