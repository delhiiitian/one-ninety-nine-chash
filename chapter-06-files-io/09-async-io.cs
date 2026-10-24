// 6.9 Asynchronous file I/O with async and await
// Run: dotnet run chapter-06-files-io/09-async-io.cs
using System.Diagnostics;

string folder = Path.Combine(Path.GetTempPath(), "csharp199-ch06-async");
Directory.CreateDirectory(folder);

// Async methods let the program do other work while the disk (or network) is busy.
// In top-level statements you can use await directly.
string[] chapters = { "intro", "basics", "loops", "methods", "files" };

var clock = Stopwatch.StartNew();
foreach (string name in chapters)
{
    string text = $"# {name}\n" + string.Join('\n', Enumerable.Repeat($"Content of {name}.", 20_000));
    await File.WriteAllTextAsync(Path.Combine(folder, $"{name}.md"), text);
}
Console.WriteLine($"Wrote {chapters.Length} files one after another in {clock.ElapsedMilliseconds} ms");

// Start every read at once, then wait for all of them: Task.WhenAll.
clock.Restart();
var readTasks = new List<Task<string>>();
foreach (string name in chapters)
    readTasks.Add(File.ReadAllTextAsync(Path.Combine(folder, $"{name}.md")));
string[] contents = await Task.WhenAll(readTasks);
Console.WriteLine($"Read {contents.Length} files concurrently in {clock.ElapsedMilliseconds} ms");

for (int i = 0; i < chapters.Length; i++)
    Console.WriteLine($"  {chapters[i],-8} {contents[i].Length,9:N0} characters");

// An async method of your own returns Task<T>.
int words = await CountWordsAsync(Path.Combine(folder, "loops.md"));
Console.WriteLine($"Words in loops.md: {words:N0}");

// Streams have async versions too: read line by line without blocking.
int lineCount = 0;
using (var reader = new StreamReader(Path.Combine(folder, "intro.md")))
{
    while (await reader.ReadLineAsync() is not null) lineCount++;
}
Console.WriteLine($"Lines in intro.md: {lineCount:N0}");

Directory.Delete(folder, recursive: true);

static async Task<int> CountWordsAsync(string path)
{
    string text = await File.ReadAllTextAsync(path);
    return text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
}
