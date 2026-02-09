using JetBrains.Annotations;
using MasterData.Domain.Item.Rules;
using Xunit;

namespace MasterData.Domain.Tests.Item.Rules;

[TestSubject(typeof(PriceMustBeGreaterThanZeroRule))]
public class PriceMustBeGreaterThanZeroRuleTest
{
    [Fact]
    public void should_return_false_when_price_is_0()
    {
        var price    = 0;
        var rule     = new PriceMustBeGreaterThanZeroRule(price);
        var isBroken = rule.IsBroken();

        Assert.True(isBroken);
    }

    [Fact]
    public void should_return_success_when_price_greater_than_0()

    {
        var price    = 10;
        var rule     = new PriceMustBeGreaterThanZeroRule(price);
        var isBroken = rule.IsBroken();
        Assert.False(isBroken);
    }
}