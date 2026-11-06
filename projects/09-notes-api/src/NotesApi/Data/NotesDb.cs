using Microsoft.EntityFrameworkCore;
using NotesApi.Models;

namespace NotesApi.Data;

public class NotesDb(DbContextOptions<NotesDb> options) : DbContext(options)
{
    public DbSet<Note> Notes => Set<Note>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Note>(note =>
        {
            note.Property(n => n.Title).HasMaxLength(NoteValidator.MaxTitleLength);
            note.HasIndex(n => n.UpdatedAt);
        });
    }

    // SQLite cannot sort DateTimeOffset columns natively, so store them as UTC ticks (a sortable number).
    protected override void ConfigureConventions(ModelConfigurationBuilder conventions)
    {
        conventions.Properties<DateTimeOffset>().HaveConversion<DateTimeOffsetToUtcTicksConverter>();
    }
}

public class DateTimeOffsetToUtcTicksConverter()
    : Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTimeOffset, long>(
        v => v.UtcTicks,
        v => new DateTimeOffset(v, TimeSpan.Zero));
