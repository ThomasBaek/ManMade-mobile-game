# Task 022: Milestone Service

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-021
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 3)
- **Requires Design Input**: NO

---

## Formål

Implementer milestone trigger detection og skill selection pool logic.

**Hvorfor dette er vigtigt:**
- Bestemmer hvornår spilleren får skill valg
- Korrekt selection pool baseret på regler
- Event til UI for at vise modal

---

## Analyse - Hvad Skal Implementeres

### IMilestoneService.cs & MilestoneService.cs
**Placering**: `Services/`

Funktioner:
- `CheckForMilestone()` - Check om ny milestone er nået
- `GetNextMilestoneThreshold()` - Næste threshold
- `GetMilestoneProgress()` - Progress mod næste (0-1)
- `GetSelectionPool()` - Vælg 3 tilfældige skills
- `OnMilestoneReached` - Event til UI

---

## Implementation Guide

### Step 1: Opret IMilestoneService.cs

**Sti**: `src/MadeMan.IdleEmpire/Services/IMilestoneService.cs`

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

### Step 2: Opret MilestoneService.cs

**Sti**: `src/MadeMan.IdleEmpire/Services/MilestoneService.cs`

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

- [ ] IMilestoneService interface oprettet
- [ ] MilestoneService implementerer alle metoder
- [ ] CheckForMilestone returnerer true/false korrekt
- [ ] GetSelectionPool returnerer max 3 skills
- [ ] Selection respekterer "5 skills = only upgrade" regel
- [ ] OnMilestoneReached event fires korrekt
- [ ] CompleteMilestone opdaterer state
- [ ] Build succeeds med 0 errors

---

## Edge Cases Håndteret

- [ ] Færre end 3 skills tilgængelige → Vis færre
- [ ] Ingen skills tilgængelige → Skip milestone
- [ ] Spilleren har præcis 5 skills → Kun upgrades
- [ ] MilestoneCount >= 10 → Ingen flere milestones

---

**Task Status**: BLOCKED (venter på TASK-021)
**Last Updated**: 2024-12-25
