# Epic 2 - Task 2.2
## Configuration & Dependency Injection

You are working on an existing enterprise ASP.NET Core solution.

Read before making changes:

- CLAUDE.md
- docs/product/00-ProductVision.md
- docs/product/01-SRS.md (if available)
- docs/architecture/02-Architecture.md (if available)

---

## Objective

Prepare the application infrastructure.

This task does NOT implement any business functionality.

---

## Requirements

### Configuration

Create strongly typed configuration classes for:

- ApplicationOptions
- JwtOptions (placeholder only)
- DatabaseOptions

Store them under:

Application/Common/Configuration

Register them using the Options pattern.

---

### Dependency Injection

Create extension methods:

Application

```text
Application/DependencyInjection.cs
```

Infrastructure

```text
Infrastructure/DependencyInjection.cs
```

API

```text
Api/Extensions/ServiceCollectionExtensions.cs
```

Move all service registration into extension methods.

Program.cs should remain minimal.

---

### CORS

Configure a named CORS policy.

Do NOT allow all origins permanently.

Use configuration values.

---

### Configuration Files

Review:

appsettings.json

appsettings.Development.json

Ensure sections exist for:

Application

Database

Logging

AllowedHosts

Cors

Jwt

Do NOT add secrets.

---

### Folder Structure

Ensure these folders exist.

Application

```text
Common/

Configuration/

Interfaces/

Exceptions/

Models/
```

Infrastructure

```text
Persistence/

Context/

Configurations/

Seed/
```

Shared

```text
Constants/

Enums/

Extensions/

Utilities/
```

Create folders only.

Do not create entities.

---

### Constraints

Do NOT

- Add EF Core DbContext
- Add Controllers
- Add Authentication
- Add Domain Entities
- Add Business Logic

---

### Verification

Solution builds.

No warnings.

No errors.

---

### Deliverables

Provide:

- Files created
- Files modified
- Why each change was made
- Build output
- Suggested Git commit message