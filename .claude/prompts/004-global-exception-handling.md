# Epic 2 – Task 2.4
## Global Exception Handling

You are working on an enterprise ASP.NET Core (.NET 10) application following Clean Architecture.

Read before making changes:

- CLAUDE.md
- Existing solution
- Existing logging implementation

---

## Objective

Implement centralized exception handling and request correlation.

No business logic.

No controllers.

No authentication.

---

## Requirements

### 1. Correlation ID Middleware

Create:

Certification.Api/Common/Middleware/CorrelationIdMiddleware.cs

Responsibilities:

- Read request header:

X-Correlation-ID

- If missing, generate a GUID.

- Store it in HttpContext.Items.

- Add it to the response header.

Continue the pipeline.

---

### 2. Exception Handling Middleware

Create:

Certification.Api/Common/Middleware/ExceptionHandlingMiddleware.cs

Responsibilities:

- Catch every unhandled exception.
- Log using ILogger.
- Never expose stack traces.
- Produce RFC7807 ProblemDetails responses.
- Include CorrelationId in ProblemDetails.Extensions.

Map exceptions:

ValidationException → 400

UnauthorizedAccessException → 401

KeyNotFoundException → 404

NotImplementedException → 501

Everything else → 500

---

### 3. Extension Methods

Create:

Certification.Api/Common/Extensions/ApplicationBuilderExtensions.cs

Expose:

app.UseCorrelationId();

app.UseGlobalExceptionHandler();

Program.cs must only call the extension methods.

---

### 4. Logging

Every exception log must include:

- CorrelationId
- Request Method
- Request Path
- Exception Type
- Exception Message

---

### 5. ProblemDetails

Use Microsoft's built-in ProblemDetails.

Do NOT create a custom implementation.

---

### 6. Folder Structure

Create folders if missing:

Certification.Shared

Exceptions/

Create only:

BusinessException.cs

No implementation yet.

---

### Constraints

Do NOT:

- Create controllers
- Create authentication
- Create EF Core DbContext
- Create entities
- Create repositories
- Create business logic

---

### Verification

Build must succeed.

0 warnings.

0 errors.

---

### Deliverables

Provide:

- Files created
- Files modified
- Build output
- Suggested Git commit