// 3.8 Lambdas, Func and Action
// Run: dotnet run chapter-03-methods/08-lambdas.cs

// A lambda is a method without a name, written inline: (input) => result
Func<int, int> square = x => x * x;
Func<decimal, decimal, decimal> applyDiscount = (price, percent) => price - price * percent / 100m;
Action<string> shout = message => Console.WriteLine(message.ToUpper() + "!");
Predicate<int> isAdult = age => age >= 18;

Console.WriteLine($"Square of 9: {square(9)}");
Console.WriteLine($"2000 with 15% off: {applyDiscount(2000m, 15m)}");
shout("sale starts now");
Console.WriteLine($"Is 16 an adult? {isAdult(16)}");

// The power of lambdas: pass behaviour into another method.
int[] scores = { 45, 78, 92, 61, 88, 33 };
Console.WriteLine($"Passed: {string.Join(", ", Filter(scores, s => s >= 60))}");
Console.WriteLine($"Top marks: {string.Join(", ", Filter(scores, s => s >= 90))}");

// Built-in methods take lambdas too.
Array.Sort(scores, (a, b) => b.CompareTo(a));     // sort descending
Console.WriteLine($"Ranked: {string.Join(", ", scores)}");
Console.WriteLine($"Any perfect score? {Array.Exists(scores, s => s == 100)}");

// Lambdas can capture outside variables: a simple counter factory.
Func<int> MakeCounter()
{
    int count = 0;
    return () => ++count;
}
var ticketNumber = MakeCounter();
Console.WriteLine($"Tickets: {ticketNumber()}, {ticketNumber()}, {ticketNumber()}");

// Run a list of steps stored as Actions.
var pipeline = new List<Action>
{
    () => Console.WriteLine("1. Validate order"),
    () => Console.WriteLine("2. Charge card"),
    () => Console.WriteLine("3. Send receipt"),
};
foreach (Action step in pipeline) step();

static List<int> Filter(int[] values, Func<int, bool> keep)
{
    var result = new List<int>();
    foreach (int v in values)
        if (keep(v)) result.Add(v);
    return result;
}
