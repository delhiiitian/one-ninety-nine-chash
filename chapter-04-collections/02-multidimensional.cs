// 4.2 Multidimensional and jagged arrays
// Run: dotnet run chapter-04-collections/02-multidimensional.cs

// A 2D (rectangular) array: rows x columns, like a spreadsheet or a cinema hall.
char[,] seats = new char[4, 6];
for (int row = 0; row < seats.GetLength(0); row++)
    for (int col = 0; col < seats.GetLength(1); col++)
        seats[row, col] = '.';

// Book some seats.
seats[0, 2] = 'X';
seats[0, 3] = 'X';
seats[2, 5] = 'X';

Console.WriteLine("   1 2 3 4 5 6   (X = booked)");
for (int row = 0; row < seats.GetLength(0); row++)
{
    Console.Write($"{(char)('A' + row)}  ");
    for (int col = 0; col < seats.GetLength(1); col++)
        Console.Write($"{seats[row, col]} ");
    Console.WriteLine();
}

int free = 0;
foreach (char seat in seats) if (seat == '.') free++;
Console.WriteLine($"{free} of {seats.Length} seats free");

// A tic-tac-toe board with an initializer.
char[,] board =
{
    { 'X', 'O', 'X' },
    { ' ', 'X', 'O' },
    { 'O', ' ', 'X' },
};
bool diagonalWin = board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2];
Console.WriteLine($"Diagonal win for {board[1, 1]}: {diagonalWin}");

// A jagged array: an array of arrays, where each row can have a different length.
// Example: marks per student, where students took different numbers of tests.
int[][] marks =
{
    new[] { 78, 85, 90 },
    new[] { 66, 72 },
    new[] { 88, 91, 79, 95 },
};
string[] students = { "Arjun", "Bela", "Chen" };
for (int i = 0; i < marks.Length; i++)
    Console.WriteLine($"{students[i],-6} took {marks[i].Length} tests, average {marks[i].Average():F1}");
