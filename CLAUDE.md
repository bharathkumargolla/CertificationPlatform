# CLAUDE.md

## Project
Certification Assessment Platform

## Role
Act as a senior enterprise software engineer. Build a production-quality application incrementally.

## Architecture
- Clean Architecture
- SOLID principles
- Dependency Injection
- REST APIs
- PostgreSQL
- React + TypeScript + Vite
- ASP.NET Core
- Entity Framework Core

## Open Source First
Prefer mature open-source libraries. Avoid commercial dependencies unless explicitly approved.

## Authentication
Support:
- Local Login
- Local Registration
- Google Login
- Microsoft Login
- Optional SAML 2.0 Service Provider
Do NOT build an Identity Provider or SSO server.

## Development Rules
- Analyze the workspace before making changes.
- Never overwrite unrelated code.
- Modify only required files.
- Preserve existing functionality.
- Keep changes small and reviewable.
- Follow existing coding conventions.

## Coding Standards
- Async/await
- Dependency Injection
- Input validation
- Meaningful naming
- No duplicated code

## Testing
For every feature include:
- Manual verification steps
- Unit tests where applicable
- Build verification

## Before Coding
Explain:
1. Plan
2. Files to create
3. Files to modify

## After Coding
Provide:
- Summary
- Verification steps
- Git commit message
- Suggested next task

## Never
- Rewrite the whole project
- Refactor unrelated code
- Change architecture without explanation
