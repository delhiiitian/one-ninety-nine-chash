// 4.1 Arrays
// Run: dotnet run chapter-04-collections/01-arrays.cs

// An array is a fixed-size row of slots, all of the same type, numbered from 0.
string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri" };
int[] dailySteps = new int[5];             // five slots, all 0 to start

dailySteps[0] = 7200;
dailySteps[1] = 10450;
dailySteps[2] = 6300;
dailySteps[3] = 12010;
dailySteps[4] = 8800;

Console.WriteLine($"{weekdays.Length} days tracked");
for (int i = 0; i < weekdays.Length; i++)
    Console.WriteLine($"{weekdays[i]}: {dailySteps[i],6:N0} steps");

// Index from the end with ^ and take slices with ranges (start..end, end not included).
Console.WriteLine($"Last day: {weekdays[^1]}");
string[] midweek = weekdays[1..4];          // Tue, Wed, Thu
Console.WriteLine($"Midweek: {string.Join(", ", midweek)}");

// Built-in helpers on the Array class.
int best = dailySteps.Max();
int bestIndex = Array.IndexOf(dailySteps, best);
Console.WriteLine($"Best day: {weekdays[bestIndex]} with {best:N0}");
Console.WriteLine($"Average: {dailySteps.Average():N0}");

int[] sorted = (int[])dailySteps.Clone();   // copy before sorting, keep the original order
Array.Sort(sorted);
Console.WriteLine($"Sorted: {string.Join(", ", sorted)}");
Array.Reverse(sorted);
Console.WriteLine($"Highest first: {string.Join(", ", sorted)}");

// Arrays cannot grow. Going past the end throws IndexOutOfRangeException.
try
{
    dailySteps[5] = 100;
}
catch (IndexOutOfRangeException)
{
    Console.WriteLine("There is no slot 5: arrays have a fixed size.");
}
