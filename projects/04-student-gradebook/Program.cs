// Project 4: Student Gradebook
// Uses Chapter 4 ideas: List, Dictionary, HashSet, Stack, tuples, records, sorting and searching.
// Run: dotnet run --project projects/04-student-gradebook
// Commands are typed one per line, so the app can be driven by a script file too.

var book = new Gradebook();
SampleData.Load(book);
Console.WriteLine($"Loaded {book.Count} students, subjects: {string.Join(", ", book.Subjects)}");
Console.WriteLine("Commands: report | student <id> | add <id> <subject> <mark> | top <subject> | undo | help | quit");

string? line;
while ((line = Prompt()) is not null)
{
    string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (parts.Length == 0) continue;

    switch (parts[0].ToLower())
    {
        case "report":
            PrintReport(book.Report());
            break;

        case "student" when parts.Length == 2:
            PrintStudent(book.Find(parts[1]));
            break;

        case "add" when parts.Length == 4 && int.TryParse(parts[3], out int mark):
            Console.WriteLine(book.AddMark(parts[1], parts[2], mark)
                ? $"  Added {mark} in {parts[2]} for {parts[1]}"
                : "  Could not add: unknown student or mark not between 0 and 100");
            break;

        case "top" when parts.Length == 2:
            var top = book.TopIn(parts[1]);
            Console.WriteLine(top is { } t
                ? $"  Top in {parts[1]}: {t.Name} ({t.Average:F1})"
                : $"  Nobody has marks in {parts[1]}");
            break;

        case "undo":
            Console.WriteLine($"  {book.UndoLastMark() ?? "Nothing to undo"}");
            break;

        case "quit":
            return;

        default:
            Console.WriteLine("  Commands: report | student <id> | add <id> <subject> <mark> | top <subject> | undo | quit");
            break;
    }
}

static string? Prompt()
{
    Console.Write("> ");
    string? input = Console.ReadLine();
    if (input is not null) Console.WriteLine(input);   // echo, so piped sessions read clearly
    return input;
}

static void PrintReport(List<ReportRow> rows)
{
    Console.WriteLine($"  {"#",-3}{"Name",-10}{"Average",8}  Grade");
    for (int i = 0; i < rows.Count; i++)
        Console.WriteLine($"  {i + 1,-3}{rows[i].Name,-10}{rows[i].Average,8:F1}  {rows[i].Grade}");
}

static void PrintStudent(Student? student)
{
    if (student is null) { Console.WriteLine("  No student with that ID"); return; }
    Console.WriteLine($"  {student.Name} ({student.Id})");
    foreach (var (subject, marks) in student.Marks)
        Console.WriteLine($"    {subject,-8} {string.Join(", ", marks),-16} avg {student.Average(subject):F1}");
    Console.WriteLine($"    Overall {student.OverallAverage():F1} -> {Gradebook.LetterGrade(student.OverallAverage())}");
}
