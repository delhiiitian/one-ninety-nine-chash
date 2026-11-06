namespace NotesApi.Models;

// Input rules live in one place, so every endpoint applies them the same way.
public static class NoteValidator
{
    public const int MaxTitleLength = 120;
    public const int MaxBodyLength = 10_000;

    public static Dictionary<string, string[]> Validate(NoteInput input, NotesOptions options)
    {
        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(input.Title))
            errors["title"] = ["Title is required."];
        else if (input.Title.Length > MaxTitleLength)
            errors["title"] = [$"Title must be at most {MaxTitleLength} characters."];

        if (input.Body is { Length: > MaxBodyLength })
            errors["body"] = [$"Body must be at most {MaxBodyLength} characters."];

        var tags = input.Tags ?? [];
        if (tags.Count > options.MaxTags)
            errors["tags"] = [$"At most {options.MaxTags} tags are allowed."];
        else if (tags.Any(t => string.IsNullOrWhiteSpace(t) || t.Contains(' ')))
            errors["tags"] = ["Tags must be single words."];
        return errors;
    }

    // Tags are stored lower-case and without duplicates so searching is simple.
    public static List<string> NormalizeTags(IEnumerable<string>? tags) =>
        (tags ?? []).Select(t => t.Trim().ToLowerInvariant()).Distinct().Order().ToList();
}
