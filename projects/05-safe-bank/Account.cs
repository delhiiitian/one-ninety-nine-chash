// One bank account. The balance can only change through Deposit and Withdraw,
// and both refuse bad input with guard clauses before touching any data.

class Account
{
    public const decimal DailyWithdrawLimit = 20_000m;

    public string Id { get; }
    public string Owner { get; }
    public decimal Balance { get; private set; }
    public bool IsFrozen { get; private set; }
    private decimal _withdrawnToday;

    public Account(string id, string owner, decimal openingBalance = 0m)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(owner);
        ArgumentOutOfRangeException.ThrowIfNegative(openingBalance);
        Id = id;
        Owner = owner;
        Balance = openingBalance;
    }

    public void Deposit(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        if (IsFrozen) throw new AccountFrozenException(Id);
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        if (IsFrozen) throw new AccountFrozenException(Id);
        if (amount > Balance) throw new InsufficientFundsException(Id, Balance, amount);
        if (_withdrawnToday + amount > DailyWithdrawLimit)
            throw new BankException($"Daily limit of {DailyWithdrawLimit:N0} reached for {Id}.");
        Balance -= amount;
        _withdrawnToday += amount;
    }

    public void Freeze() => IsFrozen = true;
}
