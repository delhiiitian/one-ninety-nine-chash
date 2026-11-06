// 3.9 Recursion
// Run: dotnet run chapter-03-methods/09-recursion.cs

// A recursive method calls itself on a smaller problem until it reaches a base case.
Console.WriteLine($"5! = {Factorial(5)}");
Console.WriteLine($"Fibonacci(10) = {Fibonacci(10)}");
Console.WriteLine($"Sum of digits of 98765 = {DigitSum(98765)}");

// Recursion shines on tree shapes, like folders inside folders.
var root = new Folder("Documents", 120, new[]
{
    new Folder("Taxes", 300, new[] { new Folder("2025", 450, []), new Folder("2026", 510, []) }),
    new Folder("Photos", 2000, new[] { new Folder("Trip", 3500, []) }),
    new Folder("Notes", 15, []),
});
PrintTree(root, 0);
Console.WriteLine($"Total size: {TotalSize(root)} KB");

// Base case first, then the recursive step.
static long Factorial(int n) => n <= 1 ? 1 : n * Factorial(n - 1);

static int DigitSum(int n) => n < 10 ? n : n % 10 + DigitSum(n / 10);

// Naive Fibonacci repeats work; memoization (a cache) fixes that.
static long Fibonacci(int n, Dictionary<int, long>? memo = null)
{
    memo ??= new Dictionary<int, long>();
    if (n <= 1) return n;
    if (memo.TryGetValue(n, out long cached)) return cached;
    long value = Fibonacci(n - 1, memo) + Fibonacci(n - 2, memo);
    memo[n] = value;
    return value;
}

static void PrintTree(Folder folder, int depth)
{
    Console.WriteLine($"{new string(' ', depth * 2)}{folder.Name}/ ({folder.SizeKb} KB)");
    foreach (Folder child in folder.Children)
        PrintTree(child, depth + 1);
}

static int TotalSize(Folder folder)
{
    int total = folder.SizeKb;
    foreach (Folder child in folder.Children)
        total += TotalSize(child);
    return total;
}

// A record is a compact data type (see Chapter 4).
record Folder(string Name, int SizeKb, Folder[] Children);
