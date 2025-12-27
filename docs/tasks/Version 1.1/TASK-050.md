# TASK-050: Update Offline Efficiency

## Bundle
**Bundle J: Economy Rebalancing - Core Operations**

## Description
Reduce base offline efficiency to make active play more rewarding.

## Changes Required

**File:** `Services/GameEngine.cs` or `Models/GameConfig.cs`

| Setting | OLD Value | NEW Value |
|---------|-----------|-----------|
| Base offline efficiency | 0.5 (50%) | 0.4 (40%) |

Find the offline earnings calculation and update the base efficiency multiplier.

## Acceptance Criteria
- [ ] Offline efficiency base updated to 40%
- [ ] Build succeeds without errors
- [ ] Welcome back modal shows reduced offline earnings

## Dependencies
TASK-049

## Estimate
15 min

## Bundle J Complete Verification
After this task, verify:
- [ ] Pickpocket = $0.5/s
- [ ] Car Theft unlock = $75
- [ ] Prestige at $25,000
- [ ] First prestige takes longer than before
