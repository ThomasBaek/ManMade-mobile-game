# Task 017: Final Testing

## Metadata
- **Phase**: 5 - Polish
- **Dependencies**: TASK-016
- **Estimated Time**: 1 time
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 1009-1030)
- **Requires Design Input**: NO

---

## Formål

Fuld gennemgang af alle features før release.

**Test Checkliste:**

### Funktionel Test
- [ ] App starter uden crash
- [ ] Cash ticker opdaterer hvert 100ms
- [ ] Pickpocket er auto-unlocked ved start
- [ ] Kan købe upgrades når man har råd
- [ ] Knapper er disabled når man ikke har råd
- [ ] Nye operations unlocker ved korrekt beløb
- [ ] Prestige knap vises ved $10,000 total earned
- [ ] Prestige nulstiller korrekt men beholder bonus
- [ ] Data gemmes ved app close
- [ ] Data loader ved app open
- [ ] Offline earnings beregnes korrekt

### UX Test
- [ ] Første upgrade mulig inden 15 sekunder
- [ ] Første unlock (Car Theft) mulig inden 2 minutter
- [ ] Tal formateres korrekt (K, M, B)
- [ ] Knap farver indikerer affordability tydeligt
- [ ] Ingen UI jank eller stuttering

### Platform Test
- [ ] Kører på Android emulator
- [ ] Kører på fysisk Android device (hvis tilgængelig)

---

## Acceptance Criteria

- [ ] Alle funktionelle tests bestået
- [ ] Alle UX tests bestået
- [ ] Platform test bestået
- [ ] MVP klar til "release"

---

**Task Status**: BLOCKED (venter på TASK-016)
**Last Updated**: 2024-12-25
