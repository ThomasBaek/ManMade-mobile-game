# 🎰 MADE MAN: IDLE EMPIRE
## Skill System Design Specification v1.0

---

# 1. KONCEPT

## Hvad er Skill System?

Et **roguelike-inspireret progressionssystem** hvor spilleren vælger og opgraderer skills ved milestones. Dette skaber forskellige "builds" og øger replayability.

## Narrativ Ramme

> *"Dit ry vokser i New Porto. Talentfulde folk hører om dig og søger dig ud. Men du kan kun have plads til de mest loyale i din indre kreds. Vælg med omhu - disse forbindelser vil definere dit imperium."*

---

# 2. CORE REGLER

| Regel | Værdi | Begrundelse |
|-------|-------|-------------|
| **Max samtidige skills** | 5 | Tvinger specialisering, skaber unikke builds |
| **Max level per skill** | 5 | Progression uden grind |
| **Skill valg per milestone** | 3 | Variation uden overwhelm |
| **Total skill pool** | 18 | Stor variation mellem runs |
| **Reset ved prestige** | Ja | Ny build hver run |

---

# 3. MILESTONE SYSTEM

## Triggers

Milestones udløses når **TotalEarned** (akkumuleret indkomst i nuværende run) når bestemte thresholds.

| Milestone | Threshold | Narrativ |
|-----------|-----------|----------|
| 1 | $1,000 | "Første rekrut" |
| 2 | $5,000 | "Rygtet spredes" |
| 3 | $15,000 | "Folk banker på døren" |
| 4 | $50,000 | "Du har et crew nu" |
| 5 | $150,000 | "Familien vokser" |
| 6 | $500,000 | "Loyalitet belønnes" |
| 7 | $1,500,000 | "The inner circle" |
| 8 | $5,000,000 | "Legenden vokser" |
| 9 | $15,000,000 | "Ubestridt" |
| 10 | $50,000,000 | "Godfather's velsignelse" |

**Efter milestone 10:** Ingen flere milestones i denne run.

## Selection Logic

Ved milestone:

1. **Bestem tilgængelig pool:**
   - Start med alle 18 skills
   - Fjern skills der er level 5 (maxed)
   - Hvis spilleren har 5 skills → behold KUN spillerens eksisterende skills i pool

2. **Vælg 3 tilfældige** fra pool
   - Kan inkludere skills spilleren allerede har (for upgrade)
   - Hvis færre end 3 tilgængelige → vis færre
   - Hvis ingen tilgængelige → skip milestone (alle maxed)

3. **Spilleren vælger én:**
   - Ny skill → tilføjes med level 1
   - Eksisterende skill → level øges med 1

---

# 4. SKILL DEFINITIONER

## Kategorier

| Kategori | Ikon | Fokus |
|----------|------|-------|
| **Income** | 💰 | Øger indkomst generelt |
| **Operations** | ⚡ | Booster specifikke operationer |
| **Offline** | 📴 | Forbedrer offline indtjening |
| **Prestige** | ⭐ | Påvirker costs og prestige |

---

## 💰 INCOME SKILLS (6 stk)

### Cash Flow
| | |
|---|---|
| **Effekt** | Øger total indkomst |
| **Per level** | +4% |
| **Max (lvl 5)** | +20% total income |
| **Beregning** | Multiplicer samlet income |

### Street Smarts
| | |
|---|---|
| **Effekt** | Booster Tier 1 operationer (Pickpocket, Car Theft, Burglary) |
| **Per level** | +10% |
| **Max (lvl 5)** | +50% Tier 1 income |
| **Beregning** | Multiplicer kun Tier 1 operations |

### Business Acumen
| | |
|---|---|
| **Effekt** | Booster Tier 2 operationer (Speakeasy, Casino) |
| **Per level** | +12% |
| **Max (lvl 5)** | +60% Tier 2 income |
| **Beregning** | Multiplicer kun Tier 2 operations |

### Lucky Break
| | |
|---|---|
| **Effekt** | Chance for dobbelt indkomst per tick |
| **Per level** | +4% chance |
| **Max (lvl 5)** | 20% chance for 2x |
| **Beregning** | Ved hvert tick: random check, hvis success → 2x income dette tick |

### The Skim
| | |
|---|---|
| **Effekt** | Cashback på alle køb (upgrades og unlocks) |
| **Per level** | +5% |
| **Max (lvl 5)** | 25% cashback |
| **Beregning** | Ved køb: refunder (cost × cashback%) til spilleren |

### Compound Interest
| | |
|---|---|
| **Effekt** | Indkomst stiger over tid i aktiv session |
| **Per level** | +3% per 5 minutter |
| **Max (lvl 5)** | +15% per 5 min (max +90% efter 30 min) |
| **Beregning** | Beregn minutter siden session start, divider med 5, cap ved 6 intervaller |

---

