# Task 004: Implement GameEngine

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: TASK-003
- **Estimated Time**: 1 hour
- **Status**: COMPLETED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (lines 243-392)
- **Requires Design Input**: NO

---

## Purpose

Implement GameEngine service - the heart of the game that handles all game logic.

**Why this is important:**
- Core gameplay loop lives here
- All income calculation happens here
- Upgrade/unlock/prestige logic

---

## Risks

### Potential Problems
1. **State Management**:
   - Edge case: Null references in State
   - Impact: NullReferenceException

2. **Math Errors**:
   - Edge case: Division by zero, negative values
   - Impact: Corrupt game state

### Mitigation
- Null checks on all state operations
- Guard clauses in calculations
- Unit tests (future)

---

## Analysis - What Needs to be Implemented

### 1. IGameEngine.cs
**Location**: `Services/IGameEngine.cs`
- Interface for dependency injection

### 2. GameEngine.cs
**Location**: `Services/GameEngine.cs`
- Initialize() - Load or create new game
- Tick(deltaSeconds) - Update cash
- CanUnlock/CanUpgrade - Affordability checks
- UnlockOrUpgrade - Purchase operation
- CanPrestige/DoPrestige - Prestige system
- CalculateOfflineEarnings - Offline progression

---

## Dependencies Check

**Required Before Start**:
- [x] TASK-002 completed (Models)
- [x] TASK-003 completed (SaveManager)

**Assumptions**:
- SaveManager works correctly
- GameConfig is correctly defined

**Blockers**: TASK-003 must be complete

---

## Implementation Guide

### Step 1: Create IGameEngine.cs

**Path**: `src/MadeMan.IdleEmpire/Services/IGameEngine.cs`

```csharp
using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public interface IGameEngine
{
    GameState State { get; }
    double IncomePerSecond { get; }

    void Initialize();
    void Tick(double deltaSeconds);
    bool CanUnlock(string operationId);
    bool CanUpgrade(string operationId);
    void UnlockOrUpgrade(string operationId);
    bool CanPrestige();
    void DoPrestige();
}
```

### Step 2: Create GameEngine.cs

**Path**: `src/MadeMan.IdleEmpire/Services/GameEngine.cs`

```csharp
using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public class GameEngine : IGameEngine
{
    private readonly SaveManager _saveManager;

    public GameState State { get; private set; } = new();

    public double IncomePerSecond
    {
        get
        {
            double total = 0;
            foreach (var op in GameConfig.Operations)
            {
                var state = GetOperationState(op.Id);
                if (state != null)
                {
                    total += op.GetIncome(state.Level, State.PrestigeBonus);
                }
            }
            return total;
        }
    }

    public GameEngine(SaveManager saveManager)
    {
        _saveManager = saveManager;
    }

    public void Initialize()
    {
        State = _saveManager.Load() ?? CreateNewGame();
        CalculateOfflineEarnings();
    }

    private GameState CreateNewGame()
    {
        var state = new GameState();

        foreach (var op in GameConfig.Operations)
        {
            state.Operations.Add(new OperationState
            {
                Id = op.Id,
                Level = op.UnlockCost == 0 ? 1 : 0
            });
        }

        return state;
    }

    public void Tick(double deltaSeconds)
    {
        double earned = IncomePerSecond * deltaSeconds;
        State.Cash += earned;
        State.TotalEarned += earned;
    }

    public bool CanUnlock(string operationId)
    {
        var op = GetOperation(operationId);
        var state = GetOperationState(operationId);
        if (op == null || state == null) return false;
        return state.Level == 0 && State.Cash >= op.UnlockCost;
    }

    public bool CanUpgrade(string operationId)
    {
        var op = GetOperation(operationId);
        var state = GetOperationState(operationId);
        if (op == null || state == null) return false;
        if (state.Level == 0) return false;
        return State.Cash >= op.GetUpgradeCost(state.Level);
    }

    public void UnlockOrUpgrade(string operationId)
    {
        var op = GetOperation(operationId);
        var state = GetOperationState(operationId);
        if (op == null || state == null) return;

        double cost;
        if (state.Level == 0)
        {
            cost = op.UnlockCost;
        }
        else
        {
            cost = op.GetUpgradeCost(state.Level);
        }

        if (State.Cash >= cost)
        {
            State.Cash -= cost;
            state.Level++;
        }
    }

    public bool CanPrestige()
    {
        return State.TotalEarned >= GameConfig.PrestigeThreshold;
    }

    public void DoPrestige()
    {
        if (!CanPrestige()) return;

        State.PrestigeCount++;
        State.PrestigeBonus += GameConfig.PrestigeBonusPerReset;

        State.Cash = 0;
        State.TotalEarned = 0;

        State.Operations.Clear();
        foreach (var op in GameConfig.Operations)
        {
            State.Operations.Add(new OperationState
            {
                Id = op.Id,
                Level = op.UnlockCost == 0 ? 1 : 0
            });
        }
    }

    private void CalculateOfflineEarnings()
    {
        var offlineTime = DateTime.UtcNow - State.LastPlayedUtc;
        var hours = Math.Min(offlineTime.TotalHours, GameConfig.MaxOfflineHours);

        if (hours > 0.01)
        {
            double earnings = IncomePerSecond * hours * 3600 * GameConfig.OfflineEfficiency;
            State.Cash += earnings;
            State.TotalEarned += earnings;
        }

        State.LastPlayedUtc = DateTime.UtcNow;
    }

    private Operation? GetOperation(string id)
    {
        return GameConfig.Operations.FirstOrDefault(o => o.Id == id);
    }

    private OperationState? GetOperationState(string id)
    {
        return State.Operations.FirstOrDefault(o => o.Id == id);
    }
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Logic Verification (conceptual)
- Initialize with empty save -> Pickpocket level 1, others level 0
- Tick(1.0) -> Cash += IncomePerSecond
- CanUpgrade on locked operation -> false
- DoPrestige -> State reset, PrestigeBonus increased

---

## Acceptance Criteria

- [x] IGameEngine.cs interface created
- [x] GameEngine.cs implemented with all methods
- [x] Initialize handles both load and new game
- [x] Tick calculates income correctly
- [x] Unlock/Upgrade deducts correct cost
- [x] Prestige resets state and increases bonus
- [x] Offline earnings calculated correctly
- [x] Build succeeds with 0 errors

---

## Code Evaluation

### Simplification Check
- **Single responsibility**: GameEngine = game logic only
- **Null safety**: All lookups return nullable
- **No events**: Simple pull-model

### Alternatives Considered

**Alternative: Event-based state changes**
```csharp
public event Action<double> OnCashChanged;
```
**Why rejected**: Over-engineering, ViewModel can poll state

### Known Limitations
- No undo functionality
- No transaction rollback
- Acceptable for MVP

---

## Code Quality Checklist

- [x] **KISS**: Simplest game logic implementation
- [x] **Single Responsibility**: Only game logic
- [x] **Null Safety**: All nullable returns handled
- [x] **Edge Cases**: Level 0 check, empty operations

---

## Design Files Reference

- **Spec Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (lines 243-392)
- **Related Tasks**: TASK-003, TASK-005

---

## Notes

- GameEngine is Singleton (registered in TASK-005)
- SaveManager injected via constructor

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Completed**: 2024-12-25
