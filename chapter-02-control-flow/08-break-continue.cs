// 2.8 break, continue and loop control
// Run: dotnet run chapter-02-control-flow/08-break-continue.cs

// continue skips the rest of this pass; break leaves the loop completely.
int[] readings = { 21, 22, -999, 23, 24, 85, 25 };   // -999 is a sensor error

foreach (int reading in readings)
{
    if (reading == -999)
    {
        Console.WriteLine("Skipping bad reading");
        continue;
    }
    if (reading > 60)
    {
        Console.WriteLine($"ALARM: {reading} degrees. Stopping the scan.");
        break;
    }
    Console.WriteLine($"Temperature OK: {reading}");
}

// Search: stop as soon as you find what you want.
string[] emails = { "a@x.com", "b@x.com", "boss@x.com", "c@x.com" };
int foundAt = -1;
for (int i = 0; i < emails.Length; i++)
{
    if (emails[i].StartsWith("boss"))
    {
        foundAt = i;
        break;
    }
}
Console.WriteLine(foundAt >= 0 ? $"Found boss at position {foundAt}" : "Not found");

// Infinite loop with an exit: a simple menu loop.
string[] simulatedKeys = { "h", "x", "h", "q", "h" };
int pos = 0;
while (true)
{
    string key = simulatedKeys[pos++];
    if (key == "q") { Console.WriteLine("Quit pressed"); break; }
    if (key != "h") { Console.WriteLine($"Ignoring '{key}'"); continue; }
    Console.WriteLine("Help: press q to quit");
}

// Breaking out of nested loops: use a flag (or put the loops in a method and return).
bool done = false;
for (int r = 0; r < 3 && !done; r++)
    for (int c = 0; c < 3; c++)
        if (r * c == 2) { Console.WriteLine($"First product of 2 at ({r},{c})"); done = true; break; }
