# TASK-062: Test LuckyBreak Skill

## Bundle
**Bundle M: Skill Tests - Income & Operations**

## Description
Create tests for the LuckyBreak skill that provides critical hit chance.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/IncomeSkillsTests.cs` (add to existing)

```csharp
public class LuckyBreakTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 0.03)]  // 3% chance
    [InlineData(2, 0.06)]  // 6% chance
    [InlineData(3, 0.09)]  // 9% chance
    [InlineData(4, 0.12)]  // 12% chance
    [InlineData(5, 0.15)]  // 15% chance max
    public void LuckyBreak_HasCorrectChance(int level, double expectedChance)
    {
        // Arrange
        AddSkill("lucky_break", level);

        // Act
        var chance = SkillService.GetLuckyBreakChance();

        // Assert
        Assert.Equal(expectedChance, chance, precision: 2);
    }

    [Fact]
    public void LuckyBreak_DoublesDamageWhenTriggered()
    {
        // Arrange
        AddSkill("lucky_break", 5);
        var baseIncome = 100.0;

        // Act - Force a "lucky" roll
        var critIncome = SkillService.ApplyLuckyBreak(baseIncome, forceSuccess: true);

        // Assert
        Assert.Equal(200.0, critIncome);
    }

    [Fact]
    public void LuckyBreak_NoEffectWithoutSkill()
    {
        // Arrange - no skill added

        // Act
        var chance = SkillService.GetLuckyBreakChance();

        // Assert
        Assert.Equal(0.0, chance);
    }
}
```

## Acceptance Criteria
- [ ] Chance calculation tests pass
- [ ] Double damage test passes
- [ ] No effect without skill test passes

## Dependencies
TASK-061

## Estimate
20 min
