# TASK-065: Test NightOwl Skill

## Bundle
**Bundle N: Skill Tests - Offline & Prestige**

## Description
Create tests for the NightOwl skill that increases offline efficiency.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/OfflineSkillsTests.cs`

```csharp
public class NightOwlTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 0.40)]  // No skill: base 40%
    [InlineData(1, 0.46)]  // Level 1: 40% + 6% = 46%
    [InlineData(2, 0.52)]  // Level 2: 40% + 12% = 52%
    [InlineData(3, 0.58)]  // Level 3: 40% + 18% = 58%
    [InlineData(4, 0.64)]  // Level 4: 40% + 24% = 64%
    [InlineData(5, 0.70)]  // Level 5: 40% + 30% = 70% (max)
    public void NightOwl_IncreasesOfflineEfficiency(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("night_owl", level);

        // Act
        var efficiency = SkillService.GetOfflineEfficiency();

        // Assert
        Assert.Equal(expected, efficiency, precision: 2);
    }

    [Fact]
    public void NightOwl_EfficiencyNeverExceedsOne()
    {
        // Arrange - even at max level
        AddSkill("night_owl", 5);

        // Act
        var efficiency = SkillService.GetOfflineEfficiency();

        // Assert
        Assert.True(efficiency <= 1.0);
    }
}
```

## Acceptance Criteria
- [ ] All level tests pass
- [ ] Base efficiency is 40% without skill
- [ ] Efficiency caps at reasonable value

## Dependencies
TASK-059, Bundle K complete

## Estimate
15 min
