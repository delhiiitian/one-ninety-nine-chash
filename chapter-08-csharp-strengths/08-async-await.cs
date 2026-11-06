// 8.8 async, await and Task
// Run: dotnet run chapter-08-csharp-strengths/08-async-await.cs
using System.Diagnostics;

// Simulated slow services (Task.Delay stands in for network calls).
var clock = Stopwatch.StartNew();

// 1. Sequential: each await waits before starting the next call.
decimal price = await GetPriceAsync("SKU-1");
int stock = await GetStockAsync("SKU-1");
string eta = await GetDeliveryEstimateAsync("411001");
Console.WriteLine($"Sequential: price {price}, stock {stock}, {eta} in {clock.ElapsedMilliseconds} ms");

// 2. Concurrent: start all three, then await them together. Total time = slowest call.
clock.Restart();
Task<decimal> priceTask = GetPriceAsync("SKU-1");
Task<int> stockTask = GetStockAsync("SKU-1");
Task<string> etaTask = GetDeliveryEstimateAsync("411001");
await Task.WhenAll(priceTask, stockTask, etaTask);
Console.WriteLine($"Concurrent: price {priceTask.Result}, stock {stockTask.Result}, {etaTask.Result} in {clock.ElapsedMilliseconds} ms");

// 3. Errors: an exception inside an async method surfaces at the await.
try
{
    await GetStockAsync("SKU-BROKEN");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Caught from async call: {ex.Message}");
}

// 4. Timeouts and cancellation with CancellationToken.
using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(150));
try
{
    await SlowReportAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Report cancelled after 150 ms timeout");
}

// 5. WhenAny: use whichever server answers first.
Task<string> mirrorA = FetchFromAsync("mirror-A", 300);
Task<string> mirrorB = FetchFromAsync("mirror-B", 120);
Task<string> winner = await Task.WhenAny(mirrorA, mirrorB);
Console.WriteLine($"Fastest: {await winner}");

// 6. Process items with limited parallelism.
var processed = new System.Collections.Concurrent.ConcurrentBag<int>();
await Parallel.ForEachAsync(Enumerable.Range(1, 8), new ParallelOptions { MaxDegreeOfParallelism = 4 },
    async (n, token) => { await Task.Delay(50, token); processed.Add(n); });
Console.WriteLine($"Processed {processed.Count} items, 4 at a time");

static async Task<decimal> GetPriceAsync(string sku) { await Task.Delay(200); return 1499m; }

static async Task<int> GetStockAsync(string sku)
{
    await Task.Delay(200);
    if (sku.Contains("BROKEN")) throw new InvalidOperationException($"Stock service has no record of {sku}");
    return 12;
}

static async Task<string> GetDeliveryEstimateAsync(string pin) { await Task.Delay(200); return "2 days"; }

static async Task SlowReportAsync(CancellationToken token)
{
    for (int step = 1; step <= 10; step++)
        await Task.Delay(50, token);      // throws OperationCanceledException when cancelled
}

static async Task<string> FetchFromAsync(string server, int ms) { await Task.Delay(ms); return $"{server} ({ms} ms)"; }
