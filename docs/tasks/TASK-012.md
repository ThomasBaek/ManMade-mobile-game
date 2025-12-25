# Task 012: Verify Prestige

## Metadata
- **Phase**: 3 - Gameplay
- **Dependencies**: TASK-011
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Purpose

Verify that the prestige system works correctly.

**Test Scenarios:**

1. **Prestige Panel Visibility**
   - Not visible at start
   - Visible when TotalEarned >= $10,000

2. **Prestige Execution**
   - Cash reset to $0
   - TotalEarned reset to $0
   - PrestigeCount +1
   - PrestigeBonus +0.25

3. **Post-Prestige State**
   - Only Pickpocket unlocked
   - Income = BaseIncome * 1.25
   - Other operations locked again

4. **Multiple Prestiges**
   - 2nd prestige: PrestigeBonus = 1.50
   - Income scales correctly

---

## Quick Test Method

For faster testing, GameConfig.PrestigeThreshold can temporarily be set to 100.

---

## Acceptance Criteria

- [ ] Prestige panel shows at threshold
- [ ] Prestige resets state correctly
- [ ] PrestigeBonus increases
- [ ] Income scales with bonus
- [ ] UI updates after prestige

---

**Task Status**: BLOCKED (waiting for TASK-011)
**Last Updated**: 2024-12-25
