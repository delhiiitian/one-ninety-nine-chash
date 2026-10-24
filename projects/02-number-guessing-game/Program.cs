// Project 2: Number Guessing Game
// Uses Chapter 2 ideas: if/else, switch expressions, while, do-while, for, break/continue, ?? and ?:
// Run: dotnet run --project projects/02-number-guessing-game
// Optional seed for repeatable games: dotnet run --project projects/02-number-guessing-game -- 42

// ---------------------------------------------------------------
// 1. Setup
// ---------------------------------------------------------------
var random = args.Length > 0 && int.TryParse(args[0], out int seed)
    ? new Random(seed)       // same secret numbers every time (handy for testing)
    : new Random();

int gamesPlayed = 0;
int gamesWon = 0;
int? bestScore = null;       // fewest guesses in a winning game; null until someone wins

Console.WriteLine("=== Number Guessing Game ===");

// ---------------------------------------------------------------
// 2. Game loop: one pass = one full game
// ---------------------------------------------------------------
bool playAgain;
do
{
    // 2a. Pick a difficulty (switch expression turns a letter into settings)
    Console.Write("Difficulty: (e)asy, (m)edium, (h)ard? ");
    string level = Console.ReadLine()?.Trim().ToLower() ?? "q";
    if (level == "q") break;                       // input ended

    (int maxNumber, int maxGuesses) = level switch
    {
        "e" or "easy" => (20, 6),
        "h" or "hard" => (200, 8),
        _ => (100, 7)                               // medium is the default
    };

    int secret = random.Next(1, maxNumber + 1);
    Console.WriteLine($"I picked a number from 1 to {maxNumber}. You have {maxGuesses} guesses.");

    // 2b. Guessing loop: a for loop, because the number of tries is limited
    bool won = false;
    int guessesUsed = 0;
    for (int attempt = 1; attempt <= maxGuesses; attempt++)
    {
        Console.Write($"Guess {attempt}/{maxGuesses}: ");
        string? input = Console.ReadLine();
        if (input is null) break;                   // input ended mid-game

        if (!int.TryParse(input, out int guess) || guess < 1 || guess > maxNumber)
        {
            Console.WriteLine($"  Please type a whole number from 1 to {maxNumber}. (That one did not count.)");
            attempt--;                              // do not use up a turn
            continue;
        }

        guessesUsed = attempt;
        int distance = Math.Abs(guess - secret);

        if (guess == secret)
        {
            won = true;
            break;
        }

        string direction = guess < secret ? "higher" : "lower";
        string hint = distance switch
        {
            <= 3 => "very hot",
            <= 10 => "warm",
            <= 25 => "cool",
            _ => "ice cold"
        };
        Console.WriteLine($"  Go {direction}. You are {hint}.");
    }

    // 2c. Result and statistics
    gamesPlayed++;
    if (won)
    {
        gamesWon++;
        Console.WriteLine($"  Correct! {secret} in {guessesUsed} guess{(guessesUsed == 1 ? "" : "es")}.");
        if (bestScore is null || guessesUsed < bestScore)
        {
            bestScore = guessesUsed;
            Console.WriteLine("  New best score!");
        }
    }
    else
    {
        Console.WriteLine($"  Out of guesses. The number was {secret}.");
    }

    Console.Write("Play again? (y/n) ");
    string answer = Console.ReadLine()?.Trim().ToLower() ?? "n";
    playAgain = answer.StartsWith('y');
} while (playAgain);

// ---------------------------------------------------------------
// 3. Summary
// ---------------------------------------------------------------
Console.WriteLine();
Console.WriteLine($"Games played: {gamesPlayed}, won: {gamesWon}");
Console.WriteLine($"Best score: {(bestScore?.ToString() ?? "no wins yet")}");
Console.WriteLine("Thanks for playing!");
