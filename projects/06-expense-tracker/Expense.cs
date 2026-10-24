// One spending entry. A record: immutable, comparable, and serializes to JSON cleanly.
record Expense(int Id, DateOnly Date, decimal Amount, string Category, string Note);
