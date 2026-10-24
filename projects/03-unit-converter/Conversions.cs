// All conversion rules live here, away from the menu code.
// Each conversion is a label, a Func<double, double> (a lambda) and the result unit.

static class Conversions
{
    public static readonly string[] Categories = { "Length", "Weight", "Temperature", "Speed" };

    // Exact factors from the international definitions.
    private const double KmPerMile = 1.609344;
    private const double KgPerPound = 0.45359237;
    private const double CmPerInch = 2.54;

    private static readonly (string Label, Func<double, double> Convert, string Unit)[][] Table =
    {
        // Length
        new (string, Func<double, double>, string)[]
        {
            ("Kilometres to miles", km => km / KmPerMile, "mi"),
            ("Miles to kilometres", mi => mi * KmPerMile, "km"),
            ("Centimetres to inches", cm => cm / CmPerInch, "in"),
            ("Inches to centimetres", inch => inch * CmPerInch, "cm"),
        },
        // Weight
        new (string, Func<double, double>, string)[]
        {
            ("Kilograms to pounds", kg => kg / KgPerPound, "lb"),
            ("Pounds to kilograms", lb => lb * KgPerPound, "kg"),
        },
        // Temperature: uses named methods instead of inline lambdas
        new (string, Func<double, double>, string)[]
        {
            ("Celsius to Fahrenheit", CelsiusToFahrenheit, "F"),
            ("Fahrenheit to Celsius", FahrenheitToCelsius, "C"),
            ("Celsius to Kelvin", c => c + 273.15, "K"),
        },
        // Speed
        new (string, Func<double, double>, string)[]
        {
            ("km/h to m/s", kmh => kmh / 3.6, "m/s"),
            ("m/s to km/h", ms => ms * 3.6, "km/h"),
            ("km/h to mph", kmh => kmh / KmPerMile, "mph"),
        },
    };

    public static (string Label, Func<double, double> Convert, string Unit)[] ForCategory(int index) => Table[index];

    public static double CelsiusToFahrenheit(double c) => c * 9 / 5 + 32;
    public static double FahrenheitToCelsius(double f) => (f - 32) * 5 / 9;
}
