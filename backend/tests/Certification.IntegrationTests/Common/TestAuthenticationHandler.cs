using System.Security.Claims;
using System.Text.Encodings.Web;
using Certification.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Certification.IntegrationTests.Common;

public sealed class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "Test";
    public const string RoleHeaderName = "X-Test-Role";

    public TestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var role = Context.Request.Headers.TryGetValue(RoleHeaderName, out var headerValue)
            ? headerValue.ToString()
            : RoleNames.Admin;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new("email", "integration-tests@certification.local"),
            new("name", $"Integration Test {role}"),
            new(SecurityClaimTypes.Role, role),
        };

        var identity = new ClaimsIdentity(claims, SchemeName, nameType: ClaimTypes.NameIdentifier, roleType: SecurityClaimTypes.Role);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
