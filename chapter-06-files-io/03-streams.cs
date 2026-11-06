// 6.3 Streams: reading and writing line by line
// Run: dotnet run chapter-06-files-io/03-streams.cs

string folder = Path.Combine(Path.GetTempPath(), "csharp199-ch06-streams");
Directory.CreateDirectory(folder);
string logPath = Path.Combine(folder, "server.log");

// StreamWriter writes piece by piece. Good for big outputs you build gradually.
var random = new Random(1);
string[] levels = { "INFO", "INFO", "INFO", "WARN", "ERROR" };
using (var writer = new StreamWriter(logPath))
{
    for (int i = 1; i <= 10_000; i++)
    {
        string level = levels[random.Next(levels.Length)];
        writer.WriteLine($"2026-10-24 10:{i / 600 % 60:00}:{i / 10 % 60:00} {level} request {i} handled");
    }
}
Console.WriteLine($"Log file: {new FileInfo(logPath).Length / 1024} KB");

// StreamReader reads one line at a time, so memory use stays tiny for huge files.
int errors = 0, warnings = 0, total = 0;
string? firstError = null;
using (var reader = new StreamReader(logPath))
{
    string? line;
    while ((line = reader.ReadLine()) is not null)
    {
        total++;
        if (line.Contains(" ERROR "))
        {
            errors++;
            firstError ??= line;
        }
        else if (line.Contains(" WARN ")) warnings++;
    }
}
Console.WriteLine($"{total} lines: {errors} errors, {warnings} warnings");
Console.WriteLine($"First error: {firstError}");

// File.ReadLines is the short form: it streams lazily, unlike ReadAllLines.
int lastHourErrors = 0;
foreach (string line in File.ReadLines(logPath))
    if (line.Contains(" 10:16:") && line.Contains("ERROR")) lastHourErrors++;
Console.WriteLine($"Errors at 10:16: {lastHourErrors}");

Directory.Delete(folder, recursive: true);
