namespace Pricing;

/// <summary>
/// 7.7 A class library: reusable code with no Main method, built into Pricing.dll.
/// Any app (console, web, desktop, tests) can reference it.
/// </summary>
public static class PriceCalculator
{
    public const decimal GstPercent = 18m;

    public static decimal WithTax(decimal net) => Math.Round(net * (1 + GstPercent / 100m), 2);

    public static decimal ApplyCoupon(decimal price, string? coupon) => coupon?.ToUpperInvariant() switch
    {
        "SAVE10" => Math.Round(price * 0.90m, 2),
        "FLAT50" => Math.Max(0, price - 50m),
        _ => price,
    };

    // internal: usable inside Pricing.dll only, invisible to Storefront.
    internal static string Secret => "internal pricing rule";
}
