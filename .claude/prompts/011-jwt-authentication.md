# Epic 3 - Task 3.1
## JWT Authentication Foundation

Continue the existing enterprise ASP.NET Core solution.

Read first:

- CLAUDE.md
- Existing Identity implementation
- Existing JwtOptions
- Existing ApplicationUser
- Existing ApplicationRole
- Existing Infrastructure

---

## Objective

Implement JWT authentication.

This task creates the authentication infrastructure and APIs.

---

## Requirements

### Configure JWT

Use JwtOptions.

Configure:

- Issuer
- Audience
- Secret Key
- AccessTokenExpirationMinutes
- RefreshTokenExpirationDays

Read everything from configuration.

No hardcoded secrets.

---

### Configure Authentication

Register:

- JWT Bearer Authentication
- Authorization

Validate:

- Issuer
- Audience
- Lifetime
- Signing Key

ClockSkew = TimeSpan.Zero

---

### Create

Application/Common/Interfaces/IJwtTokenService.cs

Methods:

GenerateAccessToken(ApplicationUser user, IList<string> roles)

GenerateRefreshToken()

---

### Create

Infrastructure/Identity/JwtTokenService.cs

Requirements:

Use SecurityTokenDescriptor.

Include claims:

- sub
- email
- name
- given_name
- family_name
- role
- jti

Generate cryptographically secure refresh tokens.

---

### Refresh Token Entity

Create:

Domain/Identity/RefreshToken.cs

Properties:

- Id
- UserId
- Token
- ExpiresUtc
- CreatedUtc
- RevokedUtc
- CreatedByIp
- RevokedByIp

Configure:

RefreshTokenConfiguration

Add DbSet.

---

### Authentication Service

Create:

Application/Common/Interfaces/IAuthenticationService.cs

Infrastructure/Identity/AuthenticationService.cs

Methods:

- LoginAsync
- RefreshAsync
- LogoutAsync

---

### DTOs

Contracts/Auth/

LoginRequest
LoginResponse
RefreshTokenRequest

---

### API

Create:

Api/Controllers/AuthController.cs

Endpoints:

POST /api/v1/auth/login

POST /api/v1/auth/refresh

POST /api/v1/auth/logout

GET /api/v1/auth/me

Use ApiResponse<T>.

Return ProblemDetails on errors.

---

### Login

Authenticate using:

UserManager
SignInManager

No manual password hashing.

---

### Refresh

Validate:

- Token exists
- Not revoked
- Not expired

Rotate refresh tokens.

---

### Logout

Revoke refresh token.

---

### Authorization

/me endpoint requires authentication.

---

### Swagger

JWT authorization button must work.

---

### Migration

Create:

AddRefreshTokens

---

### Constraints

Do NOT

- Implement Google login
- Implement Microsoft login
- Implement MFA
- Implement forgot password

---

### Verification

Run:

dotnet build

dotnet test

dotnet ef migrations add AddRefreshTokens

Must succeed.

0 warnings.

0 errors.

Provide:

- Files created
- Files modified
- Migration output
- Build output