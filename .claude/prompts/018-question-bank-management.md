# Epic 5 - Task 5.3
## Question Bank Management

Continue the existing enterprise ASP.NET Core (.NET 10) solution.

Read before making changes:

- CLAUDE.md
- Module Management feature
- Certification Management feature
- Existing CQRS foundation
- Validation foundation
- Mapster foundation
- Result pattern
- IApplicationDbContext

This feature must follow the exact architecture established by Module Management and Certification Management.

---

# Objective

Implement complete Question Bank Management.

This includes both Question and QuestionOption as one feature.

---

# Domain

Create

Certification.Domain/Entities/Question.cs

Inherit

BaseEntity

Properties

Id

CertificationDefinitionId

QuestionText

Explanation

QuestionType

DifficultyLevel

Points

DisplayOrder

IsActive

Navigation

CertificationDefinition

ICollection<QuestionOption>

---

Create

Certification.Domain/Entities/QuestionOption.cs

Properties

Id

QuestionId

OptionText

IsCorrect

DisplayOrder

Navigation

Question

---

# Enums

Create

QuestionType.cs

SingleChoice = 1

MultipleChoice = 2

TrueFalse = 3

Create

DifficultyLevel.cs

Easy = 1

Medium = 2

Hard = 3

---

# Business Rules

Question

- CertificationDefinitionId required
- Certification must exist
- Certification must be active
- QuestionText required
- Points > 0

Question Options

- Minimum 2 options
- Maximum 6 options
- At least one correct option
- SingleChoice = exactly one correct answer
- MultipleChoice = one or more correct answers
- TrueFalse = exactly two options
- TrueFalse options must be "True" and "False"

Delete

Soft delete Question only.

QuestionOptions cascade automatically.

---

# EF Core Configuration

Question

Table

Questions

Indexes

CertificationDefinitionId

DisplayOrder

Constraints

QuestionText 4000

Explanation 4000

Cascade

Question

↓

QuestionOption

DeleteBehavior.Cascade

---

QuestionOption

Table

QuestionOptions

Constraints

OptionText 2000

---

# DbContext

Add

DbSet<Question>

DbSet<QuestionOption>

Generate migration

AddQuestions

---

# DTOs

QuestionDto

QuestionOptionDto

CreateQuestionRequest

UpdateQuestionRequest

The create/update request must contain

List<QuestionOptionDto>

---

# CQRS

Commands

CreateQuestionCommand

UpdateQuestionCommand

DeleteQuestionCommand

Queries

GetQuestionQuery

GetQuestionsQuery

---

# Validators

Validate every business rule above.

Do not duplicate validation in handlers.

---

# Handlers

Use

IApplicationDbContext

IMapperService

Result<T>

Requirements

Create

Reject invalid certification

Reject inactive certification

Persist Question with QuestionOptions

Update

Update Question

Replace QuestionOptions safely

Delete

Soft delete only

Queries

Never return deleted Questions

Use ApplyPaging()

Order by

DisplayOrder

QuestionText

---

# Mapping

Create Mapster registrations.

No manual property copying.

---

# Controller

QuestionsController

Route

/api/v1/questions

Endpoints

GET

GET/{id}

POST

PUT/{id}

DELETE/{id}

Authorization

GET

Policy = AdminOrTrainer

POST

Policy = AdminOnly

PUT

Policy = AdminOnly

DELETE

Policy = SuperAdminOnly

Swagger

XML comments

ProducesResponseType

---

# Tests

Unit

Create valid question

Invalid certification

Inactive certification

Invalid option count

Invalid correct answers

Update

Delete

Integration

GET

GET by id

POST

PUT

DELETE

Validation

Authorization

---

# Constraints

Do NOT

Create Exam entities

Create Candidate entities

Create Certificate entities

Stay focused on Question Bank only.

---

# Verification

Run

dotnet build

dotnet test

dotnet ef migrations add AddQuestions

Provide

Files created

Files modified

Migration output

Build output

Test output

Any design deviations

Suggested Git commit