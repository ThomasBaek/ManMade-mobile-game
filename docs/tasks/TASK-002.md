# Task 002: Implement Models

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: TASK-001
- **Estimated Time**: 45 min
- **Status**: COMPLETED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (lines 116-240)
- **Requires Design Input**: NO

---

## Purpose

Implement the three core models: GameState, Operation, and GameConfig.

**Why this is important:**
- Models are the data structure for the entire game
- GameConfig contains all balance numbers
- Correct models are the foundation for GameEngine

---

## Risks

### Potential Problems
1. **Serialization Issues**:
   - Edge case: JSON serialization of nested objects
   - Impact: Save/load doesn't work

2. **Calculation Errors**:
   - Edge case: Math.Pow with negative numbers
   - Impact: Incorrect upgrade costs

### Mitigation
- Use simple types that serialize easily
- Add guard clauses in calculations

---

## Analysis - What Needs to be Implemented

### 1. GameState.cs
**Location**: `Models/GameState.cs`
- Core game state container
- Cash, TotalEarned, PrestigeCount, PrestigeBonus
- List of OperationState

### 2. Operation.cs
**Location**: `Models/Operation.cs`
- Definition of a crime/business
- Income and cost calculations
- Unified model for all operations

### 3. GameConfig.cs
**Location**: `Models/GameConfig.cs`
- Static class with all constants
- All 5 operations defined
- Timing and balance constants

---

## Dependencies Check

**Required Before Start**:
- [x] TASK-001 completed (CommunityToolkit.Mvvm installed)

**Assumptions**:
- JSON serialization uses System.Text.Json

**Blockers**: TASK-001 must be complete

---

## Implementation Guide

### Step 1: Create GameState.cs

**Path**: `src/MadeMan.IdleEmpire/Models/GameState.cs`

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

### Step 2: Create Operation.cs

**Path**: `src/MadeMan.IdleEmpire/Models/Operation.cs`

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

### Step 3: Create GameConfig.cs

**Path**: `src/MadeMan.IdleEmpire/Models/GameConfig.cs`

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
            Description = "Steal from tourists' pockets",
            BaseIncome = 1.0,
            UnlockCost = 0,
            UpgradeMultiplier = 1.4
        },
        new Operation
        {
            Id = "cartheft",
            Name = "Car Theft",
            Icon = "icon_car.png",
            Description = "Steal cars, sell parts",
            BaseIncome = 4.0,
            UnlockCost = 50,
            UpgradeMultiplier = 1.5
        },
        new Operation
        {
            Id = "burglary",
            Name = "Burglary",
            Icon = "icon_burglary.png",
            Description = "Break into the rich's homes",
            BaseIncome = 15.0,
            UnlockCost = 250,
            UpgradeMultiplier = 1.6
        },
        new Operation
        {
            Id = "speakeasy",
            Name = "Speakeasy",
            Icon = "icon_speakeasy.png",
            Description = "Your first illegal bar",
            BaseIncome = 50.0,
            UnlockCost = 1000,
            UpgradeMultiplier = 1.8
        },
        new Operation
        {
            Id = "casino",
            Name = "Underground Casino",
            Icon = "icon_casino.png",
            Description = "The house always wins",
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
Expected: 0 errors

### 2. Manual Code Review
- Verify all properties have correct types
- Verify calculations are correct

---

## Acceptance Criteria

- [x] GameState.cs created with all properties
- [x] Operation.cs created with GetUpgradeCost and GetIncome
- [x] GameConfig.cs created with all 5 operations
- [x] Build succeeds with 0 errors
- [x] No new warnings introduced

---

## Code Evaluation

### Simplification Check
- **Unified Operation model**: No separate Crime/Business classes
- **No inheritance**: All operations use the same class
- **Simple calculations**: No complex logic

### Alternatives Considered

**Alternative: Separate Crime and Business classes**
```csharp
public class Crime : Operation { }
public class Business : Operation { }
```
**Why rejected**: Over-engineering - no functional difference in MVP

### Known Limitations
- No validation of input values
- Acceptable for MVP

---

## Code Quality Checklist

- [x] **KISS**: Is this the simplest solution?
- [x] **Readability**: Can another developer understand the code?
- [x] **Naming**: Descriptive and consistent names?
- [x] **Functions**: Short and focused?
- [x] **DRY**: No duplicated code?
- [x] **Error handling**: Errors handled?
- [x] **Edge cases**: Identified and handled?

---

## Design Files Reference

- **Spec Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (lines 116-240)
- **Related Tasks**: TASK-003, TASK-004

---

## Notes

- OperationState is separate from Operation to keep config and state separate
- GameConfig is static for easy access without DI

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Completed**: 2024-12-25
