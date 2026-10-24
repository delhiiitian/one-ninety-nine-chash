using TextToolkit.Core;

namespace TextToolkit.Tests;

public class TextAnalyzerTests
{
    [Theory]
    [InlineData("", 0)]
    [InlineData("Hello", 1)]
    [InlineData("Hello, world!", 2)]
    [InlineData("It's a dog's life.", 4)]
    [InlineData("  spaced   out  words ", 3)]
    public void Words_AreCounted(string text, int expected) =>
        Assert.Equal(expected, TextAnalyzer.Words(text).Count);

    [Theory]
    [InlineData("One. Two! Three?", 3)]
    [InlineData("No full stop", 1)]
    [InlineData("Wait... what?!", 2)]
    [InlineData("Version 2.5 is out.", 1)]
    public void Sentences_AreCounted(string text, int expected) =>
        Assert.Equal(expected, TextAnalyzer.CountSentences(text));

    [Theory]
    [InlineData("cat", 1)]
    [InlineData("table", 2)]
    [InlineData("make", 1)]
    [InlineData("reading", 2)]
    [InlineData("education", 4)]
    public void Syllables_AreApproximated(string word, int expected) =>
        Assert.Equal(expected, TextAnalyzer.CountSyllables(word));

    [Fact]
    public void SimpleText_IsEasyToRead()
    {
        TextReport r = TextAnalyzer.Analyze("The cat sat on the mat. The dog ran to the cat.");
        Assert.True(r.ReadingEase >= 90, $"ease was {r.ReadingEase}");
        Assert.StartsWith("Very easy", r.ReadingLevel);
    }

    [Fact]
    public void TopWords_IgnoreStopWordsAndCase()
    {
        TextReport r = TextAnalyzer.Analyze("Rain rain go away. The rain in Spain. Spain is sunny.", topCount: 2);
        Assert.Equal(("rain", 3), r.TopWords[0]);
        Assert.Equal(("spain", 2), r.TopWords[1]);
    }

    [Fact]
    public void EmptyText_GivesZeros()
    {
        TextReport r = TextAnalyzer.Analyze("");
        Assert.Equal(0, r.Words);
        Assert.Equal(0, r.Sentences);
        Assert.Equal("n/a", r.ReadingLevel);
        Assert.Empty(r.TopWords);
    }

    [Fact]
    public void ReadingTime_Uses230WordsPerMinute()
    {
        string text = string.Join(' ', Enumerable.Repeat("word", 460));
        Assert.Equal(TimeSpan.FromMinutes(2), TextAnalyzer.Analyze(text).ReadingTime);
    }

    [Fact]
    public void NullText_Throws() =>
        Assert.Throws<ArgumentNullException>(() => TextAnalyzer.Analyze(null!));
}
