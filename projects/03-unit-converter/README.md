# Project 3: Unit Converter

A menu-driven converter for length, weight, temperature and speed. The point of the project is *structure*: the menu, the conversion rules and the input validation each live in their own file and talk to each other only through methods.

## Run it

```bash
dotnet run --project projects/03-unit-converter
dotnet run --project projects/03-unit-converter < projects/03-unit-converter/sample-input.txt
```

## Files

| File | What it does |
| --- | --- |
| `Program.cs` | The menu loop: pick a category, pick a conversion, enter a value, see the result |
| `Conversions.cs` | A table of conversions; each one is a label, a `Func<double, double>` lambda and a unit |
| `ConsoleInput.cs` | `ReadChoice` and `TryReadNumber`: validated input with `out` parameters |
| `sample-input.txt` | A scripted session: a marathon in miles, body temperature both ways, a typo, a speed |

## Ideas to extend it

- Add a Volume category (litres, gallons, cups). You only touch `Conversions.cs`.
- Add a "convert back" option that reuses the last value.
- Let the user choose the number of decimal places (the `Format` method already has an optional parameter).
