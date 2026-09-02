# Epic 2 – Task 2.5
## Standard API Response

You are continuing an enterprise ASP.NET Core (.NET 10) application.

Read before making changes:

- CLAUDE.md
- Existing solution
- Existing exception middleware

---

## Objective

Create a reusable API response model for successful responses.

Error responses must continue to use RFC7807 ProblemDetails.

---

## Requirements

### Create

Certification.Contracts/Common/ApiResponse.cs

Create a generic model:

ApiResponse<T>

Properties:

- bool Success
- string Message
- T? Data
- DateTime TimestampUtc
- string CorrelationId

---

### Factory Methods

Provide static helper methods:

Success(data)

Success(message, data)

Failure(message)

Failure(message, correlationId)

Do NOT return ProblemDetails here.

---

### Extension Methods

Create:

Certification.Api/Common/Extensions/HttpContextExtensions.cs

Provide a helper:

GetCorrelationId()

Reads the Correlation ID from HttpContext.Items.

---

### Constraints

Do NOT:

- Modify exception middleware
- Create controllers
- Create endpoints
- Create authentication
- Create entities
- Create repositories

---

### Verification

Solution builds.

0 warnings.

0 errors.

---

### Deliverables

Provide:

- Files created
- Files modified
- Build output
- Suggested Git commit