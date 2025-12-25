# Task 019: Skill Config (18 Skills)

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-018
- **Estimated Time**: 45 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 4)
- **Requires Design Input**: NO

---

## Formål

Definér alle 18 skills med deres effekter og værdier.

**Hvorfor dette er vigtigt:**
- Alle skill effects skal være korrekt defineret
- Balance tal fra spec skal overholdes
- Central konfiguration for nem justering

---

## Analyse - Hvad Skal Implementeres

### SkillConfig.cs
**Placering**: `Models/SkillConfig.cs`

Definerer alle 18 skills:

**💰 Income (6 stk):**
- Cash Flow (+4% total income per level)
- Street Smarts (+10% Tier 1 per level)
- Business Acumen (+12% Tier 2 per level)
- Lucky Break (+4% chance for 2x per level)
- The Skim (+5% cashback per level)
- Compound Interest (+3% per 5 min per level)

**⚡ Operations (4 stk):**
- Quick Hands (+25% Pickpocket per level)
- Chop Shop (+20% Car Theft per level)
- Inside Man (+18% Burglary per level)
- Happy Hour (+15% Speakeasy per level)

**📴 Offline (4 stk):**
- Night Owl (+10% offline efficiency per level)
- Extended Shift (+1 hour max offline per level)
- Passive Income (+$1/s baseline per level)
- Godfather's Cut (+8% offline bonus per level)

**⭐ Prestige (4 stk):**
- Old Connections (+$100 start cash per level)
- Reputation (+6% prestige bonus per level)
- Fast Learner (-4% upgrade costs per level)
- Early Bird (-5% unlock costs per level)

---

## Dependencies Check

**Krævet Før Start**:
- [ ] TASK-018 completed (Skill models)

---

## Implementation Guide

### Step 1: Opret SkillConfig.cs

**Sti**: `src/MadeMan.IdleEmpire/Models/SkillConfig.cs`

