// 5.7 using and IDisposable: automatic clean-up
// Run: dotnet run chapter-05-error-handling/07-using-dispose.cs

// Objects that hold outside resources (files, network connections, locks) implement IDisposable.
// "using" calls Dispose() for you when the block ends, even if an exception is thrown.

using (var meeting = new MeetingRoom("Room A"))
{
    meeting.Present("Quarterly results");
}

// The modern "using declaration" disposes at the end of the enclosing scope.
try
{
    using var room = new MeetingRoom("Room B");
    room.Present("Roadmap");
    throw new InvalidOperationException("Projector failed!");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}

// Real example: StreamWriter is IDisposable; using guarantees the file is flushed and closed.
string path = Path.Combine(Path.GetTempPath(), "using-demo.txt");
using (var writer = new StreamWriter(path))
{
    writer.WriteLine("Line written inside using");
}
Console.WriteLine($"File says: {File.ReadAllText(path).Trim()}");
File.Delete(path);

class MeetingRoom : IDisposable
{
    private readonly string _name;

    public MeetingRoom(string name)
    {
        _name = name;
        Console.WriteLine($"{_name}: booked and lights on");
    }

    public void Present(string topic) => Console.WriteLine($"{_name}: presenting '{topic}'");

    public void Dispose() => Console.WriteLine($"{_name}: lights off, room released");
}
