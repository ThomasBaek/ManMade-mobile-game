# TASK-046: Update Operation Income Values

## Bundle
**Bundle J: Economy Rebalancing - Core Operations**

## Description
Halve all base income values for operations to slow down early game progression.

## Changes Required

**File:** `Models/GameConfig.cs`

Update operation definitions:

| Operation | OLD Income | NEW Income |
|-----------|------------|------------|
| Pickpocket | $1/s | $0.5/s |
| Car Theft | $4/s | $2/s |
| Burglary | $15/s | $8/s |
| Speakeasy | $50/s | $30/s |
| Casino | $200/s | $100/s |

## Acceptance Criteria
- [ ] All 5 operations have updated BaseIncome values
- [ ] Build succeeds without errors
- [ ] Pickpocket shows $0.5/s in game

## Dependencies
None

## Estimate
15 min
