using Certification.Domain.Identity;

namespace Certification.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(ApplicationUser user, IList<string> roles);

    string GenerateRefreshToken();
}
