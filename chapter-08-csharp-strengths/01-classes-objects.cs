// 8.1 Classes and objects
// Run: dotnet run chapter-08-csharp-strengths/01-classes-objects.cs

// A class is a blueprint. Each object made with "new" has its own data (fields)
// and shares the behaviour (methods).
var phone = new Battery("Phone", capacityMah: 4500);
var watch = new Battery("Watch", capacityMah: 300);

phone.Use(1200);
watch.Use(250);
phone.Charge(500);

Console.WriteLine(phone.Report());
Console.WriteLine(watch.Report());
Console.WriteLine($"Batteries created so far: {Battery.Count}");

// Two variables can point at the SAME object: classes are reference types.
Battery alias = phone;
alias.Use(1000);
Console.WriteLine($"After using 'alias', phone shows: {phone.Report()}");
Console.WriteLine($"Same object? {ReferenceEquals(alias, phone)}");

class Battery
{
    // Fields: the data each object carries. private = hidden from outside code.
    private readonly string _device;
    private readonly int _capacityMah;
    private int _chargeMah;

    // static: one value shared by the whole class, not per object.
    public static int Count { get; private set; }

    // Constructor: runs once when the object is created, sets up a valid state.
    public Battery(string device, int capacityMah)
    {
        _device = device;
        _capacityMah = capacityMah;
        _chargeMah = capacityMah;        // starts full
        Count++;
    }

    // Methods: what an object can do. They work on that object's own fields.
    public void Use(int mah) => _chargeMah = Math.Max(0, _chargeMah - mah);

    public void Charge(int mah) => _chargeMah = Math.Min(_capacityMah, _chargeMah + mah);

    public int Percent => (int)Math.Round(100.0 * _chargeMah / _capacityMah);

    public string Report() => $"{_device,-6} {Percent,3}% ({_chargeMah}/{_capacityMah} mAh)";
}
