using LibrarySystem.Domain;
using LibrarySystem.Infrastructure;

namespace LibrarySystem.Services;

public sealed class ItemOverdueEventArgs(Loan loan, LibraryItem item, Member member, int daysLate) : EventArgs
{
    public Loan Loan { get; } = loan;
    public LibraryItem Item { get; } = item;
    public Member Member { get; } = member;
    public int DaysLate { get; } = daysLate;
}

/// <summary>The library's rules. Storage and messaging come in through interfaces.</summary>
public sealed class LibraryService(IRepository<LibraryItem> items, IRepository<Member> members, Func<DateOnly> today)
{
    private readonly List<Loan> _loans = new();

    public event EventHandler<ItemOverdueEventArgs>? ItemOverdue;

    public IReadOnlyList<Loan> Loans => _loans;

    public Loan Borrow(string memberId, string itemId)
    {
        Member member = members.Find(memberId) ?? throw new KeyNotFoundException($"No member '{memberId}'.");
        LibraryItem item = items.Find(itemId) ?? throw new KeyNotFoundException($"No item '{itemId}'.");

        if (member.FeesOwed > 0)
            throw new InvalidOperationException($"{member.Name} owes {member.FeesOwed:N2} in fees.");
        if (_loans.Any(l => l.IsOpen && l.ItemId == item.Id))
            throw new InvalidOperationException($"{item.Title} is already on loan.");
        int open = _loans.Count(l => l.IsOpen && l.MemberId == member.Id);
        if (open >= member.BorrowLimit)
            throw new InvalidOperationException($"{member.Name} has reached the limit of {member.BorrowLimit} items.");

        var loan = new Loan(item.Id, member.Id, today(), today().AddDays(item.LoanDays));
        _loans.Add(loan);
        return loan;
    }

    public decimal Return(string itemId)
    {
        int index = _loans.FindIndex(l => l.IsOpen && l.ItemId.Equals(itemId, StringComparison.OrdinalIgnoreCase));
        if (index < 0) throw new InvalidOperationException($"Item '{itemId}' is not on loan.");

        Loan closed = _loans[index] with { ReturnedOn = today() };
        _loans[index] = closed;
        decimal fee = items.Find(closed.ItemId)!.LateFee(closed.DaysLate(today()));
        if (fee > 0) members.Find(closed.MemberId)!.Charge(fee);
        return fee;
    }

    /// <summary>Raises ItemOverdue once for every open loan past its due date.</summary>
    public int CheckOverdue()
    {
        int count = 0;
        foreach (Loan loan in _loans.Where(l => l.IsOpen && l.DaysLate(today()) > 0))
        {
            count++;
            ItemOverdue?.Invoke(this, new ItemOverdueEventArgs(
                loan, items.Find(loan.ItemId)!, members.Find(loan.MemberId)!, loan.DaysLate(today())));
        }
        return count;
    }

    // ---------------- Reports (LINQ) ----------------

    public IEnumerable<LibraryItem> Available() =>
        items.All().Where(i => !_loans.Any(l => l.IsOpen && l.ItemId == i.Id)).OrderBy(i => i.Title);

    public IEnumerable<(string Title, int Times)> MostBorrowed(int top) =>
        _loans.GroupBy(l => l.ItemId)
              .Select(g => (Title: items.Find(g.Key)!.Title, Times: g.Count()))
              .OrderByDescending(x => x.Times).ThenBy(x => x.Title)
              .Take(top);

    public IEnumerable<(string Kind, int Count, int OnLoan)> StockByKind() =>
        items.All().GroupBy(i => i.Kind)
             .Select(g => (g.Key, g.Count(), g.Count(i => _loans.Any(l => l.IsOpen && l.ItemId == i.Id))))
             .OrderBy(x => x.Key);

    public IEnumerable<(Member Member, int OpenLoans)> MembersWithLoans() =>
        from m in members.All()
        join l in _loans.Where(l => l.IsOpen) on m.Id equals l.MemberId into open
        orderby m.Name
        select (m, open.Count());
}
