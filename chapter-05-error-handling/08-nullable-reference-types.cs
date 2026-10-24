// 5.8 Nullable reference types: catching null bugs at compile time
// Run: dotnet run chapter-05-error-handling/08-nullable-reference-types.cs

// With nullable checks on (the default in new projects), the compiler tracks which
// variables may be null and warns before you can crash with NullReferenceException.

string name = "Kiran";          // never null
string? nickname = null;        // may be null: the ? says so

Console.WriteLine(name.Length);
// Console.WriteLine(nickname.Length);   // warning CS8602: possible null dereference

// Option 1: check first. After the check the compiler knows it is not null.
if (nickname is not null)
    Console.WriteLine(nickname.Length);

// Option 2: null-safe operators from Chapter 2.
Console.WriteLine($"Nickname length: {nickname?.Length ?? 0}");

// Methods say in their signatures whether they can return null.
User? found = FindUser(42);
Console.WriteLine(found is null ? "User 42 not found" : $"Found {found.Name}");

User me = FindUser(1) ?? throw new InvalidOperationException("Admin user missing");
Console.WriteLine($"Admin: {me.Name}, email: {me.Email ?? "(none)"}");

// Guard against null arguments at the edges of your code.
try
{
    Greet(null!);   // ! tells the compiler "trust me"; here it is a lie on purpose
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Guard worked: {ex.ParamName} was null");
}

static User? FindUser(int id) => id == 1 ? new User("Admin", null) : null;

static void Greet(string person)
{
    ArgumentNullException.ThrowIfNull(person);
    Console.WriteLine($"Hello {person}");
}

record User(string Name, string? Email);
