// 8.6 LINQ basics: query collections like a database
// Run: dotnet run chapter-08-csharp-strengths/06-linq-basics.cs

List<Movie> movies =
[
    new("Inception", 2010, "Sci-Fi", 8.8, 148),
    new("Dangal", 2016, "Drama", 8.3, 161),
    new("Interstellar", 2014, "Sci-Fi", 8.7, 169),
    new("Coco", 2017, "Animation", 8.4, 105),
    new("Parasite", 2019, "Thriller", 8.5, 132),
    new("Up", 2009, "Animation", 8.3, 96),
    new("Arrival", 2016, "Sci-Fi", 7.9, 116),
];

// Where filters, OrderBy sorts, Select transforms. Read it like a sentence.
var shortGoodMovies = movies
    .Where(m => m.Minutes < 140 && m.Rating >= 8.0)
    .OrderByDescending(m => m.Rating)
    .Select(m => $"{m.Title} ({m.Rating})");
Console.WriteLine($"Short and good: {string.Join(", ", shortGoodMovies)}");

// Aggregates.
Console.WriteLine($"Count: {movies.Count()}, average rating: {movies.Average(m => m.Rating):F2}");
Console.WriteLine($"Longest: {movies.MaxBy(m => m.Minutes)!.Title}, total runtime {movies.Sum(m => m.Minutes) / 60.0:F1} h");

// Element operators and checks.
Movie firstSciFi = movies.First(m => m.Genre == "Sci-Fi");
Movie? horror = movies.FirstOrDefault(m => m.Genre == "Horror");
Console.WriteLine($"First sci-fi: {firstSciFi.Title}; any horror? {horror is not null}");
Console.WriteLine($"All rated above 7.5? {movies.All(m => m.Rating > 7.5)}; any from 2019? {movies.Any(m => m.Year == 2019)}");

// Paging: Skip and Take.
int pageSize = 3;
for (int page = 0; page * pageSize < movies.Count; page++)
{
    var titles = movies.OrderBy(m => m.Title).Skip(page * pageSize).Take(pageSize).Select(m => m.Title);
    Console.WriteLine($"Page {page + 1}: {string.Join(", ", titles)}");
}

// Distinct, and query syntax (the SQL-like alternative to method syntax).
Console.WriteLine($"Genres: {string.Join(", ", movies.Select(m => m.Genre).Distinct().Order())}");
var recent = from m in movies
             where m.Year >= 2016
             orderby m.Year, m.Title
             select m.Title;
Console.WriteLine($"Since 2016: {string.Join(", ", recent)}");

// Deferred execution: the query runs when you loop over it, not when you write it.
var query = movies.Where(m => m.Genre == "Animation");
movies.Add(new("Soul", 2020, "Animation", 8.0, 100));
Console.WriteLine($"Animation now: {string.Join(", ", query.Select(m => m.Title))}");

record Movie(string Title, int Year, string Genre, double Rating, int Minutes);
