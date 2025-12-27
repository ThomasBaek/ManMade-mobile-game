# TASK-073: Test Prestige Reset

## Bundle
**Bundle O: Skill Tests - Integration & Edge Cases**

## Description
Create tests to verify prestige correctly resets skills while applying prestige bonuses.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/PrestigeResetTests.cs`

```csharp
public class PrestigeResetTests : SkillTestBase
{
    [Fact]
    public void Prestige_ResetsAllSkills()
    {
        // Arrange
        AddSkill("cash_flow", 3);
        AddSkill("street_smarts", 2);
        AddSkill("night_owl", 4);

        // Act
        SkillService.ResetForPrestige();

        // Assert
        Assert.Empty(GameState.Skills);
    }

    [Fact]
    public void Prestige_AppliesOldConnectionsBeforeReset()
    {
        // Arrange
        AddSkill("old_connections", 5);  // $375 starting cash

        // Act
        var startingCash = SkillService.GetStartingCashAfterPrestige();
        SkillService.ResetForPrestige();

        // Assert - Calculated before skills cleared
        Assert.Equal(375, startingCash);
        Assert.Empty(GameState.Skills);
    }

    [Fact]
    public void Prestige_AppliesReputationBeforeReset()
    {
        // Arrange
        AddSkill("reputation", 3);  // +12% to prestige bonus

        // Act
        var bonusRate = SkillService.GetPrestigeBonusRate();  // 0.32
        SkillService.ResetForPrestige();

        // Assert
        Assert.Equal(0.32, bonusRate, precision: 2);
        Assert.Empty(GameState.Skills);
    }

    [Fact]
    public void Prestige_MilestoneProgressResets()
    {
        // Arrange
        GameState.TotalEarned = 50000;  // Past several milestones

        // Act
        SkillService.ResetForPrestige();

        // Assert - Milestones should reset (implementation specific)
        var progress = SkillService.GetMilestoneProgress();
        Assert.Equal(0, progress);
    }

    [Fact]
    public void Prestige_PrestigeCountIncrements()
    {
        // Arrange
        GameState.PrestigeCount = 2;

        // Act
        // This would be done by GameEngine, but verify the skill effects
        var prestigeBonus = SkillService.GetPrestigeBonusRate();

        // Assert - Base 20% per prestige
        // At 2 prestiges with base rate
        Assert.True(prestigeBonus >= 0.20);
    }
}
```

## Acceptance Criteria
- [ ] All skills cleared on prestige
- [ ] OldConnections calculated before reset
- [ ] Reputation calculated before reset
- [ ] Milestone progress resets

## Dependencies
TASK-072

## Estimate
25 min
