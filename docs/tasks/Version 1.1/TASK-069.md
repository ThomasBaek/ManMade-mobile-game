# TASK-069: Test Reputation, FastLearner, and EarlyBird Skills

## Bundle
**Bundle N: Skill Tests - Offline & Prestige**

## Description
Create tests for remaining prestige skills.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/PrestigeSkillsTests.cs` (add to existing)

```csharp
public class ReputationTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 0.20)]  // No skill: base 20%
    [InlineData(1, 0.24)]  // Level 1: 20% + 4% = 24%
    [InlineData(3, 0.32)]  // Level 3: 20% + 12% = 32%
    [InlineData(5, 0.40)]  // Level 5: 20% + 20% = 40% max
    public void Reputation_IncreasesPrestigeBonus(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("reputation", level);

        // Act
        var bonusRate = SkillService.GetPrestigeBonusRate();

        // Assert
        Assert.Equal(expected, bonusRate, precision: 2);
    }
}

public class FastLearnerTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 1.00)]  // No skill: 100%
    [InlineData(1, 0.97)]  // Level 1: -3%
    [InlineData(3, 0.91)]  // Level 3: -9%
    [InlineData(5, 0.85)]  // Level 5: -15% max
    public void FastLearner_ReducesUpgradeCosts(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("fast_learner", level);

        // Act
        var multiplier = SkillService.GetUpgradeCostMultiplier();

        // Assert
        Assert.Equal(expected, multiplier, precision: 2);
    }
}

public class EarlyBirdTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 1.00)]  // No skill: 100%
    [InlineData(1, 0.96)]  // Level 1: -4%
    [InlineData(3, 0.88)]  // Level 3: -12%
    [InlineData(5, 0.80)]  // Level 5: -20% max
    public void EarlyBird_ReducesUnlockCosts(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("early_bird", level);

        // Act
        var multiplier = SkillService.GetUnlockCostMultiplier();

        // Assert
        Assert.Equal(expected, multiplier, precision: 2);
    }
}
```

## Acceptance Criteria
- [ ] Reputation bonus calculation tests pass
- [ ] FastLearner cost reduction tests pass
- [ ] EarlyBird unlock reduction tests pass

## Dependencies
TASK-068

## Estimate
20 min

## Bundle N Complete Verification
After this task, verify:
- [ ] All offline skill tests pass
- [ ] All prestige skill tests pass
- [ ] `dotnet test` shows all green
