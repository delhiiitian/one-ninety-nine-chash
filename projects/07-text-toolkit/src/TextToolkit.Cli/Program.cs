// Project 7: Text Toolkit CLI
// Usage: texttool [file ...] [--top N] [--json]
//        cat essay.txt | texttool
// Run:   dotnet run --project projects/07-text-toolkit/src/TextToolkit.Cli -- projects/07-text-toolkit/samples/story.txt
using System.Text.Json;
using TextToolkit.Core;

int top = 5;
bool asJson = false;
var files = new List<string>();

for (int i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--top" when i + 1 < args.Length && int.TryParse(args[i + 1], out int n) && n >= 0:
            top = n;
            i++;
            break;
        case "--json":
            asJson = true;
            break;
        case "--help" or "-h":
            Console.WriteLine("Usage: texttool [file ...] [--top N] [--json]   (reads standard input if no file is given)");
            return 0;
        default:
            files.Add(args[i]);
            break;
    }
}

// Exit codes tell scripts what happened: 0 = success, 1 = bad input.
var inputs = new List<(string Name, string Text)>();
if (files.Count == 0)
{
    inputs.Add(("stdin", Console.In.ReadToEnd()));
}
foreach (string file in files)
{
    if (!File.Exists(file))
    {
        Console.Error.WriteLine($"texttool: file not found: {file}");
        return 1;
    }
    inputs.Add((Path.GetFileName(file), File.ReadAllText(file)));
}

foreach (var (name, text) in inputs)
{
    TextReport report = TextAnalyzer.Analyze(text, top);
    if (asJson)
    {
        var shape = new
        {
            file = name, report.Words, report.Sentences, report.Paragraphs, report.ReadingEase,
            report.ReadingLevel, readingSeconds = (int)report.ReadingTime.TotalSeconds,
            topWords = report.TopWords.Select(t => new { t.Word, t.Count }),
        };
        Console.WriteLine(JsonSerializer.Serialize(shape, new JsonSerializerOptions { WriteIndented = true }));
        continue;
    }
    Console.WriteLine($"== {name}");
    Console.WriteLine($"  Characters     {report.Characters,8:N0}");
    Console.WriteLine($"  Words          {report.Words,8:N0}");
    Console.WriteLine($"  Sentences      {report.Sentences,8:N0}");
    Console.WriteLine($"  Paragraphs     {report.Paragraphs,8:N0}");
    Console.WriteLine($"  Avg word len   {report.AverageWordLength,8:F1}");
    Console.WriteLine($"  Reading ease   {report.ReadingEase,8:F1}  {report.ReadingLevel}");
    Console.WriteLine($"  Reading time   {report.ReadingTime:m\\:ss} min");
    if (report.TopWords.Count > 0)
        Console.WriteLine($"  Top words      {string.Join(", ", report.TopWords.Select(t => $"{t.Word} ({t.Count})"))}");
}
return 0;
