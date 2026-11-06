// 2.2 The switch statement
// Run: dotnet run chapter-02-control-flow/02-switch-statement.cs

// A menu-driven program: each choice runs a different block.
string[] choices = { "1", "2", "3", "9", "q" };

foreach (string choice in choices)
{
    Console.Write($"Choice '{choice}': ");
    switch (choice)
    {
        case "1":
            Console.WriteLine("Check balance");
            break;
        case "2":
            Console.WriteLine("Deposit money");
            break;
        case "3":
            Console.WriteLine("Withdraw money");
            break;
        case "q":
        case "Q":                       // two labels share one block
            Console.WriteLine("Goodbye");
            break;
        default:
            Console.WriteLine("Unknown option, try again");
            break;
    }
}

// switch works on enums too. The compiler can warn if you forget a value.
DayOfWeek today = DayOfWeek.Saturday;
switch (today)
{
    case DayOfWeek.Saturday:
    case DayOfWeek.Sunday:
        Console.WriteLine($"{today}: shop opens at 10:00");
        break;
    default:
        Console.WriteLine($"{today}: shop opens at 09:00");
        break;
}

// Case guards (when) add extra conditions.
int temperature = 38;
switch (temperature)
{
    case int t when t >= 40:
        Console.WriteLine("Heatwave warning");
        break;
    case int t when t >= 30:
        Console.WriteLine("Hot day: drink water");
        break;
    default:
        Console.WriteLine("Pleasant weather");
        break;
}
