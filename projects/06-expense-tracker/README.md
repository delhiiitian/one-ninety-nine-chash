# Project 6: Expense Tracker

Track spending from the command line. Every change is saved to a JSON file immediately (and safely), a bank-style CSV can be imported with bad rows reported, and reports show spending by category and month. Data survives between runs.

## Run it

```bash
dotnet run --project projects/06-expense-tracker                       # data in ./expense-data
dotnet run --project projects/06-expense-tracker -- /path/to/folder    # choose the data folder
dotnet run --project projects/06-expense-tracker < projects/06-expense-tracker/sample-input.txt
```

Commands: `add <yyyy-mm-dd|today> <amount> <category> <note>`, `list`, `summary`, `months`, `export <file.csv>`, `import <file.csv>`, `quit`.

## Files

| File | What it does |
| --- | --- |
| `Program.cs` | Loads data, runs the command loop, saves after every change |
| `Expense.cs` | The `Expense` record (uses `DateOnly` for dates) |
| `ExpenseStore.cs` | Async JSON load/save; writes to a temp file and swaps it in; moves a corrupt file aside |
| `CsvFile.cs` | CSV export and import with quoted fields; bad rows are skipped and listed |
| `Reports.cs` | Totals by category (with a text bar chart) and by month |
| `sample-bank-export.csv` | A bank-style export with two deliberately broken rows |
| `sample-input.txt` | A scripted session |

## Ideas to extend it

- `delete <id>` and `edit <id> <amount>` commands.
- A monthly budget per category with a warning when you go over.
- Import the CSV format of your own bank.
