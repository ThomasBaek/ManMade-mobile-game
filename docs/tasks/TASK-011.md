# Task 011: Verify Game Loop

## Metadata
- **Phase**: 3 - Gameplay
- **Dependencies**: TASK-010
- **Estimated Time**: 1 hour
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Purpose

Verify that the entire game loop works correctly on emulator.

**Test Scenarios:**

1. **New Game Start**
   - Cash: $0
   - Pickpocket: Level 1, $1/s
   - Others: Locked

2. **Cash Generation**
   - Cash increases by ~$1/s
   - Display updates smoothly

3. **First Upgrade**
   - After ~14 sec: $14 cash
   - Can buy Pickpocket Lvl 2 ($14)
   - Income increases to $2/s

4. **First Unlock**
   - After ~50 cash: Can buy Car Theft
   - Income increases to $6/s

5. **All Operations**
   - Verify all 5 can unlock
   - Verify upgrade costs increase

---

## Acceptance Criteria

- [ ] App starts without crash
- [ ] Cash ticker updates continuously
- [ ] Can buy upgrades
- [ ] Can unlock new operations
- [ ] Button colors change based on affordability
- [ ] No UI lag or stuttering

---

**Task Status**: BLOCKED (waiting for TASK-010)
**Last Updated**: 2024-12-25
