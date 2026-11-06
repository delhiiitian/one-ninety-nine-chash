# Project 5: Safe Bank

A small bank simulator that never crashes on bad input and never loses money. Accounts guard their own balance, every rule break is a named exception, transfers roll back if the second half fails, and an audit log records every successful operation.

## Run it

```bash
dotnet run --project projects/05-safe-bank
dotnet run --project projects/05-safe-bank < projects/05-safe-bank/sample-input.txt
```

Commands: `list`, `deposit <id> <amount>`, `withdraw <id> <amount>`, `transfer <from> <to> <amount>`, `freeze <id>`, `quit`.

## Files

| File | What it does |
| --- | --- |
| `Program.cs` | Command loop; one `try` with ordered `catch` blocks turns every error into a friendly message |
| `Bank.cs` | Holds accounts, looks them up, and performs all-or-nothing transfers |
| `Account.cs` | Balance rules with guard clauses: positive amounts, no overdraft, daily limit, frozen accounts |
| `BankErrors.cs` | `BankException` and its children: not found, insufficient funds, frozen |
| `AuditLog.cs` | An `IDisposable` file logger, opened with `using` |
| `sample-input.txt` | A session that hits every error path |

## Ideas to extend it

- Add an `unfreeze` command that only works with a manager PIN.
- Reset `_withdrawnToday` when the date changes.
- Save accounts to JSON on exit and load them on start (Chapter 6).
