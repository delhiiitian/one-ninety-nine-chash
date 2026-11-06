// Project 6: Expense Tracker
// Uses Chapter 6 ideas: paths, JSON files, CSV import/export, directories, safe saving and async I/O.
// Run: dotnet run --project projects/06-expense-tracker [data-folder]
using System.Globalization;

string dataFolder = args.Length > 0 ? args[0] : Path.Combine(Environment.CurrentDirectory, "expense-data");
var store = new ExpenseStore(dataFolder);
List<Expense> expenses = await store.LoadAsync();
Console.WriteLine($"Expense Tracker. Data file: {store.FilePath} ({expenses.Count} expenses loaded)");
Console.WriteLine("Commands: add <date|today> <amount> <category> <note> | list | summary | months | export <file.csv> | import <file.csv> | quit");

string? line;
while ((line = ReadCommand()) is not null && line != "quit")
{
    string[] p = line.Split(' ', 5, StringSplitOptions.RemoveEmptyEntries);
    switch (p.FirstOrDefault())
    {
        case "add" when p.Length >= 4:
            DateOnly date = p[1] == "today" ? DateOnly.FromDateTime(DateTime.Today)
                          : DateOnly.TryParseExact(p[1], "yyyy-MM-dd", out var d) ? d : default;
            if (date == default || !decimal.TryParse(p[2], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
            {
                Console.WriteLine("  Usage: add 2026-10-24 250.00 food lunch with team");
                break;
            }
            var expense = new Expense(NextId(), date, amount, p[3].ToLowerInvariant(), p.Length > 4 ? p[4] : "");
            expenses.Add(expense);
            await store.SaveAsync(expenses);                    // save after every change
            Console.WriteLine($"  Saved #{expense.Id}: {amount:N2} on {expense.Category}");
            break;

        case "list":
            foreach (Expense e in expenses.OrderBy(e => e.Date))
                Console.WriteLine($"  #{e.Id,-3} {e.Date:yyyy-MM-dd} {e.Amount,10:N2}  {e.Category,-12} {e.Note}");
            break;

        case "summary":
            Reports.ByCategory(expenses);
            break;

        case "months":
            Reports.ByMonth(expenses);
            break;

        case "export" when p.Length == 2:
            string outPath = Path.GetFullPath(p[1]);
            CsvFile.Export(outPath, expenses.OrderBy(e => e.Date));
            Console.WriteLine($"  Exported {expenses.Count} rows to {Path.GetFileName(outPath)} ({new FileInfo(outPath).Length} bytes)");
            break;

        case "import" when p.Length == 2:
            if (!File.Exists(p[1])) { Console.WriteLine($"  File not found: {p[1]}"); break; }
            var (rows, errors) = CsvFile.Import(p[1]);
            foreach (var r in rows) expenses.Add(new Expense(NextId(), r.Date, r.Amount, r.Category, r.Note));
            await store.SaveAsync(expenses);
            Console.WriteLine($"  Imported {rows.Count} rows, skipped {errors.Count}");
            foreach (string error in errors) Console.WriteLine($"    {error}");
            break;

        default:
            Console.WriteLine("  Commands: add | list | summary | months | export <file> | import <file> | quit");
            break;
    }
}

int NextId() => expenses.Count == 0 ? 1 : expenses.Max(e => e.Id) + 1;

static string? ReadCommand()
{
    Console.Write("> ");
    string? input = Console.ReadLine()?.Trim();
    if (input is not null) Console.WriteLine(input);
    return input;
}
