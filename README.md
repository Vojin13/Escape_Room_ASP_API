# Cipher Escape — Escape Room Booking API

A REST API for an escape room company ("Cipher Escape") that lets customers browse rooms, check timeslot availability, book a session, and leave reviews after playing — while giving admins full management of rooms, timeslots, users, bookings, and system oversight (audit trail, error logs).

Built with **ASP.NET Core 8** and **PostgreSQL**, following **Clean Architecture** with a custom **use-case–based authorization** model (rather than plain role checks on controllers).

## Tech stack

- **ASP.NET Core 8 Web API**
- **Entity Framework Core 8** (Npgsql provider) — code-first, migrations
- **PostgreSQL**
- **JWT bearer authentication** (access + refresh tokens)
- **FluentValidation** for request/business-rule validation
- **AutoMapper** for DTO ↔ entity mapping
- **BCrypt.Net** for password hashing
- **Handlebars.Net** for HTML email templates
- **Bogus** for realistic demo/seed data
- **Swashbuckle (Swagger)** for interactive API docs

## Architecture

The solution is split into five projects with a strict, one-directional dependency flow:

```
API → Implementation → Application ← Domain ← Infrastructure
```

| Project | Responsibility |
|---|---|
| **Domain** | Entities, enums, base entity (Id + CreatedAt) — no dependencies |
| **Application** | Use-case contracts (`ICommand<T>`, `IQuery<TParam, TResult>`), DTOs, custom exceptions — depends only on Domain |
| **Infrastructure** | EF Core `DbContext`, entity configurations, migrations — depends only on Domain |
| **Implementation** | Concrete use-case implementations, validators, AutoMapper profiles, email templating, dependency injection wiring |
| **API** | Controllers, JWT issuing/validation, middleware, configuration |

### Use-case pattern

Every feature (register, create booking, cancel booking, admin update user, …) is modeled as its own **use case** class rather than a fat service:

- Commands (state-changing, no return value): `ICommand<TRequest>`
- Queries (return data, may still write — e.g. locking a timeslot): `IQuery<TParam, TResponse>`
- Each use case carries a stable string `Id` (a kebab-case slug, e.g. `create-booking`) and a human-readable `Name`
- Controllers never contain business logic — they resolve the relevant use case via DI and delegate to a shared `UseCaseHandler`

### Authorization model

Authorization is **use-case–based**, not just role-based at the controller level:

- Each role (e.g. `User`, `Admin`) is granted a set of use-case slugs it may execute
- On login, the issued JWT embeds the caller's allowed use-case slugs as a claim
- A central handler checks the caller's allowed slugs before running *any* use case, and rejects with `403` otherwise
- Requests without a valid token still resolve to an explicit "unauthorized" identity with a minimal, hardcoded allowlist of genuinely public actions (browsing rooms, registering, logging in, etc.)

### Cross-cutting concerns

- **Global exception handling middleware** maps exceptions to correct HTTP status codes and persists unhandled (500) errors to an `ErrorLogs` table with a support code, request context, and the acting user (if any) — with an admin endpoint to search them.
- **Audit log**: every single use-case invocation — successful or blocked — is recorded (acting user, use case, HTTP method, execution time, whether it was authorized, timestamp). Searchable by user, use case, HTTP method, and date range via a dedicated admin endpoint.

## Feature overview

### Public / customer-facing
- Browse rooms with pagination, keyword search, and filtering; view room detail (description, difficulty, price, image gallery, average rating)
- Check real-time timeslot availability for a room on a given date
- Register with email verification (confirmation email + activation link), login/logout, JWT refresh
- **Booking flow**: temporarily lock a timeslot while completing checkout, then confirm the booking (prevents double-booking race conditions); cancel an upcoming booking; view booking history and a single booking's detail
- Leave a review for a room after completing a booking there (eligibility is enforced server-side — can't review a room you haven't actually played)
- Browse reviews per room, paginated

