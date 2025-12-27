# TASK-067: Test PassiveIncome and GodfathersCut Skills

## Bundle
**Bundle N: Skill Tests - Offline & Prestige**

## Description
Create tests for remaining offline skills.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/OfflineSkillsTests.cs` (add to existing)

```csharp
public class PassiveIncomeTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 0.0)]   // No skill: $0/s
    [InlineData(1, 0.5)]   // Level 1: $0.5/s
    [InlineData(2, 1.0)]   // Level 2: $1.0/s
    [InlineData(3, 1.5)]   // Level 3: $1.5/s
    [InlineData(4, 2.0)]   // Level 4: $2.0/s
    [InlineData(5, 2.5)]   // Level 5: $2.5/s max
    public void PassiveIncome_AddsBaselineIncome(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("passive_income", level);

        // Act
        var passive = SkillService.GetPassiveIncomePerSecond();

        // Assert
        Assert.Equal(expected, passive, precision: 2);
    }
}

public class GodfathersCutTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 1.00)]  // No skill: 1x
    [InlineData(1, 1.06)]  // Level 1: +6%
    [InlineData(2, 1.12)]  // Level 2: +12%
    [InlineData(3, 1.18)]  // Level 3: +18%
    [InlineData(4, 1.24)]  // Level 4: +24%
    [InlineData(5, 1.30)]  // Level 5: +30% max
    public void GodfathersCut_MultipliesOfflineEarnings(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("godfathers_cut", level);
        var baseEarnings = 1000.0;

        // Act
        var boosted = SkillService.ApplyOfflineBonus(baseEarnings);

        // Assert
        Assert.Equal(baseEarnings * expected, boosted, precision: 2);
    }
}
```

## Acceptance Criteria
- [ ] PassiveIncome tests pass
- [ ] GodfathersCut multiplier tests pass
- [ ] Both return neutral values without skill

## Dependencies
TASK-066

## Estimate
20 min
