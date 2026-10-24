// 7.1 Namespaces and using directives
// Run: dotnet run chapter-07-modules-tooling/01-namespaces.cs

// "using" imports a namespace so you can write short names.
using System.Text;
using Shop.Billing;                         // our own namespace, declared below
using Inv = Shop.Inventory;                 // an alias for a long or clashing name
using static System.Math;                   // use Math members without "Math."

var sb = new StringBuilder();               // System.Text.StringBuilder
sb.Append("Square root of 81 is ").Append(Sqrt(81));   // Math.Sqrt via using static
Console.WriteLine(sb);

// Two classes with the same name live happily in different namespaces.
var billingItem = new Item("Invoice line", 499m);
var stockItem = new Inv.Item("SKU-778", Quantity: 12);
Console.WriteLine($"Billing: {billingItem}");
Console.WriteLine($"Stock:   {stockItem}");

// The fully qualified name always works, even without a using.
var fullName = new Shop.Inventory.Item("SKU-901", 3);
Console.WriteLine($"Fully qualified type: {fullName.GetType().FullName}");

// In normal projects "global using" lines (and ImplicitUsings in the .csproj) import
// common namespaces like System, System.IO and System.Linq for every file at once.
Console.WriteLine($"Console lives in: {typeof(Console).Namespace}");
Console.WriteLine($"List<T> lives in: {typeof(List<int>).Namespace}");

// Namespaces group related types, like folders group files.
// They come after top-level statements in a file.
namespace Shop.Billing
{
    record Item(string Description, decimal Price);
}

namespace Shop.Inventory
{
    record Item(string Sku, int Quantity);
}
