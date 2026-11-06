// 3.4 Method overloading
// Run: dotnet run chapter-03-methods/04-overloading.cs

// Overloads: same name, different parameter lists. The compiler picks the best match.
Console.WriteLine(Shapes.Area(5));               // square
Console.WriteLine(Shapes.Area(4, 6));            // rectangle
Console.WriteLine(Shapes.Area(3.0));             // circle (a double radius)
Console.WriteLine(Shapes.Area(3, 4, 5));         // triangle from three sides

// Console.WriteLine itself has many overloads: string, int, bool, decimal, and more.
Logger.Log("Server started");
Logger.Log("Disk usage", 87.5);
Logger.Log(new Exception("Connection lost"));

// Note: local functions (methods written among top-level statements) cannot be overloaded,
// so overloads live in a class. A static class is just a named home for methods.
static class Shapes
{
    public static int Area(int side) => side * side;

    public static int Area(int width, int height) => width * height;

    public static double Area(double radius) => Math.Round(Math.PI * radius * radius, 2);

    public static double Area(int a, int b, int c)
    {
        // Heron's formula
        double s = (a + b + c) / 2.0;
        return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
    }
}

static class Logger
{
    public static void Log(string message) => Console.WriteLine($"INFO   {message}");
    public static void Log(string metric, double value) => Console.WriteLine($"METRIC {metric} = {value}%");
    public static void Log(Exception error) => Console.WriteLine($"ERROR  {error.Message}");
}
