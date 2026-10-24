// Project 1: Restaurant Bill Splitter
// Uses only Chapter 1 ideas: variables, numbers, strings, parsing, console input and output.
// Run: dotnet run --project projects/01-bill-splitter
using System.Globalization;

// ---------------------------------------------------------------
// 1. Settings: values that never change while the program runs
// ---------------------------------------------------------------
const string AppTitle = "Bill Splitter";
const decimal DefaultTipPercent = 10m;
const decimal TaxPercent = 5m;
const int ReceiptWidth = 34;
CultureInfo money = CultureInfo.GetCultureInfo("en-US");

// ---------------------------------------------------------------
// 2. Input: ask the user, falling back to safe defaults
// ---------------------------------------------------------------
Console.WriteLine(new string('=', ReceiptWidth));
Console.WriteLine(AppTitle.PadLeft((ReceiptWidth + AppTitle.Length) / 2));
Console.WriteLine(new string('=', ReceiptWidth));

Console.Write("Restaurant name: ");
string restaurant = Console.ReadLine()?.Trim() ?? "";
if (restaurant.Length == 0) restaurant = "Our dinner";

Console.Write("Bill amount before tax (e.g. 84.50): ");
bool billOk = decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal bill);
if (!billOk || bill < 0) bill = 0m;

Console.Write($"Tip percent [{DefaultTipPercent}]: ");
bool tipOk = decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal tipPercent);
if (!tipOk || tipPercent < 0) tipPercent = DefaultTipPercent;

Console.Write("Number of people [1]: ");
bool peopleOk = int.TryParse(Console.ReadLine(), out int people);
if (!peopleOk || people < 1) people = 1;

// ---------------------------------------------------------------
// 3. Calculation: decimal keeps every cent exact
// ---------------------------------------------------------------
decimal tax = Math.Round(bill * TaxPercent / 100m, 2);
decimal tip = Math.Round(bill * tipPercent / 100m, 2);
decimal total = bill + tax + tip;
decimal perPerson = Math.Round(total / people, 2, MidpointRounding.AwayFromZero);
// Rounding each share can leave a few cents over or short; the first person absorbs it.
decimal firstPersonPays = total - perPerson * (people - 1);

// ---------------------------------------------------------------
// 4. Output: a receipt aligned with interpolation widths
// ---------------------------------------------------------------
string Line(string label, decimal amount) =>
    $"{label,-20}{amount.ToString("C", money),14}";

Console.WriteLine();
Console.WriteLine(new string('-', ReceiptWidth));
Console.WriteLine(restaurant.ToUpper());
Console.WriteLine(new string('-', ReceiptWidth));
Console.WriteLine(Line("Food and drinks", bill));
Console.WriteLine(Line($"Tax ({TaxPercent}%)", tax));
Console.WriteLine(Line($"Tip ({tipPercent}%)", tip));
Console.WriteLine(new string('-', ReceiptWidth));
Console.WriteLine(Line("TOTAL", total));
Console.WriteLine(Line($"Split {people} way(s)", perPerson));
if (firstPersonPays != perPerson)
{
    Console.WriteLine($"(Person 1 pays {firstPersonPays.ToString("C", money)} to cover rounding)");
}
Console.WriteLine(new string('=', ReceiptWidth));
