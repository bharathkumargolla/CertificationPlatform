# Epic 4 - Task 4.2
## Validation & Mapping Foundation

Continue the existing enterprise ASP.NET Core (.NET 10) solution.

Read before making changes:

- CLAUDE.md
- Existing CQRS foundation
- Existing Application layer
- Existing Result pattern
- Existing DependencyInjection

---

## Objective

Complete the final Application-layer infrastructure before implementing business features.

Do NOT create business validators.

Do NOT create commands.

Do NOT create queries.

Do NOT create controllers.

---

# Part 1 - FluentValidation

Install

- FluentValidation
- FluentValidation.DependencyInjectionExtensions

Register validators using assembly scanning.

Update ValidationBehavior so it automatically executes validators.

If no validators exist:

Continue normally.

Do not throw.

Remove previous TODO comments.

---

# Part 2 - Mapster

Install

- Mapster
- Mapster.DependencyInjection

Create

Application/Common/Mappings/

MapsterConfiguration.cs

Requirements

Register Mapster globally.

Enable assembly scanning.

No mappings yet.

No DTOs.

---

# Part 3 - Mapping Abstraction

Create

Application/Common/Interfaces/IMapperService.cs

Methods

Map<TDestination>(object source)

Map<TSource, TDestination>(TSource source)

Create

Infrastructure/Mapping/MapsterMapperService.cs

Register with DI.

---

# Part 4 - Validation Base Classes

Create

Application/Common/Validation/

BaseValidator.cs

ValidationError.cs

These become the base for every validator.

---

# Part 5 - Dependency Injection

Update

Application/DependencyInjection.cs

Register

- FluentValidation
- Mapster
- IMapperService

Use assembly scanning.

---

# Constraints

Do NOT

Create DTOs

Create Validators

Create Commands

Create Queries

Create Controllers

Modify Authentication

Modify JWT

---

# Verification

Run

dotnet build

dotnet test

Must succeed.

0 warnings.

0 errors.

Provide

Files created

Files modified

Build output

Suggested Git commit