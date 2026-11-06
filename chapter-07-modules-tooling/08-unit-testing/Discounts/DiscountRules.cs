namespace Discounts;

/// <summary>7.8 The code under test: festival-sale discount rules.</summary>
public static class DiscountRules
{
    /// <summary>
    /// 10% off orders of 1,000 or more, 20% off 5,000 or more.
    /// Members get an extra 5%. The discount never goes above 25%.
    /// </summary>
    public static decimal DiscountPercent(decimal orderTotal, bool isMember)
    {
        if (orderTotal < 0) throw new ArgumentOutOfRangeException(nameof(orderTotal), "Total cannot be negative.");

        decimal percent = orderTotal switch
        {
            >= 5000m => 20m,
            >= 1000m => 10m,
            _ => 0m,
        };
        if (isMember) percent += 5m;
        return Math.Min(percent, 25m);
    }

    public static decimal FinalPrice(decimal orderTotal, bool isMember) =>
        Math.Round(orderTotal * (1 - DiscountPercent(orderTotal, isMember) / 100m), 2);
}
