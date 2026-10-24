// 2.1 if, else if and else
// Run: dotnet run chapter-02-control-flow/01-if-else.cs

// Decide a delivery charge from the order total, like most shopping sites.
decimal[] orderTotals = { 120m, 450m, 999m, 1500m };

foreach (decimal total in orderTotals)
{
    decimal delivery;

    if (total >= 1000m)
    {
        delivery = 0m;                 // free delivery for big orders
    }
    else if (total >= 500m)
    {
        delivery = 25m;                // reduced charge
    }
    else
    {
        delivery = 50m;                // standard charge
    }

    Console.WriteLine($"Order {total,6}: delivery {delivery,3}, pay {total + delivery}");
}

// Conditions can be combined with && and ||.
int hour = 22;
bool isWeekend = true;
if (hour >= 22 && !isWeekend)
{
    Console.WriteLine("Quiet hours: notifications muted.");
}
else if (hour >= 22)
{
    Console.WriteLine("Late, but it is the weekend: notifications stay on.");
}

// Nested ifs: check the cheap condition first.
string? username = "meera";
string password = "pa55word!";
if (username is not null)
{
    if (password.Length < 8)
        Console.WriteLine("Password too short.");
    else
        Console.WriteLine($"Welcome back, {username}!");
}
