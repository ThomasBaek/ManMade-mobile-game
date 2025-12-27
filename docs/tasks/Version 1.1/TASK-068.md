# TASK-068: Test OldConnections Skill

## Bundle
**Bundle N: Skill Tests - Offline & Prestige**

## Description
Create tests for the OldConnections skill that provides starting cash after prestige.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/PrestigeSkillsTests.cs`

```csharp
public class OldConnectionsTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 0)]     // No skill: $0
    [InlineData(1, 75)]    // Level 1: $75
    [InlineData(2, 150)]   // Level 2: $150
    [InlineData(3, 225)]   // Level 3: $225
    [InlineData(4, 300)]   // Level 4: $300
    [InlineData(5, 375)]   // Level 5: $375 max
    public void OldConnections_SetsStartingCash(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("old_connections", level);

        // Act
        var startCash = SkillService.GetStartingCashAfterPrestige();

        // Assert
        Assert.Equal(expected, startCash);
    }

    [Fact]
    public void OldConnections_AppliedBeforeSkillReset()
    {
        // Arrange
        AddSkill("old_connections", 5);

        // Act - Get starting cash, then reset
        var startCash = SkillService.GetStartingCashAfterPrestige();
        SkillService.ResetForPrestige();

        // Assert - Starting cash should have been calculated before reset
        Assert.Equal(375, startCash);
        Assert.Empty(GameState.Skills);  // Skills are now reset
    }
}
```

## Acceptance Criteria
- [ ] All level tests pass
- [ ] Starting cash calculated before skill reset
- [ ] $0 without the skill

## Dependencies
TASK-059, Bundle K complete

## Estimate
15 min
