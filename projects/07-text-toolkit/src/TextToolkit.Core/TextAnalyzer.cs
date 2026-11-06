using System.Text.RegularExpressions;

namespace TextToolkit.Core;

/// <summary>Analyzes plain text: counts, readability and most frequent words.</summary>
public static partial class TextAnalyzer
{
    public const int WordsPerMinute = 230;

    // Common words that say little about a text's topic.
    private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "the", "a", "an", "and", "or", "but", "of", "to", "in", "on", "at", "for", "is", "are",
        "was", "were", "it", "this", "that", "with", "as", "by", "be", "you", "i", "we", "they",
    };

    [GeneratedRegex(@"[\p{L}']+")]
    private static partial Regex WordPattern();

    [GeneratedRegex(@"[.!?]+(\s|$)")]
    private static partial Regex SentenceEnd();

    public static IReadOnlyList<string> Words(string text) =>
        WordPattern().Matches(text).Select(m => m.Value.Trim('\'')).Where(w => w.Length > 0).ToList();

    public static int CountSentences(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 0;
        int ends = SentenceEnd().Matches(text.Trim()).Count;
        return Math.Max(ends, 1);       // text without a full stop is still one sentence
    }

    /// <summary>Approximate syllables: groups of vowels, minus a silent final 'e'.</summary>
    public static int CountSyllables(string word)
    {
        string w = word.ToLowerInvariant();
        int count = 0;
        bool previousWasVowel = false;
        foreach (char c in w)
        {
            bool vowel = "aeiouy".Contains(c);
            if (vowel && !previousWasVowel) count++;
            previousWasVowel = vowel;
        }
        if (w.EndsWith('e') && !w.EndsWith("le") && count > 1) count--;
        return Math.Max(count, 1);
    }

    /// <summary>Flesch reading ease: higher is easier (90+ very easy, below 30 very hard).</summary>
    public static double FleschReadingEase(int words, int sentences, int syllables)
    {
        if (words == 0 || sentences == 0) return 0;
        return 206.835 - 1.015 * ((double)words / sentences) - 84.6 * ((double)syllables / words);
    }

    public static string LevelFor(double ease) => ease switch
    {
        >= 90 => "Very easy (age 11)",
        >= 70 => "Easy (age 13)",
        >= 60 => "Plain English (age 15)",
        >= 50 => "Fairly hard (age 17)",
        >= 30 => "Hard (university)",
        _ => "Very hard (graduate)",
    };

    public static TextReport Analyze(string text, int topCount = 5)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentOutOfRangeException.ThrowIfNegative(topCount);

        IReadOnlyList<string> words = Words(text);
        int sentences = CountSentences(text);
        int syllables = words.Sum(CountSyllables);
        double ease = Math.Round(FleschReadingEase(words.Count, sentences, syllables), 1);

        var top = words
            .Where(w => w.Length > 2 && !StopWords.Contains(w))
            .GroupBy(w => w.ToLowerInvariant())
            .Select(g => (Word: g.Key, Count: g.Count()))
            .OrderByDescending(x => x.Count).ThenBy(x => x.Word, StringComparer.Ordinal)
            .Take(topCount)
            .ToList();

        int paragraphs = text.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries)
                             .Count(p => !string.IsNullOrWhiteSpace(p));

        return new TextReport(
            Characters: text.Length,
            Words: words.Count,
            Sentences: words.Count == 0 ? 0 : sentences,
            Paragraphs: paragraphs,
            AverageWordLength: words.Count == 0 ? 0 : Math.Round(words.Average(w => w.Length), 1),
            ReadingEase: ease,
            ReadingLevel: words.Count == 0 ? "n/a" : LevelFor(ease),
            ReadingTime: TimeSpan.FromMinutes((double)words.Count / WordsPerMinute),
            TopWords: top);
    }
}
