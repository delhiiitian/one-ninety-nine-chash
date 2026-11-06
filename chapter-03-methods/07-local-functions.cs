// 3.7 Expression-bodied members and local functions
// Run: dotnet run chapter-03-methods/07-local-functions.cs

// Expression-bodied methods: a one-line method uses => instead of braces and return.
static double CelsiusToFahrenheit(double c) => c * 9 / 5 + 32;
static bool IsEven(int n) => n % 2 == 0;

Console.WriteLine($"30 C = {CelsiusToFahrenheit(30)} F");
Console.WriteLine($"Is 42 even? {IsEven(42)}");

// Local functions live inside another method and are only visible there.
// They keep helper code next to the only place that uses it.
string report = BuildInvoice(new[] { ("Keyboard", 1, 1499m), ("Mouse", 2, 499m), ("Cable", 3, 199m) });
Console.WriteLine(report);

static string BuildInvoice((string Name, int Qty, decimal Price)[] lines)
{
    var text = new System.Text.StringBuilder();
    decimal grandTotal = 0m;

    foreach (var line in lines)
    {
        decimal lineTotal = line.Qty * line.Price;
        grandTotal += lineTotal;
        text.AppendLine(Row(line.Name, $"{line.Qty} x {line.Price}", lineTotal));
    }
    text.AppendLine(new string('-', 40));
    text.Append(Row("TOTAL", "", grandTotal));
    return text.ToString();

    // Local function: can read the outer method's variables if it is not static.
    static string Row(string name, string detail, decimal amount) =>
        $"{name,-12}{detail,-14}{amount,14:N2}";
}

// A local function can also capture variables from the enclosing code.
int callCount = 0;
int Next() => ++callCount;
Next(); Next();
Console.WriteLine($"Called {Next()} times");
