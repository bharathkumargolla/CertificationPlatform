# Epic 3 - Task 3.0
## Identity Foundation

You are continuing an enterprise ASP.NET Core (.NET 10) application following Clean Architecture.

Read before making changes:

- CLAUDE.md
- Existing solution
- ApplicationDbContext
- Module entity
- BaseEntity
- DependencyInjection

---

## Objective

Integrate ASP.NET Core Identity with Entity Framework Core.

Do NOT create authentication APIs yet.

Do NOT create JWT endpoints yet.

This task only builds the Identity foundation.

---

## 1. Create Domain Identity

Create:

Domain/Identity/ApplicationUser.cs

Inherit from:

IdentityUser<Guid>

Add:

- FirstName
- LastName
- IsActive (default true)

---

Create:

Domain/Identity/ApplicationRole.cs

Inherit from:

IdentityRole<Guid>

No additional properties.

---

## 2. Update DbContext

ApplicationDbContext

Inherit from

IdentityDbContext<ApplicationUser, ApplicationRole, Guid>

Retain:

DbSet<Module>

Retain:

ApplyConfigurationsFromAssembly()

---

## 3. Configure Identity

Modify

Infrastructure/DependencyInjection.cs

Register Identity.

Use Guid keys.

Configure password policy:

Minimum length = 12

Require uppercase

Require lowercase

Require digit

Require non-alphanumeric

Require unique email

Lockout:

5 failed attempts

15 minute lockout

Allowed username characters:

Letters
Numbers
.
_
-

Do NOT disable security defaults.

---

## 4. Seed Roles

Create

Infrastructure/Identity/IdentitySeeder.cs

Seed roles:

- SuperAdmin
- Admin
- Trainer
- Candidate

Do NOT seed users.

---

## 5. Configuration

Do NOT create JWT yet.

Do NOT create login endpoints.

---

## 6. Migration

Create migration:

AddIdentity

---

## Constraints

Do NOT

- Create controllers
- Create endpoints
- Create repositories
- Create JWT
- Create refresh tokens

---

## Verification

Run

dotnet build

dotnet test

dotnet ef migrations add AddIdentity

Must succeed.

0 warnings.

0 errors.

---

Provide:

- Files created
- Files modified
- Migration output
- Build output
- Suggested Git commit