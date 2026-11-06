// 2.5 The do-while loop
// Run: dotnet run chapter-02-control-flow/05-do-while.cs
// Try: echo -e "abc\n0\n7" | dotnet run chapter-02-control-flow/05-do-while.cs

// do-while runs the body first and checks afterwards, so it always runs at least once.
// That fits "ask until the answer is valid".
int tickets;
bool valid;
int attempts = 0;
do
{
    attempts++;
    Console.Write("How many tickets (1-6)? ");
    string? answer = Console.ReadLine();
    if (answer is null)              // input ended: stop asking
    {
        tickets = 1;
        Console.WriteLine("(no input, defaulting to 1)");
        break;
    }
    valid = int.TryParse(answer, out tickets) && tickets is >= 1 and <= 6;
    if (!valid)
        Console.WriteLine("  Please enter a number from 1 to 6.");
} while (!valid);

Console.WriteLine($"Booked {tickets} ticket(s) after {attempts} attempt(s).");

// A dice game round: roll at least once, keep rolling until you get a six.
var random = new Random(7);          // fixed seed: the same rolls every run
int roll, rolls = 0;
do
{
    roll = random.Next(1, 7);
    rolls++;
    Console.WriteLine($"Rolled a {roll}");
} while (roll != 6);
Console.WriteLine($"Got a six after {rolls} roll(s).");
