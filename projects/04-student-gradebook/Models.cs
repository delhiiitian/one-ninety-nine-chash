// The data types of the gradebook. Records keep them short and comparable by value.

/// <summary>One student and their marks, keyed by subject.</summary>
record Student(string Id, string Name, Dictionary<string, List<int>> Marks)
{
    public double Average(string subject) =>
        Marks.TryGetValue(subject, out var list) && list.Count > 0 ? list.Average() : 0;

    public double OverallAverage()
    {
        var all = new List<int>();
        foreach (var list in Marks.Values) all.AddRange(list);
        return all.Count == 0 ? 0 : all.Average();
    }
}

/// <summary>A row in the class report: who, their average and their letter grade.</summary>
record ReportRow(string Name, double Average, string Grade);
