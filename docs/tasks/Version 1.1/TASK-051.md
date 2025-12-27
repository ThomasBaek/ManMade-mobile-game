# TASK-051: Update Income Skill Effects

## Bundle
**Bundle K: Economy Rebalancing - Skills & Milestones**

## Description
Reduce Income skill effects by ~25% to make skills less overpowered.

## Changes Required

**File:** `Models/SkillConfig.cs`

| Skill | OLD Effect | NEW Effect |
|-------|------------|------------|
| Cash Flow | +4%/lvl (max 20%) | +3%/lvl (max 15%) |
| Street Smarts | +10%/lvl (max 50%) | +8%/lvl (max 40%) |
| Business Acumen | +12%/lvl (max 60%) | +8%/lvl (max 40%) |
| Lucky Break | +4% chance/lvl | +3% chance/lvl (max 15%) |
| The Skim | +5%/lvl (max 25%) | +4%/lvl (max 20%) |
| Compound Interest | +3%/lvl (max 15%) | +2%/lvl (max 10%) |

## Acceptance Criteria
- [ ] All 6 income skills have updated EffectPerLevel values
- [ ] Descriptions updated to match new values
- [ ] Build succeeds without errors

## Dependencies
Bundle J complete

## Estimate
20 min
