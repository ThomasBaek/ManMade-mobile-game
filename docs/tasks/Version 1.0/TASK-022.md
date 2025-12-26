# Task 022: Milestone Service

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-021
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 3)
- **Requires Design Input**: NO

---

## Purpose

Implement milestone trigger detection and skill selection pool logic.

**Why this is important:**
- Determines when player gets skill choices
- Correct selection pool based on rules
- Event to UI for showing modal

---

## Analysis - What to Implement

### IMilestoneService.cs & MilestoneService.cs
**Location**: `Services/`

Functions:
- `CheckForMilestone()` - Check if new milestone is reached
- `GetNextMilestoneThreshold()` - Next threshold
- `GetMilestoneProgress()` - Progress towards next (0-1)
- `GetSelectionPool()` - Select 3 random skills
- `OnMilestoneReached` - Event to UI

---

## Implementation Guide

### Step 1: Create IMilestoneService.cs

**Path**: `src/MadeMan.IdleEmpire/Services/IMilestoneService.cs`

```csharp
using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public interface IMilestoneService
{
    event Action<List<SkillDefinition>>? OnMilestoneReached;

    bool CheckForMilestone();
    double GetNextMilestoneThreshold();
    double GetMilestoneProgress();
    List<SkillDefinition> GetSelectionPool();
    void CompleteMilestone(string selectedSkillId);
}
```

### Step 2: Create MilestoneService.cs

**Path**: `src/MadeMan.IdleEmpire/Services/MilestoneService.cs`

```csharp
using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public class MilestoneService : IMilestoneService
{
    private readonly Func<GameState> _getState;
    private readonly ISkillService _skillService;
    private readonly Random _random = new();

    public event Action<List<SkillDefinition>>? OnMilestoneReached;

    public MilestoneService(Func<GameState> getState, ISkillService skillService)
    {
        _getState = getState;
        _skillService = skillService;
    }

    private GameState State => _getState();

    public bool CheckForMilestone()
    {
        // Already at max milestones?
        if (State.MilestoneCount >= SkillConfig.MilestoneThresholds.Length)
            return false;

        var nextThreshold = GetNextMilestoneThreshold();
        if (State.TotalEarned >= nextThreshold)
        {
            var pool = GetSelectionPool();
            if (pool.Count > 0)
            {
                OnMilestoneReached?.Invoke(pool);
                return true;
            }
        }

        return false;
    }

    public double GetNextMilestoneThreshold()
    {
        if (State.MilestoneCount >= SkillConfig.MilestoneThresholds.Length)
            return double.MaxValue;

        return SkillConfig.MilestoneThresholds[State.MilestoneCount];
    }

    public double GetMilestoneProgress()
    {
        if (State.MilestoneCount >= SkillConfig.MilestoneThresholds.Length)
            return 1.0;

        var threshold = GetNextMilestoneThreshold();
        var previousThreshold = State.MilestoneCount > 0
            ? SkillConfig.MilestoneThresholds[State.MilestoneCount - 1]
            : 0;

        var progress = State.TotalEarned - previousThreshold;
        var range = threshold - previousThreshold;

        return Math.Clamp(progress / range, 0, 1);
    }

    public List<SkillDefinition> GetSelectionPool()
    {
        var availableSkills = new List<SkillDefinition>();
        var playerHasMaxSkills = _skillService.GetActiveSkillCount() >= SkillConfig.MaxSkills;

        foreach (var skill in SkillConfig.Skills)
        {
            // Skip maxed skills
            if (_skillService.IsSkillMaxed(skill.Id))
                continue;

            // If player has 5 skills, only show existing skills
            if (playerHasMaxSkills && !_skillService.HasSkill(skill.Id))
                continue;

            availableSkills.Add(skill);
        }

        // Shuffle and take up to 3
        var shuffled = availableSkills.OrderBy(_ => _random.Next()).ToList();
        return shuffled.Take(SkillConfig.SkillChoicesPerMilestone).ToList();
    }

    public void CompleteMilestone(string selectedSkillId)
    {
        _skillService.AddOrUpgradeSkill(selectedSkillId);
        State.MilestoneCount++;
    }
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Logic Verification (conceptual)
| Scenario | Expected |
|----------|----------|
| TotalEarned = $500, MilestoneCount = 0 | No trigger ($1000 needed) |
| TotalEarned = $1500, MilestoneCount = 0 | Trigger! Pool generated |
| Player has 5 skills | Pool only contains existing |
| All skills maxed | Empty pool, no trigger |
| MilestoneCount = 10 | No more triggers |

---

## Acceptance Criteria

- [ ] IMilestoneService interface created
- [ ] MilestoneService implements all methods
- [ ] CheckForMilestone returns true/false correctly
- [ ] GetSelectionPool returns max 3 skills
- [ ] Selection respects "5 skills = only upgrade" rule
- [ ] OnMilestoneReached event fires correctly
- [ ] CompleteMilestone updates state
- [ ] Build succeeds with 0 errors

---

## Edge Cases Handled

- [ ] Fewer than 3 skills available → Show fewer
- [ ] No skills available → Skip milestone
- [ ] Player has exactly 5 skills → Only upgrades
- [ ] MilestoneCount >= 10 → No more milestones

---

**Task Status**: BLOCKED (waiting for TASK-021)
**Last Updated**: 2024-12-25
