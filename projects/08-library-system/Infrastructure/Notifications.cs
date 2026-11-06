namespace LibrarySystem.Infrastructure;

public interface INotifier
{
    Task SendAsync(string to, string subject, string body);
}

/// <summary>Pretends to send email: waits like a network call would, then prints.</summary>
public sealed class ConsoleEmailNotifier : INotifier
{
    public int Sent { get; private set; }

    public async Task SendAsync(string to, string subject, string body)
    {
        await Task.Delay(20);                       // simulated network latency
        Sent++;
        Console.WriteLine($"    email -> {to}: {subject} | {body}");
    }
}
