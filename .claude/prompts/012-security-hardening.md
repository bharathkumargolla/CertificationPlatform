# Epic 3 - Task 3.2
## Security & Production Readiness

Continue the existing enterprise ASP.NET Core (.NET 10) solution.

Read first:

- CLAUDE.md
- Existing JWT implementation
- AuthenticationService
- JwtTokenService
- AuthController
- RefreshToken entity
- Program.cs

---

## Objective

Harden the authentication subsystem and prepare it for production.

Do not change any API contracts.

Existing endpoints must continue to work.

---

## 1. Refresh Token Security

Do NOT store raw refresh tokens.

Instead:

- Generate a cryptographically secure random token.
- Return the raw token to the client.
- Store only a SHA-256 hash.
- During refresh:
    - Hash the incoming token.
    - Compare hashes.
- During logout:
    - Hash incoming token before lookup.

Never log refresh tokens.

---

## 2. ASP.NET Core Rate Limiting

Use the built-in rate limiting middleware.

Create policies:

AuthenticationPolicy

Login:
5 requests/minute/IP

Refresh:
10 requests/minute/IP

Do not rate limit Swagger.

---

## 3. Security Headers

Create middleware.

Add:

- X-Content-Type-Options
- X-Frame-Options
- Referrer-Policy
- Permissions-Policy

Do NOT add Content-Security-Policy yet.

The API currently has Swagger and CSP requires additional planning.

---

## 4. Authorization Policies

Create:

Shared/Constants/AuthorizationPolicies.cs

Policies:

SuperAdminOnly

AdminOnly

TrainerOnly

CandidateOnly

Register policies.

Controllers must reference constants.

Never hardcode role names.

---

## 5. Login Auditing

Use Serilog.

Log:

- Login success
- Login failure
- Lockout
- Refresh token revoked

Never log:

- Passwords
- JWT
- Refresh tokens

---

## 6. Health Checks

Improve /health

Include:

- PostgreSQL
- Identity

Return detailed results only in Development.

---

## 7. Constraints

Do NOT

- Change endpoints
- Change DTOs
- Change contracts
- Change authentication flow

---

## Verification

Run:

dotnet build

dotnet test

Must produce:

0 warnings

0 errors

Provide:

Files created

Files modified

Build output

Suggested Git commit