```csharp
namespace MadeMan.IdleEmpire.Models;

public static class SkillConfig
{
    // === MILESTONE THRESHOLDS ===
    public static readonly double[] MilestoneThresholds = new[]
    {
        1_000,
        5_000,
        15_000,
        50_000,
        150_000,
        500_000,
        1_500_000,
        5_000_000,
        15_000_000,
        50_000_000
    };

    public const int MaxSkills = 5;
    public const int MaxSkillLevel = 5;
    public const int SkillChoicesPerMilestone = 3;

    // === ALL SKILLS ===
    public static readonly SkillDefinition[] Skills = new[]
    {
        // === INCOME SKILLS (6) ===
        new SkillDefinition
        {
            Id = "cash_flow",
            Name = "Cash Flow",
            Description = "Øger total indkomst",
            Icon = "skill_cash_flow.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 4.0 // +4% per level
        },
        new SkillDefinition
        {
            Id = "street_smarts",
            Name = "Street Smarts",
            Description = "Booster Tier 1 operationer",
            Icon = "skill_street_smarts.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 10.0, // +10% per level
            TargetTier = 1
        },
        new SkillDefinition
        {
            Id = "business_acumen",
            Name = "Business Acumen",
            Description = "Booster Tier 2 operationer",
            Icon = "skill_business_acumen.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 12.0, // +12% per level
            TargetTier = 2
        },
        new SkillDefinition
        {
            Id = "lucky_break",
            Name = "Lucky Break",
            Description = "Chance for dobbelt indkomst",
            Icon = "skill_lucky_break.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Chance,
            EffectPerLevel = 4.0 // +4% chance per level
        },
        new SkillDefinition
        {
            Id = "the_skim",
            Name = "The Skim",
            Description = "Cashback på alle køb",
            Icon = "skill_the_skim.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 5.0 // +5% cashback per level
        },
        new SkillDefinition
        {
            Id = "compound_interest",
            Name = "Compound Interest",
            Description = "Indkomst stiger over tid",
            Icon = "skill_compound_interest.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 3.0 // +3% per 5 min per level (max 6 intervals)
        },

        // === OPERATIONS SKILLS (4) ===
        new SkillDefinition
        {
            Id = "quick_hands",
            Name = "Quick Hands",
            Description = "Booster Pickpocketing",
            Icon = "skill_quick_hands.png",
            Category = SkillCategory.Operations,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 25.0, // +25% per level
            TargetOperationId = "pickpocket"
        },
        new SkillDefinition
        {
            Id = "chop_shop",
            Name = "Chop Shop",
            Description = "Booster Car Theft",
            Icon = "skill_chop_shop.png",
            Category = SkillCategory.Operations,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 20.0, // +20% per level
            TargetOperationId = "cartheft"
        },
        new SkillDefinition
        {
            Id = "inside_man",
            Name = "Inside Man",
            Description = "Booster Burglary",
            Icon = "skill_inside_man.png",
            Category = SkillCategory.Operations,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 18.0, // +18% per level
            TargetOperationId = "burglary"
        },
        new SkillDefinition
        {
            Id = "happy_hour",
            Name = "Happy Hour",
            Description = "Booster Speakeasy",
            Icon = "skill_happy_hour.png",
            Category = SkillCategory.Operations,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 15.0, // +15% per level
            TargetOperationId = "speakeasy"
        },

        // === OFFLINE SKILLS (4) ===
        new SkillDefinition
        {
            Id = "night_owl",
            Name = "Night Owl",
            Description = "Øger offline efficiency",
            Icon = "skill_night_owl.png",
            Category = SkillCategory.Offline,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 10.0 // +10% efficiency per level (50% -> 100%)
        },
        new SkillDefinition
        {
            Id = "extended_shift",
            Name = "Extended Shift",
            Description = "Forlænger max offline tid",
            Icon = "skill_extended_shift.png",
            Category = SkillCategory.Offline,
            EffectType = SkillEffectType.Duration,
            EffectPerLevel = 1.0 // +1 hour per level
        },
        new SkillDefinition
        {
            Id = "passive_income",
            Name = "Passive Income",
            Description = "Baseline indkomst uanset operationer",
            Icon = "skill_passive_income.png",
            Category = SkillCategory.Offline,
            EffectType = SkillEffectType.FlatBonus,
            EffectPerLevel = 1.0 // +$1/s per level
        },
        new SkillDefinition
        {
            Id = "godfathers_cut",
            Name = "Godfather's Cut",
            Description = "Bonus på offline earnings",
            Icon = "skill_godfathers_cut.png",
            Category = SkillCategory.Offline,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 8.0 // +8% per level
        },

        // === PRESTIGE SKILLS (4) ===
        new SkillDefinition
        {
            Id = "old_connections",
            Name = "Old Connections",
            Description = "Start med bonus cash efter prestige",
            Icon = "skill_old_connections.png",
            Category = SkillCategory.Prestige,
            EffectType = SkillEffectType.FlatBonus,
            EffectPerLevel = 100.0 // +$100 per level
        },
        new SkillDefinition
        {
            Id = "reputation",
            Name = "Reputation",
            Description = "Øger prestige bonus",
            Icon = "skill_reputation.png",
            Category = SkillCategory.Prestige,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 6.0 // +6% per level
        },
        new SkillDefinition
        {
            Id = "fast_learner",
            Name = "Fast Learner",
            Description = "Reducerer upgrade costs",
            Icon = "skill_fast_learner.png",
            Category = SkillCategory.Prestige,
            EffectType = SkillEffectType.Reduction,
            EffectPerLevel = 4.0 // -4% per level
        },
        new SkillDefinition
        {
            Id = "early_bird",
            Name = "Early Bird",
            Description = "Reducerer unlock costs",
            Icon = "skill_early_bird.png",
            Category = SkillCategory.Prestige,
            EffectType = SkillEffectType.Reduction,
            EffectPerLevel = 5.0 // -5% per level
        }
    };

    // === HELPER: Get operation tier ===
    public static int GetOperationTier(string operationId)
    {
        return operationId switch
        {
            "pickpocket" or "cartheft" or "burglary" => 1,
            "speakeasy" or "casino" => 2,
            _ => 0
        };
    }
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Forventet: 0 errors

### 2. Count Verification
- 6 Income skills
- 4 Operations skills
- 4 Offline skills
- 4 Prestige skills
- Total: 18 skills

---

## Acceptance Criteria

- [ ] Alle 18 skills defineret
- [ ] MilestoneThresholds array med 10 værdier
- [ ] MaxSkills = 5, MaxSkillLevel = 5
- [ ] Alle effect values matcher spec
- [ ] GetOperationTier helper metode
- [ ] Build succeeds med 0 errors

---

## Kode Kvalitet Checklist

- [ ] **Balance**: Alle tal matcher SKILL_SYSTEM_SPECIFICATION.md
- [ ] **Kategorisering**: Skills korrekt grupperet
- [ ] **Naming**: Konsistente skill IDs (snake_case)

---

## Design Files Reference

- **Spec Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 4)
- **Related Tasks**: TASK-018, TASK-020

---

**Task Status**: BLOCKED (venter på TASK-018)
**Last Updated**: 2024-12-25
