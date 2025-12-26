# TASK-043: Interval-Based Operation Yields - Model & Engine

## Metadata
- **Phase**: 2 - Core Features
- **Dependencies**: Bundle C completed
- **Estimated Time**: 2-3 hours
- **Status**: Pending
- **Design Reference**: CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Frequency Impact**: NO

---

## Formål

Ændre operations fra kontinuerlig income ($/sekund) til **interval-baseret yields med progress tracking**.

**Hvorfor dette er vigtigt:**
- **Visuel feedback**: Spilleren ser progress bars fylde op og får tilfredsstillelse når de "popper"
- **Tier-differentiering**: Højere tier = længere venten, men STOR payout (dopamin-hit)
- **Engagement**: Giver spilleren noget at kigge på - "når den her bar fylder op får jeg $300!"

---

## Risici

### Potentielle Problemer
1. **Progress tab ved tab-switch**:
   - Edge case: Spiller skifter tab, progress resettes
   - Impact: Frustration, tabt indtjening

2. **Offline earnings kompleksitet**:
   - Edge case: Beregning af yields over timer er anderledes end tick-baseret
   - Impact: Forkert offline earnings

3. **Skill integration**:
   - Edge case: Skills der ændrer income skal nu ændre yield
   - Impact: Broken skill effects

### Mitigering
- Persistér `AccumulatedTime` i `OperationState` (gemmes med game state)
- Offline earnings: Beregn antal komplette cycles + partial progress
- Review alle skill effects og opdater til yield-baseret

---

## Analyse - Hvad Skal Implementeres

### 1. Operation Model Udvidelse
**Description**: Tilføj interval og yield properties til Operation config
**Location**: `Models/Operation.cs`
**Key Requirements**:
- Tilføj `Interval` property (sekunder mellem yields)
- Tilføj `BaseYield` property (payout per cycle ved level 1)
- Opdater `GetIncome()` til `GetYield()` semantik
- Tilføj `GetIncomePerSecond()` for display beregning

### 2. OperationState Udvidelse
**Description**: Track progress mod næste yield
**Location**: `Models/GameState.cs`
**Key Requirements**:
- Tilføj `AccumulatedTime` property til OperationState
- Default til 0, persisteres med save

### 3. GameConfig Opdatering
**Description**: Definer interval/yield for hver operation
**Location**: `Models/GameConfig.cs`
**Key Requirements**:
- Pickpocket: 1s interval, $1 yield
- Car Theft: 5s interval, $20 yield
- Burglary: 20s interval, $300 yield
- Speakeasy: 60s interval, $3000 yield
- Casino: 120s interval, $24000 yield

### 4. GameEngine Tick Logic
**Description**: Ny tick-logic der akkumulerer tid og yielder ved interval completion
**Location**: `Services/GameEngine.cs`
**Key Requirements**:
- Tick opdaterer `AccumulatedTime` for hver aktiv operation
- Når `AccumulatedTime >= Interval`: yield payout, reset progress
- Lucky Break applies på yield, ikke per-tick
- Invalider income cache ved yield

### 5. Pickpocket Starter Fix
**Description**: Nye spil starter med Pickpocket på level 1
**Location**: `Services/GameEngine.cs` - `CreateNewGame()`
**Key Requirements**:
- Pickpocket får `UnlockCost > 0` (kan ikke spam-købes)
- `CreateNewGame()` sætter Pickpocket til level 1 eksplicit

**Business Rules**:
```csharp
// Yield beregning med skills og prestige
var baseYield = operation.BaseYield * level * prestigeBonus;
var skillMultiplier = _skillService.GetTotalIncomeMultiplier();
var operationMultiplier = _skillService.GetOperationMultiplier(opId);
var finalYield = baseYield * skillMultiplier * operationMultiplier;

// Lucky Break på yield (ikke per-tick)
if (_skillService.RollLuckyBreak(Random.Shared))
{
    finalYield *= 2.0;
}
```

---

## Dependencies Check

✅ **Required Before Starting**:
- [x] Bundle C completed (Welcome Back modal)
- [x] GameEngine fungerer korrekt
- [x] Skill system integreret

⚠️ **Assumptions**:
- Offline earnings håndteres i separat task (TASK-045)
- UI ændringer i separat task (TASK-044)

❌ **Blockers**: None

---

## Implementation Guide

### Step 1: Udvid Operation Model
Path: `Models/Operation.cs`

```csharp
namespace MadeMan.IdleEmpire.Models;

public class Operation
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Economics - Interval Based
    public double Interval { get; set; } = 1.0;   // Seconds between yields
    public double BaseYield { get; set; }          // Payout per cycle at level 1
    public double UnlockCost { get; set; }
    public double BaseUpgradeCost { get; set; }
    public double UpgradeMultiplier { get; set; } = 1.5;

    // Calculate yield for given level
    public double GetYield(int level, double prestigeBonus)
    {
        if (level <= 0) return 0;
        return BaseYield * level * prestigeBonus;
    }

    // Calculate effective income/second for display
    public double GetIncomePerSecond(int level, double prestigeBonus)
    {
        if (level <= 0) return 0;
        return GetYield(level, prestigeBonus) / Interval;
    }

    // Upgrade cost calculation (unchanged)
    public double GetUpgradeCost(int currentLevel)
    {
        if (currentLevel <= 0) return UnlockCost;
        var baseCost = BaseUpgradeCost > 0 ? BaseUpgradeCost : UnlockCost;
        return baseCost * Math.Pow(UpgradeMultiplier, currentLevel);
    }
}
```

