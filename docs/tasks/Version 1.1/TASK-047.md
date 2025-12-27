# TASK-047: Update Operation Unlock Costs

## Bundle
**Bundle J: Economy Rebalancing - Core Operations**

## Description
Increase unlock costs by 50-100% to make progression more meaningful.

## Changes Required

**File:** `Models/GameConfig.cs`

Update operation definitions:

| Operation | OLD Unlock | NEW Unlock |
|-----------|------------|------------|
| Pickpocket | FREE | FREE |
| Car Theft | $50 | $75 |
| Burglary | $250 | $400 |
| Speakeasy | $1,000 | $2,000 |
| Casino | $5,000 | $10,000 |

## Acceptance Criteria
- [ ] All unlock costs updated
- [ ] Build succeeds without errors
- [ ] Car Theft shows $75 unlock cost in game

## Dependencies
TASK-046

## Estimate
15 min
