using System.Security.Cryptography;
using System.Text;
using Certification.Application.Common.Configuration;
using Certification.Application.Common.Interfaces;
using Certification.Contracts.Auth;
using Certification.Domain.Identity;
using Certification.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Certification.Infrastructure.Identity;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ApplicationDbContext _dbContext;
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtTokenService,
        ApplicationDbContext dbContext,
        IOptions<JwtOptions> jwtOptions,
        ILogger<AuthenticationService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _dbContext = dbContext;
        _jwtOptions = jwtOptions.Value;
        _logger = logger;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.IsActive)
        {
            _logger.LogWarning("Login failed for email {Email} from IP {IpAddress}: unknown or inactive account.", request.Email, ipAddress);
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (signInResult.IsLockedOut)
        {
            _logger.LogWarning("Account locked out for email {Email} from IP {IpAddress}.", request.Email, ipAddress);
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        if (!signInResult.Succeeded)
        {
            _logger.LogWarning("Login failed for email {Email} from IP {IpAddress}: invalid password.", request.Email, ipAddress);
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        _logger.LogInformation("Login succeeded for user {UserId} from IP {IpAddress}.", user.Id, ipAddress);

        var roles = await _userManager.GetRolesAsync(user);

        return await IssueTokensAsync(user, roles, ipAddress, cancellationToken);
    }

    public async Task<LoginResponse> RefreshAsync(RefreshTokenRequest request, string? ipAddress, CancellationToken cancellationToken = default)
    {
        var incomingTokenHash = HashToken(request.RefreshToken);

        var existingToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(token => token.TokenHash == incomingTokenHash, cancellationToken);

        if (existingToken is null || existingToken.RevokedUtc is not null || existingToken.ExpiresUtc <= DateTime.UtcNow)
        {
            _logger.LogWarning("Refresh token rejected from IP {IpAddress}: invalid or expired.", ipAddress);
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        var user = await _userManager.FindByIdAsync(existingToken.UserId.ToString());
        if (user is null || !user.IsActive)
        {
            _logger.LogWarning("Refresh token rejected from IP {IpAddress}: unknown or inactive user.", ipAddress);
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        existingToken.RevokedUtc = DateTime.UtcNow;
        existingToken.RevokedByIp = ipAddress;
        _logger.LogInformation("Refresh token revoked for user {UserId} from IP {IpAddress}.", user.Id, ipAddress);

        var roles = await _userManager.GetRolesAsync(user);

        return await IssueTokensAsync(user, roles, ipAddress, cancellationToken);
    }

    public async Task LogoutAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var incomingTokenHash = HashToken(request.RefreshToken);

        var existingToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(token => token.TokenHash == incomingTokenHash, cancellationToken);

        if (existingToken is null || existingToken.RevokedUtc is not null)
        {
            return;
        }

        existingToken.RevokedUtc = DateTime.UtcNow;
        _logger.LogInformation("Refresh token revoked for user {UserId} due to logout.", existingToken.UserId);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<LoginResponse> IssueTokensAsync(ApplicationUser user, IList<string> roles, string? ipAddress, CancellationToken cancellationToken)
    {
        var accessToken = _jwtTokenService.GenerateAccessToken(user, roles);
        var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpiresAtUtc = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);

        _dbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = HashToken(refreshTokenValue),
            ExpiresUtc = refreshTokenExpiresAtUtc,
            CreatedByIp = ipAddress,
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            AccessTokenExpiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
            RefreshTokenExpiresAtUtc = refreshTokenExpiresAtUtc,
        };
    }

    private static string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
