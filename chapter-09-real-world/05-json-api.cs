// 9.5 Consuming a JSON API
// Run: dotnet run chapter-09-real-world/05-json-api.cs
// A local fake "weather API" is started first so the example runs offline.
#:property PublishAot=false
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using var api = FakeWeatherApi.Start("http://localhost:5198/");
using var http = new HttpClient { BaseAddress = new Uri("http://localhost:5198/") };

// 1. GET and deserialize straight into a record (JSON names are camelCase, C# names PascalCase).
Forecast? pune = await http.GetFromJsonAsync<Forecast>("forecast?city=Pune");
Console.WriteLine($"{pune!.City}: {pune.TempC} C, {pune.Condition}, humidity {pune.Humidity}%");

// 2. A list of objects, then LINQ over the result.
List<Forecast> all = await http.GetFromJsonAsync<List<Forecast>>("forecast/all") ?? [];
Console.WriteLine($"Hottest of {all.Count} cities: {all.MaxBy(f => f.TempC)!.City}");
foreach (Forecast f in all.OrderBy(f => f.City))
    Console.WriteLine($"  {f.City,-8} {f.TempC,5:0.0} C  {f.Condition}");

// 3. POST JSON: serialize a C# object as the request body.
var alert = new AlertRequest("Pune", "rain", ThresholdMm: 20);
using HttpResponseMessage created = await http.PostAsJsonAsync("alerts", alert);
AlertCreated? result = await created.Content.ReadFromJsonAsync<AlertCreated>();
Console.WriteLine($"POST /alerts -> {(int)created.StatusCode}, alert id {result!.Id} for {result.City}");

// 4. Errors usually come back as a JSON "problem" body with a 4xx status.
using HttpResponseMessage bad = await http.GetAsync("forecast?city=Atlantis");
if (!bad.IsSuccessStatusCode)
{
    ApiError? error = await bad.Content.ReadFromJsonAsync<ApiError>();
    Console.WriteLine($"GET Atlantis -> {(int)bad.StatusCode}: {error!.Message}");
}

// 5. When you only need one field, walk the JSON with JsonDocument instead of making a class.
string raw = await http.GetStringAsync("forecast?city=Delhi");
using JsonDocument doc = JsonDocument.Parse(raw);
double delhiTemp = doc.RootElement.GetProperty("tempC").GetDouble();
Console.WriteLine($"Delhi temperature via JsonDocument: {delhiTemp}");

record Forecast(string City, double TempC, string Condition, int Humidity);
record AlertRequest(string City, string Kind, int ThresholdMm);
record AlertCreated(int Id, string City);
record ApiError(string Message);

// --- fake server, only here so the example is self-contained ---
sealed class FakeWeatherApi : IDisposable
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);
    private static readonly Dictionary<string, Forecast> Data = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Pune"] = new("Pune", 27.5, "Partly cloudy", 61),
        ["Delhi"] = new("Delhi", 31.2, "Hazy", 40),
        ["Chennai"] = new("Chennai", 29.8, "Rain", 82),
        ["Shimla"] = new("Shimla", 12.4, "Clear", 55),
    };
    private readonly HttpListener _listener = new();
    private int _nextAlert = 100;

    public static FakeWeatherApi Start(string prefix)
    {
        var api = new FakeWeatherApi();
        api._listener.Prefixes.Add(prefix);
        api._listener.Start();
        _ = Task.Run(api.LoopAsync);
        return api;
    }

    private async Task LoopAsync()
    {
        while (_listener.IsListening)
        {
            HttpListenerContext ctx;
            try { ctx = await _listener.GetContextAsync(); } catch { return; }
            var req = ctx.Request;
            (int code, object body) = (req.HttpMethod, req.Url!.AbsolutePath) switch
            {
                ("GET", "/forecast/all") => (200, Data.Values.ToList()),
                ("GET", "/forecast") when Data.TryGetValue(req.QueryString["city"] ?? "", out var f) => (200, f),
                ("GET", "/forecast") => (404, new ApiError($"No forecast for '{req.QueryString["city"]}'")),
                ("POST", "/alerts") => (201, await CreateAlertAsync(req)),
                _ => (404, (object)new ApiError("Unknown endpoint"))
            };
            ctx.Response.StatusCode = code;
            ctx.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(ctx.Response.OutputStream, body, body.GetType(), Json);
            ctx.Response.Close();
        }
    }

    private async Task<object> CreateAlertAsync(HttpListenerRequest req)
    {
        var alert = await JsonSerializer.DeserializeAsync<AlertRequest>(req.InputStream, Json);
        return new AlertCreated(_nextAlert++, alert!.City);
    }

    public void Dispose() => _listener.Stop();
}
