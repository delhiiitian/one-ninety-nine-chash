// 8.5 Generics: one piece of code for many types
// Run: dotnet run chapter-08-csharp-strengths/05-generics.cs

// List<T> and Dictionary<TKey,TValue> are generic. You can write your own.
var recentSearches = new RecentItems<string>(capacity: 3);
foreach (string s in new[] { "shoes", "laptop", "phone case", "headphones" })
    recentSearches.Add(s);
Console.WriteLine($"Recent searches: {string.Join(", ", recentSearches.Items)}");

var recentPrices = new RecentItems<decimal>(capacity: 2);
recentPrices.Add(999m);
recentPrices.Add(1299m);
recentPrices.Add(849m);
Console.WriteLine($"Recent prices: {string.Join(", ", recentPrices.Items)}");

// Generic methods: the type is inferred from the arguments.
Console.WriteLine($"Max of ints: {MaxOf(4, 19, 7)}");
Console.WriteLine($"Max of strings: {MaxOf("pear", "apple", "zucchini")}");
Console.WriteLine($"Max of dates: {MaxOf(new DateOnly(2026, 1, 5), new DateOnly(2025, 12, 31)):yyyy-MM-dd}");

// A generic result type: success with a value, or failure with an error.
Result<int> ok = ParseAge("34");
Result<int> bad = ParseAge("old");
Console.WriteLine(ok.IsSuccess ? $"Age {ok.Value}" : ok.Error);
Console.WriteLine(bad.IsSuccess ? $"Age {bad.Value}" : bad.Error);

// A swap that works for any type.
string a = "left", b = "right";
Swap(ref a, ref b);
Console.WriteLine($"a={a}, b={b}");

// Constraint: T must be comparable, so CompareTo is allowed.
static T MaxOf<T>(params T[] values) where T : IComparable<T>
{
    T best = values[0];
    foreach (T v in values)
        if (v.CompareTo(best) > 0) best = v;
    return best;
}

static void Swap<T>(ref T x, ref T y) => (x, y) = (y, x);

static Result<int> ParseAge(string text) =>
    int.TryParse(text, out int age) && age is >= 0 and <= 130
        ? Result<int>.Success(age)
        : Result<int>.Failure($"'{text}' is not a valid age");

/// <summary>Keeps only the newest N items, like a "recently viewed" list.</summary>
class RecentItems<T>(int capacity)
{
    private readonly LinkedList<T> _items = new();
    public IEnumerable<T> Items => _items;

    public void Add(T item)
    {
        _items.Remove(item);           // no duplicates; move to the front instead
        _items.AddFirst(item);
        if (_items.Count > capacity) _items.RemoveLast();
    }
}

record Result<T>(bool IsSuccess, T? Value, string? Error)
{
    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}
