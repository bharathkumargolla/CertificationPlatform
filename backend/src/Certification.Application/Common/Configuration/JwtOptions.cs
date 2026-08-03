using System.ComponentModel.DataAnnotations;

namespace Certification.Application.Common.Configuration;

/// <summary>
/// SecretKey must never be hardcoded or committed; it is sourced from environment variables or a
/// secret manager, the same way ConnectionStrings:DefaultConnection is supplied.
/// </summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Required]
    [MinLength(32)]
    public string SecretKey { get; init; } = string.Empty;

    public int AccessTokenExpirationMinutes { get; init; } = 60;

    public int RefreshTokenExpirationDays { get; init; } = 7;
}
