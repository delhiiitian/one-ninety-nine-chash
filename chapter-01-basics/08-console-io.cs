// 1.8 Reading input from the console
// Run: dotnet run chapter-01-basics/08-console-io.cs
// Try it with piped input too: echo -e "Sam\n31" | dotnet run chapter-01-basics/08-console-io.cs

Console.Write("What is your name? ");
string? name = Console.ReadLine();

// ReadLine returns null when there is no more input, so give a default.
if (string.IsNullOrWhiteSpace(name))
{
    name = "friend";
}

Console.Write("How old are you? ");
string? ageText = Console.ReadLine();

if (int.TryParse(ageText, out int age))
{
    int yearsTo100 = 100 - age;
    Console.WriteLine($"Nice to meet you, {name}! You will be 100 in {yearsTo100} years.");
}
else
{
    Console.WriteLine($"Nice to meet you, {name}! (I could not read '{ageText}' as an age.)");
}

// Colour makes important lines stand out in real command-line tools.
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Profile saved.");
Console.ResetColor();
