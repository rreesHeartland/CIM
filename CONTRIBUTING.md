# Contributing to Heartland Calibrated Instruments Manager

Thanks for taking the time to contribute! This document explains how to set up
your environment, the branching/commit conventions we follow, and how changes
get reviewed and merged.

## Table of Contents
- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Branch Naming](#branch-naming)
- [Commit Messages](#commit-messages)
- [Coding Standards](#coding-standards)
- [Database Changes](#database-changes)
- [Testing](#testing)
- [Pull Requests](#pull-requests)
- [Reporting Bugs & Requesting Features](#reporting-bugs--requesting-features)

## Code of Conduct
Be respectful and constructive. Assume good intent, keep discussions technical,
and help newcomers get up to speed.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Microsoft SQL Server (Express, Developer, LocalDB, or full)
- `dotnet-ef` global tool: `dotnet tool install --global dotnet-ef`

### First-time setup
```bash
git clone https://github.com/YOUR_USERNAME/HeartlandCIM.git
cd HeartlandCIM
dotnet restore
# configure src/HeartlandCIM.Web/appsettings.json -> ConnectionStrings:DefaultConnection
cd src/HeartlandCIM.Web
dotnet ef database update      # or run database/schema.sql + database/seed.sql
dotnet run
```

## Development Workflow
1. Create a branch off `main` (see [Branch Naming](#branch-naming)).
2. Make your changes with clear, focused commits.
3. Ensure `dotnet build` succeeds with **no warnings** and the app runs.
4. Push your branch and open a Pull Request against `main`.
5. Address review feedback; a maintainer merges once CI is green and approved.

Never commit directly to `main`. All changes flow through Pull Requests.

## Branch Naming
Use a short, descriptive, hyphenated name prefixed by type:
- `feature/<short-description>` — new functionality
- `fix/<short-description>` — bug fixes
- `chore/<short-description>` — tooling, docs, refactors
- `db/<short-description>` — schema/migration changes

Examples: `feature/instrument-export`, `fix/cycle-cutoff-timezone`.

## Commit Messages
Follow [Conventional Commits](https://www.conventionalcommits.org/):

```
<type>(optional scope): <short summary>

<optional body explaining what and why>
```

Types: `feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`, `chore`, `db`.

Examples:
- `feat(calibrations): add slide-to-confirm for verify action`
- `fix(admin): prevent cycle reset while an area is Open`
- `db: add index on CalibrationInstruments.Next_Cal_Date`

## Coding Standards
- Target **.NET 8** / C# 12; enable nullable reference types where practical.
- Keep **business logic in service classes** (`Services/`), not in controllers.
- Controllers stay thin: validate input, call a service, return a result.
- `CalibrationInstrument.Status` is **always computed** from timestamp fields —
  never persist it as a column.
- Use `async`/`await` for all database and I/O operations.
- Follow existing naming: PascalCase for types/methods, camelCase for locals.
- Run `dotnet format` before committing to keep style consistent.
- Prefer ViewModels (`ViewModels/`) for passing data to views over raw entities.

## Database Changes
- Make schema changes via **EF Core migrations**:
  ```bash
  cd src/HeartlandCIM.Web
  dotnet ef migrations add <MeaningfulName>
  dotnet ef database update
  ```
- Keep `database/schema.sql` (standalone script) in sync with the migration.
- Update `database/seed.sql` if new columns need sample values.
- Note migrations in your PR description.

## Testing
- Verify the full **Remove → Install → Verify** workflow still works.
- Confirm the app builds and runs against a real SQL Server instance.
- Manually smoke-test any screen you touched (and adjacent ones).
- If you add automated tests, place them in a `tests/` project and ensure they
  pass with `dotnet test`.

## Pull Requests
A good PR:
- Has a clear title and description of **what** changed and **why**.
- References any related issue (e.g., `Closes #12`).
- Is scoped to a single concern — split unrelated changes.
- Passes CI (build + any tests).
- Includes screenshots for UI changes when helpful.

## Reporting Bugs & Requesting Features
Open a GitHub Issue and include:
- **Bugs:** steps to reproduce, expected vs. actual behavior, environment
  (OS, .NET version, SQL Server version), and relevant logs/screenshots.
- **Features:** the problem you're trying to solve and your proposed approach.

Thanks for contributing! 🎉
