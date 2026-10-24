// Reusable input helpers. Every prompt in the app goes through these two methods,
// so validation rules live in one place.
using System.Globalization;

static class ConsoleInput
{
    /// <summary>Asks for a menu number from 0 to max. Returns 0 if input ends.</summary>
    public static int ReadChoice(string prompt, int max)
    {
        while (true)
        {
            Console.Write($"{prompt} (0-{max}): ");
            string? text = Console.ReadLine();
            if (text is null) return 0;
            if (int.TryParse(text, out int choice) && choice >= 0 && choice <= max)
                return choice;
            Console.WriteLine($"  Please enter a number from 0 to {max}.");
        }
    }

    /// <summary>Asks for a number. Returns false only if input ends.</summary>
    public static bool TryReadNumber(string prompt, out double value)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            string? text = Console.ReadLine();
            if (text is null) { value = 0; return false; }
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                return true;
            Console.WriteLine("  That is not a number. Try again (use . for decimals).");
        }
    }
}
