# TASK-061: Test StreetSmarts and BusinessAcumen Skills

## Bundle
**Bundle M: Skill Tests - Income & Operations**

## Description
Create tests for tier-specific income skills.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/IncomeSkillsTests.cs` (add to existing)

```csharp
public class TierSkillsTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 1.08)]  // Level 1: +8%
    [InlineData(3, 1.24)]  // Level 3: +24%
    [InlineData(5, 1.40)]  // Level 5: +40% (max)
    public void StreetSmarts_BoostsTier1Only(int level, double expected)
    {
        // Arrange
        AddSkill("street_smarts", level);

        // Act
        var pickpocketMult = SkillService.GetTierMultiplier("pickpocket");
        var carTheftMult = SkillService.GetTierMultiplier("cartheft");
        var speakeasyMult = SkillService.GetTierMultiplier("speakeasy");

        // Assert - Tier 1 (pickpocket, cartheft) boosted, Tier 2 not
        Assert.Equal(expected, pickpocketMult, precision: 2);
        Assert.Equal(expected, carTheftMult, precision: 2);
        Assert.Equal(1.0, speakeasyMult);  // Tier 2 unaffected
    }

    [Theory]
    [InlineData(1, 1.08)]
    [InlineData(5, 1.40)]
    public void BusinessAcumen_BoostsTier2Only(int level, double expected)
    {
        // Arrange
        AddSkill("business_acumen", level);

        // Act
        var pickpocketMult = SkillService.GetTierMultiplier("pickpocket");
        var speakeasyMult = SkillService.GetTierMultiplier("speakeasy");
        var casinoMult = SkillService.GetTierMultiplier("casino");

        // Assert - Tier 2 (speakeasy, casino) boosted, Tier 1 not
        Assert.Equal(1.0, pickpocketMult);  // Tier 1 unaffected
        Assert.Equal(expected, speakeasyMult, precision: 2);
        Assert.Equal(expected, casinoMult, precision: 2);
    }
}
```

## Acceptance Criteria
- [ ] StreetSmarts tests pass (Tier 1 boost only)
- [ ] BusinessAcumen tests pass (Tier 2 boost only)
- [ ] Tiers don't affect each other

## Dependencies
TASK-060

## Estimate
20 min
