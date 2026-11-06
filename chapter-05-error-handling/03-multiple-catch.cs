// 5.3 Multiple catch blocks and exception filters
// Run: dotnet run chapter-05-error-handling/03-multiple-catch.cs

// Several catch blocks are checked top to bottom. Put specific types before general ones.
string[] inputs = { "4", "0", "x", "99999999999" };
foreach (string input in inputs)
{
    try
    {
        int people = int.Parse(input);
        int share = 1200 / people;
        Console.WriteLine($"{input,-12} -> each pays {share}");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine($"{input,-12} -> need at least one person");
    }
    catch (FormatException)
    {
        Console.WriteLine($"{input,-12} -> not a whole number");
    }
    catch (OverflowException)
    {
        Console.WriteLine($"{input,-12} -> number too large");
    }
    catch (Exception ex)                 // last resort: anything else
    {
        Console.WriteLine($"{input,-12} -> unexpected: {ex.Message}");
    }
}

// Exception filters (when) catch only when a condition holds.
int[] statusCodes = { 404, 503, 500, 401 };
foreach (int code in statusCodes)
{
    try
    {
        CallServer(code);
    }
    catch (HttpError ex) when (ex.StatusCode is 500 or 503)
    {
        Console.WriteLine($"{ex.StatusCode}: server problem, will retry later");
    }
    catch (HttpError ex) when (ex.StatusCode == 401)
    {
        Console.WriteLine($"{ex.StatusCode}: please log in again");
    }
    catch (HttpError ex)
    {
        Console.WriteLine($"{ex.StatusCode}: {ex.Message}");
    }
}

static void CallServer(int simulatedStatus) =>
    throw new HttpError(simulatedStatus, simulatedStatus == 404 ? "page not found" : "request failed");

class HttpError(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
