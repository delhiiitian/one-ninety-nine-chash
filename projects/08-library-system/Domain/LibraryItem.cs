namespace LibrarySystem.Domain;

/// <summary>Anything stored by id in a repository.</summary>
public interface IEntity
{
    string Id { get; }
}

/// <summary>
/// Base class for everything the library lends. Each kind decides its own loan period
/// and late fee (polymorphism), while shared rules live here once (inheritance).
/// </summary>
public abstract class LibraryItem(string id, string title, int year) : IEntity
{
    public string Id { get; } = id;
    public string Title { get; } = title;
    public int Year { get; } = year;

    public abstract int LoanDays { get; }
    public abstract decimal DailyLateFee { get; }
    public virtual string Kind => GetType().Name;

    public decimal LateFee(int daysLate) => daysLate <= 0 ? 0 : Math.Min(daysLate * DailyLateFee, MaxFee);
    protected virtual decimal MaxFee => 200m;

    public override string ToString() => $"{Kind} '{Title}' ({Year})";
}

public sealed class Book(string id, string title, int year, string author, int pages) : LibraryItem(id, title, year)
{
    public string Author { get; } = author;
    public int Pages { get; } = pages;
    public override int LoanDays => 21;
    public override decimal DailyLateFee => 5m;
}

public sealed class Dvd(string id, string title, int year, int minutes) : LibraryItem(id, title, year)
{
    public int Minutes { get; } = minutes;
    public override int LoanDays => 7;
    public override decimal DailyLateFee => 20m;
    public override string Kind => "DVD";
}

public sealed class Magazine(string id, string title, int year, int issue) : LibraryItem(id, title, year)
{
    public int Issue { get; } = issue;
    public override int LoanDays => 3;
    public override decimal DailyLateFee => 2m;
    protected override decimal MaxFee => 20m;
}
