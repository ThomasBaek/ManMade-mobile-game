# Task 013: Test Save/Load

## Metadata
- **Phase**: 4 - Persistence
- **Dependencies**: TASK-012
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: N/A
- **Requires Design Input**: NO

---

## Formål

Verificer at game state gemmes og hentes korrekt.

**Test Scenarler:**

1. **Auto-Save**
   - Spil i 30+ sek
   - Luk app (ikke kill)
   - Genåbn: State bevaret

2. **Manual Save (OnSleep)**
   - Spil og opnå progress
   - Tryk Home knap
   - Kill app fra task manager
   - Genåbn: State bevaret

3. **Data Integrity**
   - Cash værdier korrekte
   - Operation levels korrekte
   - PrestigeCount korrekt

4. **Fresh Install**
   - Uninstall app
   - Reinstall
   - Start med clean state

---

## Acceptance Criteria

- [ ] Auto-save hvert 30 sek
- [ ] Save ved app sleep
- [ ] Load ved app start
- [ ] Alle værdier bevaret
- [ ] Ny installation starter fresh

---

**Task Status**: BLOCKED (venter på TASK-012)
**Last Updated**: 2024-12-25
