// 2.4 The while loop
// Run: dotnet run chapter-02-control-flow/04-while-loop.cs

// while repeats as long as the condition is true. It checks before each pass.
decimal savings = 0m;
decimal monthlyDeposit = 2500m;
decimal goal = 12000m;
int months = 0;

while (savings < goal)
{
    savings += monthlyDeposit;
    months++;
    Console.WriteLine($"Month {months}: saved {savings}");
}
Console.WriteLine($"Goal of {goal} reached in {months} months.");

// A while loop is the natural choice when you do not know how many passes you need.
// Example: how many times can you halve a 1 GB file before it is under 1 KB?
long size = 1_073_741_824;
int halvings = 0;
while (size >= 1024)
{
    size /= 2;
    halvings++;
}
Console.WriteLine($"Halved {halvings} times to reach {size} bytes.");

// Reading input until the user types an empty line (or input ends).
Console.WriteLine("Enter shopping items, blank line to finish:");
int count = 0;
string? line;
while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
{
    count++;
    Console.WriteLine($"  added #{count}: {line.Trim()}");
}
Console.WriteLine($"{count} item(s) on the list.");