### Step 2: Udvid OperationState
Path: `Models/GameState.cs`

```csharp
public class OperationState
{
    public string Id { get; set; } = string.Empty;
    public int Level { get; set; } = 0; // 0 = locked
    public double AccumulatedTime { get; set; } = 0; // Progress toward next yield
}
```

### Step 3: Opdater GameConfig
Path: `Models/GameConfig.cs`

```csharp
public static readonly Operation[] Operations = new[]
{
    new Operation
    {
        Id = "pickpocket",
        Name = "Pickpocketing",
        Icon = "icon_pickpocket.png",
        Description = "Lift wallets from unsuspecting tourists",
        Interval = 1.0,      // 1 second
        BaseYield = 1.0,     // $1 per cycle = $1/s effective
        UnlockCost = 5,      // Costs $5 to unlock (but starts unlocked)
        BaseUpgradeCost = 5,
        UpgradeMultiplier = 1.4
    },
    new Operation
    {
        Id = "cartheft",
        Name = "Car Theft",
        Icon = "icon_car.png",
        Description = "Steal cars, sell the parts",
        Interval = 5.0,      // 5 seconds
        BaseYield = 20.0,    // $20 per cycle = $4/s effective
        UnlockCost = 50,
        UpgradeMultiplier = 1.5
    },
    new Operation
    {
        Id = "burglary",
        Name = "Burglary",
        Icon = "icon_burglary.png",
        Description = "Break into the homes of the wealthy",
        Interval = 20.0,     // 20 seconds
        BaseYield = 300.0,   // $300 per cycle = $15/s effective
        UnlockCost = 250,
        UpgradeMultiplier = 1.6
    },
    new Operation
    {
        Id = "speakeasy",
        Name = "Speakeasy",
        Icon = "icon_speakeasy.png",
        Description = "Your first illegal watering hole",
        Interval = 60.0,     // 60 seconds
        BaseYield = 3000.0,  // $3000 per cycle = $50/s effective
        UnlockCost = 1000,
        UpgradeMultiplier = 1.8
    },
    new Operation
    {
        Id = "casino",
        Name = "Underground Casino",
        Icon = "icon_casino.png",
        Description = "The house always wins",
        Interval = 120.0,    // 120 seconds
        BaseYield = 24000.0, // $24000 per cycle = $200/s effective
        UnlockCost = 5000,
        UpgradeMultiplier = 2.0
    }
};
```

### Step 4: Opdater GameEngine Tick
Path: `Services/GameEngine.cs`

```csharp
public void Tick(double deltaSeconds)
{
    foreach (var op in GameConfig.Operations)
    {
        var opState = GetOperationState(op.Id);
        if (opState == null || opState.Level <= 0) continue;

        // Accumulate time
        opState.AccumulatedTime += deltaSeconds;

        // Check for yield(s) - handle multiple yields if delta is large
        while (opState.AccumulatedTime >= op.Interval)
        {
            opState.AccumulatedTime -= op.Interval;

            // Calculate and apply yield
            var yield = CalculateOperationYield(opState, op);
            State.Cash += yield;
            State.TotalEarned += yield;
        }
    }

    // Check for milestone
    _milestoneService.CheckForMilestone();
}

private double CalculateOperationYield(OperationState opState, Operation config)
{
    var baseYield = config.GetYield(opState.Level, State.PrestigeBonus);

    // Apply skill multipliers
    var skillMultiplier = _skillService.GetTotalIncomeMultiplier();
    var operationMultiplier = _skillService.GetOperationMultiplier(opState.Id);
    var compoundMultiplier = _skillService.GetCompoundInterestMultiplier(State.SessionStartUtc);

    var finalYield = baseYield * skillMultiplier * operationMultiplier * compoundMultiplier;

    // Lucky Break check (2x yield chance) - per yield, not per tick
    if (_skillService.RollLuckyBreak(Random.Shared))
    {
        finalYield *= 2.0;
    }

    return finalYield;
}
```

### Step 5: Opdater IncomePerSecond Property
Path: `Services/GameEngine.cs`

```csharp
public double IncomePerSecond
{
    get
    {
        if (!_incomeCacheDirty)
        {
            return _cachedBaseIncome;
        }

        double total = 0;
        foreach (var op in GameConfig.Operations)
        {
            var opState = GetOperationState(op.Id);
            if (opState != null && opState.Level > 0)
            {
                total += CalculateEffectiveIncomePerSecond(opState, op);
            }
        }
        total += _skillService.GetPassiveIncomePerSecond();

        _cachedBaseIncome = total;
        _incomeCacheDirty = false;
        return total;
    }
}

private double CalculateEffectiveIncomePerSecond(OperationState opState, Operation config)
{
    var yieldPerCycle = config.GetYield(opState.Level, State.PrestigeBonus);
    var skillMultiplier = _skillService.GetTotalIncomeMultiplier();
    var operationMultiplier = _skillService.GetOperationMultiplier(opState.Id);

    return (yieldPerCycle * skillMultiplier * operationMultiplier) / config.Interval;
}
```

