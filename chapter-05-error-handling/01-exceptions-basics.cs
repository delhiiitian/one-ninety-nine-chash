// 5.1 What exceptions are
// Run: dotnet run chapter-05-error-handling/01-exceptions-basics.cs

// An exception is an object that says "something went wrong here". If nobody catches it,
// the program stops and prints it. This file triggers a few common ones on purpose and
// catches them, so you can see their names and messages.

Demo("Divide by zero", () => { int a = 10, b = 0; Console.WriteLine(a / b); });
Demo("Bad number format", () => int.Parse("twelve"));
Demo("Index out of range", () => { int[] scores = { 90, 80 }; Console.WriteLine(scores[5]); });
Demo("Missing dictionary key", () => new Dictionary<string, int>()["unknown"].ToString());
Demo("Null reference", () => { string? name = null; Console.WriteLine(name!.Length); });
Demo("Invalid cast", () => { object o = "hello"; int n = (int)o; });
Demo("File not found", () => File.ReadAllText("does-not-exist.txt"));

// Exceptions carry useful information.
try
{
    int.Parse("abc");
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine($"Type:    {ex.GetType().FullName}");
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Where:   {ex.StackTrace?.Split('\n')[0].Trim()}");
}

static void Demo(string title, Action risky)
{
    try
    {
        risky();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{title,-24} -> {ex.GetType().Name}");
    }
}
