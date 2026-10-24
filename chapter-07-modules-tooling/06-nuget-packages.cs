// 7.6 NuGet packages: using other people's code
// Run: dotnet run chapter-07-modules-tooling/06-nuget-packages.cs
//
// In a single-file app, #:package pulls a package from nuget.org.
// In a project, the same thing is: dotnet add package Humanizer --version 2.14.1
// which adds <PackageReference Include="Humanizer" Version="2.14.1" /> to the .csproj.
#:package Humanizer@2.14.1

using Humanizer;

// Humanizer turns computer values into friendly text.
Console.WriteLine(TimeSpan.FromMinutes(135).Humanize(precision: 2));        // 2 hours, 15 minutes
Console.WriteLine(DateTime.UtcNow.AddHours(-3).Humanize());                  // 3 hours ago
Console.WriteLine(DateTime.UtcNow.AddDays(2).Humanize());                    // 2 days from now
Console.WriteLine("customer_order_id".Humanize());                           // Customer order id
Console.WriteLine("PaymentFailedException".Humanize(LetterCasing.Title));    // Payment Failed Exception
Console.WriteLine(1_234_567.ToWords());                                      // one million two hundred ...
Console.WriteLine(3.Ordinalize() + " place");                                // 3rd place
Console.WriteLine("item".ToQuantity(5));                                     // 5 items
Console.WriteLine((15_728_640L).Bytes().Humanize("0.#"));                    // 15 MB
Console.WriteLine("This sentence is far too long for a notification".Truncate(25)); // with ellipsis
