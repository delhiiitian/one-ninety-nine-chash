// 6.5 CSV files: the spreadsheet format
// Run: dotnet run chapter-06-files-io/05-csv.cs
using System.Globalization;
using System.Text;

string folder = Path.Combine(Path.GetTempPath(), "csharp199-ch06-csv");
Directory.CreateDirectory(folder);
string path = Path.Combine(folder, "products.csv");

var products = new List<Product>
{
    new("P01", "Notebook", 45.50m, 120),
    new("P02", "Pen, blue", 10m, 500),              // a comma inside a value
    new("P03", "Sticky \"Post-it\" notes", 89.99m, 0), // quotes inside a value
};

// Writing: header row, then one line per item. Quote fields that need it.
var lines = new List<string> { "Id,Name,Price,Stock" };
foreach (Product p in products)
    lines.Add(string.Join(',', p.Id, Quote(p.Name), p.Price.ToString(CultureInfo.InvariantCulture), p.Stock));
File.WriteAllLines(path, lines);
Console.WriteLine("--- products.csv (open it in Excel and it just works) ---");
Console.WriteLine(File.ReadAllText(path).TrimEnd());

// Reading: skip the header, split each line respecting quotes, convert types.
var loaded = new List<Product>();
foreach (string line in File.ReadLines(path).Skip(1))
{
    List<string> f = SplitCsvLine(line);
    loaded.Add(new Product(f[0], f[1], decimal.Parse(f[2], CultureInfo.InvariantCulture), int.Parse(f[3])));
}
Console.WriteLine("--- loaded back ---");
foreach (Product p in loaded)
    Console.WriteLine($"{p.Id} {p.Name,-24} {p.Price,7:F2} {(p.Stock == 0 ? "OUT" : p.Stock.ToString()),5}");

decimal stockValue = 0;
foreach (Product p in loaded) stockValue += p.Price * p.Stock;
Console.WriteLine($"Total stock value: {stockValue:N2}");

Directory.Delete(folder, recursive: true);

static string Quote(string value) =>
    value.Contains(',') || value.Contains('"') ? $"\"{value.Replace("\"", "\"\"")}\"" : value;

static List<string> SplitCsvLine(string line)
{
    var fields = new List<string>();
    var current = new StringBuilder();
    bool inQuotes = false;
    for (int i = 0; i < line.Length; i++)
    {
        char c = line[i];
        if (inQuotes)
        {
            if (c == '"' && i + 1 < line.Length && line[i + 1] == '"') { current.Append('"'); i++; }
            else if (c == '"') inQuotes = false;
            else current.Append(c);
        }
        else if (c == '"') inQuotes = true;
        else if (c == ',') { fields.Add(current.ToString()); current.Clear(); }
        else current.Append(c);
    }
    fields.Add(current.ToString());
    return fields;
}

record Product(string Id, string Name, decimal Price, int Stock);
