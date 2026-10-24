// 5.9 Validation without exceptions: Try methods, guard clauses and result objects
// Run: dotnet run chapter-05-error-handling/09-validation-patterns.cs

// Exceptions are for unexpected problems. Expected problems (bad user input) are
// better handled with normal return values. Three common patterns:

// 1. The Try pattern: return bool, give the result through out.
foreach (string input in new[] { "user@example.com", "not-an-email" })
{
    if (TryParseEmail(input, out var email))
        Console.WriteLine($"Valid email, domain is {email.Domain}");
    else
        Console.WriteLine($"'{input}' is not a valid email");
}

// 2. Guard clauses: check inputs at the top of a method and exit early.
Console.WriteLine(Transfer(from: "ACC1", to: "ACC1", amount: 100));
Console.WriteLine(Transfer(from: "ACC1", to: "ACC2", amount: -5));
Console.WriteLine(Transfer(from: "ACC1", to: "ACC2", amount: 250));

// 3. A result object: collects every problem instead of stopping at the first.
var result = ValidateSignup(username: "al", password: "abc", age: 15);
if (!result.IsValid)
{
    Console.WriteLine("Signup has problems:");
    foreach (string error in result.Errors) Console.WriteLine($"  - {error}");
}
var ok = ValidateSignup(username: "alice99", password: "Secur3!pass", age: 30);
Console.WriteLine($"Second signup valid: {ok.IsValid}");

static bool TryParseEmail(string text, out (string User, string Domain) email)
{
    email = default;
    int at = text.IndexOf('@');
    if (at <= 0 || at == text.Length - 1 || !text[(at + 1)..].Contains('.')) return false;
    email = (text[..at], text[(at + 1)..]);
    return true;
}

static string Transfer(string from, string to, decimal amount)
{
    if (from == to) return "Error: cannot transfer to the same account";
    if (amount <= 0) return "Error: amount must be positive";
    if (amount > 10_000) return "Error: daily limit exceeded";
    return $"Transferred {amount} from {from} to {to}";
}

static ValidationResult ValidateSignup(string username, string password, int age)
{
    var errors = new List<string>();
    if (username.Length < 4) errors.Add("Username must be at least 4 characters");
    if (password.Length < 8) errors.Add("Password must be at least 8 characters");
    if (!password.Any(char.IsDigit)) errors.Add("Password needs a digit");
    if (age < 18) errors.Add("You must be 18 or older");
    return new ValidationResult(errors);
}

record ValidationResult(List<string> Errors)
{
    public bool IsValid => Errors.Count == 0;
}
