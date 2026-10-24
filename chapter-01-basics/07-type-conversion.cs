// 1.7 Type conversion and parsing
// Run: dotnet run chapter-01-basics/07-type-conversion.cs

// Implicit conversion: small to big is safe, so C# does it for you.
int wholeKm = 42;
double asDouble = wholeKm;
Console.WriteLine($"int {wholeKm} -> double {asDouble}");

// Explicit cast: big to small can lose data, so you must ask for it.
double temperature = 36.9;
int truncated = (int)temperature;
Console.WriteLine($"double {temperature} -> int {truncated} (fraction dropped)");
Console.WriteLine($"Math.Round first: {(int)Math.Round(temperature)}");

// Text to number: Parse throws on bad input, TryParse returns false instead.
string ageText = "27";
int age = int.Parse(ageText);
Console.WriteLine($"Parsed age + 1 = {age + 1}");

string[] inputs = { "120", "12.5", "abc", "" };
foreach (string input in inputs)
{
    if (int.TryParse(input, out int value))
        Console.WriteLine($"'{input}' is a valid whole number: {value}");
    else
        Console.WriteLine($"'{input}' is not a whole number");
}

// Decimal parsing with an invariant culture (dot as decimal separator).
var culture = System.Globalization.CultureInfo.InvariantCulture;
decimal price = decimal.Parse("19.99", culture);
Console.WriteLine($"Price with tax: {price * 1.18m:F2}");

// Number to text, and the Convert class.
int pin = 0042;
Console.WriteLine($"PIN as 4 digits: {pin.ToString("D4")}");
Console.WriteLine($"Convert.ToInt32(\"99\") = {Convert.ToInt32("99")}");
Console.WriteLine($"Convert.ToBoolean(\"true\") = {Convert.ToBoolean("true")}");
