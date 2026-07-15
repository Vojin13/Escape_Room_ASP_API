---
name: clean-architecture-cqrs
description: Conventions and known pitfalls for adding features to this Escape Room API's Clean Architecture / CQRS-style codebase (Domain/Application/Infrastructure/Implementation/API layers, ICommand/IQuery use cases). Use when adding a new use case, endpoint, entity, or migration.
---

# Clean Architecture / CQRS conventions for this project

This project already documents the base pattern in `CLAUDE.md` (layers, dependency direction, "Adding a new use case" recipe, authorization model). Read that first. This skill exists for the operational pitfalls that aren't written down anywhere else — things this project actually broke on during development.

## Quick pattern recap

- Every feature is a use case: `ICommand<TRequest>` (void, state-changing) or `IQuery<TParam, TResponse>` (returns data, may still mutate — e.g. `LockTimeslotQuery`).
- Implementations live in `Implementation/UseCases/Commands|Queries/**`, prefixed `Ef*`, extend `EfUseCase` (gives `protected AppDbContext _ctx` and `AssignRoleUseCases`).
- Each use case has a stable kebab-case `Id` slug and a human `Name`.
- Controllers never contain logic — they resolve the use case via `[FromServices]` and call `_handler.ExecuteCommand`/`ExecuteQuery`.
- If a use case needs to know "who is calling," inject `IApplicationUser` into the `Ef*` class's constructor directly (not via the controller setting a DTO field) — this is the current, correct pattern after a security bug was found where a controller forgot to set `dto.UserId`.

## Pitfalls actually hit in this codebase

1. **Seed idempotency**: `EfSeedCommand`'s `SeedRolesAndUseCases()` (and other `Seed*` methods) short-circuit with `if (_ctx.Roles.Any()) return;`. Adding a new use-case slug to the seed list does **not** retroactively grant it to existing users/roles in an already-seeded DB. Either reseed from empty, or manually patch: `INSERT INTO "RoleUseCases"/"UserUseCases" ... ON CONFLICT DO NOTHING`.

2. **Lookup tables seeded via EF migrations, not `EfSeedCommand`**: `Difficulties` and `BookingStatuses` are populated via `migrationBuilder.InsertData(...)` inside their migration files (`HasData()` in `DifficultyConfiguration`/`BookingStatusConfiguration`), not via the seed command. If you ever wipe these tables (e.g. a broad `TRUNCATE`), EF will **not** repopulate them — the migration is already marked applied. You must manually re-insert matching the migration's values, or the app breaks (`SeedRooms` throws "array is empty" when `Difficulties` is empty).

3. **`DateTime.SpecifyKind(..., DateTimeKind.Utc)` required before any Npgsql write/compare** on `timestamptz` columns — Npgsql throws on `Kind=Unspecified`. Recurring source of bugs in booking/lock date handling.

4. **`TimeOnly` JSON binding requires `"HH:mm:ss"`** (with seconds) — `"HH:mm"` fails model binding with a 400, not a validation error. Same for any `TimeOnly` request field.

5. **`return Created();` (no args) returns 204, not 201.** Always use `return StatusCode(201);` for Create actions — this exact bug was found independently in two different controllers (`ReviewsController`, `AdminUsersController`).

6. **`TRUNCATE ... CASCADE` cascades further than you think.** `ErrorLogs.UserId` and `AuditLogs.UserId` both have FKs to `Users`. Truncating `Users` with `CASCADE` silently wipes those log tables too. Prefer scoping deletes/truncates per-table and checking `\d "TableName"` for incoming FKs first.

7. **`.ToString("HH:mm")` on `TimeOnly`/`DateTime` needs an escaped colon** (`@"HH\:mm"`) — bare `:` is a culture-sensitive time-separator placeholder in .NET format strings, not a literal character.

8. **Building while the app is running fails the file copy step** (`MSB3027`/`MSB3021`, DLL/exe locked), not a compile error — check for a running `dotnet run`/`API.exe` process (yours or the user's) before assuming a real build break. Only kill processes you started yourself without asking first.

9. **`UserUseCase` is a denormalized snapshot of `RoleUseCase`**, copied at registration/role-change time via `AssignRoleUseCases`. It is not derived live — changing what a role can do does not retroactively affect existing users. This is intentional groundwork for a possible future "per-user use-case override" admin feature, not a bug, but treat any `RoleUseCase` change as needing a migration/backfill plan for existing users.

## Before adding a new use case, check

- [ ] DTO in `Application/DTO/**`
- [ ] Interface in `Application/Commands/**` or `Application/Queries/**`
- [ ] `Ef*` implementation extends `EfUseCase`, sets `Id`/`Name`, injects `IApplicationUser` if it needs caller identity
- [ ] Validator in `Implementation/UseCases/Validators/**` if state-changing
- [ ] Registered in `Implementation/DependencyInjection.cs`
- [ ] Slug added to the correct role list(s) in `EfSeedCommand.SeedRolesAndUseCases()` **and** patched into an already-seeded DB if not doing a full reseed
- [ ] Controller action returns the correct status code (201/204/200 — not `Created()`/`NoContent()` mismatches)
