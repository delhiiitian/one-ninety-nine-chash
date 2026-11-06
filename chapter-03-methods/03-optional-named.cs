// 3.3 Optional and named arguments
// Run: dotnet run chapter-03-methods/03-optional-named.cs

// Optional parameters have a default value, so callers can leave them out.
Console.WriteLine(FormatPrice(250m));
Console.WriteLine(FormatPrice(250m, "EUR"));
Console.WriteLine(FormatPrice(250m, "INR", decimals: 0));

// Named arguments make calls self-explaining and let you skip optional ones.
SendNotification("Your order shipped");
SendNotification("Payment failed", urgent: true);
SendNotification(message: "Weekly digest", channel: "email", urgent: false);

// Without names, a call like this is hard to read:
BookRoom("Deluxe", 2, true, false);
// With names it documents itself:
BookRoom(roomType: "Deluxe", nights: 2, breakfast: true, lateCheckout: false);

static string FormatPrice(decimal amount, string currency = "USD", int decimals = 2)
{
    return $"{currency} {amount.ToString("F" + decimals)}";
}

static void SendNotification(string message, string channel = "push", bool urgent = false)
{
    string prefix = urgent ? "[URGENT] " : "";
    Console.WriteLine($"({channel}) {prefix}{message}");
}

static void BookRoom(string roomType, int nights, bool breakfast, bool lateCheckout)
{
    Console.WriteLine($"Booked {roomType} for {nights} night(s); breakfast: {breakfast}; late checkout: {lateCheckout}");
}
