# Task 012: Verify Prestige

## Metadata
- **Phase**: 3 - Gameplay
- **Dependencies**: TASK-011
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Formål

Verificer at prestige system fungerer korrekt.

**Test Scenarler:**

1. **Prestige Panel Visibility**
   - Ikke synlig ved start
   - Synlig når TotalEarned >= $10,000

2. **Prestige Execution**
   - Cash reset til $0
   - TotalEarned reset til $0
   - PrestigeCount +1
   - PrestigeBonus +0.25

3. **Post-Prestige State**
   - Kun Pickpocket unlocked
   - Income = BaseIncome * 1.25
   - Andre operations locked igen

4. **Multiple Prestiges**
   - 2nd prestige: PrestigeBonus = 1.50
   - Income scales korrekt

---

## Quick Test Method

For hurtigere test, kan GameConfig.PrestigeThreshold midlertidigt sættes til 100.

---

## Acceptance Criteria

- [ ] Prestige panel vises ved threshold
- [ ] Prestige resetter state korrekt
- [ ] PrestigeBonus øges
- [ ] Income scales med bonus
- [ ] UI opdateres efter prestige

---

**Task Status**: BLOCKED (venter på TASK-011)
**Last Updated**: 2024-12-25
