# TASK-060: Test CashFlow Skill

## Bundle
**Bundle M: Skill Tests - Income & Operations**

## Description
Create tests for the CashFlow skill that provides global income bonus.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/Skills/IncomeSkillsTests.cs`

```csharp
public class CashFlowTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 1.03)]  // Level 1: +3%
    [InlineData(2, 1.06)]  // Level 2: +6%
    [InlineData(3, 1.09)]  // Level 3: +9%
    [InlineData(4, 1.12)]  // Level 4: +12%
    [InlineData(5, 1.15)]  // Level 5: +15% (max)
    public void CashFlow_AppliesCorrectMultiplier(int level, double expected)
    {
        // Arrange
        AddSkill("cash_flow", level);

        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();

        // Assert
        Assert.Equal(expected, multiplier, precision: 2);
    }

    [Fact]
    public void CashFlow_AffectsAllOperations()
    {
        // Arrange
        AddSkill("cash_flow", 3);

        // Act & Assert - verify all operations get the bonus
        foreach (var opId in new[] { "pickpocket", "cartheft", "burglary", "speakeasy", "casino" })
        {
            var multiplier = SkillService.GetIncomeMultiplierForOperation(opId);
            Assert.True(multiplier >= 1.09);
        }
    }
}
```

## Acceptance Criteria
- [ ] CashFlow test class created
- [ ] All 5 level tests pass
- [ ] All operations affected test passes
- [ ] `dotnet test` shows tests passing

## Dependencies
TASK-059, Bundle K complete

## Estimate
20 min
