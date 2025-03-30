using AppointmentSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppointmentSystem.Data
{
    public class DbInitializer
    {
        public static async Task SeedDataAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<DbInitializer> logger)
        {
            string[] roleNames = { "SuperAdmin","Admin", "Doctor", "Patient" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (roleResult.Succeeded)
                    {
                        logger.LogInformation($"Role '{roleName}' created successfully.");
                    }
                    else
                    {
                        logger.LogError($"Error creating role '{roleName}': {string.Join(", ", roleResult.Errors)}");
                    }
                }
            }

            string adminEmail = "mhk@lng.com";
            string adminPassword = "Dhawan@123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "SuperAdmin");
                    logger.LogInformation($"Admin user '{adminEmail}' created and assigned to 'SuperAdmin' role.");
                }
                else
                {
                    logger.LogError($"Error creating admin user '{adminEmail}': {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                logger.LogInformation($"Admin user '{adminEmail}' already exists.");
            }
        }
    }
}
