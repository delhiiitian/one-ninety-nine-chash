// 6.4 Appending to files: a simple logger
// Run: dotnet run chapter-06-files-io/04-append-logging.cs

string folder = Path.Combine(Path.GetTempPath(), "csharp199-ch06-append");
Directory.CreateDirectory(folder);
string logFile = Path.Combine(folder, "app.log");
File.Delete(logFile);                                   // start fresh for the demo (no error if missing)

// AppendAllText adds to the end, creating the file if needed.
Log("App started");
Log("User asha logged in");
Log("Payment of 499.00 received", level: "AUDIT");
Log("Disk 91% full", level: "WARN");

Console.WriteLine("--- app.log ---");
Console.Write(File.ReadAllText(logFile));

// StreamWriter with append: true keeps the file open for many writes.
using (var writer = new StreamWriter(logFile, append: true))
{
    for (int i = 1; i <= 3; i++)
        writer.WriteLine($"{Stamp()} [INFO] batch item {i} processed");
}
Console.WriteLine($"Now {File.ReadAllLines(logFile).Length} lines");

// Simple rotation: when the log gets too big, rename it and start a new one.
const long MaxBytes = 200;
if (new FileInfo(logFile).Length > MaxBytes)
{
    string archived = Path.Combine(folder, $"app-{DateTime.Now:yyyyMMdd-HHmmss}.log");
    File.Move(logFile, archived);
    Log("Log rotated");
    Console.WriteLine($"Rotated: archive has {File.ReadAllLines(archived).Length} lines, new log has {File.ReadAllLines(logFile).Length}");
}

Directory.Delete(folder, recursive: true);

void Log(string message, string level = "INFO") =>
    File.AppendAllText(logFile, $"{Stamp()} [{level}] {message}{Environment.NewLine}");

static string Stamp() => DateTime.Now.ToString("HH:mm:ss");
