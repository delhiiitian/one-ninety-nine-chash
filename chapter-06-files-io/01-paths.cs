// 6.1 Paths and the file system
// Run: dotnet run chapter-06-files-io/01-paths.cs

// Never glue paths together with "/" or "\". Path.Combine uses the right separator
// for Windows, macOS and Linux.
string docs = Path.Combine("users", "meera", "documents");
string report = Path.Combine(docs, "2026", "sales-report.final.pdf");
Console.WriteLine($"Combined path: {report}");

// Pull a path apart.
Console.WriteLine($"File name:      {Path.GetFileName(report)}");
Console.WriteLine($"Without ext:    {Path.GetFileNameWithoutExtension(report)}");
Console.WriteLine($"Extension:      {Path.GetExtension(report)}");
Console.WriteLine($"Folder:         {Path.GetDirectoryName(report)}");
Console.WriteLine($"Change ext:     {Path.ChangeExtension(report, ".docx")}");

// Relative vs absolute paths. Relative paths start from the current directory.
Console.WriteLine($"Current dir:    {Directory.GetCurrentDirectory()}");
Console.WriteLine($"Is rooted?      {Path.IsPathRooted(report)}");
Console.WriteLine($"Full path:      {Path.GetFullPath(report)}");

// Well-known places.
Console.WriteLine($"Temp folder:    {Path.GetTempPath()}");
Console.WriteLine($"Home folder:    {Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}");
Console.WriteLine($"App folder:     {AppContext.BaseDirectory}");
Console.WriteLine($"Separator:      '{Path.DirectorySeparatorChar}'");

// Make a safe file name from user text.
string title = "Q3: Sales / Marketing?";
foreach (char bad in Path.GetInvalidFileNameChars())
    title = title.Replace(bad, '_');
Console.WriteLine($"Safe file name: {title.Replace(':', '_').Replace('?', '_')}.txt");
