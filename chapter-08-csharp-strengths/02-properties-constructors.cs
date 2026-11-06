// 8.2 Properties, constructors and object initializers
// Run: dotnet run chapter-08-csharp-strengths/02-properties-constructors.cs

// Primary constructor + required/init properties + validation in a setter.
var booking = new HotelBooking("Asha Rao", nights: 3)
{
    RoomType = "Deluxe",         // object initializer sets init/settable properties
    Breakfast = true,
};
Console.WriteLine(booking);

booking.Guests = 2;
try
{
    booking.Guests = 9;          // the setter refuses invalid values
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Rejected: {ex.Message.Split('(')[0].Trim()}");
}

// booking.RoomType = "Suite";  // error: init-only after creation
Console.WriteLine($"Total: {booking.Total:N2} for {booking.Guests} guest(s)");

// Chained constructors: a short form that calls the full one.
var walkIn = new HotelBooking("Walk-in guest");
Console.WriteLine(walkIn);

class HotelBooking(string guestName, int nights)
{
    private int _guests = 1;

    // Get-only property from the primary constructor parameter.
    public string GuestName { get; } = guestName;
    public int Nights { get; } = nights > 0 ? nights : throw new ArgumentOutOfRangeException(nameof(nights));

    // init: can be set in the initializer, then never again.
    public string RoomType { get; init; } = "Standard";
    public bool Breakfast { get; init; }

    // A full property with validation in the setter.
    public int Guests
    {
        get => _guests;
        set
        {
            if (value is < 1 or > 4) throw new ArgumentOutOfRangeException(nameof(value), "Rooms hold 1 to 4 guests.");
            _guests = value;
        }
    }

    // Computed (read-only) property: no storage, worked out each time.
    public decimal NightlyRate => RoomType == "Deluxe" ? 4500m : 3000m;
    public decimal Total => Nights * (NightlyRate + (Breakfast ? 400m * Guests : 0m));

    // A second constructor that chains to the primary one.
    public HotelBooking(string guestName) : this(guestName, 1) { }

    public override string ToString() =>
        $"{GuestName}: {Nights} night(s), {RoomType}{(Breakfast ? " + breakfast" : "")}";
}
