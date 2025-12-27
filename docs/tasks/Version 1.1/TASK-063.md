# TASK-063: Test TheSkim and CompoundInterest Skills

## Bundle
**Bundle M: Skill Tests - Income & Operations**

## Description
Create tests for cashback and time-based bonus skills.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/IncomeSkillsTests.cs` (add to existing)

```csharp
public class TheSkimTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 0.04)]  // 4% cashback
    [InlineData(3, 0.12)]  // 12% cashback
    [InlineData(5, 0.20)]  // 20% cashback max
    public void TheSkim_ReturnsCorrectCashback(int level, double expectedRate)
    {
        // Arrange
        AddSkill("the_skim", level);
        var purchaseCost = 1000.0;

        // Act
        var cashback = SkillService.CalculateCashback(purchaseCost);

        // Assert
        Assert.Equal(purchaseCost * expectedRate, cashback, precision: 2);
    }
}

public class CompoundInterestTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 1.0, 1.02)]   // Level 1, 1 hour: +2%
    [InlineData(5, 1.0, 1.10)]   // Level 5, 1 hour: +10%
    [InlineData(5, 2.0, 1.20)]   // Level 5, 2 hours: +20%
    public void CompoundInterest_ScalesWithSessionTime(int level, double hours, double expected)
    {
        // Arrange
        AddSkill("compound_interest", level);

        // Act
        var multiplier = SkillService.GetCompoundMultiplier(hours);

        // Assert
        Assert.Equal(expected, multiplier, precision: 2);
    }

    [Fact]
    public void CompoundInterest_CapsAtMaxMultiplier()
    {
        // Arrange
        AddSkill("compound_interest", 5);  // 10%/hr

        // Act - After many hours
        var multiplier = SkillService.GetCompoundMultiplier(hours: 10.0);

        // Assert - Should cap at reasonable value
        Assert.True(multiplier <= 2.0);
    }
}
```

## Acceptance Criteria
- [ ] TheSkim cashback calculation tests pass
- [ ] CompoundInterest time scaling tests pass
- [ ] Cap test passes

## Dependencies
TASK-062

## Estimate
20 min