### Admin
- Manage rooms: create (with multi-image upload), update, delete, activate/deactivate
- Manage timeslots: create, update, delete
- Manage users: list/search, create, update (including role changes, which re-sync that user's permissions), delete
- View and search all bookings across all customers; change a booking's status (e.g. confirm, cancel, mark completed)
- Moderate reviews (delete)
- Search the **error log** (by support code, keyword, date) and the **audit log** (by user, use case, HTTP method, date range)

### Cross-cutting features required by the assignment brief
- Layered Clean Architecture with strict dependency direction
- Server-side validation (FluentValidation) on every state-changing endpoint — input shape, business rules (uniqueness, referential validity, date ranges, etc.)
- Pagination + search/filtering on every collection endpoint
- Relational, code-first database design (see [Data model](#data-model))
- Use-case-level authorization
- Transactional email sending (account verification)
- Correct HTTP status codes across the board (201/204/401/403/404/409/422/500)
- Audit log with multi-criteria search
- File upload (room images)

## Data model

The schema is fully code-first (EF Core migrations) and includes entities such as: `User`, `Role`, `Room`, `RoomImage`, `Difficulty`, `Timeslot`, `RoomTimeslot`, `TimeslotLock`, `Booking`, `BookingStatus`, `Review`, `AuthToken`, `UserUseCase`, `RoleUseCase`, `ErrorLog`, and `AuditLog` — covering one-to-many, many-to-many, and optional (nullable FK) relationships.

## Getting started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) (a local instance is fine)
- The EF Core CLI tool: `dotnet tool install --global dotnet-ef`

### 1. Restore & build
From the repository root:
```bash
dotnet restore
dotnet build
```

### 2. Configure settings
The API project reads configuration from `appsettings.json` / `appsettings.Development.json`, following the standard ASP.NET Core configuration model. Create (or fill in) an `appsettings.Development.json` under the API project with:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=<your-db-name>;Username=<your-user>;Password=<your-password>;"
  },
  "JwtSettings": {
    "SecretKey": "<a long random secret>",
    "Issuer": "EscapeRoomAPI",
    "DurationSeconds": 600,
    "RefreshTokenHours": 24
  },
  "MailSettings": {
    "FromEmail": "<sender email address>",
    "AppPassword": "<SMTP app password>"
  }
}
```
This file is intentionally excluded from source control since it holds real secrets (DB credentials, JWT signing key, mail credentials).

> Room images referenced by the demo seed data live under the API project's static files folder, which is also excluded from source control. To see room photos after seeding, either add your own images and point the seed logic at them, or ignore the image fields — the rest of the app functions the same either way.

### 3. Apply database migrations
```bash
dotnet ef database update --project Infrastructure --startup-project API
```

### 4. Run the API
```bash
dotnet run --project API
```
Swagger UI will be available at the URL printed in the console (e.g. `http://localhost:5134/swagger`).

### 5. Seed demo data
With the API running, send an (unauthenticated) request:
```
POST /api/seed
```
This populates roles/permissions, timeslots, an admin account, a batch of demo users, rooms (with difficulty, pricing, and image galleries), bookings, and reviews — enough to explore every feature end-to-end.

**Seeded credentials** (after seeding):
- Admin: `admin@cipherescape.com` / `admin123`
- Regular demo users: password `test123` (see the `/api/admin/users` endpoint, or the database, for generated usernames/emails)

## Project structure

```
Domain/            entities, enums, base entity
Application/       use-case interfaces, DTOs, custom exceptions
Infrastructure/     EF Core DbContext, entity configurations, migrations
Implementation/     use-case implementations, validators, mappings, email templates, DI wiring
API/               controllers, JWT handling, middleware, configuration
```

## Adding a new use case

1. Define the request/response DTO(s) in `Application/DTO`
2. Define the use-case interface in `Application/Commands` or `Application/Queries`
3. Implement it in `Implementation/UseCases/Commands` or `.../Queries`
4. Add a FluentValidation validator if it mutates state
5. Register the use case (and validator) in the dependency injection setup
6. Grant the new use-case slug to the appropriate role(s) in the seed logic
7. Expose it through a controller action that delegates to the shared use-case handler
