# Task 023: Integrate Skills into GameEngine

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-021, TASK-022
- **Estimated Time**: 45 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 5)
- **Requires Design Input**: NO

---

## Formål

Integrer skill effects i GameEngine's core calculations.

**Hvorfor dette er vigtigt:**
- Skills skal påvirke indkomst beregninger
- Milestones skal tjekkes hver tick
- Skill bonuses skal anvendes korrekt

---

## Analyse - Hvad Skal Implementeres

### GameEngine.cs (udvid)
**Ændringer:**

1. Tilføj ISkillService og IMilestoneService dependencies
2. Modificer indkomst beregning til at bruge skill multipliers
3. Kald `CheckForMilestone()` i game loop
4. Håndter offline earnings med skill bonuses

---

## Implementation Guide

### Step 1: Udvid GameEngine constructor

Tilføj skill service dependencies:

```csharp
private readonly ISkillService _skillService;
private readonly IMilestoneService _milestoneService;

public GameEngine(
    Func<GameState> getState,
    ISkillService skillService,
    IMilestoneService milestoneService)
{
    _getState = getState;
    _skillService = skillService;
    _milestoneService = milestoneService;
}
```

### Step 2: Modificer CalculateIncome

Opdater indkomst beregning:

```csharp
public double CalculateOperationIncome(OperationState operation)
{
    var config = GetOperationConfig(operation.Id);
    var baseIncome = config.BaseIncome * operation.Level;

    // Apply skill multipliers
    var skillMultiplier = _skillService.GetTotalIncomeMultiplier();
    var operationMultiplier = _skillService.GetOperationMultiplier(operation.Id);
    var compoundMultiplier = _skillService.GetCompoundInterestMultiplier(State.SessionStartUtc);

    var totalIncome = baseIncome * skillMultiplier * operationMultiplier * compoundMultiplier;

    // Lucky Break check
    if (_skillService.RollLuckyBreak(_random))
    {
        totalIncome *= 2.0;
    }

    return totalIncome;
}
```

### Step 3: Tilføj milestone check i game loop

```csharp
public void Tick(double deltaSeconds)
{
    // Existing income calculation...

    // Add passive income
    var passiveIncome = _skillService.GetPassiveIncomePerSecond() * deltaSeconds;
    State.Cash += passiveIncome;
    State.TotalEarned += passiveIncome;

    // Check for milestone
    _milestoneService.CheckForMilestone();
}
```

### Step 4: Opdater upgrade costs

```csharp
public double GetUpgradeCost(OperationState operation)
{
    var config = GetOperationConfig(operation.Id);
    var baseCost = config.BaseCost * Math.Pow(config.CostMultiplier, operation.Level);

    // Apply skill reduction
    return baseCost * _skillService.GetUpgradeCostMultiplier();
}

public double GetUnlockCost(string operationId)
{
    var config = GetOperationConfig(operationId);

    // Apply skill reduction
    return config.UnlockCost * _skillService.GetUnlockCostMultiplier();
}
```

### Step 5: Opdater offline earnings

```csharp
public double CalculateOfflineEarnings(TimeSpan offlineTime)
{
    // Max hours with skill bonus
    var maxHours = 4.0 + _skillService.GetOfflineMaxHoursBonus();
    var cappedHours = Math.Min(offlineTime.TotalHours, maxHours);

    // Base efficiency + skill bonus
    var efficiency = 0.5 + _skillService.GetOfflineEfficiencyBonus();
    efficiency = Math.Min(efficiency, 1.0); // Cap at 100%

    var baseEarnings = CalculateTotalIncomePerSecond() * cappedHours * 3600 * efficiency;

    // Apply offline multiplier
    return baseEarnings * _skillService.GetOfflineEarningsMultiplier();
}
```

### Step 6: Tilføj cashback ved køb

```csharp
public bool TryUpgrade(OperationState operation)
{
    var cost = GetUpgradeCost(operation);
    if (State.Cash < cost) return false;

    State.Cash -= cost;
    operation.Level++;

    // Cashback
    var cashback = cost * (_skillService.GetCashbackPercent() / 100.0);
    State.Cash += cashback;

    return true;
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Integration Check
- [ ] GameEngine compiles med nye dependencies
- [ ] Skill effects anvendes i income calculation
- [ ] Milestone check kører hver tick
- [ ] Offline earnings bruger skill bonuses

---

## Acceptance Criteria

- [ ] ISkillService og IMilestoneService injected
- [ ] CalculateOperationIncome bruger skill multipliers
- [ ] GetUpgradeCost bruger reduction
- [ ] GetUnlockCost bruger reduction
- [ ] CalculateOfflineEarnings bruger skill bonuses
- [ ] Cashback implementeret ved køb
- [ ] Passive income tilføjet i Tick
- [ ] CheckForMilestone kaldes i loop
- [ ] Lucky Break rolls på hver income
- [ ] Build succeeds med 0 errors

---

## Edge Cases

- [ ] Ingen skills → Alle multipliers = 1.0
- [ ] Compound Interest → Korrekt tid tracking
- [ ] Lucky Break → Random roll hver gang

---

**Task Status**: BLOCKED (venter på TASK-021 og TASK-022)
**Last Updated**: 2024-12-25
