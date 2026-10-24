namespace TextToolkit.Core;

/// <summary>Everything the analyzer found out about a text.</summary>
public sealed record TextReport(
    int Characters,
    int Words,
    int Sentences,
    int Paragraphs,
    double AverageWordLength,
    double ReadingEase,
    string ReadingLevel,
    TimeSpan ReadingTime,
    IReadOnlyList<(string Word, int Count)> TopWords);
