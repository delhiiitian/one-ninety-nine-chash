namespace LibrarySystem.Domain;

public sealed class Member(string id, string name, string email, bool isPremium = false) : IEntity
{
    public const int StandardLimit = 3;
    public const int PremiumLimit = 6;

    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Email { get; } = email;
    public bool IsPremium { get; } = isPremium;
    public int BorrowLimit => IsPremium ? PremiumLimit : StandardLimit;
    public decimal FeesOwed { get; private set; }

    public void Charge(decimal amount) => FeesOwed += amount;
    public void Pay(decimal amount) => FeesOwed = Math.Max(0, FeesOwed - amount);
}

/// <summary>One borrowing. Immutable: returning creates a new copy with ReturnedOn set.</summary>
public sealed record Loan(string ItemId, string MemberId, DateOnly BorrowedOn, DateOnly DueOn, DateOnly? ReturnedOn = null)
{
    public bool IsOpen => ReturnedOn is null;
    public int DaysLate(DateOnly today) => Math.Max(0, (ReturnedOn ?? today).DayNumber - DueOn.DayNumber);
}
