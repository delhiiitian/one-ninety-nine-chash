// 9.6 Building a minimal web API with ASP.NET Core
// Run the server: dotnet run chapter-09-real-world/06-minimal-api.cs
//   then in another terminal: curl http://localhost:5080/menu
// Quick check without a second terminal: dotnet run chapter-09-real-world/06-minimal-api.cs -- --self-test
#:sdk Microsoft.NET.Sdk.Web
#:property PublishAot=false

using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5080");
builder.Logging.SetMinimumLevel(LogLevel.Warning); // keep the console quiet for the book
var app = builder.Build();

// In-memory data for a cafe menu (project 9 stores data in SQLite instead).
var menu = new List<MenuItem>
{
    new(1, "Masala chai", 30m, "drinks"),
    new(2, "Filter coffee", 40m, "drinks"),
    new(3, "Samosa", 25m, "snacks"),
};
int nextId = 4;

// 1. Map URL + HTTP verb to a lambda. Return values are turned into JSON automatically.
app.MapGet("/", () => "Cafe API is running. Try /menu");

// 2. Query string values bind to parameters by name: /menu?category=drinks
app.MapGet("/menu", (string? category) =>
    category is null ? menu : menu.Where(m => m.Category == category).ToList());

// 3. Route values: /menu/2. Results.* helpers pick the status code.
app.MapGet("/menu/{id:int}", (int id) =>
    menu.FirstOrDefault(m => m.Id == id) is MenuItem item ? Results.Ok(item) : Results.NotFound());

// 4. POST: the JSON body binds to a record. Validate, then return 201 Created with a Location header.
app.MapPost("/menu", (NewMenuItem input) =>
{
    if (string.IsNullOrWhiteSpace(input.Name) || input.Price <= 0)
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["item"] = ["Name is required and price must be positive."]
        });
    var item = new MenuItem(nextId++, input.Name.Trim(), input.Price, input.Category ?? "other");
    menu.Add(item);
    return Results.Created($"/menu/{item.Id}", item);
});

// 5. DELETE returns 204 No Content when it works.
app.MapDelete("/menu/{id:int}", (int id) =>
    menu.RemoveAll(m => m.Id == id) > 0 ? Results.NoContent() : Results.NotFound());

if (args.Contains("--self-test"))
{
    await app.StartAsync();
    using var http = new HttpClient { BaseAddress = new Uri("http://localhost:5080") };
    Console.WriteLine($"GET /            -> {await http.GetStringAsync("/")}");
    var drinks = await http.GetFromJsonAsync<List<MenuItem>>("/menu?category=drinks");
    Console.WriteLine($"GET /menu?category=drinks -> {string.Join(", ", drinks!.Select(d => d.Name))}");
    var created = await http.PostAsJsonAsync("/menu", new NewMenuItem("Vada pav", 35m, "snacks"));
    Console.WriteLine($"POST /menu       -> {(int)created.StatusCode}, Location {created.Headers.Location}");
    var invalid = await http.PostAsJsonAsync("/menu", new NewMenuItem("", 0m, null));
    Console.WriteLine($"POST invalid     -> {(int)invalid.StatusCode}");
    Console.WriteLine($"GET /menu/4      -> {await http.GetStringAsync("/menu/4")}");
    Console.WriteLine($"GET /menu/99     -> {(int)(await http.GetAsync("/menu/99")).StatusCode}");
    Console.WriteLine($"DELETE /menu/3   -> {(int)(await http.DeleteAsync("/menu/3")).StatusCode}");
    Console.WriteLine($"Items left       -> {(await http.GetFromJsonAsync<List<MenuItem>>("/menu"))!.Count}");
    await app.StopAsync();
}
else
{
    Console.WriteLine("Listening on http://localhost:5080 (Ctrl+C to stop)");
    app.Run();
}

record MenuItem(int Id, string Name, decimal Price, string Category);
record NewMenuItem(string Name, decimal Price, string? Category);
