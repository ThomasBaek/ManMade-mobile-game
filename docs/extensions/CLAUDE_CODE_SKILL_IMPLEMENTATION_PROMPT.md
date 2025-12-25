# 🤖 CLAUDE CODE IMPLEMENTATION PROMPT
## Skill System for Made Man: Idle Empire

---

# OPGAVE

Implementer et **Skill System** til Made Man: Idle Empire spillet som beskrevet i `SKILL_SYSTEM_SPECIFICATION.md`.

---

# FASE 1: ANALYSE

Før du implementerer noget, skal du:

1. **Læs og forstå skill specifikationen**
   - Læs `SKILL_SYSTEM_SPECIFICATION.md` grundigt
   - Forstå de 18 skills og deres effects
   - Forstå milestone systemet
   - Forstå begrænsningen: max 5 skills, max level 5

2. **Analyser det eksisterende projekt**
   - Undersøg den nuværende mappestruktur
   - Forstå hvordan GameState er struktureret
   - Forstå hvordan GameEngine beregner income
   - Forstå hvordan UI er opbygget (XAML + ViewModels)
   - Identificer hvor skill calculations skal integreres

3. **Identificer integration points**
   - Hvor skal skill effects påvirke income beregning?
   - Hvor skal milestone checks ske?
   - Hvordan skal UI integreres med eksisterende layout?
   - Hvordan påvirker skills prestige systemet?

---

# FASE 2: PLANLÆGNING

Lav en **task liste** med følgende format:

```
## TASK [nummer]: [Titel]
**Beskrivelse:** Hvad skal laves
**Dependencies:** Hvilke tasks skal være færdige først (eller "Ingen")
**Filer:** Hvilke filer oprettes/ændres
**Acceptkriterier:** Hvordan ved vi det virker
```

Tasks skal som minimum dække disse områder:

| Område | Beskrivelse |
|--------|-------------|
| **Data Layer** | Models for skills, udvidelse af GameState |
| **Service Layer** | Skill service, integration med GameEngine |
| **Calculations** | Alle skill effects integreret i beregninger |
| **Milestone System** | Trigger, selection, begrænsninger |
| **UI Layer** | Modal, display, progress indicator |
| **Assets** | Ikoner for skills |

---

# FASE 3: IMPLEMENTATION

Implementer tasks i den rækkefølge du har planlagt baseret på dependencies.

For hver task:
1. Implementer koden
2. Verificer at det compiler
3. Test at det fungerer isoleret
4. Gå videre til næste task

---

# FUNKTIONELLE KRAV

## Core Regler

| Regel | Værdi |
|-------|-------|
| Max samtidige skills | 5 |
| Max level per skill | 5 |
| Skill valg per milestone | 3 |
| Total milestones per run | 10 |
| Reset ved prestige | Ja |

## Milestone Thresholds

| # | TotalEarned |
|---|-------------|
| 1 | $1,000 |
| 2 | $5,000 |
| 3 | $15,000 |
| 4 | $50,000 |
| 5 | $150,000 |
| 6 | $500,000 |
| 7 | $1,500,000 |
| 8 | $5,000,000 |
| 9 | $15,000,000 |
| 10 | $50,000,000 |

## De 18 Skills

Se `SKILL_SYSTEM_SPECIFICATION.md` for komplette definitioner. Opsummering:

**💰 Income (6 skills):** Cash Flow, Street Smarts, Business Acumen, Lucky Break, The Skim, Compound Interest

**⚡ Operations (4 skills):** Quick Hands, Chop Shop, Inside Man, Happy Hour

**📴 Offline (4 skills):** Night Owl, Extended Shift, Passive Income, Godfather's Cut

**⭐ Prestige (4 skills):** Old Connections, Reputation, Fast Learner, Early Bird

---

# VIGTIGE BEREGNINGSREGLER

## Multipliers (additive til 1.0)

```
Skill effect = 1.0 + (level × effectPerLevel / 100)

Eksempel: Cash Flow level 3, 4% per level
= 1.0 + (3 × 4 / 100) = 1.12x multiplier
```

## Reductions

```
Cost multiplier = 1.0 - (level × effectPerLevel / 100)

Eksempel: Fast Learner level 3, 4% per level  
= 1.0 - (3 × 4 / 100) = 0.88x cost
```

## Milestone Selection Logic

```
1. TotalEarned >= næste threshold? → Trigger milestone

2. Bestem tilgængelige skills:
   - Fjern skills der er level 5 (maxed)
   - Hvis spilleren har 5 skills → kun eksisterende i pool
   - Ellers → alle ikke-maxede skills i pool

3. Vælg 3 tilfældige fra pool
   - Hvis færre end 3 tilgængelige → vis færre
   - Hvis ingen tilgængelige → skip milestone

4. Spiller vælger → skill tilføjes/opgraderes → MilestoneCount++
```

## Special Cases

| Case | Handling |
|------|----------|
| Spilleren har < 5 skills | Kan vælge ny skill ELLER upgrade eksisterende |
| Spilleren har = 5 skills | Kan KUN upgrade eksisterende |
| Skill er level 5 | Vises ikke som valg |
| Alle skills maxed | Ingen flere milestones |
| Prestige | Skills + MilestoneCount resetter |
| Compound Interest | Baseret på session tid, reset ved app start |

---

# UI RETNINGSLINJER

- Følg eksisterende farvetema (se Theme.xaml eller Colors.xaml)
- Modal skal være overlay der blokerer baggrund
- Skill display skal være kompakt (ikke dominere skærmen)
- Brug samme styling patterns som resten af appen

---

# VERIFIKATION

Test følgende når implementation er færdig:

| Test | Forventet resultat |
|------|-------------------|
| Start nyt spil | Ingen skills, milestone progress vises |
| Nå $1,000 | Modal med 3 skill valg vises |
| Vælg skill | Skill vises i display, effect virker |
| Nå milestone med 5 skills | Modal viser kun upgrade-muligheder |
| Cash Flow skill | Total income er højere |
| Fast Learner skill | Upgrade costs er lavere |
| Night Owl skill | Offline earnings er højere |
| Prestige | Skills forsvinder, milestones resetter |
| Old Connections efter prestige | Start cash > 0 |

---

# OUTPUT FORMAT

Præsenter din analyse og plan før implementation:

```
## ANALYSE RESULTAT

### Projekt Struktur
[Beskriv hvad du fandt]

### Integration Points  
[Beskriv hvor skills skal integreres]

### Potentielle Udfordringer
[Beskriv eventuelle issues]

---

## TASK LISTE

### TASK 1: [Titel]
**Beskrivelse:** ...
**Dependencies:** Ingen
**Filer:** ...
**Acceptkriterier:** ...

### TASK 2: [Titel]
**Beskrivelse:** ...
**Dependencies:** Task 1
**Filer:** ...
**Acceptkriterier:** ...

[osv.]

---

## KLAR TIL IMPLEMENTATION

Skal jeg starte med Task 1?
```

---

# START

Begynd med **FASE 1: ANALYSE**.

Læs `SKILL_SYSTEM_SPECIFICATION.md` og undersøg det eksisterende projekt. Præsenter derefter din plan før du starter implementation.
