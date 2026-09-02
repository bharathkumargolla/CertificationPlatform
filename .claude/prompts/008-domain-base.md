# Epic 2 - Task 2.8
## Domain Base

Objective

Create the common domain foundation for all business entities.

Requirements

Create:

Domain/Common/

- IEntity.cs
- IAuditable.cs
- ISoftDelete.cs
- BaseEntity.cs

Infrastructure/Persistence/Configurations/

- EntityConfigurationBase.cs

BaseEntity must contain:

- Guid Id
- DateTime CreatedAtUtc
- string? CreatedBy
- DateTime? ModifiedAtUtc
- string? ModifiedBy
- bool IsDeleted
- DateTime? DeletedAtUtc
- string? DeletedBy
- byte[] RowVersion

Configure RowVersion as an EF Core concurrency token.

Do NOT create any business entities.

Do NOT create migrations.

Do NOT create DbSets.

Build the solution.

Deliver:

- Files created
- Files modified
- Build output
- Suggested Git commit