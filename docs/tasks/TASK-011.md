# Task 011: Verify Game Loop

## Metadata
- **Phase**: 3 - Gameplay
- **Dependencies**: TASK-010
- **Estimated Time**: 1 time
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Formål

Verificer at hele game loop fungerer korrekt på emulator.

**Test Scenarler:**

1. **Ny Spil Start**
   - Cash: $0
   - Pickpocket: Level 1, $1/s
   - Andre: Locked

2. **Cash Generering**
   - Cash stiger med ~$1/s
   - Display opdateres smooth

3. **Første Upgrade**
   - Efter ~14 sek: $14 cash
   - Kan købe Pickpocket Lvl 2 ($14)
   - Income øges til $2/s

4. **Første Unlock**
   - Efter ~50 cash: Kan købe Car Theft
   - Income øges til $6/s

5. **Alle Operations**
   - Verificer alle 5 kan unlocks
   - Verificer upgrade costs stiger

---

## Acceptance Criteria

- [ ] App starter uden crash
- [ ] Cash ticker opdaterer kontinuerligt
- [ ] Kan købe upgrades
- [ ] Kan unlocke nye operations
- [ ] Knap farver ændrer sig baseret på affordability
- [ ] Ingen UI lag eller stuttering

---

**Task Status**: BLOCKED (venter på TASK-010)
**Last Updated**: 2024-12-25
