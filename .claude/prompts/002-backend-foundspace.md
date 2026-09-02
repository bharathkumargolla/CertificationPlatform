# Task 2.1 - Backend Foundation

You are continuing an existing enterprise project.

Before making changes read:

- CLAUDE.md

Objective

Prepare the backend solution for enterprise development.

Tasks

1. Verify the solution structure.

2. Create missing projects:

- Certification.Contracts
- Certification.Shared

3. Create test projects:

- Certification.UnitTests
- Certification.IntegrationTests

4. Add all projects to the solution.

5. Configure project references following Clean Architecture.

6. Install required NuGet packages:

API

- Serilog.AspNetCore
- Swashbuckle.AspNetCore
- Asp.Versioning.Http
- FluentValidation.AspNetCore
- Microsoft.Extensions.Diagnostics.HealthChecks

Infrastructure

- Microsoft.EntityFrameworkCore
- Npgsql.EntityFrameworkCore.PostgreSQL

Application

- FluentValidation

Tests

- xUnit
- FluentAssertions

7. Build the entire solution.

Do NOT implement business logic.

Do NOT create controllers.

Do NOT create authentication.

Do NOT create entities.

At the end provide:

- Files created
- Packages installed
- Project references
- Build status
- Suggested Git commit message