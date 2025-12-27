# TASK-054: Update Prestige Skill Effects

## Bundle
**Bundle K: Economy Rebalancing - Skills & Milestones**

## Description
Reduce prestige skill effects to balance with new 20% base prestige bonus.

## Changes Required

**File:** `Models/SkillConfig.cs`

| Skill | OLD Effect | NEW Effect |
|-------|------------|------------|
| Old Connections | +$100/lvl | +$75/lvl (max $375) |
| Reputation | +6%/lvl prestige bonus | +4%/lvl (max +20%) |
| Fast Learner | -4%/lvl upgrade cost | -3%/lvl (max -15%) |
| Early Bird | -5%/lvl unlock cost | -4%/lvl (max -20%) |

## Acceptance Criteria
- [ ] All 4 prestige skills have updated EffectPerLevel values
- [ ] Descriptions updated to match new values
- [ ] Build succeeds without errors

## Dependencies
TASK-053

## Estimate
15 min
