// CSV export and import, so the data can go to and from Excel or Google Sheets.
using System.Globalization;
using System.Text;

static class CsvFile
{
    private const string Header = "Date,Amount,Category,Note";

    public static void Export(string path, IEnumerable<Expense> expenses)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        using var writer = new StreamWriter(path, append: false, Encoding.UTF8);
        writer.WriteLine(Header);
        foreach (Expense e in expenses)
            writer.WriteLine(string.Join(',',
                e.Date.ToString("yyyy-MM-dd"), e.Amount.ToString(CultureInfo.InvariantCulture), Quote(e.Category), Quote(e.Note)));
    }

    /// <summary>Reads rows; bad rows are skipped and reported, not fatal.</summary>
    public static (List<(DateOnly Date, decimal Amount, string Category, string Note)> Rows, List<string> Errors) Import(string path)
    {
        var rows = new List<(DateOnly, decimal, string, string)>();
        var errors = new List<string>();
        int lineNumber = 0;
        foreach (string line in File.ReadLines(path))
        {
            lineNumber++;
            if (lineNumber == 1 || string.IsNullOrWhiteSpace(line)) continue;   // header or blank
            List<string> f = Split(line);
            if (f.Count != 4) { errors.Add($"line {lineNumber}: expected 4 fields, found {f.Count}"); continue; }
            if (!DateOnly.TryParseExact(f[0], "yyyy-MM-dd", out DateOnly date)) { errors.Add($"line {lineNumber}: bad date '{f[0]}'"); continue; }
            if (!decimal.TryParse(f[1], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
            { errors.Add($"line {lineNumber}: bad amount '{f[1]}'"); continue; }
            rows.Add((date, amount, f[2].Trim().ToLowerInvariant(), f[3].Trim()));
        }
        return (rows, errors);
    }

    private static string Quote(string v) => v.Contains(',') || v.Contains('"') ? $"\"{v.Replace("\"", "\"\"")}\"" : v;

    private static List<string> Split(string line)
    {
        var fields = new List<string>();
        var current = new StringBuilder();
        bool quoted = false;
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (quoted && c == '"' && i + 1 < line.Length && line[i + 1] == '"') { current.Append('"'); i++; }
            else if (c == '"') quoted = !quoted;
            else if (c == ',' && !quoted) { fields.Add(current.ToString()); current.Clear(); }
            else current.Append(c);
        }
        fields.Add(current.ToString());
        return fields;
    }
}
