// The bank holds all accounts and performs operations that touch more than one account.

class Bank(AuditLog log)
{
    private readonly Dictionary<string, Account> _accounts = new(StringComparer.OrdinalIgnoreCase);

    public void Open(string id, string owner, decimal opening)
    {
        if (_accounts.ContainsKey(id)) throw new BankException($"Account {id} already exists.");
        _accounts[id] = new Account(id, owner, opening);
        log.Write($"OPEN {id} {owner} {opening:F2}");
    }

    public Account Get(string id) =>
        _accounts.TryGetValue(id, out Account? account) ? account : throw new AccountNotFoundException(id);

    public void Deposit(string id, decimal amount)
    {
        Get(id).Deposit(amount);
        log.Write($"DEPOSIT {id} {amount:F2}");
    }

    public void Withdraw(string id, decimal amount)
    {
        Get(id).Withdraw(amount);
        log.Write($"WITHDRAW {id} {amount:F2}");
    }

    /// <summary>Moves money between accounts. Either both sides happen or neither does.</summary>
    public void Transfer(string fromId, string toId, decimal amount)
    {
        if (string.Equals(fromId, toId, StringComparison.OrdinalIgnoreCase))
            throw new BankException("Cannot transfer to the same account.");

        Account from = Get(fromId);
        Account to = Get(toId);        // look up both first: a typo fails before any money moves

        from.Withdraw(amount);
        try
        {
            to.Deposit(amount);
        }
        catch
        {
            from.Deposit(amount);       // roll back the withdrawal, then let the error continue
            log.Write($"ROLLBACK {fromId}->{toId} {amount:F2}");
            throw;
        }
        log.Write($"TRANSFER {fromId}->{toId} {amount:F2}");
    }

    public void Freeze(string id)
    {
        Get(id).Freeze();
        log.Write($"FREEZE {id}");
    }

    public IEnumerable<Account> All => _accounts.Values;
}
