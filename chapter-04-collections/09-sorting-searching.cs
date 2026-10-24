// 4.9 Sorting and searching collections
// Run: dotnet run chapter-04-collections/09-sorting-searching.cs

var products = new List<Product>
{
    new("Headphones", 2999m, 4.5),
    new("USB cable", 299m, 4.1),
    new("Keyboard", 1899m, 4.7),
    new("Monitor", 12999m, 4.4),
    new("Mouse", 899m, 4.7),
};

// Sort with a comparison lambda: price low to high.
products.Sort((a, b) => a.Price.CompareTo(b.Price));
Show("Price: low to high", products);

// Sort by rating (high first), then by name for ties.
products.Sort((a, b) =>
{
    int byRating = b.Rating.CompareTo(a.Rating);
    return byRating != 0 ? byRating : string.Compare(a.Name, b.Name, StringComparison.Ordinal);
});
Show("Rating: high to low", products);

// Linear search: check each item until found (works on any list).
Product? cheapGood = products.Find(p => p.Price < 1000m && p.Rating >= 4.5);
Console.WriteLine($"Cheap and good: {cheapGood?.Name ?? "none"}");

// Binary search: very fast, but the list must be sorted by the thing you search for.
int[] ticketNumbers = { 104, 220, 315, 402, 589, 677, 731, 850 };
int index = Array.BinarySearch(ticketNumbers, 589);
Console.WriteLine($"Ticket 589 found at index {index}");
int missing = Array.BinarySearch(ticketNumbers, 500);
Console.WriteLine($"Ticket 500 found? {missing >= 0}");

// How binary search works, written by hand: halve the range each step.
Console.WriteLine($"Hand-written search for 731: index {BinarySearch(ticketNumbers, 731)}");

// Sorting strings: ordinal (by character code) vs culture-aware, and ignoring case.
var names = new List<string> { "bob", "Alice", "carol", "Dave" };
names.Sort(StringComparer.OrdinalIgnoreCase);
Console.WriteLine($"Names A-Z: {string.Join(", ", names)}");

static int BinarySearch(int[] sorted, int target)
{
    int low = 0, high = sorted.Length - 1, steps = 0;
    while (low <= high)
    {
        steps++;
        int mid = low + (high - low) / 2;
        if (sorted[mid] == target) { Console.WriteLine($"  (found in {steps} steps)"); return mid; }
        if (sorted[mid] < target) low = mid + 1;
        else high = mid - 1;
    }
    return -1;
}

static void Show(string title, List<Product> items)
{
    Console.WriteLine(title);
    foreach (var p in items) Console.WriteLine($"  {p.Name,-11} {p.Price,7:N0}  {p.Rating:F1}*");
}

record Product(string Name, decimal Price, double Rating);
