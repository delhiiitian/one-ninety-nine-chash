// 3.1 Defining and calling methods
// Run: dotnet run chapter-03-methods/01-defining-methods.cs

// Call methods by name. With top-level statements, methods can be declared below the code that uses them.
PrintHeader("Morning report");
PrintLine();
Console.WriteLine("All systems running.");
PrintLine();

PrintHeader("Evening report");
PrintLine();
Console.WriteLine("2 warnings, 0 errors.");
PrintLine();

// A method is a named, reusable block of code.
// static: belongs to the program, not to an object (more in Chapter 8)
// void:   it does not hand back a value
static void PrintLine()
{
    Console.WriteLine(new string('-', 30));
}

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(title.ToUpper());
}
