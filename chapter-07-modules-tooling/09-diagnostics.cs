// 7.9 Debugging, logging and code-quality tools
// Run: dotnet run chapter-07-modules-tooling/09-diagnostics.cs
#:package Microsoft.Extensions.Logging.Console@10.0.0

using System.Diagnostics;
using Microsoft.Extensions.Logging;

// 1. Structured logging with ILogger: levels, categories and named values.
using ILoggerFactory factory = LoggerFactory.Create(builder => builder
    .AddSimpleConsole(o => { o.SingleLine = true; o.TimestampFormat = "HH:mm:ss "; })
    .SetMinimumLevel(LogLevel.Debug));
ILogger log = factory.CreateLogger("Checkout");

log.LogInformation("Order {OrderId} placed by {Customer} for {Amount}", 1042, "asha", 2499.00m);
log.LogDebug("Cart had {Items} items", 3);
log.LogWarning("Stock low for {Sku}: {Left} left", "SKU-778", 2);
try
{
    throw new TimeoutException("Payment gateway did not respond");
}
catch (Exception ex)
{
    log.LogError(ex, "Payment failed for order {OrderId}", 1042);
}

// 2. Measuring time with Stopwatch: find the slow part before optimising.
var sw = Stopwatch.StartNew();
long sum = 0;
for (int i = 0; i < 5_000_000; i++) sum += i % 7;
sw.Stop();
log.LogInformation("Loop of 5,000,000 took {Ms} ms (sum {Sum})", sw.ElapsedMilliseconds, sum);

// 3. Debug-only checks: Debug.Assert and Debug.WriteLine vanish from Release builds.
int stock = 5;
Debug.Assert(stock >= 0, "Stock must never be negative");
Debug.WriteLine($"Debug output goes to the debugger, not the console (stock = {stock})");

// 4. Caller information: methods can learn who called them, which helps in log messages.
Trace("about to ship");

// 5. Code-quality tools you run from the terminal:
//      dotnet format            fixes spacing and style using .editorconfig rules
//      dotnet build -warnaserror  treats every warning as an error (common in CI)
//      dotnet test --collect:"XPlat Code Coverage"  measures how much code your tests cover
// In an IDE, set a breakpoint (F9), start debugging (F5) and step through lines (F10/F11).

static void Trace(string message,
    [System.Runtime.CompilerServices.CallerMemberName] string member = "",
    [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
{
    Console.WriteLine($"TRACE {message} (called from {member}, line {line})");
}
