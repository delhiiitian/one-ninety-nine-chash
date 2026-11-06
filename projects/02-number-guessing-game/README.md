# Project 2: Number Guessing Game

The computer picks a secret number and you guess it with "higher / lower" and "hot / cold" hints. Three difficulty levels, a play-again loop and a best-score tracker. It uses only Chapter 1 and 2 ideas: `if/else`, switch expressions, `for`, `do-while`, `break`, `continue`, `??` and `?:`.

## Run it

```bash
dotnet run --project projects/02-number-guessing-game
# repeatable game (seed 42) with scripted answers:
dotnet run --project projects/02-number-guessing-game -- 42 < projects/02-number-guessing-game/sample-input.txt
```

## Files

| File | What it does |
| --- | --- |
| `NumberGuessingGame.csproj` | Project file (.NET 10) |
| `Program.cs` | Setup, the game loop (difficulty, guessing, result) and the summary |
| `sample-input.txt` | Two scripted games for seed 42: an easy win in 4, a medium win in 5 |

## Ideas to extend it

- Let the player pick the range themselves.
- Save the best score to a file (Chapter 6).
- Two-player mode: player 1 types the secret, player 2 guesses.
