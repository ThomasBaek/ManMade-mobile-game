# Task 023: Integrate Skills into GameEngine

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-021, TASK-022
- **Estimated Time**: 45 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 5)
- **Requires Design Input**: NO

---

## Purpose

Integrate skill effects into GameEngine's core calculations.

**Why this is important:**
- Skills must affect income calculations
- Milestones must be checked each tick
- Skill bonuses must be applied correctly

---

## Analysis - What to Implement

### GameEngine.cs (extend)
**Changes:**

1. Add ISkillService and IMilestoneService dependencies
2. Modify income calculation to use skill multipliers
3. Call `CheckForMilestone()` in game loop
4. Handle offline earnings with skill bonuses

---

## Implementation Guide

### Step 1: Extend GameEngine constructor

Add skill service dependencies:

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

### Step 2: Modify CalculateIncome

Update income calculation:

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

### Step 3: Add milestone check in game loop

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

### Step 4: Update upgrade costs

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

### Step 5: Update offline earnings

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

### Step 6: Add cashback on purchase

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
- [ ] GameEngine compiles with new dependencies
- [ ] Skill effects applied in income calculation
- [ ] Milestone check runs every tick
- [ ] Offline earnings uses skill bonuses

---

## Acceptance Criteria

- [ ] ISkillService and IMilestoneService injected
- [ ] CalculateOperationIncome uses skill multipliers
- [ ] GetUpgradeCost uses reduction
- [ ] GetUnlockCost uses reduction
- [ ] CalculateOfflineEarnings uses skill bonuses
- [ ] Cashback implemented on purchase
- [ ] Passive income added in Tick
- [ ] CheckForMilestone called in loop
- [ ] Lucky Break rolls on each income
- [ ] Build succeeds with 0 errors

---

## Edge Cases

- [ ] No skills → All multipliers = 1.0
- [ ] Compound Interest → Correct time tracking
- [ ] Lucky Break → Random roll each time

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-26
**Commit**: b4a026c
