// 2.9 The ternary operator and null-safe operators
// Run: dotnet run chapter-02-control-flow/09-null-operators.cs

// condition ? valueIfTrue : valueIfFalse - a compact if/else that gives a value.
int unread = 3;
string badge = unread > 0 ? $"{unread} new" : "No new mail";
Console.WriteLine(badge);
Console.WriteLine($"You have {unread} message{(unread == 1 ? "" : "s")}");

// Null means "no value". These operators make null handling short and safe.
string? nickname = null;
string? city = "Chennai";

// ?? : use the right side when the left is null
Console.WriteLine($"Hello, {nickname ?? "guest"}");

// ?. : only call the member if the object is not null (otherwise the result is null)
Console.WriteLine($"Nickname length: {nickname?.Length}");      // prints nothing
Console.WriteLine($"City length: {city?.Length}");

// Combine them: length or 0
int nameLength = nickname?.Length ?? 0;
Console.WriteLine($"Name length or zero: {nameLength}");

// ??= : assign only if currently null (lazy default)
nickname ??= "Champ";
Console.WriteLine($"Nickname now: {nickname}");

// ?[] : safe indexing into a possibly-null array
string[]? tags = null;
Console.WriteLine($"First tag: {tags?[0] ?? "(none)"}");

// Nullable value types: int? can hold a number or null.
int? lastScore = null;
Console.WriteLine(lastScore.HasValue ? $"Last score {lastScore.Value}" : "No games played yet");
lastScore = 87;
Console.WriteLine($"Last score + bonus: {lastScore + 10}");
