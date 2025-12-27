# TASK-071: Test Cost Reduction Skills Stacking

## Bundle
**Bundle O: Skill Tests - Integration & Edge Cases**

## Description
Create tests to verify cost reduction skills work together correctly.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/SkillStackingTests.cs` (add to existing)

```csharp
public class CostStackingTests : SkillTestBase
{
    [Fact]
    public void CostReductionSkills_StackCorrectly()
    {
        // Arrange
        AddSkill("fast_learner", 3);   // -9% upgrade cost
        AddSkill("early_bird", 3);     // -12% unlock cost

        // Act
        var upgradeMult = SkillService.GetUpgradeCostMultiplier();
        var unlockMult = SkillService.GetUnlockCostMultiplier();

        // Assert - Each affects only its type
        Assert.Equal(0.91, upgradeMult, precision: 2);
        Assert.Equal(0.88, unlockMult, precision: 2);
    }

    [Fact]
    public void CostReductions_ApplyToActualCosts()
    {
        // Arrange
        AddSkill("fast_learner", 5);   // -15% upgrade cost
        AddSkill("early_bird", 5);     // -20% unlock cost

        var baseUpgradeCost = 100.0;
        var baseUnlockCost = 75.0;  // Car Theft

        // Act
        var actualUpgrade = SkillService.CalculateUpgradeCost(baseUpgradeCost);
        var actualUnlock = SkillService.CalculateUnlockCost(baseUnlockCost);

        // Assert
        Assert.Equal(85.0, actualUpgrade, precision: 2);   // 100 * 0.85
        Assert.Equal(60.0, actualUnlock, precision: 2);    // 75 * 0.80
    }

    [Fact]
    public void CostReductions_NeverGoBelowMinimum()
    {
        // Arrange - Even with max reduction
        AddSkill("fast_learner", 5);
        AddSkill("early_bird", 5);

        // Act
        var upgradeMult = SkillService.GetUpgradeCostMultiplier();
        var unlockMult = SkillService.GetUnlockCostMultiplier();

        // Assert - Multipliers should be > 0
        Assert.True(upgradeMult > 0);
        Assert.True(unlockMult > 0);
    }
}
```

## Acceptance Criteria
- [ ] Cost skills don't interfere with each other
- [ ] Actual cost calculation works
- [ ] Costs never go negative

## Dependencies
TASK-070

## Estimate
20 min
