// 1.6 Operators and expressions
// Run: dotnet run chapter-01-basics/06-operators.cs

// Arithmetic: + - * / %
decimal price = 250m;
int quantity = 4;
decimal subtotal = price * quantity;
decimal discount = subtotal * 0.10m;
decimal total = subtotal - discount;
Console.WriteLine($"Subtotal {subtotal}, discount {discount}, total {total}");

// Precedence: * and / happen before + and -. Use brackets to be clear.
Console.WriteLine($"2 + 3 * 4 = {2 + 3 * 4}");
Console.WriteLine($"(2 + 3) * 4 = {(2 + 3) * 4}");

// Compound assignment and increment.
int score = 10;
score += 5;   // 15
score *= 2;   // 30
score--;      // 29
Console.WriteLine($"Score: {score}");

// Comparison operators give a bool.
int speed = 72, limit = 60;
Console.WriteLine($"Speeding? {speed > limit}  Exactly at limit? {speed == limit}  Not equal? {speed != limit}");

// Modulo (%) answers "is it divisible?" and "what is left over?".
int year = 2028;
bool leap = (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
Console.WriteLine($"{year} is a leap year: {leap}");
int totalMinutes = 135;
Console.WriteLine($"{totalMinutes} minutes = {totalMinutes / 60} h {totalMinutes % 60} min");

// Prefix vs postfix increment.
int a = 5;
int b = a++; // b gets 5, then a becomes 6
int c = ++a; // a becomes 7, then c gets 7
Console.WriteLine($"a={a}, b={b}, c={c}");
