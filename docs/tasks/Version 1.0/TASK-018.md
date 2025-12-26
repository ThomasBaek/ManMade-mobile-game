# Task 018: Skill Models

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-002
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md
- **Requires Design Input**: NO

---

## Purpose

Create data models for the skill system and extend GameState.

**Why this is important:**
- Skills are the foundation for the entire skill system
- GameState needs to be able to hold skill data
- Correct models ensure easy serialization

---

## Risks

### Potential Issues
1. **Serialization**:
   - Edge case: Nested objects in JSON
   - Impact: Save/load errors

### Mitigation
- Use simple types
- Test serialization early

---

## Analysis - What to Implement

### 1. Skill.cs
**Location**: `Models/Skill.cs`

Contains:
- `SkillDefinition` - Static skill data (id, name, effect, etc.)
- `SkillState` - Player's skill instance (id, level)

### 2. GameState.cs (extend)
**Changes:**
- Add `List<SkillState> Skills`
- Add `int MilestoneCount`
- Add `DateTime SessionStartUtc`

---

## Dependencies Check

**Required Before Start**:
- [x] TASK-002 completed (Models exist)

**Assumptions**:
- System.Text.Json is used for serialization

**Blockers**: None

---

## Implementation Guide

### Step 1: Create Skill.cs

**Path**: `src/MadeMan.IdleEmpire/Models/Skill.cs`

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
    Multiplier,      // Multiply value (1.0 + bonus)
    Reduction,       // Reduce value (1.0 - bonus)
    FlatBonus,       // Add flat value
    Chance,          // Percentage chance
    Duration         // Time in hours/minutes
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

### Step 2: Extend GameState.cs

**Path**: `src/MadeMan.IdleEmpire/Models/GameState.cs`

Add fields:

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
Expected: 0 errors

### 2. Serialization Test (conceptual)
- GameState with skills can be serialized to JSON
- JSON can be deserialized back to GameState

---

## Acceptance Criteria

- [ ] SkillDefinition class created with all properties
- [ ] SkillState class created
- [ ] SkillCategory enum created
- [ ] SkillEffectType enum created
- [ ] GameState extended with Skills, MilestoneCount, SessionStartUtc
- [ ] Build succeeds with 0 errors

---

## Code Evaluation

### Simplification Check
- **Simple types**: All properties are primitives or simple collections
- **No logic**: Models are pure data containers
- **Enum categories**: Easy to group and filter

### Alternatives Considered

**Alternative: Inheritance for skill types**
```csharp
public abstract class SkillDefinition { }
public class IncomeSkill : SkillDefinition { }
```
**Why rejected**: Over-engineering, all skills have the same structure

---

## Code Quality Checklist

- [ ] **KISS**: Simplest data structure
- [ ] **Serializable**: All types are JSON-friendly
- [ ] **Nullable**: TargetOperationId and TargetTier are nullable for flexibility

---

## Design Files Reference

- **Spec Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 4)
- **Related Tasks**: TASK-019, TASK-020

---

## Notes

- SkillDefinition is immutable (defined in SkillConfig)
- SkillState is mutable (player's progression)
- SessionStartUtc resets on app start and prestige

---

**Task Status**: BLOCKED (waiting for TASK-006 completion for workflow)
**Last Updated**: 2024-12-25
