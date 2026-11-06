// 9.1 Command-line arguments
// Run: dotnet run chapter-09-real-world/01-command-line-args.cs -- resize photo.jpg --width 800 --verbose
// With no arguments the program prints its usage text.

// In a file-based app (top-level statements) the arguments arrive in the built-in `args` array.
Console.WriteLine($"Received {args.Length} argument(s): [{string.Join(", ", args)}]");

if (args.Length == 0 || args[0] is "-h" or "--help")
{
    PrintUsage();
    return 0;
}

// 1. The first word is the command, the rest are options and file names.
string command = args[0];
var files = new List<string>();
var options = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
var flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

// 2. A tiny parser: "--name value" pairs, "--flag" switches and plain values.
for (int i = 1; i < args.Length; i++)
{
    string arg = args[i];
    if (arg.StartsWith("--"))
    {
        string name = arg[2..];
        bool hasValue = i + 1 < args.Length && !args[i + 1].StartsWith("--");
        if (hasValue) options[name] = args[++i];
        else flags.Add(name);
    }
    else
    {
        files.Add(arg);
    }
}

// 3. Validate and convert, returning a non-zero exit code on bad input.
if (command != "resize")
{
    Console.Error.WriteLine($"Unknown command '{command}'.");
    return 2;
}
if (files.Count == 0)
{
    Console.Error.WriteLine("resize needs at least one file name.");
    return 2;
}
int width = 1024; // default
if (options.TryGetValue("width", out string? widthText) && !int.TryParse(widthText, out width))
{
    Console.Error.WriteLine($"--width must be a number, got '{widthText}'.");
    return 2;
}
bool verbose = flags.Contains("verbose");

foreach (string file in files)
{
    Console.WriteLine($"Resizing {file} to {width}px wide");
    if (verbose) Console.WriteLine($"  (verbose) keeping aspect ratio, output {Path.GetFileNameWithoutExtension(file)}-{width}{Path.GetExtension(file)}");
}

// 4. The return value of top-level code becomes the process exit code (0 = success).
return 0;

static void PrintUsage()
{
    Console.WriteLine("""
        Usage: imgtool resize <file>... [--width <px>] [--verbose]
          --width    target width in pixels (default 1024)
          --verbose  print extra details
        """);
}
