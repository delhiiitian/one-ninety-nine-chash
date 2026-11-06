// 5.2 try and catch
// Run: dotnet run chapter-05-error-handling/02-try-catch.cs

// Put risky code in try. If it throws, control jumps to the matching catch,
// and the program carries on instead of crashing.
string[] amounts = { "250", "abc", "1000", "", "75.5" };
decimal total = 0;

foreach (string text in amounts)
{
    try
    {
        decimal amount = decimal.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
        total += amount;
        Console.WriteLine($"Added {amount}");
    }
    catch (FormatException)
    {
        Console.WriteLine($"Skipped '{text}': not a number");
    }
}
Console.WriteLine($"Total of valid amounts: {total}");

// Catch specific exceptions where you can do something useful about them.
int[] seatsPerRow = { 10, 12, 12, 8 };
int row = 6;
try
{
    Console.WriteLine($"Row {row} has {seatsPerRow[row]} seats");
}
catch (IndexOutOfRangeException ex)
{
    Console.WriteLine($"No row {row}. ({ex.Message})");
}

// Code after a throw inside try is skipped.
try
{
    Console.WriteLine("Step 1: connect");
    throw new TimeoutException("Server did not answer in 5 s");
#pragma warning disable CS0162 // unreachable code, shown on purpose
    Console.WriteLine("Step 2: never printed");
#pragma warning restore CS0162
}
catch (TimeoutException ex)
{
    Console.WriteLine($"Failed: {ex.Message}. Showing cached data instead.");
}
Console.WriteLine("The program is still running.");
