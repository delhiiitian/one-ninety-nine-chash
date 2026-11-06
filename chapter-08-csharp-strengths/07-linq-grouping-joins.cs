// 8.7 LINQ: grouping, joining and shaping results
// Run: dotnet run chapter-08-csharp-strengths/07-linq-grouping-joins.cs

List<Customer> customers =
[
    new(1, "Asha", "Pune"), new(2, "Ben", "Delhi"), new(3, "Chitra", "Pune"), new(4, "Dev", "Mumbai"),
];
List<Order> orders =
[
    new(101, 1, new DateOnly(2026, 9, 3), 1200m), new(102, 2, new DateOnly(2026, 9, 5), 450m),
    new(103, 1, new DateOnly(2026, 10, 1), 3100m), new(104, 3, new DateOnly(2026, 10, 2), 800m),
    new(105, 1, new DateOnly(2026, 10, 9), 650m), new(106, 2, new DateOnly(2026, 10, 15), 2200m),
];

// GroupBy: one group per key, then summarise each group.
Console.WriteLine("Revenue by month:");
foreach (var g in orders.GroupBy(o => o.Date.ToString("yyyy-MM")).OrderBy(g => g.Key))
    Console.WriteLine($"  {g.Key}: {g.Count()} orders, {g.Sum(o => o.Amount),9:N2}");

// Join: match orders to customers by id (like SQL INNER JOIN).
var lines = orders.Join(customers, o => o.CustomerId, c => c.Id,
                        (o, c) => new { c.Name, c.City, o.Id, o.Amount });
Console.WriteLine("Orders with names:");
foreach (var l in lines.Take(3)) Console.WriteLine($"  #{l.Id} {l.Name,-7} {l.City,-7} {l.Amount,9:N2}");

// Join + GroupBy: top customers by total spend. Anonymous types shape the output.
var top = from c in customers
          join o in orders on c.Id equals o.CustomerId into theirOrders
          let total = theirOrders.Sum(o => o.Amount)
          orderby total descending
          select new { c.Name, Orders = theirOrders.Count(), Total = total };
Console.WriteLine("Customers by spend (left join keeps Dev with 0):");
foreach (var t in top) Console.WriteLine($"  {t.Name,-7} {t.Orders} order(s) {t.Total,10:N2}");

// ToDictionary and ToLookup for fast lookups after a query.
Dictionary<int, string> nameById = customers.ToDictionary(c => c.Id, c => c.Name);
ILookup<string, Customer> byCity = customers.ToLookup(c => c.City);
Console.WriteLine($"Customer 3 is {nameById[3]}; Pune has {byCity["Pune"].Count()} customers");

// CountBy and AggregateBy (.NET 9+): grouping shortcuts.
foreach (var (city, count) in customers.CountBy(c => c.City))
    Console.WriteLine($"  {city}: {count}");

// Chunk: split into batches, e.g. to send 2 emails at a time.
foreach (Customer[] batch in customers.Chunk(2))
    Console.WriteLine($"  Email batch: {string.Join(", ", batch.Select(c => c.Name))}");

record Customer(int Id, string Name, string City);
record Order(int Id, int CustomerId, DateOnly Date, decimal Amount);
