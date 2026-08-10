using Certification.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Certification.Infrastructure.HealthChecks;

public sealed class IdentityHealthCheck : IHealthCheck
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public IdentityHealthCheck(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _roleManager.Roles.AnyAsync(cancellationToken);
            return HealthCheckResult.Healthy("Identity store is reachable.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("Identity store is not reachable.", exception);
        }
    }
}
