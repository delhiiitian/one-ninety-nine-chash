// 4.8 Records and structs: your own data types
// Run: dotnet run chapter-04-collections/08-records-structs.cs

// A record is a one-line class for data. It gets equality, printing and copying for free.
var book1 = new Book("Clean Code", "Robert C. Martin", 2008, 450m);
var book2 = new Book("Clean Code", "Robert C. Martin", 2008, 450m);
Console.WriteLine(book1);                                      // readable ToString
Console.WriteLine($"Same book? {book1 == book2}");             // compares values, not identity

// Records are immutable by default: make a changed copy with "with".
var discounted = book1 with { Price = 360m };
Console.WriteLine($"Was {book1.Price}, now {discounted.Price}");

// Collections of records make tidy, typed data.
var shelf = new List<Book>
{
    book1,
    new("The Pragmatic Programmer", "Hunt and Thomas", 1999, 520m),
    new("C# in Depth", "Jon Skeet", 2019, 610m),
};
foreach (Book b in shelf)
    Console.WriteLine($"{b.Title,-26} {b.Year}  {b.Price,6:N0}");

// A struct is a value type: copied on assignment. Good for small values like points or money.
var start = new Point(2, 3);
var moved = start;            // a full copy
moved.X = 10;
Console.WriteLine($"start = {start}, moved = {moved}");

// A record struct combines both: value semantics plus record conveniences.
var price = new Money(99.99m, "USD");
Console.WriteLine($"{price} doubled is {price.Times(2)}");

// Declarations go after the top-level code.
record Book(string Title, string Author, int Year, decimal Price);

struct Point
{
    public int X;
    public int Y;
    public Point(int x, int y) { X = x; Y = y; }
    public override string ToString() => $"({X}, {Y})";
}

readonly record struct Money(decimal Amount, string Currency)
{
    public Money Times(int n) => this with { Amount = Amount * n };
    public override string ToString() => $"{Amount:F2} {Currency}";
}
