# Epic 2 - Task 2.7
## Persistence Foundation

You are continuing an enterprise ASP.NET Core (.NET 10) solution using Clean Architecture.

Read before making changes:

- CLAUDE.md
- Existing solution
- Existing DependencyInjection.cs
- Existing DatabaseOptions

---

## Objective

Build the persistence infrastructure.

This task does NOT implement any business entities.

---

## Tasks

### 1. Create ApplicationDbContext

Create:

Infrastructure/Persistence/Context/ApplicationDbContext.cs

Requirements:

- Inherit from DbContext
- Constructor accepts DbContextOptions<ApplicationDbContext>
- No DbSet properties
- Override OnModelCreating()
- Call base.OnModelCreating()

---

### 2. Register EF Core

Modify:

Infrastructure/DependencyInjection.cs

Register:

ApplicationDbContext

using:

- UseNpgsql()

Read:

ConnectionStrings:DefaultConnection

Read DatabaseOptions:

- CommandTimeout
- MaxRetryCount

Enable:

EnableRetryOnFailure()

Do NOT enable SensitiveDataLogging by default.

Use DatabaseOptions instead.

---

### 3. Design-time Factory

Create:

Infrastructure/Persistence/Context/ApplicationDbContextFactory.cs

Implement:

IDesignTimeDbContextFactory<ApplicationDbContext>

Purpose:

Support:

dotnet ef

without running the API.

Read configuration from appsettings.json.

---

### 4. Folder Structure

Ensure these folders exist:

Infrastructure/

Persistence/

Context/

Configurations/

Interceptors/

Seed/

Extensions/

---

### 5. Configuration

Verify appsettings.json contains:

ConnectionStrings

DefaultConnection

Leave value empty.

Do NOT add secrets.

---

### 6. Constraints

Do NOT

- Create DbSets
- Create Entities
- Create Migrations
- Create Repositories
- Create Controllers
- Create Authentication
- Create Business Logic

---

### 7. Verification

Run:

dotnet build

dotnet ef dbcontext info

Build must have:

0 warnings

0 errors

---

Provide:

- Files created
- Files modified
- Build output
- dbcontext info output
- Suggested Git commit