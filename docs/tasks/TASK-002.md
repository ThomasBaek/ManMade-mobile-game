# Task 002: Implement Models

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: TASK-001
- **Estimated Time**: 45 min
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 116-240)
- **Requires Design Input**: NO

---

## Formål

Implementer de tre core models: GameState, Operation, og GameConfig.

**Hvorfor dette er vigtigt:**
- Models er datastrukturen for hele spillet
- GameConfig indeholder alle balance-tal
- Korrekte models er fundamentet for GameEngine

---

## Risici

### Potentielle Problemer
1. **Serialization Issues**:
   - Edge case: JSON serialization af nested objects
   - Impact: Save/load fungerer ikke

2. **Calculation Errors**:
   - Edge case: Math.Pow med negative tal
   - Impact: Forkerte upgrade costs

### Mitigering
- Brug simple typer der serialiserer nemt
- Tilføj guard clauses i beregninger

---

## Analyse - Hvad Skal Implementeres

### 1. GameState.cs
**Placering**: `Models/GameState.cs`
- Core game state container
- Cash, TotalEarned, PrestigeCount, PrestigeBonus
- List af OperationState

### 2. Operation.cs
**Placering**: `Models/Operation.cs`
- Definition af en crime/business
- Income og cost beregninger
- Unified model for alle operationer

### 3. GameConfig.cs
**Placering**: `Models/GameConfig.cs`
- Static klasse med alle konstanter
- Alle 5 operations defineret
- Timing og balance konstanter

---

## Dependencies Check

**Krævet Før Start**:
- [ ] TASK-001 completed (CommunityToolkit.Mvvm installeret)

**Antagelser**:
- JSON serialization bruger System.Text.Json

**Blockers**: TASK-001 skal være færdig

---

## Implementation Guide

### Step 1: Opret GameState.cs

**Sti**: `src/MadeMan.IdleEmpire/Models/GameState.cs`

```csharp
namespace MadeMan.IdleEmpire.Models;

public class GameState
{
    // Core resources
    public double Cash { get; set; } = 0;
    public double TotalEarned { get; set; } = 0;

    // Prestige
    public int PrestigeCount { get; set; } = 0;
    public double PrestigeBonus { get; set; } = 1.0;

    // Operations
    public List<OperationState> Operations { get; set; } = new();

    // Meta
    public DateTime LastPlayedUtc { get; set; } = DateTime.UtcNow;
}

public class OperationState
{
    public string Id { get; set; } = string.Empty;
    public int Level { get; set; } = 0; // 0 = locked
}
```

### Step 2: Opret Operation.cs

**Sti**: `src/MadeMan.IdleEmpire/Models/Operation.cs`

```csharp
namespace MadeMan.IdleEmpire.Models;

public class Operation
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Economics
    public double BaseIncome { get; set; }
    public double UnlockCost { get; set; }
    public double UpgradeMultiplier { get; set; } = 1.5;

    // Calculated
    public double GetUpgradeCost(int currentLevel)
    {
        if (currentLevel <= 0) return UnlockCost;
        return UnlockCost * Math.Pow(UpgradeMultiplier, currentLevel);
    }

    public double GetIncome(int level, double prestigeBonus)
    {
        if (level <= 0) return 0;
        return BaseIncome * level * prestigeBonus;
    }
}
```

### Step 3: Opret GameConfig.cs

**Sti**: `src/MadeMan.IdleEmpire/Models/GameConfig.cs`

```csharp
namespace MadeMan.IdleEmpire.Models;

public static class GameConfig
{
    // === OPERATIONS ===
    public static readonly Operation[] Operations = new[]
    {
        new Operation
        {
            Id = "pickpocket",
            Name = "Pickpocketing",
            Icon = "icon_pickpocket.png",
            Description = "Snup lommerne på turisterne",
            BaseIncome = 1.0,
            UnlockCost = 0,
            UpgradeMultiplier = 1.4
        },
        new Operation
        {
            Id = "cartheft",
            Name = "Car Theft",
            Icon = "icon_car.png",
            Description = "Stjæl biler, sælg dele",
            BaseIncome = 4.0,
            UnlockCost = 50,
            UpgradeMultiplier = 1.5
        },
        new Operation
        {
            Id = "burglary",
            Name = "Burglary",
            Icon = "icon_burglary.png",
            Description = "Bryd ind i de riges hjem",
            BaseIncome = 15.0,
            UnlockCost = 250,
            UpgradeMultiplier = 1.6
        },
        new Operation
        {
            Id = "speakeasy",
            Name = "Speakeasy",
            Icon = "icon_speakeasy.png",
            Description = "Din første illegale bar",
            BaseIncome = 50.0,
            UnlockCost = 1000,
            UpgradeMultiplier = 1.8
        },
        new Operation
        {
            Id = "casino",
            Name = "Underground Casino",
            Icon = "icon_casino.png",
            Description = "Huset vinder altid",
            BaseIncome = 200.0,
            UnlockCost = 5000,
            UpgradeMultiplier = 2.0
        }
    };

    // === PRESTIGE ===
    public const double PrestigeThreshold = 10_000;
    public const double PrestigeBonusPerReset = 0.25;

    // === OFFLINE ===
    public const double MaxOfflineHours = 4;
    public const double OfflineEfficiency = 0.5;

    // === TIMING ===
    public const int TicksPerSecond = 10;
    public const int AutoSaveIntervalSeconds = 30;
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Forventet: 0 errors

### 2. Manual Code Review
- Verificer at alle properties har korrekte typer
- Verificer at beregninger er korrekte

---

## Acceptance Criteria

- [ ] GameState.cs oprettet med alle properties
- [ ] Operation.cs oprettet med GetUpgradeCost og GetIncome
- [ ] GameConfig.cs oprettet med alle 5 operations
- [ ] Build succeeds med 0 errors
- [ ] Ingen nye warnings introduceret

---

## Kode Evaluering

### Simplifikations-tjek
- **Unified Operation model**: Ingen separate Crime/Business klasser
- **Ingen inheritance**: Alle operations bruger samme klasse
- **Simple beregninger**: Ingen kompleks logik

### Alternativer overvejet

**Alternativ: Separate Crime og Business klasser**
```csharp
public class Crime : Operation { }
public class Business : Operation { }
```
**Hvorfor fravalgt**: Over-engineering - ingen funktionel forskel i MVP

### Kendte begrænsninger
- Ingen validation af input værdier
- Acceptabelt for MVP

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Er dette den simpleste løsning?
- [ ] **Læsbarhed**: Kan en anden udvikler forstå koden?
- [ ] **Navngivning**: Beskrivende og konsistente navne?
- [ ] **Funktioner**: Korte og fokuserede?
- [ ] **DRY**: Ingen duplikeret kode?
- [ ] **Error handling**: Fejl håndteret?
- [ ] **Edge cases**: Identificeret og håndteret?

---

## Design Files Reference

- **Spec Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 116-240)
- **Related Tasks**: TASK-003, TASK-004

---

## Notes

- OperationState er separat fra Operation for at holde config og state adskilt
- GameConfig er static for nem adgang uden DI

---

**Task Status**: BLOCKED (venter på TASK-001)
**Last Updated**: 2024-12-25
