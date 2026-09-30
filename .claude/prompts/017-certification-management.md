# Epic 5 - Task 5.2
## Certification Management

Continue the existing enterprise ASP.NET Core (.NET 10) solution.

Read before making changes:

- CLAUDE.md
- Module Management feature
- Existing CQRS foundation
- Validation foundation
- Mapster foundation
- Result pattern
- IApplicationDbContext

The Certification feature must follow the exact architectural pattern established by Module Management.

---

# Objective

Implement complete Certification Management.

This becomes the second reference business feature.

---

# Domain

Create

Certification.Domain/Entities/Certification.cs

Inherit

BaseEntity

Properties

Id

ModuleId

Code

Name

Version

Vendor

Description

DurationInMinutes

PassingScore

CertificateValidityMonths

DisplayOrder

IsActive

Navigation

Module

Business Rules

- ModuleId required
- Code unique
- Name required
- PassingScore between 0 and 100
- DurationInMinutes > 0
- CertificateValidityMonths >= 0
- Soft delete only

---

# EF Core Configuration

Create

CertificationConfiguration.cs

Requirements

Table

Certifications

Relationships

Module (1)

↓

Many Certifications

Cascade delete disabled

Indexes

Unique Code

ModuleId

Constraints

Code 50

Name 200

Vendor 100

Version 50

Description 1000

---

# DbContext

Add

DbSet<Certification>

Apply configuration automatically.

Generate EF migration

AddCertification

---

# DTOs

CertificationDto

CreateCertificationRequest

UpdateCertificationRequest

---

# CQRS

Commands

CreateCertificationCommand

UpdateCertificationCommand

DeleteCertificationCommand

Queries

GetCertificationQuery

GetCertificationsQuery

---

# Validators

CreateCertificationValidator

UpdateCertificationValidator

Rules

ModuleId required

Code required

Name required

PassingScore

0-100

Duration > 0

CertificateValidityMonths >=0

---

# Handlers

Use

IApplicationDbContext

IMapperService

Result<T>

Requirements

Create

Reject duplicate Code

Reject invalid ModuleId

Reject inactive Module

Update

Return NotFound if missing

Delete

Soft delete

Queries

Never return deleted records

Use ApplyPaging()

Order by

DisplayOrder

then

Name

---

# Controller

CertificationController

Route

/api/v1/certifications

Endpoints

GET

GET/{id}

POST

PUT/{id}

DELETE

Authorization

GET

AdminOrTrainer

POST

AdminOnly

PUT

AdminOnly

DELETE

SuperAdminOnly

Swagger

XML comments

ProducesResponseType

---

# Mapping

Create Mapster registrations.

No manual property copying.

---

# Tests

Unit

Create

Duplicate

Invalid Module

Delete

Integration

GET

POST

Validation

Authorization

---

# Constraints

Do NOT

Create Question entities

Create Exam entities

Create Users APIs

Stay focused on Certification Management.

---

# Verification

Run

dotnet build

dotnet test

dotnet ef migrations add AddCertification

Provide

Files created

Files modified

Migration output

Build output

Suggested Git commit