// 6.8 FileInfo: sizes, dates, copy, move and delete
// Run: dotnet run chapter-06-files-io/08-file-info.cs

string folder = Path.Combine(Path.GetTempPath(), "csharp199-ch06-info");
Directory.CreateDirectory(folder);
string original = Path.Combine(folder, "invoice-1042.txt");
File.WriteAllText(original, string.Join('\n', Enumerable.Repeat("Item line", 200)));

// FileInfo describes a file and offers operations on it.
var info = new FileInfo(original);
Console.WriteLine($"Name:      {info.Name}");
Console.WriteLine($"Size:      {info.Length:N0} bytes ({FormatSize(info.Length)})");
Console.WriteLine($"Extension: {info.Extension}");
Console.WriteLine($"Folder:    {info.Directory!.Name}");
Console.WriteLine($"Created:   {info.CreationTime:yyyy-MM-dd HH:mm}");
Console.WriteLine($"Modified:  {info.LastWriteTime:yyyy-MM-dd HH:mm}");
Console.WriteLine($"Read-only: {info.IsReadOnly}");

// Copy (keep the original), then move/rename the copy.
string backup = Path.Combine(folder, "invoice-1042.bak");
File.Copy(original, backup, overwrite: true);
string renamed = Path.Combine(folder, "invoice-1042-paid.txt");
File.Move(backup, renamed, overwrite: true);
Console.WriteLine($"Files now: {string.Join(", ", Directory.GetFiles(folder).Select(Path.GetFileName).Order())}");

// Find files older than a cut-off (here: pretend the copy is 40 days old).
File.SetLastWriteTime(renamed, DateTime.Now.AddDays(-40));
foreach (FileInfo f in new DirectoryInfo(folder).GetFiles())
{
    double ageDays = (DateTime.Now - f.LastWriteTime).TotalDays;
    string action = ageDays > 30 ? "-> would archive" : "";
    Console.WriteLine($"  {f.Name,-22} {ageDays,5:F0} days old {action}");
}

// Disk space for the drive holding the temp folder.
var drive = new DriveInfo(Path.GetPathRoot(folder)!);
Console.WriteLine($"Drive {drive.Name}: {FormatSize(drive.AvailableFreeSpace)} free of {FormatSize(drive.TotalSize)}");

Directory.Delete(folder, recursive: true);

static string FormatSize(long bytes)
{
    string[] units = { "B", "KB", "MB", "GB", "TB" };
    double size = bytes;
    int unit = 0;
    while (size >= 1024 && unit < units.Length - 1) { size /= 1024; unit++; }
    return $"{size:0.#} {units[unit]}";
}
