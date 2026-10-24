// 1.1 Hello, World and top-level programs
// Run: dotnet run chapter-01-basics/01-hello-world.cs
//
// In modern C# a program can start with plain statements ("top-level statements").
// The compiler wraps them in a hidden Main method for you.

Console.WriteLine("Hello, World!");
Console.WriteLine("Welcome to C# in 199 Pages.");

// Write prints without moving to a new line; WriteLine adds a line break.
Console.Write("Loading");
Console.Write(".");
Console.Write(".");
Console.WriteLine(".");

// A real program usually prints a short banner and the runtime it is on.
Console.WriteLine($"Running on .NET {Environment.Version}");
Console.WriteLine($"Today is {DateTime.Now:dddd}");
