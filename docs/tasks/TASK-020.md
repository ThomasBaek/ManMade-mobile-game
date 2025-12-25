# Task 020: Skill Calculator

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-019
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 5)
- **Requires Design Input**: NO

---

## Purpose

Implement calculation logic for all skill effects.

**Why this is important:**
- Centralized calculation of all skill multipliers
- Correct formula for each effect type
- Reusable across GameEngine

---

## Analysis - What to Implement

### SkillCalculator.cs
**Location**: `Services/SkillCalculator.cs`

Calculates:
- Multiplier: `1.0 + (level × effectPerLevel / 100)`
- Reduction: `1.0 - (level × effectPerLevel / 100)`
- FlatBonus: `level × effectPerLevel`
- Chance: `level × effectPerLevel` (as percent)
- Duration: `level × effectPerLevel` (as hours)

---

## Implementation Guide

### Step 1: Create SkillCalculator.cs

**Path**: `src/MadeMan.IdleEmpire/Services/SkillCalculator.cs`

```csharp
using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public static class SkillCalculator
{
    /// <summary>
    /// Calculate multiplier for a skill (1.0 + bonus)
    /// </summary>
    public static double GetMultiplier(SkillDefinition skill, int level)
    {
        if (level <= 0) return 1.0;
        return 1.0 + (level * skill.EffectPerLevel / 100.0);
    }

    /// <summary>
    /// Calculate reduction for a skill (1.0 - reduction)
    /// </summary>
    public static double GetReduction(SkillDefinition skill, int level)
    {
        if (level <= 0) return 1.0;
        return 1.0 - (level * skill.EffectPerLevel / 100.0);
    }

    /// <summary>
    /// Calculate flat bonus for a skill
    /// </summary>
    public static double GetFlatBonus(SkillDefinition skill, int level)
    {
        if (level <= 0) return 0;
        return level * skill.EffectPerLevel;
    }

    /// <summary>
    /// Calculate chance percent for a skill (0-100)
    /// </summary>
    public static double GetChancePercent(SkillDefinition skill, int level)
    {
        if (level <= 0) return 0;
        return level * skill.EffectPerLevel;
    }

    /// <summary>
    /// Calculate duration bonus in hours
    /// </summary>
    public static double GetDurationHours(SkillDefinition skill, int level)
    {
        if (level <= 0) return 0;
        return level * skill.EffectPerLevel;
    }

    /// <summary>
    /// Calculate Compound Interest multiplier based on session time
    /// </summary>
    public static double GetCompoundInterestMultiplier(int skillLevel, DateTime sessionStart)
    {
        if (skillLevel <= 0) return 1.0;

        var sessionMinutes = (DateTime.UtcNow - sessionStart).TotalMinutes;
        var intervals = Math.Min((int)(sessionMinutes / 5), 6); // Max 6 intervals (30 min)

        // +3% per level per 5-minute interval
        var bonusPercent = skillLevel * 3.0 * intervals;
        return 1.0 + (bonusPercent / 100.0);
    }

    /// <summary>
    /// Check for Lucky Break critical hit
    /// </summary>
    public static bool RollLuckyBreak(int skillLevel, Random random)
    {
        if (skillLevel <= 0) return false;

        var chance = skillLevel * 4.0; // 4% per level
        return random.NextDouble() * 100 < chance;
    }
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Calculation Verification (conceptual)
| Skill | Level | Expected |
|-------|-------|----------|
| Cash Flow (4%) | 3 | 1.12x multiplier |
| Fast Learner (4%) | 3 | 0.88x reduction |
| Passive Income ($1) | 5 | $5 flat bonus |
| Lucky Break (4%) | 5 | 20% chance |
| Extended Shift (1h) | 3 | 3 hours |

---

## Acceptance Criteria

- [ ] GetMultiplier calculates correctly
- [ ] GetReduction calculates correctly
- [ ] GetFlatBonus calculates correctly
- [ ] GetChancePercent calculates correctly
- [ ] GetDurationHours calculates correctly
- [ ] GetCompoundInterestMultiplier with session time and max 6 intervals
- [ ] RollLuckyBreak with Random
- [ ] Build succeeds with 0 errors

---

## Code Quality Checklist

- [ ] **Pure functions**: No side effects
- [ ] **Guard clauses**: Level <= 0 checks
- [ ] **Static class**: No state, only calculations

---

## Design Files Reference

- **Spec Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 5)
- **Related Tasks**: TASK-019, TASK-021

---

**Task Status**: BLOCKED (waiting for TASK-019)
**Last Updated**: 2024-12-25
