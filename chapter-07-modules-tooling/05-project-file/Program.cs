// 7.5 Project files (.csproj) and build configurations
// Run: dotnet run --project chapter-07-modules-tooling/05-project-file
//      dotnet run --project chapter-07-modules-tooling/05-project-file -c Release
using System.Reflection;

var assembly = Assembly.GetExecutingAssembly();
Console.WriteLine($"Assembly: {assembly.GetName().Name}, version {assembly.GetName().Version}");

#if DEBUG
Console.WriteLine("Configuration: Debug (extra checks, no optimisation)");
#else
Console.WriteLine("Configuration: Release (optimised)");
#endif

#if PRODUCTION
Console.WriteLine("PRODUCTION constant is defined: using live payment gateway");
#else
Console.WriteLine("PRODUCTION constant not defined: using test payment gateway");
#endif

string dataFile = Path.Combine(AppContext.BaseDirectory, "appdata.txt");
Console.WriteLine($"Copied data file says: {File.ReadAllText(dataFile).Trim()}");
