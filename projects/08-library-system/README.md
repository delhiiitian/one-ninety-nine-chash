# Project 8: Library Management System

A small library back end that shows off C#'s object-oriented strengths. Books, DVDs and magazines share a base class but each sets its own loan period and late fee. Storage is a generic repository behind an interface, overdue items raise an event that sends (simulated) emails asynchronously, and reports are LINQ queries. A fake clock replays 25 days of library life, so each run gives the same results (only the order of emails sent at the same moment can vary).

## Run it

```bash
dotnet run --project projects/08-library-system
```

## Layout

| Path | What it holds |
| --- | --- |
| `Program.cs` | Wiring and the 25-day simulation |
| `Domain/LibraryItem.cs` | `IEntity`, abstract `LibraryItem`, sealed `Book`, `Dvd`, `Magazine` |
| `Domain/Member.cs` | `Member` (limits, fees) and the immutable `Loan` record |
| `Infrastructure/Repository.cs` | `IRepository<T>` and `InMemoryRepository<T>` (generic, constrained to `IEntity`) |
| `Infrastructure/Notifications.cs` | `INotifier` and an async console email sender |
| `Services/LibraryService.cs` | Borrow/return rules, the `ItemOverdue` event, LINQ reports |
| `Services/Extensions.cs` | Extension methods for display |

## Ideas to extend it

- Add an `AudioBook` type: you only write one new class.
- Add a reservation queue per item (`Queue<string>` of member ids).
- Replace `InMemoryRepository<T>` with a SQLite one (Chapter 9) without touching `LibraryService`.
