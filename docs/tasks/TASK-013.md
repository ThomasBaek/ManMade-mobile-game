# Task 013: Test Save/Load

## Metadata
- **Phase**: 4 - Persistence
- **Dependencies**: TASK-012
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: N/A
- **Requires Design Input**: NO

---

## Purpose

Verify that game state saves and loads correctly.

**Test Scenarios:**

1. **Auto-Save**
   - Play for 30+ sec
   - Close app (not kill)
   - Reopen: State preserved

2. **Manual Save (OnSleep)**
   - Play and achieve progress
   - Press Home button
   - Kill app from task manager
   - Reopen: State preserved

3. **Data Integrity**
   - Cash values correct
   - Operation levels correct
   - PrestigeCount correct

4. **Fresh Install**
   - Uninstall app
   - Reinstall
   - Start with clean state

---

## Acceptance Criteria

- [ ] Auto-save every 30 sec
- [ ] Save on app sleep
- [ ] Load on app start
- [ ] All values preserved
- [ ] New installation starts fresh

---

**Task Status**: BLOCKED (waiting for TASK-012)
**Last Updated**: 2024-12-25
