# Project 9: Notes REST API

A small but complete web API for personal notes, the kind of back end that sits behind a mobile notes app. It uses ASP.NET Core minimal APIs for HTTP, Entity Framework Core with SQLite for storage, options binding for configuration, dependency injection for wiring, ILogger for logs, and an integration test suite that starts the real API in memory.

## Run it

```bash
# start the server on http://localhost:5199 (creates notes.db in the current folder)
dotnet run --project projects/09-notes-api/src/NotesApi

# in a second terminal
curl -s -X POST http://localhost:5199/notes -H "Content-Type: application/json" \
     -d '{"title":"Buy milk","tags":["home"]}'
curl -s http://localhost:5199/notes

# or let a script start the server, send requests and stop it
./projects/09-notes-api/demo.sh

# run the 14 integration tests
dotnet test projects/09-notes-api/NotesApi.slnx
```

`requests.http` has every request ready to send from VS Code (REST Client), Visual Studio or Rider.

## Endpoints

| Method and path | What it does | Success | Errors |
| --- | --- | --- | --- |
| `GET /health` | Liveness check | 200 | |
| `GET /notes?search=&tag=&page=1&pageSize=10` | List notes, pinned first then newest | 200 | 400 bad paging |
| `GET /notes/{id}` | One note | 200 | 404 |
| `POST /notes` | Create a note | 201 + `Location` | 400 validation |
| `PUT /notes/{id}` | Replace a note | 200 | 400, 404 |
| `DELETE /notes/{id}` | Delete a note | 204 | 404 |
| `GET /tags` | Tag usage counts | 200 | |

## Layout

| Path | What it holds |
| --- | --- |
| `NotesApi.slnx`, `Directory.Build.props` | Solution and shared build settings |
| `src/NotesApi/Program.cs` | Configuration, DI registrations, database creation, middleware, route mapping |
| `src/NotesApi/appsettings.json` | Connection string, page-size and tag limits, log levels |
| `src/NotesApi/Models/Note.cs` | `Note` entity, `NoteInput` and `NoteResponse` DTOs, `PagedResult<T>`, `NotesOptions` |
| `src/NotesApi/Models/NoteValidator.cs` | Validation rules and tag normalisation |
| `src/NotesApi/Data/NotesDb.cs` | EF Core `DbContext` and a date converter for SQLite |
| `src/NotesApi/Endpoints/NoteEndpoints.cs` | Every `/notes` and `/tags` handler |
| `tests/NotesApi.Tests/` | `WebApplicationFactory` set-up with a temp database and fake clock, plus 14 tests |
| `requests.http`, `demo.sh` | Ways to try the API by hand |

## Ideas to extend it

- Add users and JWT authentication so each person only sees their own notes.
- Switch `EnsureCreated` to EF Core migrations (`dotnet tool install -g dotnet-ef`, then `dotnet ef migrations add Initial`).
- Add OpenAPI (`builder.Services.AddOpenApi()` and `app.MapOpenApi()`) and browse the generated document.
- Add `PATCH /notes/{id}/pin` to toggle the pin without sending the whole note.
