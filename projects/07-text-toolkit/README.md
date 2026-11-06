# Project 7: Text Toolkit

A professional-shaped .NET solution: a reusable class library that analyzes text (word and sentence counts, Flesch reading ease, reading time, top words), a command-line tool built on it, and a unit-test project. Shared build settings live in `Directory.Build.props` and style rules in `.editorconfig`.

## Run it

```bash
cd projects/07-text-toolkit
dotnet build TextToolkit.slnx
dotnet test TextToolkit.slnx
dotnet run --project src/TextToolkit.Cli -- samples/story.txt samples/terms.txt
echo "Short and sweet." | dotnet run --project src/TextToolkit.Cli -- --json --top 3
```

## Layout

| Path | What it is |
| --- | --- |
| `TextToolkit.slnx` | Solution file grouping the three projects |
| `Directory.Build.props` | Settings every project inherits (.NET 10, nullable, warnings as errors) |
| `.editorconfig` | Code-style rules used by the IDE and `dotnet format` |
| `src/TextToolkit.Core/` | Class library: `TextAnalyzer` and the `TextReport` record |
| `src/TextToolkit.Cli/` | `texttool` command-line app: arguments, files or stdin, text or JSON output, exit codes |
| `tests/TextToolkit.Tests/` | 19 xUnit tests for the library |
| `samples/` | An easy story and a hard legal paragraph to compare |

## Ideas to extend it

- Add a `--watch` option that re-analyzes a file whenever it changes.
- Publish the library as a NuGet package (`dotnet pack`).
- Add a second front end, such as a web API, that reuses `TextToolkit.Core` unchanged.
