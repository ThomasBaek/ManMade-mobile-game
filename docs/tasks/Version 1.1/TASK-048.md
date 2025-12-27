# TASK-048: Update Upgrade Cost Multiplier

## Bundle
**Bundle J: Economy Rebalancing - Core Operations**

## Description
Increase the upgrade cost multiplier to make upgrades more expensive at higher levels.

## Changes Required

**File:** `Models/GameConfig.cs` (or `Operation.cs` if formula is there)

| Setting | OLD Value | NEW Value |
|---------|-----------|-----------|
| Upgrade multiplier | 1.4-1.5 | 1.6 |

**New upgrade formula:**
```csharp
UpgradeCost = BaseUnlockCost * Math.Pow(1.6, currentLevel)
```

## Acceptance Criteria
- [ ] Upgrade cost multiplier updated to 1.6
- [ ] Build succeeds without errors
- [ ] Level 5 upgrades cost significantly more than before

## Dependencies
TASK-047

## Estimate
15 min
