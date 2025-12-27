# TASK-055: Update Milestone Thresholds

## Bundle
**Bundle K: Economy Rebalancing - Skills & Milestones**

## Description
Spread milestone thresholds further apart to slow skill acquisition.

## Changes Required

**File:** `Services/MilestoneService.cs` or `Models/SkillConfig.cs`

| # | OLD Threshold | NEW Threshold |
|---|---------------|---------------|
| 1 | $1,000 | $2,500 |
| 2 | $5,000 | $10,000 |
| 3 | $15,000 | $30,000 |
| 4 | $50,000 | $75,000 |
| 5 | $150,000 | $200,000 |
| 6 | $500,000 | $600,000 |
| 7 | $1,500,000 | $2,000,000 |
| 8 | $5,000,000 | $7,500,000 |
| 9 | $15,000,000 | $25,000,000 |
| 10 | $50,000,000 | $100,000,000 |

## Acceptance Criteria
- [ ] All 10 milestone thresholds updated
- [ ] Build succeeds without errors
- [ ] First milestone triggers at $2,500 total earned
- [ ] TopBar shows correct "Next: $2,500" text

## Dependencies
TASK-054

## Estimate
15 min

## Bundle K Complete Verification
After this task, verify:
- [ ] All 18 skills have reduced effect values
- [ ] Milestones are spread further apart
- [ ] First skill choice comes later in gameplay
