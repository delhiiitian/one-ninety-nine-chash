// 6.7 Working with directories
// Run: dotnet run chapter-06-files-io/07-directories.cs

string root = Path.Combine(Path.GetTempPath(), "csharp199-ch06-dirs");
if (Directory.Exists(root)) Directory.Delete(root, recursive: true);

// Build a small project folder tree. CreateDirectory makes every missing parent too.
string[] files =
{
    "photos/2025/beach.jpg", "photos/2025/hills.jpg", "photos/2026/birthday.png",
    "documents/resume.pdf", "documents/taxes/2026-return.pdf", "notes.txt",
};
foreach (string relative in files)
{
    string full = Path.Combine(root, relative);
    Directory.CreateDirectory(Path.GetDirectoryName(full)!);
    File.WriteAllText(full, new string('x', relative.Length * 100));   // fake content
}

// List only the top level.
Console.WriteLine("Top level:");
foreach (string dir in Directory.GetDirectories(root))
    Console.WriteLine($"  [dir]  {Path.GetFileName(dir)}");
foreach (string file in Directory.GetFiles(root))
    Console.WriteLine($"  [file] {Path.GetFileName(file)}");

// Search recursively with a pattern.
Console.WriteLine("All PDFs:");
foreach (string pdf in Directory.EnumerateFiles(root, "*.pdf", SearchOption.AllDirectories))
    Console.WriteLine($"  {Path.GetRelativePath(root, pdf)}");

// Count photos by extension across all folders.
var byExtension = new Dictionary<string, int>();
foreach (string f in Directory.EnumerateFiles(Path.Combine(root, "photos"), "*", SearchOption.AllDirectories))
{
    string ext = Path.GetExtension(f).ToLower();
    byExtension[ext] = byExtension.GetValueOrDefault(ext) + 1;
}
foreach (var (ext, count) in byExtension) Console.WriteLine($"  {ext}: {count}");

// Print the whole tree with a recursive method (Chapter 3).
Console.WriteLine("Tree:");
PrintTree(root, "");

// Move and delete.
Directory.Move(Path.Combine(root, "photos", "2025"), Path.Combine(root, "photos", "archive-2025"));
Directory.Delete(Path.Combine(root, "documents", "taxes"), recursive: true);
Console.WriteLine($"After move/delete, folders under root: {Directory.GetDirectories(root, "*", SearchOption.AllDirectories).Length}");

Directory.Delete(root, recursive: true);

static void PrintTree(string path, string indent)
{
    foreach (string dir in Directory.GetDirectories(path).Order())
    {
        Console.WriteLine($"{indent}  {Path.GetFileName(dir)}/");
        PrintTree(dir, indent + "  ");
    }
    foreach (string file in Directory.GetFiles(path).Order())
        Console.WriteLine($"{indent}  {Path.GetFileName(file)}");
}
