// 2.7 The foreach loop
// Run: dotnet run chapter-02-control-flow/07-foreach.cs

// foreach visits every item in a collection, with no index to manage.
string[] playlist = { "Intro", "Sunrise", "City Lights", "Rain", "Outro" };
foreach (string song in playlist)
{
    Console.WriteLine($"Now playing: {song}");
}

// Totals and maximums are the classic foreach jobs.
decimal[] expenses = { 340m, 1200m, 89.50m, 560m, 45m };
decimal sum = 0m;
decimal biggest = decimal.MinValue;
foreach (decimal expense in expenses)
{
    sum += expense;
    if (expense > biggest) biggest = expense;
}
Console.WriteLine($"Spent {sum} in {expenses.Length} purchases; biggest was {biggest}.");

// foreach works on strings (each char) and on lists and dictionaries too.
int vowels = 0;
foreach (char c in "Programming in CSharp")
{
    if ("aeiouAEIOU".Contains(c)) vowels++;
}
Console.WriteLine($"Vowels: {vowels}");

var stock = new Dictionary<string, int> { ["apples"] = 12, ["bananas"] = 0, ["cherries"] = 40 };
foreach (var (fruit, quantity) in stock)
{
    string status = quantity == 0 ? "OUT OF STOCK" : $"{quantity} left";
    Console.WriteLine($"{fruit,-9} {status}");
}

// Need the position as well? Use a for loop, or Index() from .NET 9+.
foreach (var (index, song) in playlist.Index())
{
    Console.WriteLine($"{index + 1}. {song}");
}
