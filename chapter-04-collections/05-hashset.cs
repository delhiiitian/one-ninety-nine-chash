// 4.5 HashSet<T>: unique items and set maths
// Run: dotnet run chapter-04-collections/05-hashset.cs

// A HashSet keeps each value only once and answers "is it in here?" instantly.
var visitors = new HashSet<string>();
string[] signIns = { "asha", "ben", "asha", "chitra", "ben", "asha" };
foreach (string user in signIns)
{
    bool isNew = visitors.Add(user);       // Add returns false if it was already there
    Console.WriteLine($"{user,-7} {(isNew ? "first visit" : "returning")}");
}
Console.WriteLine($"{signIns.Length} sign-ins, {visitors.Count} unique visitors");

// Removing duplicates from a list in one line.
int[] lotteryPicks = { 7, 3, 7, 12, 3, 45, 12 };
var unique = new HashSet<int>(lotteryPicks);
Console.WriteLine($"Unique picks: {string.Join(", ", unique)}");

// Set operations: compare two groups.
var likesPython = new HashSet<string> { "asha", "ben", "dev", "eli" };
var likesCSharp = new HashSet<string> { "ben", "chitra", "eli", "farah" };

var both = new HashSet<string>(likesPython);
both.IntersectWith(likesCSharp);
var either = new HashSet<string>(likesPython);
either.UnionWith(likesCSharp);
var onlyPython = new HashSet<string>(likesPython);
onlyPython.ExceptWith(likesCSharp);

Console.WriteLine($"Like both:        {string.Join(", ", both)}");
Console.WriteLine($"Like either:      {string.Join(", ", either)}");
Console.WriteLine($"Only Python:      {string.Join(", ", onlyPython)}");

// Case-insensitive sets: pass a comparer.
var blockedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "spam", "scam" };
Console.WriteLine($"Is 'SPAM' blocked? {blockedWords.Contains("SPAM")}");
