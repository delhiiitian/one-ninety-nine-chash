// Project 3: Unit Converter
// Uses Chapter 3 ideas: methods, parameters and return values, optional/named arguments,
// out parameters, overloading, expression-bodied members, lambdas (Func) and local functions.
// Run: dotnet run --project projects/03-unit-converter

Console.WriteLine("=== Unit Converter ===");

while (true)
{
    ShowMenu(Conversions.Categories);
    int category = ConsoleInput.ReadChoice("Pick a category", max: Conversions.Categories.Length);
    if (category == 0) break;                                   // 0 or end of input = quit

    var conversions = Conversions.ForCategory(category - 1);
    ShowMenu(conversions.Select(c => c.Label).ToArray());
    int choice = ConsoleInput.ReadChoice("Pick a conversion", max: conversions.Length);
    if (choice == 0) continue;                                  // back to categories

    var (label, convert, unit) = conversions[choice - 1];
    if (!ConsoleInput.TryReadNumber("Value", out double value)) break;

    double result = convert(value);
    Console.WriteLine($"  {label}: {Format(value)} -> {Format(result)} {unit}");
    Console.WriteLine();
}

Console.WriteLine("Goodbye!");

// Local functions: helpers used only by this file.
static void ShowMenu(string[] items)
{
    for (int i = 0; i < items.Length; i++)
        Console.WriteLine($"  {i + 1}. {items[i]}");
    Console.WriteLine("  0. Back / quit");
}

static string Format(double number, int decimals = 2) => Math.Round(number, decimals).ToString("0.##");
