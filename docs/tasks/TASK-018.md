# Task 018: Skill Models

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-002
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md
- **Requires Design Input**: NO

---

## Formål

Opret data models for skill systemet og udvid GameState.

**Hvorfor dette er vigtigt:**
- Skills er fundamentet for hele skill systemet
- GameState skal kunne holde skill data
- Korrekte models sikrer nem serialisering

---

## Risici

### Potentielle Problemer
1. **Serialization**:
   - Edge case: Nested objekter i JSON
   - Impact: Save/load fejler

### Mitigering
- Brug simple typer
- Test serialisering tidligt

---

## Analyse - Hvad Skal Implementeres

### 1. Skill.cs
**Placering**: `Models/Skill.cs`

Indeholder:
- `SkillDefinition` - Static skill data (id, name, effect, etc.)
- `SkillState` - Player's skill instance (id, level)

### 2. GameState.cs (udvid)
**Ændringer:**
- Tilføj `List<SkillState> Skills`
- Tilføj `int MilestoneCount`
- Tilføj `DateTime SessionStartUtc`

---

## Dependencies Check

**Krævet Før Start**:
- [x] TASK-002 completed (Models eksisterer)

**Antagelser**:
- System.Text.Json bruges til serialisering

**Blockers**: Ingen

---

## Implementation Guide

### Step 1: Opret Skill.cs

**Sti**: `src/MadeMan.IdleEmpire/Models/Skill.cs`

```csharp
namespace MadeMan.IdleEmpire.Models;

public enum SkillCategory
{
    Income,
    Operations,
    Offline,
    Prestige
}

public enum SkillEffectType
{
    Multiplier,      // Multiplicer værdi (1.0 + bonus)
    Reduction,       // Reducer værdi (1.0 - bonus)
    FlatBonus,       // Addér flat værdi
    Chance,          // Procentvis chance
    Duration         // Tid i timer/minutter
}

public class SkillDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public SkillEffectType EffectType { get; set; }

    // Effect values
    public double EffectPerLevel { get; set; }
    public int MaxLevel { get; set; } = 5;

    // For operation-specific skills
    public string? TargetOperationId { get; set; }

    // For tier-specific skills (1 = Pickpocket/CarTheft/Burglary, 2 = Speakeasy/Casino)
    public int? TargetTier { get; set; }
}

public class SkillState
{
    public string Id { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
}
```

### Step 2: Udvid GameState.cs

**Sti**: `src/MadeMan.IdleEmpire/Models/GameState.cs`

Tilføj felter:

```csharp
// Skills
public List<SkillState> Skills { get; set; } = new();
public int MilestoneCount { get; set; } = 0;

// Session tracking (for Compound Interest skill)
public DateTime SessionStartUtc { get; set; } = DateTime.UtcNow;
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Forventet: 0 errors

### 2. Serialization Test (conceptual)
- GameState med skills kan serialiseres til JSON
- JSON kan deserialiseres tilbage til GameState

---

## Acceptance Criteria

- [ ] SkillDefinition class oprettet med alle properties
- [ ] SkillState class oprettet
- [ ] SkillCategory enum oprettet
- [ ] SkillEffectType enum oprettet
- [ ] GameState udvidet med Skills, MilestoneCount, SessionStartUtc
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Simple typer**: Alle properties er primitives eller simple collections
- **Ingen logik**: Models er pure data containers
- **Enum categories**: Nemt at gruppere og filtrere

### Alternativer overvejet

**Alternativ: Inheritance for skill types**
```csharp
public abstract class SkillDefinition { }
public class IncomeSkill : SkillDefinition { }
```
**Hvorfor fravalgt**: Over-engineering, alle skills har samme struktur

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Simpleste data struktur
- [ ] **Serializable**: Alle typer JSON-venlige
- [ ] **Nullable**: TargetOperationId og TargetTier er nullable for fleksibilitet

---

## Design Files Reference

- **Spec Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 4)
- **Related Tasks**: TASK-019, TASK-020

---

## Notes

- SkillDefinition er immutable (defineres i SkillConfig)
- SkillState er mutable (spillerens progression)
- SessionStartUtc resettes ved app start og prestige

---

**Task Status**: BLOCKED (venter på TASK-006 completion for workflow)
**Last Updated**: 2024-12-25
