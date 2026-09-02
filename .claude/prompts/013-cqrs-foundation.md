# Epic 4 - Task 4.1
## CQRS Foundation

Continue the existing enterprise ASP.NET Core (.NET 10) solution.

Read before making changes:

- CLAUDE.md
- Existing Application layer
- Existing Authentication implementation
- Existing DependencyInjection
- Existing Contracts

---

## Objective

Introduce CQRS using MediatR.

This task establishes the Application layer architecture.

No business features should be implemented.

---

## Install Packages

Application

- MediatR
- MediatR.Extensions.Microsoft.DependencyInjection

---

## Create Folder Structure

Application/

Common/

Behaviors/
Commands/
Queries/
Results/
Exceptions/
Interfaces/

---

## Create Marker Interfaces

ICommand<TResponse>

IQuery<TResponse>

Both inherit from MediatR IRequest<TResponse>

---

## Create Result Pattern

Create:

Application/Common/Results/

Result.cs

Result<T>.cs

Requirements:

Support:

- Success
- Failure
- Error Message
- Validation Errors

Provide static helper methods.

No exceptions for expected business failures.

---

## Pipeline Behaviors

Create:

ValidationBehavior<TRequest,TResponse>

LoggingBehavior<TRequest,TResponse>

PerformanceBehavior<TRequest,TResponse>

UnhandledExceptionBehavior<TRequest,TResponse>

Requirements

LoggingBehavior

- Log request name
- Execution time

PerformanceBehavior

- Warn if execution exceeds 500ms

ValidationBehavior

- Detect FluentValidation validators
- If none exist, continue
- Leave TODO comments for future validators

UnhandledExceptionBehavior

- Log
- Re-throw

---

## Register MediatR

Update

Application/DependencyInjection.cs

Register:

- MediatR
- All pipeline behaviors

Use assembly scanning.

---

## Constraints

Do NOT

- Create Commands
- Create Queries
- Create Controllers
- Create Validators
- Create Repositories
- Modify authentication
- Modify endpoints

---

## Verification

Run:

dotnet build

dotnet test

Must succeed.

0 warnings.

0 errors.

Provide:

- Files created
- Files modified
- Build output
- Suggested Git commit