# BookMyMark

BookMyMark is a library catalog and personal reading-list application. The project
demonstrates a controller-based ASP.NET Core Web API with dependency injection,
Entity Framework Core, SQLite, Swagger, and a React frontend consumer.

## Project goals

- Expose a read-only catalog of books with cover images and metadata.
- Allow users to add catalog books to a personal reading list.
- Track whether a saved book is currently being read or has been finished.
- Demonstrate RESTful CRUD endpoints, dependency injection, persistence, and API
  consumption.

## Solution structure

```text
BookMyMark/
├── BookMyMark.sln
├── BookMyMark.Api/
│   ├── Features/          # HTTP controllers grouped by feature
│   └── Data/books.json    # Read-only catalog source
├── BookMyMark.AppService/ # Application services and use-case orchestration
├── BookMyMark.Command/    # CQRS write commands and handlers
├── BookMyMark.DTO/        # Request and response contracts
├── BookMyMark.Infrastructure/
│   ├── Data/              # EF Core DbContext
│   ├── Migrations/        # Versioned database schema
│   ├── Repositories/      # SQLite persistence
│   └── Services/          # Catalog file access
├── BookMyMark.Query/      # CQRS read queries and handlers
└── BookMyMark.Shared/     # Domain models and shared types
BookMyMark.Ui/
    └── src/
        ├── app/           # Redux store
        └── pages/         # Books and reading-list pages
```

## Architecture

### API

The API uses separate projects based on Clean Architecture and lightweight CQRS:

1. **API** exposes controllers, Swagger, middleware, and HTTP status-code decisions.
2. **DTO** defines request contracts without exposing infrastructure types.
3. **Query** contains read operations and query handlers.
4. **Command** contains write operations and command handlers.
5. **AppService** coordinates application use cases and business rules.
6. **Infrastructure** contains EF Core, SQLite repositories, migrations, and catalog file access.
7. **Shared** contains domain models and common types.
8. **SQLite** stores user reading-list records.

This is intentionally lightweight CQRS rather than a full framework-based CQRS
implementation. Commands and queries are explicit classes with handlers, without
adding a mediator dependency that would obscure the flow for this assessment.

Handlers, application services, repositories, and the EF Core context are registered
through ASP.NET Core dependency injection in `BookMyMark.Api/Program.cs`.
The catalog service is registered as a singleton because it reads static JSON data,
while the reading-list repository and service are scoped because they use a scoped
EF Core context.

### Data design

The catalog in `BookMyMark.Api/Data/books.json` is intentionally read-only. This
prevents users from changing the shared library catalog and keeps the assessment
focused on reading-list CRUD.

User-specific data is stored in:

```text
BookMyMark.Api/Data/bookmymark.db
```

The `ReadingListItems` table stores:

- `Id`
- `BookId`
- `Status` (`Reading` or `Finished`)
- `AddedAt`
- `FinishedAt`

The database schema is managed with EF Core migrations. The project uses SQLite so
the application remains easy to run locally without installing a separate database
server.

## API endpoints

### Book catalog

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/books` | Return all catalog books |
| `GET` | `/api/books/{id}` | Return one catalog book |

### Reading list

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/reading-list` | Return all saved books |
| `GET` | `/api/reading-list?status=Reading` | Filter by reading status |
| `GET` | `/api/reading-list/{id}` | Return one saved item |
| `POST` | `/api/reading-list` | Add a catalog book using `{ "bookId": 1 }` |
| `PUT` | `/api/reading-list/{id}/status` | Change status using `{ "status": "Finished" }` |
| `DELETE` | `/api/reading-list/{id}` | Remove a saved item |

The API uses conventional response codes:

- `200 OK` for successful reads and updates
- `201 Created` when a reading-list item is created
- `204 No Content` when an item is deleted
- `400 Bad Request` for invalid input or an unknown catalog book
- `404 Not Found` when a requested item does not exist

## Prerequisites

- .NET 8 SDK
- Node.js and npm
- Entity Framework Core CLI:

```powershell
dotnet tool install --global dotnet-ef
```

If `dotnet-ef` is already installed, this command is not required.

## First-time setup

From the repository root:

```powershell
dotnet restore .\BookMyMark.Api
Set-Location .\BookMyMark.Api
dotnet ef database update --project ..\BookMyMark.Infrastructure --startup-project .
Set-Location ..
Set-Location .\BookMyMark.Ui
npm install
Set-Location ..
```

`dotnet ef database update` creates or updates the local SQLite database from the
checked-in migrations.

## Run the application

### Quick start

From the `BookMyMark` folder, run the API:

```powershell
dotnet run --project .\BookMyMark.Api
```

Then open a second terminal for the React UI:

```powershell
Set-Location .\BookMyMark.Ui
npm run dev
```

Open two terminals in the `BookMyMark` folder.

### Terminal 1: ASP.NET Core API

```powershell
dotnet run --project .\BookMyMark.Api
```

The API runs at http://localhost:5087.

- Swagger UI: http://localhost:5087/swagger
- Catalog: http://localhost:5087/api/books
- Reading list: http://localhost:5087/api/reading-list

### Terminal 2: React frontend

```powershell
Set-Location .\BookMyMark.Ui
npm run dev
```

The frontend runs at http://localhost:4200.

Keep both terminals open while using the app. Press `Ctrl+C` in each terminal to stop
the API and frontend.

## Frontend approach

The frontend is a separate React and TypeScript project. It uses:

- Mantine for the UI system
- Redux Toolkit and RTK Query for API calls and cache invalidation
- Feature-oriented page folders
- `model.ts`, `service.ts`, and `constants.ts` conventions inside each page

The frontend consumes the API rather than duplicating reading-list state locally.
Adding, finishing, and deleting an item invalidates the RTK Query cache so the
catalog and shelves stay synchronized.

## Development decisions

- **SQLite instead of a server database:** keeps setup lightweight while still
  demonstrating real persistence and SQL generated by EF Core.
- **EF Core migrations:** database structure is versioned and reproducible.
- **JSON catalog:** separates shared read-only book data from user-owned state.
- **Service interfaces:** controllers depend on abstractions, making the business
  logic easier to test and replace.
- **Cancellation tokens:** async API and database operations accept cancellation
  tokens so abandoned requests can stop cleanly.
- **CORS:** the API allows the local React development origin
  `http://localhost:4200`.

## Useful commands

```powershell
# Build the API solution
dotnet build .\BookMyMark.sln

# Create a new migration after changing EF Core models
dotnet ef migrations add MigrationName --project .\BookMyMark.Infrastructure --startup-project .\BookMyMark.Api

# Apply migrations
dotnet ef database update --project .\BookMyMark.Infrastructure --startup-project .\BookMyMark.Api

# Build the frontend for production
Set-Location .\BookMyMark.Ui
npm run build
```

Generated build output and the local SQLite database are excluded from Git. The
migration source files remain committed so another developer can recreate the
database schema.
