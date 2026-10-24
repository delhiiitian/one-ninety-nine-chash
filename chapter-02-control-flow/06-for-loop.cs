// 2.6 The for loop
// Run: dotnet run chapter-02-control-flow/06-for-loop.cs

// for (start; keep going while; step) - best when you know the count.
Console.WriteLine("Countdown:");
for (int i = 5; i >= 1; i--)
{
    Console.Write($"{i} ");
}
Console.WriteLine("Lift off!");

// A multiplication table: one loop per row.
int number = 7;
for (int i = 1; i <= 10; i++)
{
    Console.WriteLine($"{number} x {i,2} = {number * i,3}");
}

// Loan repayment schedule: the counter is the month number.
decimal balance = 10000m;
decimal monthlyRate = 0.01m;     // 1% per month
decimal payment = 2000m;
for (int month = 1; balance > 0; month++)
{
    decimal interest = Math.Round(balance * monthlyRate, 2);
    decimal paid = Math.Min(payment, balance + interest);
    balance = balance + interest - paid;
    Console.WriteLine($"Month {month}: interest {interest,7}, paid {paid,8}, left {balance,9}");
}

// Nested for loops draw a pattern (rows x columns).
for (int row = 1; row <= 4; row++)
{
    for (int col = 1; col <= row; col++)
        Console.Write("* ");
    Console.WriteLine();
}
