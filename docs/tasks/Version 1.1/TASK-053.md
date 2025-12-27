# TASK-053: Update Offline Skill Effects

## Bundle
**Bundle K: Economy Rebalancing - Skills & Milestones**

## Description
Reduce offline skill effects to balance with new 40% base efficiency.

## Changes Required

**File:** `Models/SkillConfig.cs`

| Skill | OLD Effect | NEW Effect |
|-------|------------|------------|
| Night Owl | +8%/lvl efficiency | +6%/lvl (max 30%) |
| Extended Shift | +1hr/lvl (max 5hr) | +0.5hr/lvl (max 2.5hr extra) |
| Passive Income | +$1/s/lvl | +$0.5/s/lvl (max $2.5/s) |
| Godfather's Cut | +8%/lvl (max 40%) | +6%/lvl (max 30%) |

## Acceptance Criteria
- [ ] All 4 offline skills have updated EffectPerLevel values
- [ ] Descriptions updated to match new values
- [ ] Build succeeds without errors

## Dependencies
TASK-052

## Estimate
15 min
