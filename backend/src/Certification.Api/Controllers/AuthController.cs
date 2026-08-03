using System.Security.Claims;
using Asp.Versioning;
using Certification.Application.Common.Interfaces;
using Certification.Contracts.Auth;
using Certification.Contracts.Common;
using Certification.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Certification.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var response = await _authenticationService.LoginAsync(request, ipAddress, cancellationToken);
        return Ok(ApiResponse.Success(response));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var response = await _authenticationService.RefreshAsync(request, ipAddress, cancellationToken);
        return Ok(ApiResponse.Success(response));
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        await _authenticationService.LogoutAsync(request, cancellationToken);
        return Ok(ApiResponse.Success<object?>("Logged out successfully.", null));
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<CurrentUserResponse>), StatusCodes.Status200OK)]
    public IActionResult Me()
    {
        var response = new CurrentUserResponse
        {
            Id = Guid.TryParse(User.FindFirstValue("sub"), out var id) ? id : Guid.Empty,
            Email = User.FindFirstValue("email") ?? string.Empty,
            UserName = User.FindFirstValue("name") ?? string.Empty,
            FirstName = User.FindFirstValue("given_name") ?? string.Empty,
            LastName = User.FindFirstValue("family_name") ?? string.Empty,
            Roles = User.FindAll(SecurityClaimTypes.Role).Select(claim => claim.Value).ToArray(),
        };

        return Ok(ApiResponse.Success(response));
    }
}