### Step 6: Fix Pickpocket Starter
Path: `Services/GameEngine.cs` - `CreateNewGame()`

```csharp
private GameState CreateNewGame()
{
    var state = new GameState();

    foreach (var op in GameConfig.Operations)
    {
        var opState = new OperationState
        {
            Id = op.Id,
            Level = 0,
            AccumulatedTime = 0
        };

        // Pickpocket starts at level 1 (pre-unlocked)
        if (op.Id == "pickpocket")
        {
            opState.Level = 1;
        }

        state.Operations.Add(opState);
    }

    return state;
}
```

### Step 7: Reset Progress on Prestige
Path: `Services/GameEngine.cs` - `DoPrestige()`

```csharp
public void DoPrestige()
{
    if (!CanPrestige()) return;

    State.PrestigeCount++;
    // ... existing prestige logic ...

    State.Operations.Clear();
    foreach (var op in GameConfig.Operations)
    {
        var opState = new OperationState
        {
            Id = op.Id,
            Level = op.Id == "pickpocket" ? 1 : 0, // Pickpocket starts unlocked
            AccumulatedTime = 0 // Reset progress
        };
        State.Operations.Add(opState);
    }

    InvalidateIncomeCache();
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Manual Test in Emulator
- [ ] Pickpocket starter på level 1 ved nyt spil
- [ ] Progress akkumulerer over tid
- [ ] Yield pops når interval nås
- [ ] Cash stiger korrekt ved hver yield
- [ ] Tab switch bevarer progress
- [ ] Prestige resetter progress til 0
- [ ] IncomePerSecond viser korrekt beregnet værdi

---

## Acceptance Criteria

- [ ] Operation model har `Interval` og `BaseYield` properties
- [ ] OperationState har `AccumulatedTime` property
- [ ] GameConfig definerer interval/yield for alle 5 operations
- [ ] Tick logic akkumulerer tid og yielder ved interval completion
- [ ] Pickpocket starter på level 1 i nye spil
- [ ] Prestige resetter AccumulatedTime
- [ ] IncomePerSecond beregnes korrekt (yield/interval sum)
- [ ] Build succeeds with 0 errors
- [ ] Skills (income multipliers) påvirker yield korrekt

---

## Kode Evaluering

### Simplifikations-tjek
Denne implementation følger KISS princippet ved at:
- **Én property for progress**: `AccumulatedTime` i sekunder (simpelt at forstå)
- **While-loop for multiple yields**: Håndterer store delta-tider elegant
- **Bevar eksisterende skill integration**: Samme multiplier-struktur, bare anvendt på yield

### Alternativer overvejet

**Alternative 1: Separat timer per operation**
```csharp
private Dictionary<string, IDispatcherTimer> _operationTimers;
```
**Hvorfor fravalgt**: Over-engineering, kompleks lifecycle management, unødvendig

**Alternative 2: Progress som 0-1 float i state**
```csharp
public double Progress { get; set; } // 0.0 - 1.0
```
**Hvorfor fravalgt**: Kræver ekstra beregning ved load, AccumulatedTime er mere præcis

### Potentielle forbedringer (v2)
- Animation/feedback ved yield pop (TASK-044 UI)
- Lyd effekt ved yield
- "Combo" system for hurtige yields

### Kendte begrænsninger
- **Offline earnings**: Håndteres separat - denne task kun online tick
- **UI feedback**: Ingen visuel progress bar endnu (TASK-044)

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Er dette den simpleste løsning?
- [ ] **Læsbarhed**: Kan en anden udvikler forstå koden uden forklaring?
- [ ] **Navngivning**: Er alle navne beskrivende og konsistente?
- [ ] **Funktioner**: Er alle funktioner korte og fokuserede?
- [ ] **DRY**: Er der nogen duplikeret kode?
- [ ] **Error handling**: Er fejl håndteret korrekt?
- [ ] **Edge cases**: Er edge cases identificeret og håndteret?
- [ ] **Performance**: Er der åbenlyse performance problemer?
- [ ] **Testbarhed**: Kan koden nemt testes?

---

## Design Files Reference

- **Game Design**: PLAN.md
- **Implementation Guide**: CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Related**: TASK-044 (UI), TASK-045 (Offline Earnings)

---

## Notes

- Yield værdier balanceret til at matche eksisterende effective income/s
- AccumulatedTime persisteres automatisk via SaveManager (del af GameState)
- Lucky Break nu per-yield i stedet for per-tick (mere meningsfuld)

---

**Task Status**: ⏸️ Ready to implement
**Last Updated**: 2024-12-26
**Implemented By**: Pending
