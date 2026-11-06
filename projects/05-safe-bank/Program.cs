// Project 5: Safe Bank
// Uses Chapter 5 ideas: exceptions, try/catch/finally, multiple catch blocks, filters,
// throwing and rethrowing, custom exceptions, using/IDisposable, nullable types and guard clauses.
// Run: dotnet run --project projects/05-safe-bank

using System.Globalization;

string logPath = Path.Combine(Path.GetTempPath(), "safe-bank-audit.log");
using var log = new AuditLog(logPath);
var bank = new Bank(log);
bank.Open("A100", "Asha", 5_000m);
bank.Open("B200", "Ben", 1_200m);
bank.Open("C300", "Chen", 300m);

Console.WriteLine("Safe Bank. Commands: list | deposit <id> <amt> | withdraw <id> <amt> | transfer <from> <to> <amt> | freeze <id> | quit");

int ok = 0, failed = 0;
string? line;
while ((line = ReadCommand()) is not null && line != "quit")
{
    string[] p = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (p.Length == 0) continue;

    try
    {
        switch (p[0])
        {
            case "list":
                foreach (Account a in bank.All)
                    Console.WriteLine($"  {a.Id}  {a.Owner,-6} {a.Balance,12:N2}{(a.IsFrozen ? "  [FROZEN]" : "")}");
                break;
            case "deposit":
                bank.Deposit(Arg(p, 1), Amount(p, 2));
                Console.WriteLine($"  OK. {p[1]} balance: {bank.Get(p[1]).Balance:N2}");
                break;
            case "withdraw":
                bank.Withdraw(Arg(p, 1), Amount(p, 2));
                Console.WriteLine($"  OK. {p[1]} balance: {bank.Get(p[1]).Balance:N2}");
                break;
            case "transfer":
                bank.Transfer(Arg(p, 1), Arg(p, 2), Amount(p, 3));
                Console.WriteLine("  OK. Transfer complete.");
                break;
            case "freeze":
                bank.Freeze(Arg(p, 1));
                Console.WriteLine($"  {p[1]} frozen.");
                break;
            default:
                throw new FormatException($"Unknown command '{p[0]}'.");
        }
        ok++;
    }
    catch (InsufficientFundsException ex)
    {
        failed++;
        Console.WriteLine($"  DECLINED: {ex.Message} Short by {ex.Requested - ex.Balance:N2}.");
    }
    catch (BankException ex)                                   // every other bank rule
    {
        failed++;
        Console.WriteLine($"  DECLINED: {ex.Message}");
    }
    catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "amount")
    {
        failed++;
        Console.WriteLine("  INVALID: amount must be greater than zero.");
    }
    catch (FormatException ex)                                 // bad typing
    {
        failed++;
        Console.WriteLine($"  INVALID: {ex.Message}");
    }
}

Console.WriteLine($"Session over: {ok} succeeded, {failed} failed.");
Console.WriteLine("Audit log:");
log.Write("SESSION END");
foreach (string entry in File.ReadLines(logPath, System.Text.Encoding.UTF8).ToList())
    Console.WriteLine($"  {entry[21..]}");                   // skip the timestamp for a tidy printout

static string? ReadCommand()
{
    Console.Write("> ");
    string? input = Console.ReadLine()?.Trim().ToLowerInvariant();
    if (input is not null) Console.WriteLine(input);
    return input;
}

static string Arg(string[] parts, int index) =>
    index < parts.Length ? parts[index].ToUpperInvariant() : throw new FormatException("Missing account ID.");

static decimal Amount(string[] parts, int index)
{
    if (index >= parts.Length) throw new FormatException("Missing amount.");
    if (!decimal.TryParse(parts[index], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal amount))
        throw new FormatException($"'{parts[index]}' is not an amount.");
    return amount;
}
