// 1.3 Numbers: int, long, double and decimal
// Run: dotnet run chapter-01-basics/03-numbers.cs

int stepsToday = 8_532;              // whole numbers, about +/- 2.1 billion
long worldPopulation = 8_100_000_000; // bigger whole numbers
double distanceKm = 6.43;            // fast floating point, small rounding errors
decimal accountBalance = 1250.75m;   // exact base-10 numbers for money (note the m)

Console.WriteLine($"Steps: {stepsToday}, population: {worldPopulation}");
Console.WriteLine($"Distance: {distanceKm} km, balance: {accountBalance}");

// Integer division throws away the remainder.
int pizzaSlices = 10, friends = 3;
Console.WriteLine($"Each friend gets {pizzaSlices / friends} slices, {pizzaSlices % friends} left over");

// Make one side a double to keep the fraction.
Console.WriteLine($"Exact share: {(double)pizzaSlices / friends:F2} slices");

// Why money uses decimal: double cannot store 0.1 exactly.
double d = 0.1 + 0.2;
decimal m = 0.1m + 0.2m;
Console.WriteLine($"double:  0.1 + 0.2 = {d:R}");
Console.WriteLine($"decimal: 0.1 + 0.2 = {m}");

// Handy helpers from the Math class.
Console.WriteLine($"Round 2.675 to 2 places: {Math.Round(2.675m, 2)}");
Console.WriteLine($"Square root of 144: {Math.Sqrt(144)}");
Console.WriteLine($"Largest int: {int.MaxValue}");

// Formatting numbers for people.
var us = System.Globalization.CultureInfo.GetCultureInfo("en-US");
Console.WriteLine($"Balance as currency: {accountBalance.ToString("C", us)}");
Console.WriteLine($"Steps with separators: {stepsToday:N0}");
Console.WriteLine($"Completion: {0.873:P1}");
