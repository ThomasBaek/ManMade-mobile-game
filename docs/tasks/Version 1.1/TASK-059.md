# TASK-059: Create SkillTestBase Class

## Bundle
**Bundle L: Test Infrastructure**

## Description
Create a base test class with helper methods for skill testing.

## Changes Required

**File:** `tests/MadeMan.IdleEmpire.Tests/SkillTestBase.cs`

```csharp
using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;

namespace MadeMan.IdleEmpire.Tests;

public abstract class SkillTestBase
{
    protected GameState GameState { get; }
    protected ISkillService SkillService { get; }

    protected SkillTestBase()
    {
        GameState = new GameState();
        // Initialize SkillService with GameState
        // May need to mock or create test-friendly version
    }

    protected void AddSkill(string skillId, int level = 1)
    {
        var existing = GameState.Skills.FirstOrDefault(s => s.Id == skillId);
        if (existing != null)
        {
            existing.Level = level;
        }
        else
        {
            GameState.Skills.Add(new SkillState { Id = skillId, Level = level });
        }
    }

    protected void ClearSkills()
    {
        GameState.Skills.Clear();
    }
}
```

## Acceptance Criteria
- [ ] SkillTestBase class created
- [ ] AddSkill helper works
- [ ] ClearSkills helper works
- [ ] `dotnet test` runs (may have 0 tests)

## Dependencies
TASK-058

## Estimate
20 min

## Bundle L Complete Verification
After this task, verify:
- [ ] Test project builds
- [ ] `dotnet test` command works
- [ ] Base class ready for skill tests
