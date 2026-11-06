// 8.9 Delegates, events and extension methods
// Run: dotnet run chapter-08-csharp-strengths/09-events-extensions.cs

// EVENTS: an object announces "something happened"; any number of listeners react.
var sensor = new TemperatureSensor("Server room");
sensor.ThresholdCrossed += (sender, e) => Console.WriteLine($"  [alarm]  {e.Location} at {e.Celsius} C");
sensor.ThresholdCrossed += (sender, e) => Console.WriteLine($"  [sms]    Text sent to on-call engineer");
EventHandler<TemperatureEventArgs> logger = (sender, e) => Console.WriteLine($"  [log]    {e.Time:HH:mm:ss} {e.Celsius} C");
sensor.ThresholdCrossed += logger;

foreach (double reading in new[] { 24.5, 27.0, 31.2, 29.0, 35.8 })
{
    Console.WriteLine($"Reading {reading} C");
    sensor.Record(reading);
}
sensor.ThresholdCrossed -= logger;                  // unsubscribe
Console.WriteLine("Reading 40 C (logger unsubscribed)");
sensor.Record(40);

// EXTENSION METHODS: add methods to existing types without changing them.
Console.WriteLine("hello world from csharp".ToTitleCase());
Console.WriteLine($"'racecar' palindrome? {"racecar".IsPalindrome()}");
Console.WriteLine($"1536000 bytes = {1_536_000L.ToFileSize()}");
Console.WriteLine($"Weekend? {new DateOnly(2026, 10, 24).IsWeekend()}");
int[] scores = [72, 95, 88, 61];
Console.WriteLine($"Scores: {scores.ToBulletList()}");

class TemperatureEventArgs(string location, double celsius) : EventArgs
{
    public string Location { get; } = location;
    public double Celsius { get; } = celsius;
    public DateTime Time { get; } = DateTime.Now;
}

class TemperatureSensor(string location, double limit = 30)
{
    // event + EventHandler<T>: the standard .NET pattern.
    public event EventHandler<TemperatureEventArgs>? ThresholdCrossed;

    public void Record(double celsius)
    {
        if (celsius > limit)
            ThresholdCrossed?.Invoke(this, new TemperatureEventArgs(location, celsius));   // ?. = no listeners, no crash
    }
}

// Extension methods live in a static class; "this" marks the type being extended.
static class Extensions
{
    public static string ToTitleCase(this string text) =>
        string.Join(' ', text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(w => char.ToUpper(w[0]) + w[1..]));

    public static bool IsPalindrome(this string text)
    {
        string t = text.ToLowerInvariant();
        return t.SequenceEqual(t.Reverse());
    }

    public static string ToFileSize(this long bytes) =>
        bytes >= 1 << 20 ? $"{bytes / 1048576.0:0.#} MB" : bytes >= 1 << 10 ? $"{bytes / 1024.0:0.#} KB" : $"{bytes} B";

    public static bool IsWeekend(this DateOnly date) => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    public static string ToBulletList<T>(this IEnumerable<T> items) => string.Join(" ", items.Select(i => $"* {i}"));
}
