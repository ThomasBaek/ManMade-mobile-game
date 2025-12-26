# Task 028: Verify Skill Effects

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-026, TASK-027
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md
- **Requires Design Input**: NO

---

## Purpose

Verify that all skill effects work correctly in gameplay.

**Why this is important:**
- Ensures skills actually affect gameplay
- Catches any calculation errors
- Validates user experience

---

## Test Cases

### 1. Income Skills

| Skill | Test | Expected Result |
|-------|------|-----------------|
| Cash Flow (4%) | Add skill, check income | +4% per level |
| Street Smarts (10%) | Upgrade Tier 1 op | +10% on Pickpocket/CarTheft/Burglary |
| Business Acumen (12%) | Upgrade Tier 2 op | +12% on Speakeasy/Casino |
| Lucky Break (4%) | Check for 2x procs | ~4% chance per level |
| The Skim (5%) | Buy upgrade, check cashback | 5% of cost returned |
| Compound Interest (3%) | Wait 30 min, check | +3% per 5 min per level |

### 2. Operations Skills

| Skill | Test | Expected Result |
|-------|------|-----------------|
| Quick Hands | Pickpocket income | +25% per level |
| Chop Shop | Car Theft income | +20% per level |
| Inside Man | Burglary income | +18% per level |
| Happy Hour | Speakeasy income | +15% per level |

### 3. Offline Skills

| Skill | Test | Expected Result |
|-------|------|-----------------|
| Night Owl | Offline earnings | +10% efficiency per level |
| Extended Shift | Max offline time | +1 hour per level |
| Passive Income | Baseline income | +$1/s per level |
| Godfather's Cut | Offline multiplier | +8% per level |

### 4. Prestige Skills

| Skill | Test | Expected Result |
|-------|------|-----------------|
| Old Connections | Start cash after prestige | +$100 per level |
| Reputation | Prestige bonus | +6% per level |
| Fast Learner | Upgrade costs | -4% per level |
| Early Bird | Unlock costs | -5% per level |

---

## Verification Steps

### Step 1: Skill Selection

1. Earn $1,000 total → Milestone 1 trigger
2. Verify modal appears
3. Select a skill
4. Verify skill added to ActiveSkills
5. Verify modal closes

### Step 2: Skill Effects

1. Note current income
2. Add Cash Flow skill
3. Verify income increased by 4%
4. Upgrade Cash Flow to level 2
5. Verify income increased by 8% total

### Step 3: Max Skills

1. Earn enough for 5 milestones
2. Select 5 different skills
3. On 6th milestone, verify only upgrades offered
4. Verify max 5 skills enforced

### Step 4: Skill Max Level

1. Upgrade one skill to level 5
2. On next milestone, verify that skill not in pool
3. Verify "Lv 5/5" display

### Step 5: Prestige Reset

1. Perform prestige
2. Verify skills cleared
3. Verify MilestoneCount = 0
4. Verify Old Connections bonus applied (if had skill)

---

## Debugging Checklist

If something doesn't work:

- [ ] Skill registered in SkillConfig?
- [ ] SkillService correctly calculates effect?
- [ ] GameEngine calls skill service methods?
- [ ] Bindings correct in XAML?
- [ ] Events properly subscribed?

---

## Acceptance Criteria

- [ ] Milestone modal triggers at correct thresholds
- [ ] Skill selection adds skill to state
- [ ] All 18 skills have working effects
- [ ] Max 5 skills enforced
- [ ] Max level 5 enforced
- [ ] Prestige resets skills
- [ ] UI updates correctly
- [ ] No crashes or exceptions

---

## Known Edge Cases

- [ ] Rapid tapping on skills
- [ ] App backgrounded during selection
- [ ] Zero income edge case
- [ ] All skills maxed scenario

---

## Manual Test Procedure

1. **Fresh Start**
   - Launch app
   - Verify no skills
   - Verify milestone progress at 0

2. **First Milestone**
   - Play until $1,000 total
   - Modal appears
   - Select skill
   - Verify effect

3. **Skill Stacking**
   - Earn more milestones
   - Mix new and upgrades
   - Verify cumulative effects

4. **Prestige Flow**
   - Prestige with skills
   - Verify clean reset
   - Start earning again

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-26
**Commit**: 7927ff0
