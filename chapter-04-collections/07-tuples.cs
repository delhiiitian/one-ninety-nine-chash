// 4.7 Tuples
// Run: dotnet run chapter-04-collections/07-tuples.cs

// A tuple groups a few values together without declaring a new type.
(string City, double Lat, double Lon) office = ("Bengaluru", 12.9716, 77.5946);
Console.WriteLine($"{office.City} is at {office.Lat}, {office.Lon}");

// Deconstruct a tuple into separate variables.
var (city, lat, lon) = office;
Console.WriteLine($"Deconstructed: {city} / {lat} / {lon}");

// Methods return tuples to give back more than one value.
var (quotient, remainder) = Divide(17, 5);
Console.WriteLine($"17 / 5 = {quotient} remainder {remainder}");

// Lists of tuples are a quick way to hold small tables of data.
var leaderboard = new List<(string Player, int Score)>
{
    ("Nia", 4200), ("Omar", 5150), ("Priya", 3900),
};
leaderboard.Sort((a, b) => b.Score.CompareTo(a.Score));
foreach (var (player, score) in leaderboard)
    Console.WriteLine($"{player,-6} {score,6:N0}");

// Tuples compare by value, and can be used as dictionary keys.
var distances = new Dictionary<(string From, string To), int>
{
    [("Delhi", "Agra")] = 233,
    [("Mumbai", "Pune")] = 149,
};
Console.WriteLine($"Delhi to Agra: {distances[("Delhi", "Agra")]} km");
Console.WriteLine($"Same tuple? {("a", 1) == ("a", 1)}");

// Swap without a temporary variable.
int x = 1, y = 2;
(x, y) = (y, x);
Console.WriteLine($"x={x}, y={y}");

static (int Quotient, int Remainder) Divide(int a, int b) => (a / b, a % b);
