# Task 014: Test Offline Earnings

## Metadata
- **Phase**: 4 - Persistence
- **Dependencies**: TASK-013
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Purpose

Verify that offline earnings calculate correctly.

**Test Scenarios:**

1. **Short Offline (1 min)**
   - Note cash and income
   - Close app, wait 1 min
   - Reopen: Cash += Income * 60 * 0.5

2. **Medium Offline (5 min)**
   - Same calculation, scaled

3. **Max Offline (4+ hours)**
   - Close app, change device time +5 hours
   - Reopen: Max 4 hours earnings

4. **No Earnings**
   - Offline < 36 sec (0.01 hours)
   - No offline bonus

---

## Calculation

```
Offline Earnings = IncomePerSecond * Hours * 3600 * 0.5
Max Hours = 4
```

---

## Acceptance Criteria

- [ ] Offline earnings calculated on app start
- [ ] 50% efficiency applied
- [ ] Max 4 hour cap
- [ ] Very short offline = no bonus

---

**Task Status**: BLOCKED (waiting for TASK-013)
**Last Updated**: 2024-12-25
