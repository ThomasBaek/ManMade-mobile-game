# TASK-052: Update Operation Skill Effects

## Bundle
**Bundle K: Economy Rebalancing - Skills & Milestones**

## Description
Reduce operation-specific skill effects from 15%/lvl to 12%/lvl.

## Changes Required

**File:** `Models/SkillConfig.cs`

| Skill | OLD Effect | NEW Effect |
|-------|------------|------------|
| Quick Hands (Pickpocket) | +15%/lvl (max 75%) | +12%/lvl (max 60%) |
| Chop Shop (Car Theft) | +15%/lvl (max 75%) | +12%/lvl (max 60%) |
| Inside Man (Burglary) | +15%/lvl (max 75%) | +12%/lvl (max 60%) |
| Happy Hour (Speakeasy) | +15%/lvl (max 75%) | +12%/lvl (max 60%) |

## Acceptance Criteria
- [ ] All 4 operation skills have updated EffectPerLevel values (0.12)
- [ ] Descriptions updated to match new values
- [ ] Build succeeds without errors

## Dependencies
TASK-051

## Estimate
15 min
