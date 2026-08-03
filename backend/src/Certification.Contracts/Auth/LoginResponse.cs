namespace Certification.Contracts.Auth;

public sealed class LoginResponse
{
    public string AccessToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public DateTime AccessTokenExpiresAtUtc { get; init; }

    public DateTime RefreshTokenExpiresAtUtc { get; init; }
}
