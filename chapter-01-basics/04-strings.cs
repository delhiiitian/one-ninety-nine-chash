// 1.4 Strings and string interpolation
// Run: dotnet run chapter-01-basics/04-strings.cs

string first = "Ravi";
string last = "Kumar";

// Joining strings: + works, interpolation ($"...") reads better.
string full = first + " " + last;
string greeting = $"Hello, {first}! Your name has {full.Length} characters.";
Console.WriteLine(full);
Console.WriteLine(greeting);

// Common string methods. Strings never change; methods return new strings.
string email = "  Ravi.Kumar@Example.com ";
string clean = email.Trim().ToLower();
Console.WriteLine($"Cleaned email: '{clean}'");
Console.WriteLine($"Contains '@'? {clean.Contains('@')}");
Console.WriteLine($"Domain: {clean.Substring(clean.IndexOf('@') + 1)}");
Console.WriteLine($"Starts with 'ravi'? {clean.StartsWith("ravi")}");
Console.WriteLine($"Replace dot: {clean.Replace(".", "[dot]")}");

// Split and Join: turn a line of text into parts and back.
string tags = "csharp,dotnet,beginner";
string[] parts = tags.Split(',');
Console.WriteLine($"Tag count: {parts.Length}, first tag: {parts[0]}");
Console.WriteLine($"As hashtags: #{string.Join(" #", parts)}");

// Escapes, verbatim strings and raw strings.
Console.WriteLine("Line one\nLine two\tafter a tab");
string path = @"C:\Users\Ravi\Documents";
Console.WriteLine(path);
string json = """
    { "name": "Ravi", "role": "admin" }
    """;
Console.WriteLine(json);

// Alignment inside interpolation builds neat receipts.
Console.WriteLine($"{"Item",-10}{"Price",8}");
Console.WriteLine($"{"Coffee",-10}{3.5,8:F2}");
Console.WriteLine($"{"Muffin",-10}{2.25,8:F2}");
