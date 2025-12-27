# TASK-066: Test ExtendedShift Skill

## Bundle
**Bundle N: Skill Tests - Offline & Prestige**

## Description
Create tests for the ExtendedShift skill that increases max offline hours.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/OfflineSkillsTests.cs` (add to existing)

```csharp
public class ExtendedShiftTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 4.0)]   // No skill: base 4h
    [InlineData(1, 4.5)]   // Level 1: 4h + 0.5h
    [InlineData(2, 5.0)]   // Level 2: 4h + 1.0h
    [InlineData(3, 5.5)]   // Level 3: 4h + 1.5h
    [InlineData(4, 6.0)]   // Level 4: 4h + 2.0h
    [InlineData(5, 6.5)]   // Level 5: 4h + 2.5h max
    public void ExtendedShift_IncreasesMaxOfflineHours(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("extended_shift", level);

        // Act
        var maxHours = SkillService.GetMaxOfflineHours();

        // Assert
        Assert.Equal(expected, maxHours, precision: 1);
    }

    [Fact]
    public void ExtendedShift_CapsOfflineTime()
    {
        // Arrange
        AddSkill("extended_shift", 5);  // 6.5h max
        var awayTime = TimeSpan.FromHours(10);  // Been away 10h

        // Act
        var cappedTime = SkillService.CapOfflineTime(awayTime);

        // Assert
        Assert.Equal(6.5, cappedTime.TotalHours, precision: 1);
    }
}
```

## Acceptance Criteria
- [ ] All level tests pass
- [ ] Base max hours is 4h without skill
- [ ] Capping works correctly

## Dependencies
TASK-065

## Estimate
15 min
