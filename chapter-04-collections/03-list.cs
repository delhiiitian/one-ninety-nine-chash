// 4.3 List<T>: a growable list
// Run: dotnet run chapter-04-collections/03-list.cs

// List<T> is an array that grows and shrinks. T is the item type.
var cart = new List<string> { "Rice", "Milk" };
cart.Add("Eggs");
cart.AddRange(new[] { "Bread", "Butter" });
cart.Insert(0, "Coffee");                    // put at the front
Print("After adding", cart);

cart.Remove("Milk");                         // by value
cart.RemoveAt(cart.Count - 1);               // by position (the last one)
Print("After removing", cart);

Console.WriteLine($"Has eggs? {cart.Contains("Eggs")}   Position of Bread: {cart.IndexOf("Bread")}");

cart.Sort();
Print("Sorted", cart);

// Lists of numbers and lists of your own types work the same way.
var prices = new List<decimal> { 120m, 45.5m, 300m, 89.99m };
decimal total = 0;
foreach (decimal p in prices) total += p;
Console.WriteLine($"Total {total}, cheapest {prices.Min()}, priciest {prices.Max()}");

// Find items with a condition (a lambda from Chapter 3).
var tasks = new List<TodoItem>
{
    new("Pay rent", Done: true),
    new("Call plumber", Done: false),
    new("Buy gift", Done: false),
};
TodoItem? firstOpen = tasks.Find(t => !t.Done);
List<TodoItem> open = tasks.FindAll(t => !t.Done);
int removed = tasks.RemoveAll(t => t.Done);
Console.WriteLine($"First open task: {firstOpen?.Title}; {open.Count} open; removed {removed} done task(s)");

// Count vs Capacity: the list doubles its hidden array as it grows.
var numbers = new List<int>();
for (int i = 0; i < 10; i++) numbers.Add(i);
Console.WriteLine($"Count {numbers.Count}, capacity {numbers.Capacity}");

static void Print(string label, List<string> items) =>
    Console.WriteLine($"{label,-15} [{string.Join(", ", items)}] ({items.Count})");

record TodoItem(string Title, bool Done);
