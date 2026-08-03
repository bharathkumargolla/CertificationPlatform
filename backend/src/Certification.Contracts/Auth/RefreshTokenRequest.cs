using System.ComponentModel.DataAnnotations;

namespace Certification.Contracts.Auth;

public sealed class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; init; } = string.Empty;
}
