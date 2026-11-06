// 9.9 Dependency injection, logging and hosting
// Run: dotnet run chapter-09-real-world/09-dependency-injection.cs
#:package Microsoft.Extensions.Hosting@10.0.0
#:property PublishAot=false

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// 1. The host builder gives you a DI container, logging and configuration in one place.
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(o => { o.SingleLine = true; o.IncludeScopes = false; });

// 2. Register services: "when someone asks for X, give them Y".
builder.Services.AddSingleton<IClock, FixedClock>();          // one instance for the whole app
builder.Services.AddScoped<IOrderRepository, InMemoryOrderRepository>(); // one per scope (per web request)
builder.Services.AddTransient<IPaymentGateway, FakeUpiGateway>(); // a new one every time
builder.Services.AddScoped<CheckoutService>();

using IHost host = builder.Build();

// 3. Resolve the top-level service. The container builds its whole dependency tree for you.
for (int request = 1; request <= 2; request++)
{
    using IServiceScope scope = host.Services.CreateScope();   // like one web request
    var checkout = scope.ServiceProvider.GetRequiredService<CheckoutService>();
    checkout.PlaceOrder(customer: request == 1 ? "asha" : "ravi", amount: request == 1 ? 499m : 0m);
}

// 4. Lifetimes in action: singletons are shared, scoped differ per scope, transients differ per request.
//    The console logger writes on a background thread, so we log these lines too to keep the order.
ILogger log = host.Services.GetRequiredService<ILogger<Program>>();
using (IServiceScope a = host.Services.CreateScope())
using (IServiceScope b = host.Services.CreateScope())
{
    log.LogInformation("Same clock in both scopes?      {Answer}", ReferenceEquals(a.ServiceProvider.GetRequiredService<IClock>(), b.ServiceProvider.GetRequiredService<IClock>()));
    log.LogInformation("Same repository in both scopes? {Answer}", ReferenceEquals(a.ServiceProvider.GetRequiredService<IOrderRepository>(), b.ServiceProvider.GetRequiredService<IOrderRepository>()));
    log.LogInformation("Same gateway twice in one scope? {Answer}", ReferenceEquals(a.ServiceProvider.GetRequiredService<IPaymentGateway>(), a.ServiceProvider.GetRequiredService<IPaymentGateway>()));
}

// --- Abstractions ---
interface IClock { DateTimeOffset Now { get; } }
interface IOrderRepository { int Save(string customer, decimal amount); }
interface IPaymentGateway { bool Charge(string customer, decimal amount); }

// --- Implementations ---
class FixedClock : IClock { public DateTimeOffset Now => new(2026, 11, 6, 10, 0, 0, TimeSpan.Zero); }

class InMemoryOrderRepository : IOrderRepository
{
    private int _nextId = 1000;
    public int Save(string customer, decimal amount) => _nextId++;
}

class FakeUpiGateway(ILogger<FakeUpiGateway> log) : IPaymentGateway
{
    public bool Charge(string customer, decimal amount)
    {
        log.LogDebug("Charging {Customer} {Amount}", customer, amount);
        return amount > 0;
    }
}

// 5. Constructor injection: the class lists what it needs; it never calls "new" on its dependencies.
class CheckoutService(IOrderRepository orders, IPaymentGateway payments, IClock clock, ILogger<CheckoutService> log)
{
    public void PlaceOrder(string customer, decimal amount)
    {
        if (!payments.Charge(customer, amount))
        {
            log.LogWarning("Payment failed for {Customer} (amount {Amount})", customer, amount);
            return;
        }
        int id = orders.Save(customer, amount);
        log.LogInformation("Order {OrderId} for {Customer} at {Time:HH:mm}", id, customer, clock.Now);
    }
}
