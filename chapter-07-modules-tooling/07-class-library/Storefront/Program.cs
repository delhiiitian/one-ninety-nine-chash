// 7.7 Using a class library through a project reference
// Run: dotnet run --project chapter-07-modules-tooling/07-class-library/Storefront
using Pricing;

decimal shoes = 2499m;
Console.WriteLine($"Shoes: {shoes:N2}");
Console.WriteLine($"With {PriceCalculator.GstPercent}% GST: {PriceCalculator.WithTax(shoes):N2}");
Console.WriteLine($"With SAVE10 then GST: {PriceCalculator.WithTax(PriceCalculator.ApplyCoupon(shoes, "save10")):N2}");
Console.WriteLine($"With unknown coupon: {PriceCalculator.ApplyCoupon(shoes, "BOGUS"):N2}");
// Console.WriteLine(PriceCalculator.Secret);   // error CS0117: internal to the library
Console.WriteLine($"Library file: {typeof(PriceCalculator).Assembly.GetName().Name}.dll");
