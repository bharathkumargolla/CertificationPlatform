namespace Certification.Application.Common.Configuration;

/// <summary>
/// Placeholder shape for future JWT authentication. No signing secrets are stored here or in
/// configuration files; those will be sourced from a secret manager when authentication is implemented.
/// </summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public int AccessTokenExpirationMinutes { get; init; } = 60;

    public int RefreshTokenExpirationDays { get; init; } = 7;
}
