# TASK-074: Test Edge Cases

## Bundle
**Bundle O: Skill Tests - Integration & Edge Cases**

## Description
Create tests for edge cases and error conditions.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/SkillEdgeCaseTests.cs`

```csharp
public class SkillEdgeCaseTests : SkillTestBase
{
    [Fact]
    public void NoSkills_ReturnsBaseValues()
    {
        // No skills added

        // Act
        var incomeMultiplier = SkillService.GetTotalIncomeMultiplier();
        var upgradeCostMultiplier = SkillService.GetUpgradeCostMultiplier();
        var unlockCostMultiplier = SkillService.GetUnlockCostMultiplier();
        var offlineEfficiency = SkillService.GetOfflineEfficiency();
        var prestigeBonus = SkillService.GetPrestigeBonusRate();

        // Assert - All base values
        Assert.Equal(1.0, incomeMultiplier);
        Assert.Equal(1.0, upgradeCostMultiplier);
        Assert.Equal(1.0, unlockCostMultiplier);
        Assert.Equal(0.4, offlineEfficiency);  // Base 40%
        Assert.Equal(0.2, prestigeBonus);      // Base 20%
    }

    [Fact]
    public void InvalidSkillId_ReturnsNoBonus()
    {
        // Act
        var multiplier = SkillService.GetOperationMultiplier("nonexistent_operation");

        // Assert
        Assert.Equal(1.0, multiplier);
    }

    [Fact]
    public void ZeroLevel_TreatedAsNoSkill()
    {
        // Arrange - Skill with level 0 (shouldn't happen but be safe)
        GameState.Skills.Add(new SkillState { Id = "cash_flow", Level = 0 });

        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();

        // Assert
        Assert.Equal(1.0, multiplier);
    }

    [Fact]
    public void NegativeLevel_TreatedAsNoSkill()
    {
        // Arrange
        GameState.Skills.Add(new SkillState { Id = "cash_flow", Level = -1 });

        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();

        // Assert
        Assert.Equal(1.0, multiplier);
    }

    [Fact]
    public void OverMaxLevel_CapsAtMax()
    {
        // Arrange - Level 10 (max is 5)
        GameState.Skills.Add(new SkillState { Id = "cash_flow", Level = 10 });

        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();

        // Assert - Should cap at level 5 effect (15%)
        Assert.Equal(1.15, multiplier, precision: 2);
    }

    [Fact]
    public void DuplicateSkill_UsesHighestLevel()
    {
        // Arrange - Same skill added twice (shouldn't happen)
        GameState.Skills.Add(new SkillState { Id = "cash_flow", Level = 2 });
        GameState.Skills.Add(new SkillState { Id = "cash_flow", Level = 4 });

        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();

        // Assert - Should use level 4 (12%)
        Assert.Equal(1.12, multiplier, precision: 2);
    }

    [Fact]
    public void OfflineEfficiency_CapsAtOneHundredPercent()
    {
        // Arrange - Max Night Owl
        AddSkill("night_owl", 5);  // +30% = 70% total

        // Act
        var efficiency = SkillService.GetOfflineEfficiency();

        // Assert
        Assert.True(efficiency <= 1.0);
        Assert.Equal(0.70, efficiency, precision: 2);
    }

    [Fact]
    public void EmptyGameState_HandledGracefully()
    {
        // Arrange - Fresh game state with no skills list
        GameState.Skills = null!;

        // Act & Assert - Should not throw
        var exception = Record.Exception(() => SkillService.GetTotalIncomeMultiplier());
        Assert.Null(exception);
    }
}
```

## Acceptance Criteria
- [ ] Base values returned without skills
- [ ] Invalid skill IDs handled gracefully
- [ ] Zero/negative levels handled
- [ ] Over-max levels capped
- [ ] Efficiency caps work
- [ ] Null safety

## Dependencies
TASK-073

## Estimate
30 min

## Bundle O Complete Verification
After this task, verify:
- [ ] All stacking tests pass
- [ ] All reset tests pass
- [ ] All edge case tests pass
- [ ] `dotnet test` shows ALL tests green
- [ ] No test failures or skips
