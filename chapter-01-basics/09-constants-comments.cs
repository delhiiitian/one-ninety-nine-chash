// 1.9 Constants, comments and naming
// Run: dotnet run chapter-01-basics/09-constants-comments.cs

// const: a value fixed when you compile. It can never change.
const double GstRate = 0.18;
const int MaxLoginAttempts = 3;
const string AppName = "ShopEasy";

// Single-line comments start with //.
/* Multi-line comments
   are useful for longer notes. */

/// <summary>XML doc comments (three slashes) describe methods and classes for tools.</summary>
static decimal AddTax(decimal amount) => amount + amount * (decimal)GstRate;

Console.WriteLine($"{AppName} allows {MaxLoginAttempts} login attempts.");
Console.WriteLine($"Rs. 500 with GST = {AddTax(500m)}");

// readonly is for values set once at runtime (on classes), and
// naming conventions keep code readable:
//   camelCase  -> local variables and parameters (orderTotal)
//   PascalCase -> methods, classes, properties, constants (AddTax, GstRate)
//   _camelCase -> private fields (_balance)
//   Interfaces start with I (IShape)
int orderCount = 7;               // good: says what it holds
// int x = 7;                     // bad: says nothing
Console.WriteLine($"Orders today: {orderCount}");

// GstRate = 0.2;  // error CS0131: a const cannot be changed
