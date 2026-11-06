// 5.4 finally: code that always runs
// Run: dotnet run chapter-05-error-handling/04-finally.cs

// finally runs whether the try succeeded, threw, or returned early.
// Use it to release things: close files, unlock doors, stop timers, turn off spinners.
Checkout(cartTotal: 499m, cardBalance: 1000m);
Checkout(cartTotal: 4999m, cardBalance: 1000m);

Console.WriteLine();
Console.WriteLine($"Result: {FindUser("ben")}");
Console.WriteLine($"Result: {FindUser("zoe")}");

static void Checkout(decimal cartTotal, decimal cardBalance)
{
    Console.WriteLine($"--- Checkout for {cartTotal} ---");
    Console.WriteLine("Locking the cart");
    try
    {
        if (cartTotal > cardBalance)
            throw new InvalidOperationException("Card declined");
        Console.WriteLine("Payment accepted");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Payment failed: {ex.Message}");
    }
    finally
    {
        Console.WriteLine("Unlocking the cart (always)");
    }
}

// finally even runs when try returns early.
static string FindUser(string name)
{
    var timer = System.Diagnostics.Stopwatch.StartNew();
    try
    {
        if (name == "ben") return "found ben";
        return "not found";
    }
    finally
    {
        timer.Stop();
        Console.WriteLine($"(lookup for '{name}' took {timer.Elapsed.TotalMilliseconds:F3} ms)");
    }
}
