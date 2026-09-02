# Epic 2 - Task 2.9
## Module Entity

You are continuing an enterprise ASP.NET Core (.NET 10) application.

Read:

- CLAUDE.md
- Existing Domain
- Existing BaseEntity
- Existing EntityConfigurationBase

---

## Objective

Implement the first business entity: Module.

---

## Create

Domain/Entities/Module.cs

Properties

- Code
- Name
- Description
- DisplayOrder
- IsActive

Inheritance

BaseEntity

---

## Rules

Code

- Required
- Max Length 50
- Unique

Name

- Required
- Max Length 200

Description

- Optional
- Max Length 1000

DisplayOrder

- Default 0

IsActive

- Default true

---

## Create

Infrastructure/Persistence/Configurations/ModuleConfiguration.cs

Inherit from

EntityConfigurationBase<Module>

Configure

- Table name = Modules
- Primary key
- Unique index on Code
- Property lengths
- Defaults
- Required fields

---

## Update

ApplicationDbContext

Add

DbSet<Module>

Use

modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

Do NOT manually register configurations.

---

## Create Migration

InitialCreate

---

## Verification

Run

dotnet build

dotnet ef migrations add InitialCreate

Build must succeed.

No warnings.

No errors.

---

Do NOT

- Create repositories
- Create controllers
- Create APIs
- Create authentication

Provide

- Files created
- Migration output
- Build output
- Suggested commit