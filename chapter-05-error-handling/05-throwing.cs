// 5.5 Throwing and rethrowing exceptions
// Run: dotnet run chapter-05-error-handling/05-throwing.cs

// throw signals that a method cannot do its job with the input it was given.
try
{
    SetAge(-4);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Rejected: {ex.Message}");
}

// Throw expressions: throw inside ?? or ?: for compact checks.
string? configured = null;
try
{
    string apiUrl = configured ?? throw new InvalidOperationException("API_URL is not set");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Config error: {ex.Message}");
}

// Rethrow with "throw;" to log and pass the error up, keeping the original stack trace.
try
{
    ImportOrders();
}
catch (Exception ex)
{
    Console.WriteLine($"Top level caught {ex.GetType().Name}: {ex.Message}");
    // Wrapped exceptions keep the original as InnerException.
    if (ex.InnerException is not null)
        Console.WriteLine($"  caused by {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
}

static void SetAge(int age)
{
    if (age < 0 || age > 150)
        throw new ArgumentOutOfRangeException(nameof(age), age, "Age must be between 0 and 150.");
    Console.WriteLine($"Age set to {age}");
}

static void ImportOrders()
{
    try
    {
        ParseLine("ORD-1,ten,250");
    }
    catch (FormatException ex)
    {
        Console.WriteLine("ImportOrders: logging the failure, then passing it on");
        // Add context by wrapping: the caller sees which step failed and why.
        throw new InvalidDataException("Could not import orders.csv line 1", ex);
    }
}

static void ParseLine(string line)
{
    string[] parts = line.Split(',');
    try
    {
        int quantity = int.Parse(parts[1]);
    }
    catch (FormatException)
    {
        Console.WriteLine("ParseLine: bad quantity, rethrowing");
        throw;    // NOT "throw ex;" which would reset the stack trace
    }
}
