// 9.8 Entity Framework Core: objects in, rows out
// Run: dotnet run chapter-09-real-world/08-ef-core.cs
#:package Microsoft.EntityFrameworkCore.Sqlite@10.0.14
#:property PublishAot=false

using Microsoft.EntityFrameworkCore;

string dbPath = Path.Combine(Path.GetTempPath(), "c199-school.db");
if (File.Exists(dbPath)) File.Delete(dbPath);

// 1. EnsureCreated builds the tables from the C# classes (real apps use migrations, see the book text).
using (var db = new SchoolContext(dbPath))
{
    db.Database.EnsureCreated();

    // 2. Add objects; SaveChanges turns them into INSERT statements in one transaction.
    var math = new Course { Title = "Mathematics" };
    var art = new Course { Title = "Art" };
    db.Courses.AddRange(math, art);
    db.Students.AddRange(
        new Student { Name = "Asha", Year = 2, Enrollments = [new() { Course = math, Grade = 91 }, new() { Course = art, Grade = 78 }] },
        new Student { Name = "Ravi", Year = 1, Enrollments = [new() { Course = math, Grade = 64 }] },
        new Student { Name = "Meera", Year = 2, Enrollments = [new() { Course = art, Grade = 88 }] });
    int saved = db.SaveChanges();
    Console.WriteLine($"Saved {saved} rows");
}

using (var db = new SchoolContext(dbPath))
{
    // 3. Query with LINQ. EF Core translates it to SQL and runs it in the database.
    List<string> secondYears = db.Students.Where(s => s.Year == 2).OrderBy(s => s.Name).Select(s => s.Name).ToList();
    Console.WriteLine($"Year 2 students: {string.Join(", ", secondYears)}");

    // 4. See the SQL EF Core generates.
    Console.WriteLine("SQL for that query:");
    Console.WriteLine("  " + db.Students.Where(s => s.Year == 2).Select(s => s.Name).ToQueryString().Replace("\n", "\n  "));

    // 5. Include loads related rows (a JOIN) so navigation properties are filled.
    foreach (Student s in db.Students.Include(s => s.Enrollments).ThenInclude(e => e.Course).OrderBy(s => s.Name))
        Console.WriteLine($"  {s.Name,-6} {string.Join(", ", s.Enrollments.Select(e => $"{e.Course.Title} {e.Grade}"))}");

    // 6. Aggregates and grouping also run in SQL.
    var averages = db.Enrollments
        .GroupBy(e => e.Course.Title)
        .Select(g => new { Course = g.Key, Average = g.Average(e => e.Grade), Count = g.Count() })
        .OrderBy(x => x.Course)
        .ToList();
    foreach (var a in averages) Console.WriteLine($"  {a.Course}: average {a.Average:0.0} from {a.Count} student(s)");

    // 7. Update: change a tracked object and call SaveChanges.
    Student ravi = db.Students.Single(s => s.Name == "Ravi");
    ravi.Year = 2;
    db.SaveChanges();

    // 8. Delete: remove and save. Enrollments are deleted too (cascade delete).
    db.Students.Remove(db.Students.Single(s => s.Name == "Meera"));
    db.SaveChanges();
    Console.WriteLine($"After update and delete: {db.Students.Count()} students, {db.Students.Count(s => s.Year == 2)} in year 2, {db.Enrollments.Count()} enrollments");
}

File.Delete(dbPath);

// --- The model: plain C# classes. EF Core infers tables, keys and relationships by convention. ---
class Student
{
    public int Id { get; set; }                 // "Id" becomes the primary key
    public required string Name { get; set; }
    public int Year { get; set; }
    public List<Enrollment> Enrollments { get; set; } = [];
}

class Course
{
    public int Id { get; set; }
    public required string Title { get; set; }
}

class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }          // foreign key to Student
    public int CourseId { get; set; }           // foreign key to Course
    public Course Course { get; set; } = null!;
    public int Grade { get; set; }
}

// The DbContext is the gateway to the database: one DbSet per table.
class SchoolContext(string path) : DbContext
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite($"Data Source={path};Pooling=False");
}