## ⚡ OPERATIONS SKILLS (4 stk)

### Quick Hands
| | |
|---|---|
| **Effekt** | Booster Pickpocketing specifikt |
| **Per level** | +25% |
| **Max (lvl 5)** | +125% Pickpocket income |
| **Påvirker** | Kun "pickpocket" operation |

### Chop Shop
| | |
|---|---|
| **Effekt** | Booster Car Theft specifikt |
| **Per level** | +20% |
| **Max (lvl 5)** | +100% Car Theft income |
| **Påvirker** | Kun "cartheft" operation |

### Inside Man
| | |
|---|---|
| **Effekt** | Booster Burglary specifikt |
| **Per level** | +18% |
| **Max (lvl 5)** | +90% Burglary income |
| **Påvirker** | Kun "burglary" operation |

### Happy Hour
| | |
|---|---|
| **Effekt** | Booster Speakeasy specifikt |
| **Per level** | +15% |
| **Max (lvl 5)** | +75% Speakeasy income |
| **Påvirker** | Kun "speakeasy" operation |

---

## 📴 OFFLINE SKILLS (4 stk)

### Night Owl
| | |
|---|---|
| **Effekt** | Øger offline earnings efficiency |
| **Per level** | +10% |
| **Max (lvl 5)** | Offline efficiency: 50% → 100% |
| **Beregning** | Addér til base offline efficiency (normalt 50%) |

### Extended Shift
| | |
|---|---|
| **Effekt** | Forlænger max offline tid |
| **Per level** | +1 time |
| **Max (lvl 5)** | Max offline: 4 timer → 9 timer |
| **Beregning** | Addér til base max offline hours |

### Passive Income
| | |
|---|---|
| **Effekt** | Flat baseline indkomst uanset operationer |
| **Per level** | +$1/sek |
| **Max (lvl 5)** | +$5/sek baseline |
| **Beregning** | Addér til total income før andre multipliers |

### Godfather's Cut
| | |
|---|---|
| **Effekt** | Bonus på offline earnings ved tilbagevenden |
| **Per level** | +8% |
| **Max (lvl 5)** | +40% offline earnings bonus |
| **Beregning** | Multiplicer færdigberegnede offline earnings |

---

## ⭐ PRESTIGE SKILLS (4 stk)

### Old Connections
| | |
|---|---|
| **Effekt** | Start med bonus cash efter prestige |
| **Per level** | +$100 |
| **Max (lvl 5)** | +$500 start cash |
| **Beregning** | Sæt starting cash ved prestige reset |
| **Vigtigt** | Beregnes FØR skills resetter |

### Reputation
| | |
|---|---|
| **Effekt** | Øger prestige bonus multiplier |
| **Per level** | +6% |
| **Max (lvl 5)** | +30% ekstra prestige bonus |
| **Beregning** | Addér til base prestige bonus (normalt 25%) |
| **Vigtigt** | Beregnes FØR skills resetter |

### Fast Learner
| | |
|---|---|
| **Effekt** | Reducerer upgrade costs |
| **Per level** | -4% |
| **Max (lvl 5)** | -20% alle upgrade costs |
| **Beregning** | Multiplicer upgrade cost med (1 - reduction) |

### Early Bird
| | |
|---|---|
| **Effekt** | Reducerer unlock costs for nye operationer |
| **Per level** | -5% |
| **Max (lvl 5)** | -25% alle unlock costs |
| **Beregning** | Multiplicer unlock cost med (1 - reduction) |

---

# 5. BEREGNINGSREGLER

## Multiplier Formula

Alle procentvise bonusser er **additive til base 1.0**:

```
multiplier = 1.0 + (level × effectPerLevel / 100)
```

**Eksempel:** Cash Flow level 3 med 4% per level:
```
= 1.0 + (3 × 4 / 100) = 1.0 + 0.12 = 1.12x
```

## Reduction Formula

Cost reductions:

```
costMultiplier = 1.0 - (level × effectPerLevel / 100)
```

**Eksempel:** Fast Learner level 3 med 4% per level:
```
= 1.0 - (3 × 4 / 100) = 1.0 - 0.12 = 0.88x
```

## Income Calculation Order

Ved beregning af total income per sekund:

1. Start med baseline income fra Passive Income skill (eller 0)
2. For hver operation: beregn base income
3. Apply operation-specific skill multiplier (Quick Hands, etc.)
4. Apply tier multiplier (Street Smarts, Business Acumen)
5. Sum alle operations
6. Apply total income multiplier (Cash Flow)
7. Apply time-based multiplier (Compound Interest)
8. Per tick: check for critical (Lucky Break)

## Offline Calculation

Ved app resume:

