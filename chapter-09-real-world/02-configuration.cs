// 9.2 Environment variables and configuration
// Run: dotnet run chapter-09-real-world/02-configuration.cs
// Try: SHOP__Currency=EUR dotnet run chapter-09-real-world/02-configuration.cs -- --Shop:MaxCartItems=5
#:package Microsoft.Extensions.Configuration@10.0.0
#:package Microsoft.Extensions.Configuration.Json@10.0.0
#:package Microsoft.Extensions.Configuration.EnvironmentVariables@10.0.0
#:package Microsoft.Extensions.Configuration.CommandLine@10.0.0
#:package Microsoft.Extensions.Configuration.Binder@10.0.0
#:property PublishAot=false

using Microsoft.Extensions.Configuration;

// 1. Read a single environment variable directly.
string user = Environment.GetEnvironmentVariable("USER") ?? Environment.GetEnvironmentVariable("USERNAME") ?? "unknown";
string? apiKey = Environment.GetEnvironmentVariable("SHOP_API_KEY");
Console.WriteLine($"Running as {user}; API key {(apiKey is null ? "not set" : "set (" + apiKey.Length + " chars)")}");

// 2. Write a sample appsettings.json next to the program (a real app ships this file).
string settingsPath = Path.Combine(Path.GetTempPath(), "c199-appsettings.json");
File.WriteAllText(settingsPath, """
{
  "Shop": {
    "Name": "Chai Corner",
    "Currency": "INR",
    "MaxCartItems": 20,
    "Features": { "GiftWrap": true, "Coupons": false }
  },
  "ConnectionStrings": { "Main": "Data Source=shop.db" }
}
""");

// 3. Layer sources: JSON file, then environment variables (prefix SHOP__), then command line.
//    Later sources win, so ops can override the file without editing it.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile(settingsPath, optional: false)
    .AddEnvironmentVariables(prefix: "")      // SHOP__Currency maps to Shop:Currency
    .AddCommandLine(args)                     // --Shop:MaxCartItems=5
    .Build();

// 4. Read values by key path ("Section:Key").
Console.WriteLine($"Shop name: {config["Shop:Name"]}");
Console.WriteLine($"Currency : {config["Shop:Currency"]}");
Console.WriteLine($"DB       : {config.GetConnectionString("Main")}");

// 5. Bind a whole section to a strongly typed class.
ShopOptions shop = config.GetSection("Shop").Get<ShopOptions>() ?? new ShopOptions();
Console.WriteLine($"Typed    : {shop.Name}, max {shop.MaxCartItems} items, gift wrap {shop.Features.GiftWrap}, coupons {shop.Features.Coupons}");

// 6. Missing keys give null, so supply defaults.
int timeout = config.GetValue("Shop:TimeoutSeconds", 30);
Console.WriteLine($"Timeout  : {timeout}s (default used)");

File.Delete(settingsPath);

class ShopOptions
{
    public string Name { get; set; } = "";
    public string Currency { get; set; } = "USD";
    public int MaxCartItems { get; set; }
    public FeatureFlags Features { get; set; } = new();
}

class FeatureFlags
{
    public bool GiftWrap { get; set; }
    public bool Coupons { get; set; }
}
