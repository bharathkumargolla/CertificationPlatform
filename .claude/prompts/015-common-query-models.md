# Epic 4 - Task 4.3
## Common Query Models

Continue the existing enterprise ASP.NET Core (.NET 10) solution.

Read:

- CLAUDE.md
- Existing Application layer
- CQRS foundation
- Result pattern
- Validation foundation

---

## Objective

Create reusable query models that every list endpoint in the application will use.

No business features.

No controllers.

No commands.

No queries.

Only reusable infrastructure.

---

# Create

Application/Common/Models/

PagedRequest.cs

Properties

- PageNumber = 1
- PageSize = 20

Rules

Minimum PageNumber = 1

Maximum PageSize = 100

Default PageSize = 20

---

SortRequest.cs

Properties

- SortBy
- SortDirection

Create enum

SortDirection

Ascending

Descending

---

SearchRequest.cs

Properties

- Search

Trim whitespace.

Null if empty.

---

FilterRequest.cs

Properties

Dictionary<string,string>

Allow arbitrary filters.

---

PagedResult<T>

Properties

Items

TotalCount

PageNumber

PageSize

TotalPages

HasNextPage

HasPreviousPage

---

PagedResponse<T>

Wrap

PagedResult<T>

inside ApiResponse.

---

# Extension Methods

Create

Application/Common/Extensions/

QueryableExtensions.cs

Implement

ApplyPaging()

Do NOT implement sorting.

Do NOT implement searching.

Leave extension points.

---

# Constraints

Do NOT

Create repositories

Create commands

Create queries

Create controllers

Create validators

Create DTO mappings

---

# Verification

Run

dotnet build

dotnet test

Must succeed

0 warnings

0 errors

Provide

Files created

Files modified

Build output

Suggested Git commit