// 3.2 Parameters and return values
// Run: dotnet run chapter-03-methods/02-parameters-return.cs

// Parameters are the inputs; the return type says what comes back.
decimal price = 1999m;
decimal withGst = AddGst(price, 18m);
Console.WriteLine($"Price {price} + 18% GST = {withGst}");

double bmi = CalculateBmi(weightKg: 70, heightM: 1.75);
Console.WriteLine($"BMI {bmi:F1} ({BmiCategory(bmi)})");

Console.WriteLine($"Is 'racecar' a palindrome? {IsPalindrome("racecar")}");
Console.WriteLine($"Is 'rocket' a palindrome? {IsPalindrome("rocket")}");

// Return multiple values with a tuple.
var (min, max, average) = Stats(new[] { 72, 85, 90, 64, 99 });
Console.WriteLine($"Min {min}, max {max}, average {average:F1}");

static decimal AddGst(decimal amount, decimal ratePercent)
{
    decimal tax = amount * ratePercent / 100m;
    return amount + tax;          // return ends the method and hands back the value
}

static double CalculateBmi(double weightKg, double heightM) => weightKg / (heightM * heightM);

static string BmiCategory(double bmi)
{
    if (bmi < 18.5) return "underweight";   // early return keeps code flat
    if (bmi < 25) return "healthy";
    if (bmi < 30) return "overweight";
    return "obese";
}

static bool IsPalindrome(string word)
{
    string lower = word.ToLower();
    for (int i = 0; i < lower.Length / 2; i++)
    {
        if (lower[i] != lower[^(i + 1)]) return false;   // ^1 means "last"
    }
    return true;
}

static (int Min, int Max, double Average) Stats(int[] values)
{
    int min = values[0], max = values[0], sum = 0;
    foreach (int v in values)
    {
        if (v < min) min = v;
        if (v > max) max = v;
        sum += v;
    }
    return (min, max, (double)sum / values.Length);
}
