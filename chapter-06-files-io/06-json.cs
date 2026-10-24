// 6.6 JSON with System.Text.Json
// Run: dotnet run chapter-06-files-io/06-json.cs
#:property PublishAot=false
using System.Text.Json;

string folder = Path.Combine(Path.GetTempPath(), "csharp199-ch06-json");
Directory.CreateDirectory(folder);
string path = Path.Combine(folder, "settings.json");

var settings = new AppSettings(
    Theme: "dark",
    FontSize: 14,
    Notifications: true,
    RecentFiles: new List<string> { "budget.xlsx", "trip-plan.docx" },
    Window: new WindowSize(1280, 800));

// Serialize: C# object -> JSON text.
var options = new JsonSerializerOptions
{
    WriteIndented = true,                                // pretty print
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,   // FontSize -> fontSize
};
string json = JsonSerializer.Serialize(settings, options);
File.WriteAllText(path, json);
Console.WriteLine("--- settings.json ---");
Console.WriteLine(json);

// Deserialize: JSON text -> C# object, fully typed.
AppSettings? loaded = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path), options);
Console.WriteLine($"Loaded theme '{loaded!.Theme}', font {loaded.FontSize}, window {loaded.Window.Width}x{loaded.Window.Height}");
Console.WriteLine($"Recent: {string.Join(", ", loaded.RecentFiles)}");
Console.WriteLine($"Round trip equal (theme, size): {loaded.Theme == settings.Theme && loaded.FontSize == settings.FontSize}");

// Change and save: records make a modified copy easy.
var bigger = loaded with { FontSize = 18 };
File.WriteAllText(path, JsonSerializer.Serialize(bigger, options));
Console.WriteLine($"Saved new font size: {JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path), options)!.FontSize}");

// Reading JSON you did not design: JsonDocument lets you explore it without classes.
string weather = """{ "city": "Pune", "current": { "tempC": 27.5, "condition": "Cloudy" }, "alerts": [] }""";
using JsonDocument doc = JsonDocument.Parse(weather);
JsonElement current = doc.RootElement.GetProperty("current");
Console.WriteLine($"{doc.RootElement.GetProperty("city").GetString()}: {current.GetProperty("tempC").GetDouble()} C, {current.GetProperty("condition").GetString()}");
Console.WriteLine($"Alerts: {doc.RootElement.GetProperty("alerts").GetArrayLength()}");

// Bad JSON throws JsonException: catch it when the file comes from outside.
try
{
    JsonSerializer.Deserialize<AppSettings>("{ not valid json");
}
catch (JsonException ex)
{
    Console.WriteLine($"Corrupt file detected: {ex.Message.Split('.')[0]}");
}

Directory.Delete(folder, recursive: true);

record WindowSize(int Width, int Height);
record AppSettings(string Theme, int FontSize, bool Notifications, List<string> RecentFiles, WindowSize Window);
