# TASK-072: Test Max Skill Limit

## Bundle
**Bundle O: Skill Tests - Integration & Edge Cases**

## Description
Create tests to verify the 5-skill maximum is enforced correctly.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/SkillStackingTests.cs` (add to existing)

```csharp
public class SkillLimitTests : SkillTestBase
{
    [Fact]
    public void MaxSkillLimit_EnforcedAtFive()
    {
        // Arrange - Add 5 different skills
        AddSkill("cash_flow", 1);
        AddSkill("street_smarts", 1);
        AddSkill("business_acumen", 1);
        AddSkill("quick_hands", 1);
        AddSkill("night_owl", 1);

        // Act
        var canAddMore = SkillService.CanAddNewSkill();

        // Assert
        Assert.False(canAddMore);
        Assert.Equal(5, GameState.Skills.Count);
    }

    [Fact]
    public void ExistingSkill_CanBeUpgraded_EvenAtLimit()
    {
        // Arrange - Fill up skill slots
        AddSkill("cash_flow", 1);
        AddSkill("street_smarts", 1);
        AddSkill("business_acumen", 1);
        AddSkill("quick_hands", 1);
        AddSkill("night_owl", 1);

        // Act
        var canUpgrade = SkillService.CanUpgradeSkill("cash_flow");

        // Assert - Can upgrade existing skill even at limit
        Assert.True(canUpgrade);
    }

    [Fact]
    public void MaxLevelSkill_CannotBeUpgraded()
    {
        // Arrange
        AddSkill("cash_flow", 5);  // Max level

        // Act
        var canUpgrade = SkillService.CanUpgradeSkill("cash_flow");

        // Assert
        Assert.False(canUpgrade);
    }

    [Fact]
    public void SkillSelectionPool_ExcludesMaxedSkills()
    {
        // Arrange
        AddSkill("cash_flow", 5);  // Maxed

        // Act
        var pool = SkillService.GetSelectionPool();

        // Assert - Pool shouldn't include maxed skills
        Assert.DoesNotContain(pool, s => s.Id == "cash_flow");
    }

    [Fact]
    public void SkillSelectionPool_IncludesUpgradableSkills()
    {
        // Arrange
        AddSkill("cash_flow", 3);  // Can still upgrade

        // Act
        var pool = SkillService.GetSelectionPool();

        // Assert - Pool includes skill that can be upgraded
        Assert.Contains(pool, s => s.Id == "cash_flow");
    }
}
```

## Acceptance Criteria
- [ ] 5-skill limit enforced
- [ ] Existing skills can be upgraded
- [ ] Max level skills can't be upgraded
- [ ] Selection pool logic correct

## Dependencies
TASK-071

## Estimate
25 min
