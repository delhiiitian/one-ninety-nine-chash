// Loads and saves all expenses as one JSON file.
// Saving writes to a temporary file first and then swaps it in, so a crash
// halfway through a save can never leave you with a half-written data file.
using System.Text.Json;

class ExpenseStore(string dataFolder)
{
    private static readonly JsonSerializerOptions Json = new() { WriteIndented = true };
    public string FilePath { get; } = Path.Combine(dataFolder, "expenses.json");

    public async Task<List<Expense>> LoadAsync()
    {
        if (!File.Exists(FilePath)) return new List<Expense>();
        try
        {
            await using FileStream stream = File.OpenRead(FilePath);
            return await JsonSerializer.DeserializeAsync<List<Expense>>(stream, Json) ?? new List<Expense>();
        }
        catch (JsonException)
        {
            // Keep the broken file for inspection and start fresh instead of crashing.
            string broken = FilePath + $".corrupt-{DateTime.Now:yyyyMMddHHmmss}";
            File.Move(FilePath, broken);
            Console.WriteLine($"Warning: data file was corrupt, moved to {Path.GetFileName(broken)}");
            return new List<Expense>();
        }
    }

    public async Task SaveAsync(List<Expense> expenses)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        string temp = FilePath + ".tmp";
        await using (FileStream stream = File.Create(temp))
        {
            await JsonSerializer.SerializeAsync(stream, expenses, Json);
        }
        File.Move(temp, FilePath, overwrite: true);
    }
}
