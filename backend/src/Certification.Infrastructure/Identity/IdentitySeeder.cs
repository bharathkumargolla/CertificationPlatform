using Certification.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Certification.Infrastructure.Identity;

public static class IdentitySeeder
{
    private static readonly string[] RoleNames =
    [
        "SuperAdmin",
        "Admin",
        "Trainer",
        "Candidate",
    ];

    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        foreach (var roleName in RoleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
            }
        }
    }
}
