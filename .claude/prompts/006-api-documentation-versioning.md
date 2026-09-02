# Epic 2 - Task 2.6
## API Documentation & Versioning

You are working on an enterprise ASP.NET Core (.NET 10) application following Clean Architecture.

Read before making changes:

- CLAUDE.md
- Existing solution
- Existing logging
- Existing exception handling

---

## Objective

Configure OpenAPI and API Versioning.

Do NOT implement authentication.

Do NOT implement controllers.

---

## Requirements

### 1. Swagger

Create:

Certification.Api/Extensions/SwaggerExtensions.cs

Expose:

builder.Services.AddSwaggerDocumentation();

app.UseSwaggerDocumentation();

Program.cs must remain minimal.

---

### 2. Swagger Information

Configure:

Title

Certification Assessment Platform API

Version

v1

Description

Enterprise Certification Assessment Platform REST API

Contact

ISSQUARED Engineering

License

Internal Use

---

### 3. XML Documentation

Enable XML documentation generation.

Configure Swagger to read XML comments if the file exists.

---

### 4. JWT Security Definition

Prepare Swagger for JWT Bearer tokens.

Do NOT enable authentication.

Only configure the OpenAPI security scheme.

---

### 5. API Versioning

Configure:

- Default version = 1.0
- Assume default version
- Report supported versions
- URL segment versioning

Example future route:

/api/v1/auth/login

Do not create controllers.

---

### 6. OpenAPI UI

Enable:

- Display request duration
- Deep linking
- Persist authorization
- Collapse models by default

---

### 7. Configuration

Move all Swagger and Versioning configuration into extension methods.

Program.cs should only call:

builder.Services.AddSwaggerDocumentation();

app.UseSwaggerDocumentation();

---

### Constraints

Do NOT

- Add Controllers
- Add Authentication
- Add EF Core
- Add Entities
- Add Business Logic

---

### Verification

Build succeeds.

Zero warnings.

Zero errors.

---

### Deliverables

Provide:

- Files created
- Files modified
- Build output
- Suggested Git commit