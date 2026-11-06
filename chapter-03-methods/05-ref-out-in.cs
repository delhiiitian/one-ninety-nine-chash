// 3.5 ref, out and in parameters
// Run: dotnet run chapter-03-methods/05-ref-out-in.cs

// Normally a method gets a COPY of a value-type argument.
int lives = 3;
LoseLifeCopy(lives);
Console.WriteLine($"After copy version: {lives}");   // still 3

// ref passes the variable itself, so the method can change it.
LoseLife(ref lives);
Console.WriteLine($"After ref version: {lives}");    // 2

// out: the method MUST assign it; used to return extra results.
if (TryDivide(10, 4, out double result))
    Console.WriteLine($"10 / 4 = {result}");
if (!TryDivide(10, 0, out _))                         // _ discards an out value you do not need
    Console.WriteLine("Cannot divide by zero");

SplitName("Priya Sharma", out string first, out string last);
Console.WriteLine($"First: {first}, last: {last}");

// in: pass by reference but read-only (useful for large structs).
var point = (X: 3.0, Y: 4.0);
Console.WriteLine($"Distance from origin: {Distance(in point)}");

// Swapping two variables is the classic ref example.
int a = 1, b = 2;
Swap(ref a, ref b);
Console.WriteLine($"a={a}, b={b}");

static void LoseLifeCopy(int count) => count--;
static void LoseLife(ref int count) => count--;

static bool TryDivide(double x, double y, out double answer)
{
    if (y == 0) { answer = 0; return false; }
    answer = x / y;
    return true;
}

static void SplitName(string fullName, out string firstName, out string lastName)
{
    int space = fullName.IndexOf(' ');
    firstName = space < 0 ? fullName : fullName[..space];
    lastName = space < 0 ? "" : fullName[(space + 1)..];
}

static double Distance(in (double X, double Y) p) => Math.Sqrt(p.X * p.X + p.Y * p.Y);

static void Swap(ref int x, ref int y) => (x, y) = (y, x);
