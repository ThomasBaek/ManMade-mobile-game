# TASK-064: Test Operation-Specific Skills

## Bundle
**Bundle M: Skill Tests - Income & Operations**

## Description
Create tests for all 4 operation-specific skills (QuickHands, ChopShop, InsideMan, HappyHour).

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/OperationSkillsTests.cs`

```csharp
public class OperationSkillsTests : SkillTestBase
{
    [Theory]
    [InlineData("quick_hands", "pickpocket", 1, 1.12)]
    [InlineData("quick_hands", "pickpocket", 5, 1.60)]
    [InlineData("chop_shop", "cartheft", 1, 1.12)]
    [InlineData("chop_shop", "cartheft", 5, 1.60)]
    [InlineData("inside_man", "burglary", 1, 1.12)]
    [InlineData("inside_man", "burglary", 5, 1.60)]
    [InlineData("happy_hour", "speakeasy", 1, 1.12)]
    [InlineData("happy_hour", "speakeasy", 5, 1.60)]
    public void OperationSkill_BoostsCorrectOperation(
        string skillId, string operationId, int level, double expected)
    {
        // Arrange
        AddSkill(skillId, level);

        // Act
        var multiplier = SkillService.GetOperationMultiplier(operationId);

        // Assert
        Assert.Equal(expected, multiplier, precision: 2);
    }

    [Fact]
    public void QuickHands_DoesNotAffectOtherOperations()
    {
        // Arrange
        AddSkill("quick_hands", 5);

        // Act
        var pickpocket = SkillService.GetOperationMultiplier("pickpocket");
        var carTheft = SkillService.GetOperationMultiplier("cartheft");
        var burglary = SkillService.GetOperationMultiplier("burglary");

        // Assert
        Assert.Equal(1.60, pickpocket, precision: 2);
        Assert.Equal(1.0, carTheft);   // Not affected
        Assert.Equal(1.0, burglary);   // Not affected
    }

    [Fact]
    public void AllOperationSkills_Independent()
    {
        // Arrange - add all operation skills
        AddSkill("quick_hands", 3);
        AddSkill("chop_shop", 2);
        AddSkill("inside_man", 1);
        AddSkill("happy_hour", 4);

        // Act & Assert - each only affects its operation
        Assert.Equal(1.36, SkillService.GetOperationMultiplier("pickpocket"), 2);
        Assert.Equal(1.24, SkillService.GetOperationMultiplier("cartheft"), 2);
        Assert.Equal(1.12, SkillService.GetOperationMultiplier("burglary"), 2);
        Assert.Equal(1.48, SkillService.GetOperationMultiplier("speakeasy"), 2);
        Assert.Equal(1.0, SkillService.GetOperationMultiplier("casino"));
    }
}
```

## Acceptance Criteria
- [ ] All 4 operation skills tested at multiple levels
- [ ] Skills only affect their target operation
- [ ] Skills don't interfere with each other

## Dependencies
TASK-063

## Estimate
25 min

## Bundle M Complete Verification
After this task, verify:
- [ ] All income skill tests pass
- [ ] All operation skill tests pass
- [ ] `dotnet test` shows all green
