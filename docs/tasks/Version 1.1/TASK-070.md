# TASK-070: Test Multiple Income Skills Stacking

## Bundle
**Bundle O: Skill Tests - Integration & Edge Cases**

## Description
Create tests to verify multiple income-related skills stack correctly.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/SkillStackingTests.cs`

```csharp
public class IncomeStackingTests : SkillTestBase
{
    [Fact]
    public void MultipleIncomeSkills_StackMultiplicatively()
    {
        // Arrange
        AddSkill("cash_flow", 3);        // +9% global
        AddSkill("street_smarts", 2);    // +16% to tier 1
        AddSkill("quick_hands", 2);      // +24% to pickpocket

        // Act - Calculate total pickpocket income
        var baseIncome = 0.5;  // Pickpocket base after rebalance
        var operationMult = SkillService.GetOperationMultiplier("pickpocket");
        var tierMult = SkillService.GetTierMultiplier("pickpocket");
        var globalMult = SkillService.GetTotalIncomeMultiplier();

        var finalIncome = baseIncome * operationMult * tierMult * globalMult;

        // Assert - Should be significantly boosted but not crazy
        // 0.5 * 1.24 * 1.16 * 1.09 = ~0.784
        Assert.True(finalIncome > baseIncome);
        Assert.True(finalIncome < baseIncome * 2);  // Not more than 2x
    }

    [Fact]
    public void TierSkills_DontAffectWrongTiers()
    {
        // Arrange
        AddSkill("street_smarts", 5);    // +40% to tier 1
        AddSkill("business_acumen", 5);  // +40% to tier 2

        // Act
        var pickpocketMult = SkillService.GetTierMultiplier("pickpocket");  // Tier 1
        var speakeasyMult = SkillService.GetTierMultiplier("speakeasy");    // Tier 2

        // Assert - Each tier only gets its own boost
        Assert.Equal(1.40, pickpocketMult, 2);
        Assert.Equal(1.40, speakeasyMult, 2);
    }

    [Fact]
    public void GlobalAndOperationSkills_StackCorrectly()
    {
        // Arrange
        AddSkill("cash_flow", 5);        // +15% global
        AddSkill("quick_hands", 5);      // +60% pickpocket only

        // Act
        var pickpocketTotal = SkillService.GetTotalMultiplierForOperation("pickpocket");
        var carTheftTotal = SkillService.GetTotalMultiplierForOperation("cartheft");

        // Assert
        // Pickpocket: 1.15 * 1.60 = 1.84
        // CarTheft: 1.15 * 1.00 = 1.15
        Assert.True(pickpocketTotal > carTheftTotal);
    }
}
```

## Acceptance Criteria
- [ ] Multiple income skills stack correctly
- [ ] Tier skills don't cross-affect
- [ ] Global + operation skills combine properly

## Dependencies
Bundle M, Bundle N complete

## Estimate
25 min
