// Starter data so the app is useful the moment it starts.

static class SampleData
{
    public static void Load(Gradebook book)
    {
        (string Id, string Name)[] students =
        {
            ("S01", "Aarav"), ("S02", "Bianca"), ("S03", "Chen"), ("S04", "Divya"),
        };
        foreach (var (id, name) in students) book.AddStudent(id, name);

        (string Id, string Subject, int Mark)[] marks =
        {
            ("S01", "Maths", 92), ("S01", "Maths", 88), ("S01", "Science", 79),
            ("S02", "Maths", 65), ("S02", "Science", 72), ("S02", "English", 81),
            ("S03", "Maths", 98), ("S03", "Science", 94), ("S03", "English", 90),
            ("S04", "Maths", 55), ("S04", "English", 68),
        };
        foreach (var (id, subject, mark) in marks) book.AddMark(id, subject, mark);
    }
}
