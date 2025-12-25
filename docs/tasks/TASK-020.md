# Task 020: Skill Calculator

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-019
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 5)
- **Requires Design Input**: NO

---

## Formål

Implementer beregningslogik for alle skill effects.

**Hvorfor dette er vigtigt:**
- Centraliseret beregning af alle skill multipliers
- Korrekt formel for hver effect type
- Genbrugeligt på tværs af GameEngine

---

## Analyse - Hvad Skal Implementeres

### SkillCalculator.cs
**Placering**: `Services/SkillCalculator.cs`

Beregner:
- Multiplier: `1.0 + (level × effectPerLevel / 100)`
- Reduction: `1.0 - (level × effectPerLevel / 100)`
- FlatBonus: `level × effectPerLevel`
- Chance: `level × effectPerLevel` (som procent)
- Duration: `level × effectPerLevel` (som timer)

---

## Implementation Guide

### Step 1: Opret SkillCalculator.cs

**Sti**: `src/MadeMan.IdleEmpire/Services/SkillCalculator.cs`

```csharp
using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public static class SkillCalculator
{
    /// <summary>
    /// Beregn multiplier for en skill (1.0 + bonus)
    /// </summary>
    public static double GetMultiplier(SkillDefinition skill, int level)
    {
        if (level <= 0) return 1.0;
        return 1.0 + (level * skill.EffectPerLevel / 100.0);
    }

    /// <summary>
    /// Beregn reduction for en skill (1.0 - reduction)
    /// </summary>
    public static double GetReduction(SkillDefinition skill, int level)
    {
        if (level <= 0) return 1.0;
        return 1.0 - (level * skill.EffectPerLevel / 100.0);
    }

    /// <summary>
    /// Beregn flat bonus for en skill
    /// </summary>
    public static double GetFlatBonus(SkillDefinition skill, int level)
    {
        if (level <= 0) return 0;
        return level * skill.EffectPerLevel;
    }

    /// <summary>
    /// Beregn chance procent for en skill (0-100)
    /// </summary>
    public static double GetChancePercent(SkillDefinition skill, int level)
    {
        if (level <= 0) return 0;
        return level * skill.EffectPerLevel;
    }

    /// <summary>
    /// Beregn duration bonus i timer
    /// </summary>
    public static double GetDurationHours(SkillDefinition skill, int level)
    {
        if (level <= 0) return 0;
        return level * skill.EffectPerLevel;
    }

    /// <summary>
    /// Beregn Compound Interest multiplier baseret på session tid
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

- [ ] GetMultiplier beregner korrekt
- [ ] GetReduction beregner korrekt
- [ ] GetFlatBonus beregner korrekt
- [ ] GetChancePercent beregner korrekt
- [ ] GetDurationHours beregner korrekt
- [ ] GetCompoundInterestMultiplier med session tid og max 6 intervals
- [ ] RollLuckyBreak med Random
- [ ] Build succeeds med 0 errors

---

## Kode Kvalitet Checklist

- [ ] **Pure functions**: Ingen side effects
- [ ] **Guard clauses**: Level <= 0 checks
- [ ] **Static class**: Ingen state, kun beregninger

---

## Design Files Reference

- **Spec Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 5)
- **Related Tasks**: TASK-019, TASK-021

---

**Task Status**: BLOCKED (venter på TASK-019)
**Last Updated**: 2024-12-25
