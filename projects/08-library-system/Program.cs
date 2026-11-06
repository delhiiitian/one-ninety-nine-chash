// Project 8: Library Management System
// Uses Chapter 8 ideas: classes, properties, inheritance and polymorphism, interfaces,
// generics, LINQ, async/await, events and extension methods.
// Run: dotnet run --project projects/08-library-system
//
// The program simulates 30 days at a library with a fake clock, so the output is the same every run.
using LibrarySystem.Domain;
using LibrarySystem.Infrastructure;
using LibrarySystem.Services;

var clock = new DateOnly(2026, 11, 2);
var items = new InMemoryRepository<LibraryItem>();
var members = new InMemoryRepository<Member>();
var notifier = new ConsoleEmailNotifier();
var library = new LibraryService(items, members, () => clock);

SeedData();

// Events: when an item is overdue, send an email. Async work is collected and awaited together.
var pendingEmails = new List<Task>();
library.ItemOverdue += (_, e) => pendingEmails.Add(notifier.SendAsync(
    e.Member.Email,
    $"Overdue: {e.Item.Title}",
    $"{e.DaysLate.Pluralize("day")} late, fee so far {e.Item.LateFee(e.DaysLate):N2}"));

GoToDay(0);
Borrow("M1", "B1");
Borrow("M1", "D1");
Borrow("M2", "B1");      // already on loan
Borrow("M2", "MG1");
Borrow("M3", "B2");
Borrow("M3", "B3");
Borrow("M3", "D2");
Borrow("M3", "B4");      // standard limit is 3

GoToDay(5);
Try(() => Console.WriteLine($"  MG1 returned, fee {library.Return("MG1"):N2}"));
await CheckOverdueAsync();

GoToDay(10);
Try(() => Console.WriteLine($"  D1 returned, fee {library.Return("D1"):N2}"));
await CheckOverdueAsync();

GoToDay(11);
Borrow("M1", "B4");      // Kabir owes a late fee, so this is refused

GoToDay(12);
members.Find("M1")!.Pay(100m);
Console.WriteLine("  Kabir paid 100.00");
members.Find("M2")!.Pay(4m);
Console.WriteLine("  Lata paid 4.00");
Borrow("M1", "B4");
Borrow("M2", "D1");

GoToDay(25);
Try(() => Console.WriteLine($"  B1 returned, fee {library.Return("B1"):N2}"));
await CheckOverdueAsync();

Console.WriteLine();
Console.WriteLine("=== Reports ===");
Console.WriteLine("Stock by kind:");
foreach (var (kind, count, onLoan) in library.StockByKind())
    Console.WriteLine($"  {kind,-9} {count} total, {onLoan} on loan");
Console.WriteLine("Most borrowed:");
foreach (var (title, times) in library.MostBorrowed(3))
    Console.WriteLine($"  {title,-28} {times.Pluralize("time")}");
Console.WriteLine("Members:");
foreach (var (member, open) in library.MembersWithLoans())
    Console.WriteLine($"  {member.Name,-7} ({member.Badge()}) {open.Pluralize("open loan")}, owes {member.FeesOwed:N2}");
Console.WriteLine($"Available now: {string.Join(", ", library.Available().Select(i => i.Title))}");
Console.WriteLine($"Emails sent: {notifier.Sent}");

// ---------------- helpers ----------------

void SeedData()
{
    items.Add(new Book("B1", "The Pragmatic Programmer", 1999, "Hunt & Thomas", 352));
    items.Add(new Book("B2", "Clean Code", 2008, "Robert C. Martin", 464));
    items.Add(new Book("B3", "Head First C#", 2024, "Stellman & Greene", 840));
    items.Add(new Book("B4", "Atomic Habits", 2018, "James Clear", 320));
    items.Add(new Dvd("D1", "Interstellar", 2014, 169));
    items.Add(new Dvd("D2", "Coco", 2017, 105));
    items.Add(new Magazine("MG1", "Wired", 2026, 11));
    members.Add(new Member("M1", "Kabir", "kabir@example.com"));
    members.Add(new Member("M2", "Lata", "lata@example.com", isPremium: true));
    members.Add(new Member("M3", "Mohan", "mohan@example.com"));
}

void GoToDay(int offset)
{
    clock = new DateOnly(2026, 11, 2).AddDays(offset);
    Console.WriteLine($"--- Day {offset} ({clock.ShortDate()}) ---");
}

async Task CheckOverdueAsync()
{
    int n = library.CheckOverdue();
    Console.WriteLine($"  Overdue check: {n.Pluralize("item")}");
    await Task.WhenAll(pendingEmails);
    pendingEmails.Clear();
}

void Borrow(string memberId, string itemId) => Try(() =>
{
    Loan loan = library.Borrow(memberId, itemId);
    Console.WriteLine($"  {members.Find(memberId)!.Name} borrowed {items.Find(itemId)}, due {loan.DueOn.ShortDate()}");
});

void Try(Action action)
{
    try
    {
        action();
    }
    catch (Exception ex) when (ex is InvalidOperationException or KeyNotFoundException)
    {
        Console.WriteLine($"  Refused: {ex.Message}");
    }
}
