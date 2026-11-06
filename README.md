# C# in 199 Pages: code

Runnable code for the book **C# in 199 Pages** from the "199" series. The book has 9 chapters of 9 short lessons each, and every chapter ends with a project. Each lesson in the book shows only the key lines and links to the full file here.

Everything targets **.NET 10** and is checked by CI on Linux and macOS.

## Quick start

```bash
# 1. Install the .NET 10 SDK: https://dotnet.microsoft.com/download
dotnet --version                      # 10.0.x

# 2. Run any lesson (a single .cs file is a whole program since .NET 10)
dotnet run chapter-01-basics/01-hello-world.cs

# 3. Run a project
dotnet run --project projects/01-bill-splitter

# 4. Run everything: all 81 lessons, 9 projects and their tests
./run-all.sh
```

## Contents

| Folder | Chapter | Project |
| --- | --- | --- |
| [chapter-01-basics](chapter-01-basics) | Variables, numbers, strings, console input and output | [Bill splitter](projects/01-bill-splitter) |
| [chapter-02-control-flow](chapter-02-control-flow) | if, switch, pattern matching, loops | [Number guessing game](projects/02-number-guessing-game) |
| [chapter-03-methods](chapter-03-methods) | Methods, parameters, overloading, lambdas, recursion | [Unit converter](projects/03-unit-converter) |
| [chapter-04-collections](chapter-04-collections) | Arrays, List, Dictionary, HashSet, tuples, records | [Student gradebook](projects/04-student-gradebook) |
| [chapter-05-error-handling](chapter-05-error-handling) | Exceptions, custom errors, using, nullable types | [Safe bank](projects/05-safe-bank) |
| [chapter-06-files-io](chapter-06-files-io) | Paths, text files, streams, CSV, JSON, async I/O | [Expense tracker](projects/06-expense-tracker) |
| [chapter-07-modules-tooling](chapter-07-modules-tooling) | Namespaces, projects, the dotnet CLI, NuGet, libraries, xUnit | [Text toolkit](projects/07-text-toolkit) |
| [chapter-08-csharp-strengths](chapter-08-csharp-strengths) | Classes, inheritance, interfaces, generics, LINQ, async, events | [Library system](projects/08-library-system) |
| [chapter-09-real-world](chapter-09-real-world) | Arguments, configuration, dates, HttpClient, JSON APIs, minimal APIs, SQLite, EF Core, DI | [Notes REST API](projects/09-notes-api) |

Each project folder has its own `README.md` with run commands. Lessons that read from the keyboard can also take piped input; see the `Run:` comment at the top of each file.

## Notes

- Lessons that use NuGet packages declare them at the top of the file (`#:package Name@version`), and `dotnet run` downloads them on first use.
- The HTTP lessons in Chapter 9 start a small local server, so they work offline.
- Files with data or databases are written to the system temp folder and removed afterwards, except for the Expense Tracker (`expense-data/`) and the Notes API (`notes.db`), which keep your data between runs.
