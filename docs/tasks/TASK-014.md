# Task 014: Test Offline Earnings

## Metadata
- **Phase**: 4 - Persistence
- **Dependencies**: TASK-013
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Formål

Verificer at offline earnings beregnes korrekt.

**Test Scenarler:**

1. **Short Offline (1 min)**
   - Noter cash og income
   - Luk app, vent 1 min
   - Genåbn: Cash += Income * 60 * 0.5

2. **Medium Offline (5 min)**
   - Samme beregning, skaleret

3. **Max Offline (4+ timer)**
   - Luk app, ændre device tid +5 timer
   - Genåbn: Max 4 timers earnings

4. **No Earnings**
   - Offline < 36 sek (0.01 timer)
   - Ingen offline bonus

---

## Beregning

```
Offline Earnings = IncomePerSecond * Hours * 3600 * 0.5
Max Hours = 4
```

---

## Acceptance Criteria

- [ ] Offline earnings beregnes ved app start
- [ ] 50% efficiency applied
- [ ] Max 4 timer cap
- [ ] Meget kort offline = ingen bonus

---

**Task Status**: BLOCKED (venter på TASK-013)
**Last Updated**: 2024-12-25
