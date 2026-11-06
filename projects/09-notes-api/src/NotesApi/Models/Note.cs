namespace NotesApi.Models;

// The database entity. EF Core maps it to a "Notes" table.
public class Note
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string Body { get; set; } = "";
    public List<string> Tags { get; set; } = [];   // stored as a JSON column by EF Core
    public bool Pinned { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

// What clients send. Kept separate from the entity so they cannot set Id or timestamps.
public record NoteInput(string? Title, string? Body, List<string>? Tags, bool Pinned = false);

// What clients get back.
public record NoteResponse(int Id, string Title, string Body, IReadOnlyList<string> Tags, bool Pinned,
                           DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt)
{
    public static NoteResponse From(Note n) => new(n.Id, n.Title, n.Body, n.Tags, n.Pinned, n.CreatedAt, n.UpdatedAt);
}

// A page of results plus the information needed to ask for the next page.
public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}

// Settings bound from the "Notes" section of appsettings.json.
public class NotesOptions
{
    public int MaxPageSize { get; set; } = 50;
    public int MaxTags { get; set; } = 5;
}
