using Certification.Contracts.Auth;

namespace Certification.Application.Common.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress, CancellationToken cancellationToken = default);

    Task<LoginResponse> RefreshAsync(RefreshTokenRequest request, string? ipAddress, CancellationToken cancellationToken = default);

    Task LogoutAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}
