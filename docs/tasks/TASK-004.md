# Task 004: Implement GameEngine

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: TASK-003
- **Estimated Time**: 1 time
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 243-392)
- **Requires Design Input**: NO

---

## Formål

Implementer GameEngine service - hjertet af spillet der håndterer al game logic.

**Hvorfor dette er vigtigt:**
- Core gameplay loop lever her
- Al income beregning sker her
- Upgrade/unlock/prestige logik

---

## Risici

### Potentielle Problemer
1. **State Management**:
   - Edge case: Null references i State
   - Impact: NullReferenceException

2. **Math Errors**:
   - Edge case: Division by zero, negative values
   - Impact: Korrupt game state

### Mitigering
- Null checks på alle state operationer
- Guard clauses i beregninger
- Unit tests (fremtidig)

---

## Analyse - Hvad Skal Implementeres

### 1. IGameEngine.cs
**Placering**: `Services/IGameEngine.cs`
- Interface for dependency injection

### 2. GameEngine.cs
**Placering**: `Services/GameEngine.cs`
- Initialize() - Load eller opret nyt spil
- Tick(deltaSeconds) - Opdater cash
- CanUnlock/CanUpgrade - Affordability checks
- UnlockOrUpgrade - Køb operation
- CanPrestige/DoPrestige - Prestige system
- CalculateOfflineEarnings - Offline progression

---

## Dependencies Check

**Krævet Før Start**:
- [ ] TASK-002 completed (Models)
- [ ] TASK-003 completed (SaveManager)

**Antagelser**:
- SaveManager virker korrekt
- GameConfig er korrekt defineret

**Blockers**: TASK-003 skal være færdig

---

## Implementation Guide

### Step 1: Opret IGameEngine.cs

**Sti**: `src/MadeMan.IdleEmpire/Services/IGameEngine.cs`

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

### Step 2: Opret GameEngine.cs

**Sti**: `src/MadeMan.IdleEmpire/Services/GameEngine.cs`

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
Forventet: 0 errors

### 2. Logic Verification (conceptual)
- Initialize med tom save -> Pickpocket level 1, andre level 0
- Tick(1.0) -> Cash += IncomePerSecond
- CanUpgrade på locked operation -> false
- DoPrestige -> State reset, PrestigeBonus øget

---

## Acceptance Criteria

- [ ] IGameEngine.cs interface oprettet
- [ ] GameEngine.cs implementeret med alle metoder
- [ ] Initialize håndterer både load og new game
- [ ] Tick beregner income korrekt
- [ ] Unlock/Upgrade trækker korrekt cost
- [ ] Prestige resetter state og øger bonus
- [ ] Offline earnings beregnes korrekt
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Single responsibility**: GameEngine = game logic only
- **Null safety**: Alle lookups returnerer nullable
- **Ingen events**: Simpel pull-model

### Alternativer overvejet

**Alternativ: Event-based state changes**
```csharp
public event Action<double> OnCashChanged;
```
**Hvorfor fravalgt**: Over-engineering, ViewModel kan polle state

### Kendte begrænsninger
- Ingen undo funktionalitet
- Ingen transaction rollback
- Acceptabelt for MVP

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Simpleste game logic implementation
- [ ] **Single Responsibility**: Kun game logic
- [ ] **Null Safety**: Alle nullable returns håndteret
- [ ] **Edge Cases**: Level 0 check, empty operations

---

## Design Files Reference

- **Spec Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 243-392)
- **Related Tasks**: TASK-003, TASK-005

---

## Notes

- GameEngine er Singleton (registreres i TASK-005)
- SaveManager injected via constructor

---

**Task Status**: BLOCKED (venter på TASK-003)
**Last Updated**: 2024-12-25
