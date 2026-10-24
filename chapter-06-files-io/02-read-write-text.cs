// 6.2 Reading and writing whole files
// Run: dotnet run chapter-06-files-io/02-read-write-text.cs

string folder = Path.Combine(Path.GetTempPath(), "csharp199-ch06");
Directory.CreateDirectory(folder);                 // safe even if it already exists
string notePath = Path.Combine(folder, "note.txt");

// WriteAllText creates the file (or replaces it) in one call.
File.WriteAllText(notePath, "Buy milk\nCall the bank\nBook train tickets\n");
Console.WriteLine($"Wrote {new FileInfo(notePath).Length} bytes to {Path.GetFileName(notePath)}");

// ReadAllText gives the whole file as one string.
string content = File.ReadAllText(notePath);
Console.WriteLine("--- ReadAllText ---");
Console.Write(content);

// ReadAllLines splits it into an array of lines.
string[] lines = File.ReadAllLines(notePath);
Console.WriteLine($"--- ReadAllLines: {lines.Length} lines ---");
for (int i = 0; i < lines.Length; i++)
    Console.WriteLine($"{i + 1}. {lines[i]}");

// WriteAllLines writes a collection, one item per line.
var done = new List<string>();
foreach (string line in lines) done.Add($"[x] {line}");
string donePath = Path.Combine(folder, "done.txt");
File.WriteAllLines(donePath, done);
Console.WriteLine($"--- {Path.GetFileName(donePath)} ---");
Console.WriteLine(File.ReadAllText(donePath).TrimEnd());

// Always check before reading a file that may not exist.
string missing = Path.Combine(folder, "missing.txt");
Console.WriteLine(File.Exists(missing) ? File.ReadAllText(missing) : "missing.txt does not exist (no crash)");

// Encodings: UTF-8 is the default and handles every language.
File.WriteAllText(notePath, "Café, नमस्ते, 你好", System.Text.Encoding.UTF8);
Console.WriteLine($"Unicode round trip: {File.ReadAllText(notePath)}");

Directory.Delete(folder, recursive: true);         // tidy up
