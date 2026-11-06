// 3.6 params: any number of arguments
// Run: dotnet run chapter-03-methods/06-params.cs

// params lets callers pass a list of values without building an array themselves.
Console.WriteLine($"Total: {Sum(10, 20)}");
Console.WriteLine($"Total: {Sum(5, 15, 25, 35, 45)}");
Console.WriteLine($"Total: {Sum()}");                  // zero arguments is fine
int[] existing = { 1, 2, 3 };
Console.WriteLine($"Total: {Sum(existing)}");          // an array works too

// Real use: building a command or a message from parts.
Console.WriteLine(BuildPath("home", "asha", "photos", "2026"));
LogEvent("login", "user=asha", "ip=10.0.0.5", "device=phone");

// Since C# 13, params can be any collection type, such as a List or span.
Console.WriteLine($"Longest word: {Longest("kiwi", "banana", "fig", "pineapple")}");

static int Sum(params int[] numbers)
{
    int total = 0;
    foreach (int n in numbers) total += n;
    return total;
}

static string BuildPath(params string[] parts) => "/" + string.Join('/', parts);

// The params parameter must be the last one.
static void LogEvent(string name, params string[] details)
{
    Console.WriteLine($"[{name}] {string.Join(", ", details)}");
}

static string Longest(params List<string> words)
{
    string best = "";
    foreach (string w in words)
        if (w.Length > best.Length) best = w;
    return best;
}
