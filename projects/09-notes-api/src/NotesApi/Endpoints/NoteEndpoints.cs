using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NotesApi.Data;
using NotesApi.Models;

namespace NotesApi.Endpoints;

// All /notes routes in one file. Program.cs calls app.MapNoteEndpoints().
public static class NoteEndpoints
{
    public static RouteGroupBuilder MapNoteEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder notes = app.MapGroup("/notes");

        notes.MapGet("/", ListNotes);
        notes.MapGet("/{id:int}", GetNote);
        notes.MapPost("/", CreateNote);
        notes.MapPut("/{id:int}", UpdateNote);
        notes.MapDelete("/{id:int}", DeleteNote);
        app.MapGet("/tags", ListTags);
        return notes;
    }

    // GET /notes?search=milk&tag=home&page=1&pageSize=10
    // Pinned notes first, then the most recently updated.
    private static async Task<IResult> ListNotes(NotesDb db, IOptions<NotesOptions> options,
        string? search, string? tag, int page = 1, int pageSize = 10)
    {
        if (page < 1 || pageSize < 1 || pageSize > options.Value.MaxPageSize)
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["paging"] = [$"page must be 1 or more and pageSize between 1 and {options.Value.MaxPageSize}."]
            });

        IQueryable<Note> query = db.Notes.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            string pattern = $"%{search.Trim()}%";
            query = query.Where(n => EF.Functions.Like(n.Title, pattern) || EF.Functions.Like(n.Body, pattern));
        }
        if (!string.IsNullOrWhiteSpace(tag))
        {
            string t = tag.Trim().ToLowerInvariant();
            query = query.Where(n => n.Tags.Contains(t));
        }

        int total = await query.CountAsync();
        List<Note> items = await query
            .OrderByDescending(n => n.Pinned).ThenByDescending(n => n.UpdatedAt).ThenByDescending(n => n.Id)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync();
        return Results.Ok(new PagedResult<NoteResponse>(items.Select(NoteResponse.From).ToList(), page, pageSize, total));
    }

    private static async Task<IResult> GetNote(int id, NotesDb db) =>
        await db.Notes.FindAsync(id) is Note note ? Results.Ok(NoteResponse.From(note)) : Results.NotFound();

    private static async Task<IResult> CreateNote(NoteInput input, NotesDb db, TimeProvider clock,
        IOptions<NotesOptions> options, ILogger<Note> log)
    {
        var errors = NoteValidator.Validate(input, options.Value);
        if (errors.Count > 0) return Results.ValidationProblem(errors);

        DateTimeOffset now = clock.GetUtcNow();
        var note = new Note
        {
            Title = input.Title!.Trim(),
            Body = input.Body?.Trim() ?? "",
            Tags = NoteValidator.NormalizeTags(input.Tags),
            Pinned = input.Pinned,
            CreatedAt = now,
            UpdatedAt = now
        };
        db.Notes.Add(note);
        await db.SaveChangesAsync();
        log.LogInformation("Created note {NoteId} with {TagCount} tag(s)", note.Id, note.Tags.Count);
        return Results.Created($"/notes/{note.Id}", NoteResponse.From(note));
    }

    private static async Task<IResult> UpdateNote(int id, NoteInput input, NotesDb db, TimeProvider clock,
        IOptions<NotesOptions> options)
    {
        var errors = NoteValidator.Validate(input, options.Value);
        if (errors.Count > 0) return Results.ValidationProblem(errors);
        if (await db.Notes.FindAsync(id) is not Note note) return Results.NotFound();

        note.Title = input.Title!.Trim();
        note.Body = input.Body?.Trim() ?? "";
        note.Tags = NoteValidator.NormalizeTags(input.Tags);
        note.Pinned = input.Pinned;
        note.UpdatedAt = clock.GetUtcNow();
        await db.SaveChangesAsync();
        return Results.Ok(NoteResponse.From(note));
    }

    private static async Task<IResult> DeleteNote(int id, NotesDb db, ILogger<Note> log)
    {
        int deleted = await db.Notes.Where(n => n.Id == id).ExecuteDeleteAsync();
        if (deleted == 0) return Results.NotFound();
        log.LogInformation("Deleted note {NoteId}", id);
        return Results.NoContent();
    }

    // GET /tags -> [{ "tag": "home", "count": 2 }, ...]
    // Tags live inside a JSON column, so we count them in memory. Fine for a personal notes app.
    private static async Task<IResult> ListTags(NotesDb db)
    {
        List<List<string>> all = await db.Notes.AsNoTracking().Select(n => n.Tags).ToListAsync();
        var counts = all.SelectMany(t => t)
            .CountBy(t => t)
            .OrderByDescending(p => p.Value).ThenBy(p => p.Key)
            .Select(p => new { tag = p.Key, count = p.Value });
        return Results.Ok(counts);
    }
}
