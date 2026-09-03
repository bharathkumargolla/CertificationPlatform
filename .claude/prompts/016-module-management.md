# Epic 5 - Task 5.1
## Module Management

Continue the existing enterprise ASP.NET Core (.NET 10) solution.

Read before making changes:

- CLAUDE.md
- Existing CQRS foundation
- Result pattern
- Validation foundation
- Mapster foundation
- Module entity
- ApplicationDbContext
- Authorization policies

---

## Objective

Implement the first complete business feature using the established architecture.

This feature becomes the reference implementation for all future features.

---

# Folder Structure

Application/

Modules/

    Commands/
        CreateModule/
        UpdateModule/
        DeleteModule/

    Queries/
        GetModule/
        GetModules/

    DTOs/

    Validators/

---

# DTOs

ModuleDto

Properties

- Id
- Code
- Name
- Description
- DisplayOrder
- IsActive

---

CreateModuleRequest

- Code
- Name
- Description
- DisplayOrder

---

UpdateModuleRequest

- Name
- Description
- DisplayOrder
- IsActive

---

# Commands

CreateModuleCommand

Returns

Result<Guid>

---

UpdateModuleCommand

Returns

Result

---

DeleteModuleCommand

Returns

Result

Soft delete only.

Do NOT physically delete.

---

# Queries

GetModuleQuery

Returns

Result<ModuleDto>

---

GetModulesQuery

Input

PagedRequest

SearchRequest

SortRequest

Returns

ApiResponse<PagedResult<ModuleDto>>

Use ApplyPaging().

Leave ApplySorting() as future work.

---

# Validators

CreateModuleValidator

Rules

Code

Required

Maximum 50

Name

Required

Maximum 200

Description

Maximum 1000

DisplayOrder

>=0

---

UpdateModuleValidator

Validate update fields.

---

# Handlers

Use

ApplicationDbContext

IMapperService

Result<T>

Requirements

Create

Reject duplicate Code.

Return Result.Failure.

---

Update

Return NotFound failure if missing.

---

Delete

Soft delete.

Set

IsDeleted

DeletedAtUtc

Do NOT remove row.

---

Queries

Return only

IsDeleted == false

---

# Controller

ModulesController

Route

/api/v1/modules

Endpoints

GET

GET/{id}

POST

PUT/{id}

DELETE/{id}

---

Authorization

GET

AdminOnly
TrainerOnly

POST

AdminOnly

PUT

AdminOnly

DELETE

SuperAdminOnly

---

Responses

Use ApiResponse<T>

Errors

Use ProblemDetails

---

Swagger

Add XML comments.

Response types.

Authorization.

---

Tests

Create

Unit tests

- Create handler
- Duplicate module
- Delete handler

Integration tests

- GET modules
- POST module

---

Verification

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