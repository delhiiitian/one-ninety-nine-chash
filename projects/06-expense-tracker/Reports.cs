// Turns the list of expenses into summaries a person can read.

static class Reports
{
    public static void ByCategory(List<Expense> expenses)
    {
        var totals = new Dictionary<string, decimal>();
        foreach (Expense e in expenses)
            totals[e.Category] = totals.GetValueOrDefault(e.Category) + e.Amount;

        decimal grand = expenses.Sum(e => e.Amount);
        var sorted = totals.ToList();
        sorted.Sort((a, b) => b.Value.CompareTo(a.Value));

        Console.WriteLine($"  {"Category",-14}{"Total",12}  Share");
        foreach (var (category, total) in sorted)
        {
            int bar = grand == 0 ? 0 : (int)Math.Round(total / grand * 20);
            Console.WriteLine($"  {category,-14}{total,12:N2}  {new string('#', bar)} {total / grand:P0}");
        }
        Console.WriteLine($"  {"TOTAL",-14}{grand,12:N2}");
    }

    public static void ByMonth(List<Expense> expenses)
    {
        var months = new SortedDictionary<string, decimal>();   // keeps months in order
        foreach (Expense e in expenses)
        {
            string key = e.Date.ToString("yyyy-MM");
            months[key] = months.GetValueOrDefault(key) + e.Amount;
        }
        foreach (var (month, total) in months)
            Console.WriteLine($"  {month}  {total,12:N2}");
    }
}
