# TASK-049: Update Prestige Threshold and Bonus

## Bundle
**Bundle J: Economy Rebalancing - Core Operations**

## Description
Increase prestige threshold and slightly reduce bonus to extend gameplay per prestige.

## Changes Required

**File:** `Models/GameConfig.cs`

| Setting | OLD Value | NEW Value |
|---------|-----------|-----------|
| PrestigeThreshold | $10,000 | $25,000 |
| PrestigeBonusPerReset | 0.25 (25%) | 0.20 (20%) |

## Acceptance Criteria
- [ ] Prestige threshold updated to $25,000
- [ ] Prestige bonus updated to 20%
- [ ] Build succeeds without errors
- [ ] Prestige panel shows correct threshold
- [ ] Prestige grants +20% bonus (1.20x multiplier)

## Dependencies
TASK-048

## Estimate
15 min
