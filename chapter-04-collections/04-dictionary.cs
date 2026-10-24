// 4.4 Dictionary<TKey, TValue>: look things up by key
// Run: dotnet run chapter-04-collections/04-dictionary.cs

// A dictionary maps unique keys to values, like a phone book maps names to numbers.
var phoneBook = new Dictionary<string, string>
{
    ["Asha"] = "98450 11111",
    ["Ben"] = "98450 22222",
};
phoneBook["Chitra"] = "98450 33333";        // add
phoneBook["Ben"] = "98450 99999";           // update (same key)
phoneBook.Add("Dev", "98450 44444");        // Add throws if the key already exists

Console.WriteLine($"Ben's number: {phoneBook["Ben"]}");

// Safe lookup: TryGetValue never throws.
if (phoneBook.TryGetValue("Zara", out string? zara))
    Console.WriteLine($"Zara: {zara}");
else
    Console.WriteLine("Zara is not in the phone book");

phoneBook.Remove("Dev");
Console.WriteLine($"Contains Dev? {phoneBook.ContainsKey("Dev")}; {phoneBook.Count} contacts");

foreach (var (name, number) in phoneBook)
    Console.WriteLine($"  {name,-7} {number}");

// The classic dictionary job: counting.
string review = "the food was great and the service was great but the music was loud";
var wordCounts = new Dictionary<string, int>();
foreach (string word in review.Split(' '))
{
    wordCounts[word] = wordCounts.GetValueOrDefault(word) + 1;
}
Console.WriteLine("Words used more than once:");
foreach (var (word, count) in wordCounts)
    if (count > 1) Console.WriteLine($"  {word}: {count}");

// Values can be any type, including lists: group orders by customer.
var ordersByCustomer = new Dictionary<string, List<decimal>>();
(string Customer, decimal Amount)[] orders = { ("Asha", 250m), ("Ben", 90m), ("Asha", 120m) };
foreach (var (customer, amount) in orders)
{
    if (!ordersByCustomer.ContainsKey(customer))
        ordersByCustomer[customer] = new List<decimal>();
    ordersByCustomer[customer].Add(amount);
}
foreach (var (customer, amounts) in ordersByCustomer)
    Console.WriteLine($"{customer}: {amounts.Count} order(s), total {amounts.Sum()}");
