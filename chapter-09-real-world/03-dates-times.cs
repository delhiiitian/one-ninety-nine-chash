// 9.3 Dates, times and time zones
// Run: dotnet run chapter-09-real-world/03-dates-times.cs
using System.Globalization;

var culture = CultureInfo.GetCultureInfo("en-US");

// 1. A fixed moment so the output is the same every run (use DateTimeOffset.UtcNow in real code).
var orderPlaced = new DateTimeOffset(2026, 11, 6, 9, 30, 0, TimeSpan.Zero); // 09:30 UTC
Console.WriteLine($"Order placed (UTC): {orderPlaced:yyyy-MM-dd HH:mm zzz}");

// 2. Arithmetic with TimeSpan and Add methods.
DateTimeOffset delivery = orderPlaced.AddDays(3).AddHours(4);
TimeSpan wait = delivery - orderPlaced;
Console.WriteLine($"Delivery: {delivery:ddd dd MMM, HH:mm} UTC ({wait.TotalHours} hours later)");

// 3. Convert one moment to several time zones (IANA ids work on Linux, macOS and Windows on .NET 6+).
string[] zones = ["Asia/Kolkata", "Europe/London", "America/New_York", "Asia/Tokyo"];
foreach (string id in zones)
{
    TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(id);
    DateTimeOffset local = TimeZoneInfo.ConvertTime(orderPlaced, tz);
    Console.WriteLine($"  {id,-17} {local.ToString("dd MMM HH:mm", culture)} (UTC{local:zzz})");
}

// 4. DateOnly and TimeOnly for values that are only a date or only a time.
var birthday = new DateOnly(1995, 2, 14);
var today = new DateOnly(2026, 11, 6);
int age = today.Year - birthday.Year - (today < birthday.AddYears(today.Year - birthday.Year) ? 1 : 0);
Console.WriteLine($"Born {birthday.ToString("d MMMM yyyy", culture)}, age {age}");
var shopOpens = new TimeOnly(9, 0);
var shopCloses = new TimeOnly(21, 0);
var now = new TimeOnly(22, 15);
Console.WriteLine($"Shop open at {now}? {now.IsBetween(shopOpens, shopCloses)}");

// 5. Parsing text safely with an exact format.
if (DateTime.TryParseExact("06/11/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
    Console.WriteLine($"Parsed Indian-style date: {parsed:yyyy-MM-dd} ({parsed.DayOfWeek})");

// 6. ISO 8601 round-trip format for storage and APIs.
string iso = orderPlaced.ToString("O");
Console.WriteLine($"ISO 8601: {iso}");
Console.WriteLine($"Back again: {DateTimeOffset.Parse(iso, CultureInfo.InvariantCulture) == orderPlaced}");

// 7. Business days: skip weekends when counting a due date.
DateOnly due = AddBusinessDays(today, 5);
Console.WriteLine($"5 business days after {today:ddd dd MMM}: {due:ddd dd MMM}");

static DateOnly AddBusinessDays(DateOnly start, int days)
{
    DateOnly d = start;
    while (days > 0)
    {
        d = d.AddDays(1);
        if (d.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday)) days--;
    }
    return d;
}
