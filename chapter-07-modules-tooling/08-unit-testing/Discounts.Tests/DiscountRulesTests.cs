// 7.8 Unit tests with xUnit
// Run: dotnet test chapter-07-modules-tooling/08-unit-testing/Discounts.Tests
using Discounts;

public class DiscountRulesTests
{
    // [Fact]: one test, one scenario. Arrange, Act, Assert.
    [Fact]
    public void SmallOrder_NonMember_GetsNoDiscount()
    {
        // Arrange
        decimal total = 499m;

        // Act
        decimal percent = DiscountRules.DiscountPercent(total, isMember: false);

        // Assert
        Assert.Equal(0m, percent);
    }

    // [Theory] + [InlineData]: the same test with many inputs. Boundaries matter most.
    [Theory]
    [InlineData(999.99, false, 0)]
    [InlineData(1000, false, 10)]
    [InlineData(4999.99, false, 10)]
    [InlineData(5000, false, 20)]
    [InlineData(1000, true, 15)]
    [InlineData(5000, true, 25)]
    [InlineData(0, true, 5)]
    public void DiscountPercent_MatchesTheRules(decimal total, bool member, decimal expected)
    {
        Assert.Equal(expected, DiscountRules.DiscountPercent(total, member));
    }

    [Fact]
    public void FinalPrice_RoundsToTwoDecimals()
    {
        Assert.Equal(1012.49m, DiscountRules.FinalPrice(1124.99m, isMember: false));
    }

    [Fact]
    public void NegativeTotal_Throws()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => DiscountRules.DiscountPercent(-1m, false));
        Assert.Equal("orderTotal", ex.ParamName);
    }
}
