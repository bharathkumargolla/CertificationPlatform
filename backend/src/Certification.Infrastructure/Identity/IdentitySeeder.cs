using Certification.Domain.Identity;
using Certification.Shared.Constants;
using Microsoft.AspNetCore.Identity;

namespace Certification.Infrastructure.Identity;

public static class IdentitySeeder
{
    private static readonly string[] RoleNames =
    [
        Certification.Shared.Constants.RoleNames.SuperAdmin,
        Certification.Shared.Constants.RoleNames.Admin,
        Certification.Shared.Constants.RoleNames.Trainer,
        Certification.Shared.Constants.RoleNames.Candidate,
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
