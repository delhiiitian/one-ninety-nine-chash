// Writes every successful operation to a log file.
// It owns an open file, so it implements IDisposable and Program.cs creates it with "using".

class AuditLog : IDisposable
{
    private readonly StreamWriter _writer;
    public string Path { get; }

    public AuditLog(string path)
    {
        Path = path;
        _writer = new StreamWriter(path, append: false) { AutoFlush = true };
    }

    public void Write(string entry) => _writer.WriteLine($"{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ} {entry}");

    public void Dispose()
    {
        _writer.Dispose();
        Console.WriteLine($"Audit log closed: {Path}");
    }
}
