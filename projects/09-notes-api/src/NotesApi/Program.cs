using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Endpoints;
using NotesApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 1. Configuration: appsettings.json, then environment variables, then command-line args.
builder.Services.Configure<NotesOptions>(builder.Configuration.GetSection("Notes"));

// 2. Services in the DI container.
builder.Services.AddDbContext<NotesDb>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Notes")));
builder.Services.AddSingleton(TimeProvider.System);   // tests swap in a fake clock
builder.Services.AddProblemDetails();                 // consistent JSON error bodies

WebApplication app = builder.Build();

// 3. Create the database file and table on first run.
//    (A production app would use EF Core migrations: dotnet ef migrations add Initial.)
using (IServiceScope scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<NotesDb>().Database.EnsureCreated();
}

// 4. Middleware: turn unhandled exceptions into a 500 problem response instead of a stack trace.
app.UseExceptionHandler();
app.UseStatusCodePages();

// 5. Endpoints.
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapNoteEndpoints();

app.Run();

// Makes the auto-generated Program class visible to the integration tests.
public partial class Program;
