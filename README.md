# Certification Assessment Platform

An enterprise certification assessment platform built with Clean Architecture on ASP.NET Core and a React + TypeScript frontend.

> This repository currently contains the **project foundation only** — solution/project scaffolding, tooling, and configuration. No business features, authentication, or database schema have been implemented yet.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/) and npm
- [Docker](https://www.docker.com/) and Docker Compose (for containerized run)
- [PostgreSQL 16+](https://www.postgresql.org/) (if running the API outside Docker)

## How to Build

**Backend**

```bash
dotnet build backend/Certification.slnx
```

**Frontend**

```bash
cd frontend/web
npm install
npm run build
```

## How to Run

**Backend (API)**

```bash
dotnet run --project backend/src/Certification.Api
```

- Swagger UI: `http://localhost:5000/swagger` (Development environment)
- Health check: `http://localhost:5000/health`

**Frontend (Web)**

```bash
cd frontend/web
npm run dev
```

- App: `http://localhost:5173`

**Full stack via Docker Compose**

```bash
cp .env.example .env   # set POSTGRES_PASSWORD and other values
docker compose up --build
```

- Web: `http://localhost:5173`
- API: `http://localhost:8080`
- PostgreSQL: `localhost:5432`

## Folder Structure

```
backend/
    Certification.slnx
    src/
        Certification.Api             # ASP.NET Core host (DI, logging, Swagger, health checks)
        Certification.Application     # Application layer (use cases, service contracts)
        Certification.Domain          # Domain entities and business rules
        Certification.Infrastructure  # EF Core, PostgreSQL, external integrations
        Certification.Shared          # Cross-cutting types shared across layers
    tests/
        Certification.UnitTests
        Certification.IntegrationTests

frontend/
    web/                    # React + TypeScript + Vite app
        src/
            components/     # Layout and UI primitives (shadcn/ui)
            pages/          # Route-level pages
            providers/      # App-wide providers (theme, etc.)
            lib/            # Shared utilities

database/
    migrations/             # EF Core migrations output
    scripts/                # Ad hoc DB scripts
    seed/                   # Seed data

docker/                     # Shared/auxiliary Docker assets
docs/                       # SRS, architecture, API, and other project docs
scripts/                    # Repo-level automation scripts
.github/                    # GitHub configuration (workflows, templates)

docker-compose.yml
.env.example
```

## Architecture

- Clean Architecture with strict layer boundaries (Api → Application/Infrastructure → Domain)
- SOLID principles and Dependency Injection throughout
- REST APIs via ASP.NET Core
- PostgreSQL via Entity Framework Core
- Structured logging via Serilog
- API documentation via Swagger/OpenAPI
