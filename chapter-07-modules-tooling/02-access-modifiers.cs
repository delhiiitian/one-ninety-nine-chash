// 7.2 Access modifiers: public, private, protected, internal
// Run: dotnet run chapter-07-modules-tooling/02-access-modifiers.cs

var account = new SavingsAccount("Neha", 1000m);
account.Deposit(500m);
Console.WriteLine($"{account.Owner}: {account.Balance:N2}");        // public: readable from anywhere
// account.Balance = 1_000_000m;  // error: the setter is private
// account._pin = "0000";        // error: private field
account.AddMonthlyInterest();
Console.WriteLine($"After interest: {account.Balance:N2}");
Console.WriteLine($"Audit id (internal): {account.AuditId}");       // same assembly, so allowed
Console.WriteLine($"PIN check: {account.CheckPin("4321")}");

// Summary of who can see what:
//   public             everyone
//   private            only code inside the same class (the default for members)
//   protected          the class and classes that inherit from it
//   internal           any code in the same project/assembly (the default for top-level types)
//   protected internal same assembly OR derived classes
//   private protected  derived classes in the same assembly only

class BankAccount
{
    private readonly string _pin = "4321";           // nobody outside can read the PIN
    public string Owner { get; }
    public decimal Balance { get; private set; }      // anyone reads, only this class writes
    internal Guid AuditId { get; } = Guid.Empty;      // visible across this project only

    public BankAccount(string owner, decimal opening)
    {
        Owner = owner;
        Balance = opening;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        Balance += amount;
    }

    public bool CheckPin(string attempt) => attempt == _pin;

    // protected: subclasses can use it, outside code cannot.
    protected void ApplyRate(decimal percent) => Balance += Balance * percent / 100m;
}

class SavingsAccount(string owner, decimal opening) : BankAccount(owner, opening)
{
    private const decimal MonthlyRate = 0.5m;
    public void AddMonthlyInterest() => ApplyRate(MonthlyRate);   // allowed: we inherit it
}
