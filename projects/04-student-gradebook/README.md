# Project 4: Student Gradebook

A command-driven gradebook for a class: view a ranked report, look up a student, add marks, find the top student in a subject and undo mistakes. Each collection type from Chapter 4 is used for the job it does best.

## Run it

```bash
dotnet run --project projects/04-student-gradebook
dotnet run --project projects/04-student-gradebook < projects/04-student-gradebook/sample-input.txt
```

Commands: `report`, `student <id>`, `add <id> <subject> <mark>`, `top <subject>`, `undo`, `quit`.

## Files

| File | What it does |
| --- | --- |
| `Program.cs` | Command loop: reads a line, splits it, dispatches with `switch` + `when` guards, prints results |
| `Gradebook.cs` | The data and operations: `Dictionary` of students, `HashSet` of subjects, `Stack` for undo |
| `Models.cs` | `Student` and `ReportRow` records |
| `SampleData.cs` | Four students and their marks, loaded at start-up |
| `sample-input.txt` | A scripted session including invalid commands |

## Ideas to extend it

- `remove <id>` to delete a student.
- A `subject <name>` report that ranks students in one subject.
- Save and load the gradebook as JSON (Chapter 6).
