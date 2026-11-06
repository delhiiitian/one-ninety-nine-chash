using System.Net;
using System.Net.Http.Json;
using NotesApi.Models;

namespace NotesApi.Tests;

// Each test class instance gets a fresh API and database, so tests never see each other's notes.
public sealed class NotesEndpointTests : IDisposable
{
    private readonly NotesApiFactory _factory = new();
    private readonly HttpClient _client;

    public NotesEndpointTests() => _client = _factory.CreateClient();

    public void Dispose() => _factory.Dispose();

    private async Task<NoteResponse> CreateAsync(string title, string body = "", string[]? tags = null, bool pinned = false)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("/notes", new NoteInput(title, body, tags?.ToList(), pinned));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<NoteResponse>())!;
    }

    [Fact]
    public async Task Health_endpoint_reports_ok()
    {
        string body = await _client.GetStringAsync("/health");
        Assert.Contains("ok", body);
    }

    [Fact]
    public async Task Create_then_get_returns_the_same_note()
    {
        NoteResponse created = await CreateAsync("  Buy milk ", "2 litres", ["Home", "home", "Shopping"]);

        NoteResponse? fetched = await _client.GetFromJsonAsync<NoteResponse>($"/notes/{created.Id}");

        Assert.NotNull(fetched);
        Assert.Equal("Buy milk", fetched.Title);                    // trimmed
        Assert.Equal(["home", "shopping"], fetched.Tags);            // lower-case, no duplicates, sorted
        Assert.Equal(_factory.Clock.GetUtcNow(), fetched.CreatedAt);
    }

    [Fact]
    public async Task Create_sets_location_header()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("/notes", new NoteInput("Call mum", null, null));
        NoteResponse? note = await response.Content.ReadFromJsonAsync<NoteResponse>();
        Assert.Equal($"/notes/{note!.Id}", response.Headers.Location?.ToString());
    }

    [Theory]
    [InlineData("", "title")]
    [InlineData("   ", "title")]
    public async Task Create_rejects_missing_title(string title, string field)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("/notes", new NoteInput(title, "x", null));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(field, await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Create_rejects_too_many_tags()
    {
        var input = new NoteInput("Tags", "", ["a", "b", "c", "d", "e", "f"]);
        HttpResponseMessage response = await _client.PostAsJsonAsync("/notes", input);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_unknown_note_returns_404()
    {
        HttpResponseMessage response = await _client.GetAsync("/notes/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_changes_fields_and_timestamp()
    {
        NoteResponse created = await CreateAsync("Draft", "v1");
        _factory.Clock.Advance(TimeSpan.FromMinutes(5));

        HttpResponseMessage response = await _client.PutAsJsonAsync($"/notes/{created.Id}", new NoteInput("Final", "v2", ["work"], Pinned: true));
        NoteResponse? updated = await response.Content.ReadFromJsonAsync<NoteResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Final", updated!.Title);
        Assert.True(updated.Pinned);
        Assert.Equal(created.CreatedAt, updated.CreatedAt);
        Assert.Equal(created.CreatedAt.AddMinutes(5), updated.UpdatedAt);
    }

    [Fact]
    public async Task Delete_removes_the_note()
    {
        NoteResponse created = await CreateAsync("Temporary");

        Assert.Equal(HttpStatusCode.NoContent, (await _client.DeleteAsync($"/notes/{created.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await _client.GetAsync($"/notes/{created.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await _client.DeleteAsync($"/notes/{created.Id}")).StatusCode);
    }

    [Fact]
    public async Task List_puts_pinned_first_then_newest()
    {
        await CreateAsync("Old");
        _factory.Clock.Advance(TimeSpan.FromMinutes(1));
        await CreateAsync("Pinned", pinned: true);
        _factory.Clock.Advance(TimeSpan.FromMinutes(1));
        await CreateAsync("New");

        var page = await _client.GetFromJsonAsync<PagedResult<NoteResponse>>("/notes");

        Assert.Equal(["Pinned", "New", "Old"], page!.Items.Select(n => n.Title));
    }

    [Fact]
    public async Task List_filters_by_search_and_tag()
    {
        await CreateAsync("Buy milk", "from the dairy", ["home"]);
        await CreateAsync("Team meeting", "discuss milk budget", ["work"]);
        await CreateAsync("Gym", "", ["health"]);

        var bySearch = await _client.GetFromJsonAsync<PagedResult<NoteResponse>>("/notes?search=MILK");
        var byTag = await _client.GetFromJsonAsync<PagedResult<NoteResponse>>("/notes?tag=Work");

        Assert.Equal(2, bySearch!.TotalCount);                     // title or body, case-insensitive
        Assert.Equal("Team meeting", Assert.Single(byTag!.Items).Title);
    }

    [Fact]
    public async Task List_pages_results()
    {
        for (int i = 1; i <= 7; i++)
        {
            await CreateAsync($"Note {i}");
            _factory.Clock.Advance(TimeSpan.FromSeconds(1));
        }

        var page2 = await _client.GetFromJsonAsync<PagedResult<NoteResponse>>("/notes?page=2&pageSize=3");

        Assert.Equal(7, page2!.TotalCount);
        Assert.Equal(3, page2.TotalPages);
        Assert.Equal(["Note 4", "Note 3", "Note 2"], page2.Items.Select(n => n.Title));
    }

    [Fact]
    public async Task List_rejects_page_size_over_limit()
    {
        HttpResponseMessage response = await _client.GetAsync("/notes?pageSize=500");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Tags_endpoint_counts_tags()
    {
        await CreateAsync("A", tags: ["home", "urgent"]);
        await CreateAsync("B", tags: ["home"]);

        string json = await _client.GetStringAsync("/tags");

        Assert.Equal("""[{"tag":"home","count":2},{"tag":"urgent","count":1}]""", json);
    }
}
