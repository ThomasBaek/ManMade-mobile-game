# Task 013: Test Save/Load

## Metadata
- **Phase**: 4 - Persistence
- **Dependencies**: TASK-012
- **Estimated Time**: 30 min
- **Status**: COMPLETED
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

- [x] Auto-save every 30 sec
- [x] Save on app sleep
- [x] Load on app start
- [x] All values preserved
- [x] New installation starts fresh

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Commit**: a6782d5
