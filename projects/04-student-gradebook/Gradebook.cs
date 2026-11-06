// All gradebook operations. Each collection type is chosen for the job it does best.

class Gradebook
{
    // Dictionary: find a student by ID instantly.
    private readonly Dictionary<string, Student> _students = new();

    // HashSet: every subject name exactly once, whatever order marks arrive in.
    private readonly HashSet<string> _subjects = new(StringComparer.OrdinalIgnoreCase);

    // Stack: remember each change so it can be undone (last in, first out).
    private readonly Stack<(string StudentId, string Subject)> _history = new();

    public IReadOnlyCollection<string> Subjects => _subjects;
    public int Count => _students.Count;

    public bool AddStudent(string id, string name)
    {
        if (_students.ContainsKey(id)) return false;
        _students[id] = new Student(id, name, new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase));
        return true;
    }

    public bool AddMark(string id, string subject, int mark)
    {
        if (!_students.TryGetValue(id, out Student? student) || mark is < 0 or > 100) return false;
        if (!student.Marks.ContainsKey(subject)) student.Marks[subject] = new List<int>();
        student.Marks[subject].Add(mark);
        _subjects.Add(subject);
        _history.Push((id, subject));
        return true;
    }

    public string? UndoLastMark()
    {
        if (_history.Count == 0) return null;
        var (id, subject) = _history.Pop();
        List<int> marks = _students[id].Marks[subject];
        int removed = marks[^1];
        marks.RemoveAt(marks.Count - 1);
        return $"Removed {removed} from {_students[id].Name}'s {subject}";
    }

    public Student? Find(string id) => _students.GetValueOrDefault(id);

    /// <summary>Class report sorted by average, best first.</summary>
    public List<ReportRow> Report()
    {
        var rows = new List<ReportRow>();
        foreach (Student s in _students.Values)
        {
            double avg = s.OverallAverage();
            rows.Add(new ReportRow(s.Name, avg, LetterGrade(avg)));
        }
        rows.Sort((a, b) => b.Average.CompareTo(a.Average));
        return rows;
    }

    /// <summary>Top student in one subject, or null if nobody has marks in it.</summary>
    public (string Name, double Average)? TopIn(string subject)
    {
        (string Name, double Average)? best = null;
        foreach (Student s in _students.Values)
        {
            if (!s.Marks.ContainsKey(subject)) continue;
            double avg = s.Average(subject);
            if (best is null || avg > best.Value.Average) best = (s.Name, avg);
        }
        return best;
    }

    public static string LetterGrade(double average) => average switch
    {
        >= 90 => "A",
        >= 80 => "B",
        >= 70 => "C",
        >= 60 => "D",
        _ => "F"
    };
}
