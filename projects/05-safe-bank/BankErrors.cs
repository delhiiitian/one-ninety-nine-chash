// Every problem the bank can report has its own exception type.
// All of them derive from BankException, so the UI can catch the whole family in one place.

class BankException(string message) : Exception(message);

class AccountNotFoundException(string accountId)
    : BankException($"No account with ID '{accountId}'.")
{
    public string AccountId { get; } = accountId;
}

class InsufficientFundsException(string accountId, decimal balance, decimal requested)
    : BankException($"Account {accountId} has {balance:F2}, cannot withdraw {requested:F2}.")
{
    public decimal Balance { get; } = balance;
    public decimal Requested { get; } = requested;
}

class AccountFrozenException(string accountId)
    : BankException($"Account {accountId} is frozen. Contact support.");
