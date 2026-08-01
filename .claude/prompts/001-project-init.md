# Prompt 001 – Project Initialization

## Objective

Initialize the **Certification Assessment Platform** as a production-grade enterprise application.

This prompt should create the project foundation only.

**Do NOT implement any business features.**

---

## Read First

Before making any changes:

1. Analyze the existing workspace.
2. Read `CLAUDE.md` and follow all project rules.
3. Preserve all existing files.
4. Do not overwrite unrelated code.
5. If a file already exists, extend it instead of replacing it.

---

# Technology Stack

Backend

* ASP.NET Core (.NET 10)
* Clean Architecture
* Entity Framework Core
* PostgreSQL

Frontend

* React
* TypeScript
* Vite
* Tailwind CSS
* shadcn/ui
* React Router
* TanStack Query
* React Hook Form
* Zod

Infrastructure

* Docker
* Docker Compose

---

# Create the Solution Structure

Create the following structure if it does not already exist:

```
backend/
    src/
        Certification.Api
        Certification.Application
        Certification.Domain
        Certification.Infrastructure
        Certification.Shared

    tests/
        Certification.UnitTests
        Certification.IntegrationTests

frontend/
    web/

database/
    migrations/
    scripts/
    seed/

docker/

docs/

scripts/

.github/
```

---

# Backend Requirements

Create an ASP.NET Core solution.

Reference the projects correctly.

Configure dependency injection.

Configure configuration loading.

Configure logging using Serilog.

Configure Swagger.

Configure Health Checks.

Configure PostgreSQL connection placeholders.

Do NOT create:

* Controllers
* Authentication
* Business entities
* Database tables
* Repositories
* Services

---

# Frontend Requirements

Create a React + TypeScript + Vite application.

Configure:

* Tailwind CSS
* shadcn/ui
* React Router
* TanStack Query
* React Hook Form
* Zod

Create only:

* Layout
* Theme
* Empty Home page
* Placeholder routing

Do NOT implement authentication or business pages.

---

# Docker

Create:

* Dockerfile for Backend
* Dockerfile for Frontend
* docker-compose.yml

Use placeholders where configuration values are unknown.

---

# Configuration

Create placeholder configuration files only.

Do NOT hardcode secrets.

Use environment variables wherever possible.

---

# Git

Create a professional `.gitignore` suitable for:

* .NET
* React
* Node.js
* Docker
* VS Code

Do not delete any existing `.gitignore`.

---

# Documentation

Create or update:

* README.md

Include:

* Project overview
* Prerequisites
* How to build
* How to run
* Folder structure

---

# Code Quality

Follow:

* SOLID Principles
* Clean Architecture
* Async programming
* Dependency Injection
* Meaningful naming
* Production-quality code

---

# Before Writing Code

Explain:

1. Your implementation plan.
2. Files that will be created.
3. Files that will be modified.

---

# After Completing

Provide:

1. Summary of work completed.
2. Final project tree.
3. Manual verification steps.
4. Build commands.
5. Run commands.
6. Any issues encountered.
7. Suggested Git commit message.

Do not continue with authentication or any other feature after completing this task.
