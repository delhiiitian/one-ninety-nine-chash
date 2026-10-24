// 2.3 Switch expressions and pattern matching
// Run: dotnet run chapter-02-control-flow/03-switch-expressions.cs

// A switch expression returns a value. Arms are tried top to bottom.
static string Grade(int score) => score switch
{
    >= 90 => "A",
    >= 75 => "B",
    >= 60 => "C",
    >= 0 and < 60 => "Fail",
    _ => "Invalid score"            // _ is the discard: matches anything else
};

foreach (int s in new[] { 95, 81, 64, 30, -5 })
    Console.WriteLine($"Score {s,3} -> {Grade(s)}");

// Patterns on tuples: decide traffic-light behaviour from two inputs.
static string Action(string light, bool pedestrian) => (light, pedestrian) switch
{
    ("red", _) => "Stop",
    ("yellow", true) => "Stop, someone is crossing",
    ("yellow", false) => "Slow down",
    ("green", true) => "Wait for the pedestrian",
    ("green", false) => "Go",
    _ => "Light broken: treat as stop sign"
};

Console.WriteLine(Action("green", false));
Console.WriteLine(Action("yellow", true));
Console.WriteLine(Action("blue", false));

// Type patterns: act on what kind of value you have.
object[] things = { 42, "hello", 3.14, true, null! };
foreach (object thing in things)
{
    string description = thing switch
    {
        int n when n > 40 => $"a big int ({n})",
        int n => $"an int ({n})",
        string text => $"a string of length {text.Length}",
        double d => $"a double ({d})",
        null => "nothing (null)",
        _ => $"something else: {thing}"
    };
    Console.WriteLine(description);
}

// The is operator with patterns, often used in if statements.
int age = 15;
if (age is >= 13 and <= 19)
    Console.WriteLine($"{age} is a teenager");