1. Beregn tid siden sidste save
2. Cap ved max offline hours (inkl. Extended Shift bonus)
3. Beregn: `income × tid × offlineEfficiency` (inkl. Night Owl)
4. Apply offline bonus (Godfather's Cut)
5. Add til cash og totalEarned

## Prestige Calculation

Ved prestige:

1. Beregn prestige bonus (inkl. Reputation skill) - FØR reset
2. Beregn starting cash (Old Connections skill) - FØR reset
3. Apply prestige bonus til permanent multiplier
4. Reset cash til starting cash amount
5. Reset TotalEarned til 0
6. Reset alle operationer
7. Reset alle skills og MilestoneCount
8. Reset session start time

---

# 6. UI/UX KRAV

## Skill Selection Modal

Vises som **overlay** når milestone nås. Skal blokere baggrunden.

**Indhold:**
- Header med "MILESTONE REACHED" og narrativ tekst
- 3 skill cards (eller færre hvis ikke nok tilgængelige)
- Hver card viser: ikon, navn, beskrivelse, effekt, nuværende level, vælg/upgrade knap
- Indikator for brugte skill slots (f.eks. "3/5 slots")

**Interaktion:**
- Tap på card eller knap vælger skill
- Modal lukker efter valg
- Ingen "luk uden valg" option (tvinger beslutning)

## Skill Display (Hovedskærm)

Kompakt sektion der viser aktive skills.

**Indhold:**
- 5 slots (fyldte viser skill ikon + level, tomme er grå)
- Progress mod næste milestone

**Placering:**
- Skal passe ind i eksisterende layout
- Ikke dominere skærmen
- Følge eksisterende design language

## Visuel Feedback

- Ved skill valg: kort animation/feedback
- Ved critical hit (Lucky Break): visuel indikation
- Ved cashback (The Skim): vis beløb der refunderes

---

# 7. EDGE CASES

| Situation | Håndtering |
|-----------|------------|
| Færre end 3 skills tilgængelige | Vis færre cards i modal |
| Ingen skills tilgængelige (alle maxed) | Skip milestone, ingen modal |
| Spilleren har præcis 5 skills | Modal viser kun eksisterende for upgrade |
| Spilleren prestigier med 0 skills | Normal prestige, ingen skill-relaterede bonusser |
| App lukkes under milestone modal | Gem state, vis modal igen ved næste åbning |
| Compound Interest efter lang AFK | Reset session time ved app resume |

---

# 8. BALANCERING

## Forventet Progression

| Fase | Tid | Milestones | Skills |
|------|-----|------------|--------|
| Early | 0-10 min | 1-3 | 1-3 nye |
| Mid | 10-30 min | 4-6 | 4-5 total, upgrades starter |
| Late | 30-60+ min | 7-10 | 5 skills, fokus på upgrades |

## Build Arketyper

Systemet skal muliggøre forskellige playstyles:

| Build | Fokus | Nøgle-skills |
|-------|-------|--------------|
| **Speedrunner** | Hurtig prestige | Fast Learner, Early Bird, Cash Flow |
| **AFK King** | Max offline gains | Night Owl, Extended Shift, Godfather's Cut |
| **Street Hustler** | Early game dominans | Street Smarts, Quick Hands, Chop Shop |
| **Business Mogul** | Late game power | Business Acumen, Happy Hour, Cash Flow |
| **Gambler** | High variance | Lucky Break, Compound Interest, The Skim |

## Skill Værdi (Rough Tier List)

**S-Tier (Altid stærke):**
- Cash Flow, Night Owl, Fast Learner

**A-Tier (Build-afhængige):**
- Street Smarts, Business Acumen, Lucky Break

**B-Tier (Situationelle):**
- Operations-specifikke, Extended Shift

**C-Tier (Niche):**
- Passive Income, Old Connections

---

# 9. INTEGRATION NOTER

## Påvirkede Systemer

| System | Påvirkning |
|--------|------------|
| **Income calculation** | Skal inkludere alle skill multipliers |
| **Cost calculation** | Skal inkludere reductions |
| **Offline system** | Skal bruge skill-modificerede værdier |
| **Prestige system** | Skal beregne skill bonusser før reset, derefter cleare skills |
| **Game loop** | Skal checke for milestone triggers |
| **Save/Load** | Skal persistere skills og milestone count |

## Nye Data Felter

GameState skal udvides med:
- Liste af unlocked skills (id + level)
- Milestone count (antal completed)
- Session start time (for Compound Interest)

---

# 10. OPSUMMERING

## Skill System på 30 sekunder:

1. **Ved milestones** (10 i alt baseret på TotalEarned) får spilleren valget mellem **3 tilfældige skills**
2. Spilleren kan have **max 5 skills**, hver kan nå **level 5**
3. Skills giver **passive bonusser** til income, costs, offline, prestige
4. Ved **prestige resetter alle skills** → ny build næste run
5. Dette skaber **replayability** og **strategisk dybde**

---

*Design Specification v1.0*
*Made Man: Idle Empire*
