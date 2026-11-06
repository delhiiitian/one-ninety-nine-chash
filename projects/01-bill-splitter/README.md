# Project 1: Restaurant Bill Splitter

A console app that takes a restaurant bill, adds tax and tip, and splits the total fairly between friends. It uses only Chapter 1 ideas: variables, `decimal` money maths, strings, parsing and console input/output.

## Run it

```bash
dotnet run --project projects/01-bill-splitter
# or feed the sample answers:
dotnet run --project projects/01-bill-splitter < projects/01-bill-splitter/sample-input.txt
```

## Files

| File | What it does |
| --- | --- |
| `BillSplitter.csproj` | Project file: targets .NET 10, nullable checks on |
| `Program.cs` | The whole app, in four sections: settings, input, calculation, output |
| `sample-input.txt` | Answers you can pipe in to try it without typing |

## Ideas to extend it

- Ask for each person's name and print one line per person.
- Let the user choose the currency culture (en-US, en-IN, de-DE).
- Add a service charge that some restaurants apply automatically.
