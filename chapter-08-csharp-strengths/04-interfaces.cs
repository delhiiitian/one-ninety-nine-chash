// 8.4 Interfaces
// Run: dotnet run chapter-08-csharp-strengths/04-interfaces.cs

// An interface is a contract: "anything that can do X". Unrelated classes can sign it.
List<INotifier> channels = [new EmailNotifier("asha@example.com"), new SmsNotifier("+91 98450 11111"), new PushNotifier()];

foreach (INotifier channel in channels)
    channel.Send("Your order #1042 has shipped");

// Code that depends on the interface works with any implementation, including new ones.
var checkout = new Checkout(new SmsNotifier("+91 90000 22222"));
checkout.Complete(orderId: 1043);

// A class can implement several interfaces.
var device = new SmartSpeaker();
Console.WriteLine($"Speaker can notify: {device is INotifier}, can play audio: {device is IPlayable}");
device.Play("Morning playlist");
device.Send("Timer done");

// Built-in interfaces matter too: IComparable lets List.Sort order your objects.
List<Parcel> parcels = [new(3.2), new(0.5), new(12.0)];
parcels.Sort();
Console.WriteLine($"Parcels by weight: {string.Join(", ", parcels)}");

interface INotifier
{
    string Channel { get; }
    void Send(string message);

    // Default implementation: every notifier gets this for free.
    void SendUrgent(string message) => Send($"URGENT: {message}");
}

interface IPlayable
{
    void Play(string what);
}

class EmailNotifier(string address) : INotifier
{
    public string Channel => "email";
    public void Send(string message) => Console.WriteLine($"[email to {address}] {message}");
}

class SmsNotifier(string number) : INotifier
{
    public string Channel => "sms";
    public void Send(string message) => Console.WriteLine($"[sms to {number}] {message[..Math.Min(message.Length, 30)]}");
}

class PushNotifier : INotifier
{
    public string Channel => "push";
    public void Send(string message) => Console.WriteLine($"[push] {message}");
}

class SmartSpeaker : INotifier, IPlayable
{
    public string Channel => "speaker";
    public void Send(string message) => Console.WriteLine($"[speaker says] {message}");
    public void Play(string what) => Console.WriteLine($"[speaker plays] {what}");
}

// Checkout only knows the contract, not the concrete class: easy to swap or test.
class Checkout(INotifier notifier)
{
    public void Complete(int orderId)
    {
        Console.WriteLine($"Order {orderId} paid.");
        notifier.SendUrgent($"Payment received for order {orderId}");
    }
}

record Parcel(double Kg) : IComparable<Parcel>
{
    public int CompareTo(Parcel? other) => Kg.CompareTo(other?.Kg ?? 0);
    public override string ToString() => $"{Kg} kg";
}
