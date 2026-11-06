// 9.4 Calling web servers with HttpClient
// Run: dotnet run chapter-09-real-world/04-httpclient.cs
// The file starts a tiny local web server first, so it works offline and gives the same output every run.
// Point BaseAddress at a real site (for example https://example.com) to try the internet.
using System.Net;
using System.Net.Http.Headers;

using var server = FakeServer.Start("http://localhost:5199/");

// 1. Create ONE HttpClient and reuse it (creating one per request can exhaust sockets).
using var http = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5199/"),
    Timeout = TimeSpan.FromSeconds(5)
};
http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("C199-Book", "1.0"));

// 2. Simple GET that returns the body as a string.
string hello = await http.GetStringAsync("hello");
Console.WriteLine($"GET /hello -> {hello}");

// 3. Inspect the full response: status code, headers, body.
using HttpResponseMessage response = await http.GetAsync("status");
Console.WriteLine($"GET /status -> {(int)response.StatusCode} {response.StatusCode}, content type {response.Content.Headers.ContentType}");
Console.WriteLine($"  body: {await response.Content.ReadAsStringAsync()}");

// 4. Error responses do not throw by default; check IsSuccessStatusCode or call EnsureSuccessStatusCode.
using HttpResponseMessage missing = await http.GetAsync("does-not-exist");
Console.WriteLine($"GET /does-not-exist -> {(int)missing.StatusCode}, success? {missing.IsSuccessStatusCode}");
try
{
    missing.EnsureSuccessStatusCode();
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"  EnsureSuccessStatusCode threw: {ex.StatusCode}");
}

// 5. POST a form body.
var form = new FormUrlEncodedContent(new Dictionary<string, string> { ["name"] = "Asha", ["city"] = "Pune" });
using HttpResponseMessage posted = await http.PostAsync("echo", form);
Console.WriteLine($"POST /echo -> {await posted.Content.ReadAsStringAsync()}");

// 6. Timeouts and retries: a slow endpoint plus a cancellation token.
using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
try
{
    await http.GetStringAsync("slow", cts.Token);
}
catch (TaskCanceledException)
{
    Console.WriteLine("GET /slow -> gave up after 300 ms");
}

// 7. A simple retry loop for flaky endpoints (fails twice, then works).
for (int attempt = 1; attempt <= 3; attempt++)
{
    using HttpResponseMessage r = await http.GetAsync("flaky");
    Console.WriteLine($"GET /flaky attempt {attempt} -> {(int)r.StatusCode}");
    if (r.IsSuccessStatusCode) break;
    await Task.Delay(100 * attempt); // back off a little more each time
}

// A minimal web server built on HttpListener, used only to make this example self-contained.
sealed class FakeServer : IDisposable
{
    private readonly HttpListener _listener = new();
    private int _flakyCalls;

    public static FakeServer Start(string prefix)
    {
        var server = new FakeServer();
        server._listener.Prefixes.Add(prefix);
        server._listener.Start();
        _ = Task.Run(server.LoopAsync);
        return server;
    }

    private async Task LoopAsync()
    {
        while (_listener.IsListening)
        {
            HttpListenerContext ctx;
            try { ctx = await _listener.GetContextAsync(); } catch { return; }
            _ = Task.Run(() => HandleAsync(ctx));
        }
    }

    private async Task HandleAsync(HttpListenerContext ctx)
    {
        string path = ctx.Request.Url!.AbsolutePath;
        (int code, string type, string body) = path switch
        {
            "/hello" => (200, "text/plain", "Hello from the local server"),
            "/status" => (200, "application/json", """{"status":"ok","uptime":42}"""),
            "/echo" => (200, "text/plain", "you sent " + await new StreamReader(ctx.Request.InputStream).ReadToEndAsync()),
            "/slow" => await SlowAsync(),
            "/flaky" => Interlocked.Increment(ref _flakyCalls) < 3 ? (503, "text/plain", "busy") : (200, "text/plain", "ok"),
            _ => (404, "text/plain", "not found")
        };
        ctx.Response.StatusCode = code;
        ctx.Response.ContentType = type;
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(body);
        try { await ctx.Response.OutputStream.WriteAsync(bytes); ctx.Response.Close(); } catch { /* client gave up */ }
    }

    private static async Task<(int, string, string)> SlowAsync()
    {
        await Task.Delay(2000);
        return (200, "text/plain", "finally");
    }

    public void Dispose() => _listener.Stop();
}
