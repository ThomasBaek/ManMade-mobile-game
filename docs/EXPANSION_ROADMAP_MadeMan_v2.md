# 🚀 MADE MAN: IDLE EMPIRE - EXPANSION ROADMAP
## Feature Beskrivelser til Implementation

**Version:** 2.0  
**Dato:** December 2024  
**Formål:** Design-dokument til Claude Code implementation

---

# SÅDAN BRUGES DETTE DOKUMENT

Dette dokument beskriver HVAD der skal bygges, ikke HVORDAN. Claude Code skal:

1. Læse feature-beskrivelsen
2. Analysere det eksisterende projekt
3. Evaluere hvordan featuren passer ind
4. Danne sin egen implementeringsplan
5. Implementere i overensstemmelse med projektets arkitektur

**Vigtigt:** Projektstrukturen kan have ændret sig. Claude Code skal altid starte med at analysere den aktuelle kodebase før implementation.

---

# GAME LOOP OVERBLIK

## Nuværende MVP Loop
```
Operationer → Passiv Income → Cash → Upgrades → Prestige
```

## Udvidet Loop (Mål)
```
┌─────────────────────────────────────────────────────────────┐
│                                                             │
│   PASSIV INCOME              AKTIV INCOME                   │
│   (Operationer)              (Missioner)                    │
│        │                          │                         │
│        └──────────┬───────────────┘                         │
│                   ▼                                         │
│              ┌─────────┐                                    │
│              │  CASH   │                                    │
│              └────┬────┘                                    │
│                   │                                         │
│     ┌─────────────┼─────────────┬─────────────┐            │
│     ▼             ▼             ▼             ▼            │
│  Upgrades      Crew          Bribery      Gambling         │
│              (Wages)                                        │
│                   │                                         │
│                   ▼                                         │
│              ┌─────────┐                                    │
│              │EXPENSES │ (løbende omkostninger)             │
│              └─────────┘                                    │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

**Kerneidé:** Spilleren skal balancere passiv income mod løbende udgifter (crew wages), mens aktive missioner giver store engangsbeløb men med risiko.

---

# FEATURE 1: NAVIGATION & UI STRUKTUR

## Beskrivelse

Spillet skal have en klar navigationsstruktur med en top bar der altid viser økonomisk overblik, og en bottom navigation med tabs til forskellige sektioner.

## Top Bar

**Altid synlig øverst på skærmen**

Viser tre økonomiske nøgletal:
- **Cash:** Nuværende pengebeholdning
- **Income:** Brutto indkomst per sekund (fra operationer)
- **Expenses:** Udgifter per sekund (primært crew wages)
- **Net:** Netto indkomst (income minus expenses)

Formål: Spilleren skal altid kunne se sin økonomiske situation på ét blik, især forholdet mellem income og expenses.

## Bottom Navigation

**Fast i bunden af skærmen**

Fem tabs med ikoner:
1. **Empire** - Operationer og prestige (altid tilgængelig)
2. **Organized Crime** - Crew og missioner (låst indtil unlock)
3. **Casino** - Gambling minigames (låst indtil unlock)
4. **Skills** - Skill tree og progression (låst indtil unlock)
5. **Settings** - Indstillinger (altid tilgængelig)

## Tab Unlock System

Tabs unlocks progressivt gennem gameplay:

| Tab | Unlock Betingelse |
|-----|-------------------|
| Empire | Start |
| Settings | Start |
| Skills | Prestige 1 |
| Casino | Prestige 1 |
| Organized Crime | Prestige 2 + mindst 1 crew member |

**UX Note:** Låste tabs skal være synlige men visuelt "disabled" med en indikation af hvad der kræves for at unlocke.

---

# FEATURE 2: CREW SYSTEM

## Kernekoncept

Spilleren kan hyre crew members der:
- Koster løbende penge (wages per minut/time)
- Har forskellige specialer/skills
- Kan sendes på missioner
- Kan blive arresteret og kræve bail

## Crew Member Typer

Otte forskellige crew typer med stigende pris og kapabilitet:

### Tier 1: Basis
| Type | Beskrivelse | Primær Skill |
|------|-------------|--------------|
| **Thug** | Basis enforcer, billig arbejdskraft | Muscle/Styrke |
| **Wheelman** | Flugtbilchauffør | Driving/Kørsel |

### Tier 2: Specialist
| Type | Beskrivelse | Primær Skill |
|------|-------------|--------------|
| **Safecracker** | Åbner pengeskabe og låse | Lockpicking |
| **Hacker** | Deaktiverer alarmer og sikkerhed | Electronics |

### Tier 3: Ekspert
| Type | Beskrivelse | Primær Skill |
|------|-------------|--------------|
| **Chemist** | Producerer stoffer | Chemistry |
| **Fixer** | Kender alle, reducerer risiko | Connections |

### Tier 4: Elite
| Type | Beskrivelse | Primær Skill |
|------|-------------|--------------|
| **Hitman** | Professionel til farlige jobs | Combat |
| **Consigliere** | Rådgiver, giver globale bonusser | Leadership |

## Crew Økonomi

**Hire Cost:** Engangsbetaling for at hyre
**Wage:** Løbende betaling per tidsenhed (f.eks. per minut)

Wages trækkes automatisk fra spillerens cash og vises som "Expenses" i top bar.

**Balance-overvejelse:** Wages skal være mærkbare men ikke ødelæggende. En tommelfingerregel: En crew members wage bør kunne dækkes af 2-3 operationers income.

## Crew Status

Et crew member kan have følgende statusser:
- **Available:** Klar til at blive assignet til mission
- **On Mission:** Utilgængelig, på aktiv mission
- **In Jail:** Arresteret, kræver bail eller ventetid
- **Recovering:** Midlertidigt utilgængelig efter fejlet mission

## Crew UI Elementer

Crew oversigten skal vise:
- Grupperet liste af crew efter type
- Antal i hver kategori (f.eks. "Thugs: 5")
- Hvor mange der er available/on mission/in jail
- Total wage for hele crewet
- Mulighed for at hyre flere af hver type
- Liste over crew i jail med bail-mulighed

---

# FEATURE 3: MISSION SYSTEM

## Kernekoncept

Missioner er aktive opgaver der:
- Kræver specifikke crew members med bestemte skills
- Tager real-time at gennemføre (timer)
- Har en risiko-procent for at fejle
- Giver store engangsbeløb ved succes
- Kan resultere i arrest ved fejl

## Mission Flow

```
1. VÆLG MISSION
   ↓
2. SE REQUIREMENTS (hvilke crew skills kræves)
   ↓
3. ASSIGN CREW (vælg crew members til missionen)
   ↓
4. SE RISIKO % (beregnet ud fra crew vs. mission difficulty)
   ↓
5. START MISSION (crew bliver "On Mission")
   ↓
6. VENT (real-time timer, kan være minutter til timer)
   ↓
7. RESULTAT
   ├── SUCCESS: Fuld payout, crew bliver available
   ├── PARTIAL: Delvis payout, nogle crew arresteret
   └── FAILURE: Ingen payout, crew arresteret
```

## Mission Kategorier

Missioner er grupperet i kategorier med stigende risiko og reward:

### Petty Crime (Unlock: Prestige 2)
- Små jobs som biltyveri, indbrud
- Kort varighed (5-30 minutter)
- Lav risiko (5-20%)
- Reward: $1,000 - $10,000
- Kræver: 1-2 crew members

### Robbery (Unlock: Prestige 2)
- Røverier af butikker, banker
- Medium varighed (30-120 minutter)
- Medium risiko (15-35%)
- Reward: $10,000 - $50,000
- Kræver: 2-4 crew members med varierede skills

### Bootlegging (Unlock: Prestige 3)
- Transport og salg af ulovlig alkohol
- Lang varighed (1-3 timer)
- Medium-høj risiko (20-40%)
- Reward: $20,000 - $100,000
- Kræver: Wheelmen + Muscle

### Drug Running (Unlock: Prestige 3)
- Produktion og distribution af stoffer
- Lang varighed (2-6 timer)
- Høj risiko (25-50%)
- Reward: $50,000 - $200,000
- Kræver: Chemist + Distribution crew

### Assassination (Unlock: Prestige 5)
- Eliminering af mål
- Meget lang varighed (4-8 timer)
- Meget høj risiko (40-70%)
- Reward: $100,000 - $500,000
- Kræver: Hitman + Support crew

### Political (Unlock: Prestige 5)
- Bestikkelse, afpresning af politikere
- Ekstremt lang varighed (6-12 timer)
- Variabel risiko (30-60%)
- Reward: $200,000 - $1,000,000
- Kræver: Fixer + Consigliere

## Mission Requirements

Hver mission specificerer:
- Hvilke skill-typer der kræves
- Minimum skill-niveau for hver
- Antal crew members med hver skill

**Eksempel:** Bank Heist kræver:
- 2x Muscle (min niveau 20)
- 1x Lockpicking (min niveau 30)
- 1x Electronics (min niveau 20)
- 1x Driving (min niveau 25)

## Risk Calculation

Missionens endelige risiko beregnes ud fra:

**Base Risk** (missionens grundrisiko)
- Crew Skill Bonus (højere skill = lavere risiko)
+ Police Heat (højere heat = højere risiko)
- Active Bribes (bestikkelse reducerer risiko)
- Fixer Bonus (Fixers i crewet reducerer risiko)
- Consigliere Bonus (global reduktion hvis du har en)
= **Final Risk %**

Spilleren skal kunne se denne beregning før de starter missionen.

## Mission UI Elementer

### Mission Liste
- Viser tilgængelige missioner
- Locked missioner vises med unlock-krav
- For hver mission: Navn, reward, duration, base risk
- Indikation af om spilleren har nødvendige crew

### Active Missions
- Viser igangværende missioner med countdown timer
- Progress bar
- Assigned crew
- Forventet reward

### Mission Detail View
- Fuld beskrivelse
- Alle requirements med check/X for om de er opfyldt
- Risk breakdown (hvordan risikoen er beregnet)
- Crew assignment interface
- Start button (kun aktiv hvis requirements er opfyldt)

---

# FEATURE 4: POLICE & RISK SYSTEM

## Kernekoncept

Kriminel aktivitet tiltrækker opmærksomhed fra politiet. Jo mere aktiv spilleren er, jo højere risiko for at missioner fejler.

## Heat System

**Heat** er et tal fra 0-100% der repræsenterer politiets opmærksomhed.

- Heat stiger når spilleren gennemfører missioner
- Højere tier missioner genererer mere heat
- Heat falder langsomt over tid (f.eks. -1% per time)
- Heat påvirker risikoen på alle missioner

**Heat til Risk:** Ved 100% heat får alle missioner +30% ekstra risiko.

## Bribery System

Spilleren kan bestikke politiet for at reducere risiko.

### Bribe Typer

| Type | Effekt | Varighed | Pris-niveau |
|------|--------|----------|-------------|
| **Single Mission** | -15% risk | Kun næste mission | Billig |
| **Kortvarig Beskyttelse** | -20% risk | 4 timer | Medium |
| **Langvarig Beskyttelse** | -30% risk | 24 timer | Dyr |
| **Permanent Beskyttelse** | -40% risk | Permanent | Ekstrem dyr |

**UX:** Aktive bribes skal vises tydeligt med nedtælling til udløb.

## Arrest & Bail

Når en mission fejler, kan crew members blive arresteret.

### Arrest
- Crew member får status "In Jail"
- De er utilgængelige for missioner
- Deres wage fortsætter IKKE mens de er i jail

### Bail Options
Spilleren har tre valg for et arresteret crew member:

1. **Pay Bail:** Betal et beløb for at få dem fri med det samme
   - Bail = Cirka 10x deres timeløn
   
2. **Wait:** Vent en periode (f.eks. 24 timer) og de løslades gratis

3. **Abandon:** Fyrer crew member permanent
   - Sparer bail-penge
   - MEN: Alle andre crew members mister "loyalty" (fremtidig feature)
   - Viser at bossen ikke bakker op om sit crew

### Jail UI
- Liste over crew i jail
- For hver: Navn, type, bail amount, tid til automatisk løsladelse
- Knapper for Pay Bail / Wait / Abandon

---

# FEATURE 5: SKILL SYSTEM

## Kernekoncept

Spilleren optjener skill points og kan investere dem i permanente bonusser. Ved visse milestones skal spilleren vælge mellem forskellige upgrade-paths.

## Skill Points

Optjenes via:
- Prestige (+1 per prestige)
- Milestones ($100K, $1M, $10M earned osv.)
- Achievements (visse achievements giver +1)

## Skill Kategorier

### 💰 Income Skills
Bonusser til passiv indkomst:
- **Efficient Operations:** +X% income fra alle operationer per level
- **Offline Boss:** +X timer max offline earnings per level
- **Prestige Master:** +X% ekstra prestige bonus per level

### 💼 Operations Skills
Bonusser til missioner og crew:
- **Quick Jobs:** -X% mission duration per level
- **Crew Discount:** -X% crew hire cost per level
- **Budget Operations:** -X% crew wages per level

### 🛡️ Protection Skills
Bonusser til risiko-reduktion:
- **Police Connections:** -X% base risk på alle missions per level
- **Legal Connections:** -X% bail cost per level
- **Low Profile:** -X% heat generation per level

## Skill Progression

Hver skill har:
- Max level (typisk 3-5)
- Cost per level (1 skill point)
- Unlock requirement (min prestige level)
- Effect per level (den bonus du får)

## Milestone Choices

Ved vigtige milestones (f.eks. $100K, $1M, $10M total earned) får spilleren et VALG mellem forskellige permanente bonusser.

**Vigtigt:** Dette valg er PERMANENT for denne save. Det skaber replayability da spilleren kan prøve forskellige builds.

### Eksempel: $1,000,000 Milestone

Spilleren vælger ÉN af:
1. **Unlock Operation:** Lås en ny operation op (f.eks. Money Laundering)
2. **Unlock Crew Type:** Lås en ny crew type op (f.eks. Hitman)
3. **Permanent Bonus:** Få en permanent stat-bonus (f.eks. -5% risk)

### Milestone UI
- Popup når milestone nås
- Viser de 2-3 valgmuligheder med beskrivelser
- Advarsel om at valget er permanent
- Confirmation før valg gemmes

## Skills Tab UI Elementer

Skal vise:
- Tilgængelige skill points
- Skill tree/liste grupperet efter kategori
- For hver skill: Navn, level, max level, effekt, upgrade-knap
- Locked skills med unlock-krav
- Oversigt over optjente milestone bonusser

---

# FEATURE 6: CASINO

## Kernekoncept

Et interaktivt casino hvor spilleren kan gamble sine penge. Høj risiko, høj reward - men huset vinder altid på lang sigt.

**Unlock:** Prestige 1

## Casino Spil

### 🎡 Roulette (Unlock: Prestige 1)

Klassisk roulette med betting options:
- Rød/Sort: 2x payout, ~48.6% chance
- Lige/Ulige: 2x payout, ~48.6% chance
- 1-18 / 19-36: 2x payout, ~48.6% chance
- Grøn (0): 14x payout, ~2.7% chance
- Specifikt tal: 35x payout, ~2.7% chance

**UX:** Visuelt roulette-hjul der spinner, historie over sidste 10 resultater.

### 🎲 High-Low (Unlock: Prestige 1)

Simpelt og addictive spil:
1. Vis et kort (1-13)
2. Spiller gætter: Er næste kort HØJERE eller LAVERE?
3. Korrekt gæt: Pot ganges med 1.8x
4. Forkert gæt: Tab hele pot
5. Spiller kan CASH OUT når som helst

**UX:** Viser nuværende pot, streak counter, potentiel næste gevinst. "Cash Out" knap skal være prominent.

### 🃏 Blackjack (Unlock: Prestige 3)

Standard blackjack regler:
- Mål: Kom tættere på 21 end dealer uden at gå over
- Hit: Træk et kort
- Stand: Hold nuværende hånd
- Double Down: Fordobl indsats, få præcis ét kort mere
- Blackjack (21 på første 2 kort) betaler 3:2
- Dealer står på 17

### 🎰 Slots (Unlock: Prestige 3)

Slot machine med symboler:
- Match 3 ens = Payout afhængig af symbol
- Almindelige symboler: Lav payout (5-10x)
- Sjældne symboler: Høj payout (50-100x)
- To ens: Lille payout (2x)

## Betting Limits

- **Minimum bet:** Fast beløb (f.eks. $100)
- **Maximum bet:** Procent af nuværende cash (f.eks. 10%)
- **Daglig tab limit:** Valgfri feature spilleren kan slå til/fra

## Streak System

Konsekutive wins giver bonus:
- 3 wins i træk: +5% på næste gevinst
- 5 wins i træk: +10% på næste gevinst
- 10 wins i træk: +25% på næste gevinst

## Crew Integration

Hvis spilleren har en **Fixer** i sit crew, reduceres house edge på alle casino spil.

## Casino UI Elementer

- Oversigt med alle spil (låste spil vises med unlock-krav)
- Balance og dagens profit/tab
- Streak indicator
- Statistik (antal spil, største gevinst, net profit all-time)

---

# FEATURE 7: STORY & LORE

## Kernekoncept

En narrativ lag der giver kontekst og engagement. Kapitler unlocks progressivt og giver spilleren en følelse af at bygge et imperium.

**Unlock:** Prestige 5

## Story Struktur

### Kapitler

Fortællingen er opdelt i kapitler der unlocks ved gameplay milestones:

| Kapitel | Unlock | Handling |
|---------|--------|----------|
| 1: Ankomsten | Start | Spiller ankommer til New Porto |
| 2: Første Job | $5,000 earned | Første rigtige kriminelle job |
| 3: Speakeasy | Unlock Speakeasy | Åbn din første bar |
| 4: De Fem Familier | $100,000 earned | Introduktion til byens magthavere |
| 5: Krig | $500,000 earned | Konflikt med rival familie |
| 6: The Commission | $5,000,000 earned | Din plads ved magtens bord |

### Karakter Profiler

Vigtige karakterer med:
- Navn og alias
- Rolle i historien
- Kort baggrund
- Relation til spilleren (ally/neutral/rival)

### Valg med Konsekvenser

Ved visse story beats kan spilleren træffe valg der påvirker gameplay:
- Vælg side i en konflikt
- Invester i ét område frem for et andet
- Disse valg kan unlocke unikke bonusser eller operationer

## Story UI Elementer

Story tab skal vise:
- Liste over kapitler (completed/available/locked)
- Mulighed for at genlæse gamle kapitler
- Karakter galleri med unlockede karakterer
- Progress mod næste kapitel

---

# FEATURE 8: EXPANDED OPERATIONS

## Kernekoncept

Udvid antallet af operationer fra 5 til 15, organiseret i tiers med stigende income og unlock cost.

## Operations by Tier

### Tier 1: Street (Unlock: Start - $250)
| Operation | Base Income | Beskrivelse |
|-----------|-------------|-------------|
| Pickpocketing | Lav | Lette lommer på turister |
| Car Theft | Lav-medium | Stjæl biler, sælg dele |
| Burglary | Medium | Indbrud i private hjem |

### Tier 2: Organized ($1K - $5K)
| Operation | Base Income | Beskrivelse |
|-----------|-------------|-------------|
| Speakeasy | Medium | Illegal bar |
| Loan Sharking | Medium-høj | Udlån med "renter" |
| Fight Club | Høj | Underground boksning |

### Tier 3: Empire ($15K - $100K)
| Operation | Base Income | Beskrivelse |
|-----------|-------------|-------------|
| Underground Casino | Høj | Gambling den |
| Bootleg Distillery | Høj | Producér whiskey |
| Protection Racket | Meget høj | Beskyttelsespenge |

### Tier 4: Godfather ($250K - $1.5M)
| Operation | Base Income | Beskrivelse |
|-----------|-------------|-------------|
| Drug Network | Meget høj | Stoffdistribution |
| Weapons Smuggling | Ekstrem | Våbenimport |
| Money Laundering | Ekstrem | Hvidvask |

### Tier 5: The Commission ($5M - $50M)
| Operation | Base Income | Beskrivelse |
|-----------|-------------|-------------|
| Political Corruption | Ekstrem | Politikere på lønningslisten |
| Union Control | Ekstrem | Kontrol over fagforeninger |
| The Commission Seat | Maximum | Sæde ved de fem familiers bord |

## Operation UI Opdateringer

- Gruppér operationer visuelt efter tier
- Vis tier-header med samlet income for den tier
- Locked operationer i højere tiers viser unlock-krav

---

# IMPLEMENTATION PRIORITERING

## Anbefalet Rækkefølge

### Phase 1: Core UI (Høj prioritet)
1. Top bar med income/expenses/net
2. Bottom navigation med tabs
3. Tab unlock system

**Hvorfor først:** Al anden funktionalitet bygger på denne navigation.

### Phase 2: Crew System (Høj prioritet)
1. Crew data model og state management
2. Crew list UI med hire/fire
3. Wage system (beregn og træk fra cash)
4. Integration med top bar expenses

**Hvorfor:** Crew er fundamentet for mission systemet.

### Phase 3: Mission System (Høj prioritet)
1. Mission data og tilgængelige missioner
2. Mission list UI
3. Crew assignment flow
4. Mission timer og completion
5. Success/failure logic

**Hvorfor:** Missions er den primære nye gameplay loop.

### Phase 4: Police System (Medium prioritet)
1. Heat tracking
2. Risk calculation integration
3. Bribery UI og logic
4. Arrest og bail system

**Hvorfor:** Tilføjer dybde og risiko til missions.

### Phase 5: Skills System (Medium prioritet)
1. Skill data model
2. Skills tab UI
3. Skill point tracking
4. Milestone choice popups
5. Skill effects application

**Hvorfor:** Progression system der giver langsigtede mål.

### Phase 6: Casino (Lav-medium prioritet)
1. Casino tab
2. Roulette implementation
3. High-Low implementation
4. Blackjack implementation
5. Slots implementation

**Hvorfor:** Retention feature, men ikke core gameplay.

### Phase 7: Story & Polish (Lav prioritet)
1. Story tab struktur
2. Kapitel content
3. Karakter profiler
4. Valg system

**Hvorfor:** Engagement feature, kan komme efter core gameplay virker.

---

# BALANCE GUIDELINES

## Økonomi Balance

### Income vs. Expenses
- En nyligt hyret crew members wage bør kunne dækkes af 2-3 operation upgrades
- Spilleren skal føle at crew koster noget, men ikke være i konstant underskud
- Net income (income - expenses) skal typisk være positiv

### Mission Rewards vs. Time
- Kortere missioner: Lavere total reward, men hurtigere turnaround
- Længere missioner: Højere reward, men mere risiko ved fejl
- "Time value of money" - en 2-timers mission bør give mere end 4x en 30-min mission

### Risk vs. Reward
- Højere risiko missioner skal give proportionalt højere reward
- Men ikke lineært - en 50% risk mission bør give mere end 2.5x en 20% risk mission
- Spilleren skal føle at risiko er et reelt valg, ikke bare "tag altid den højeste"

## Progression Balance

### Prestige Pacing
- Prestige 1: Inden for første 15-30 minutter
- Prestige 2: Inden for første 1-2 timer
- Prestige 3: Inden for første dag
- Prestige 5: Inden for første uge
- Prestige 10: Langsigtet mål (uger til måneder)

### Content Gating
- Nye features skal føles som belønninger, ikke forhindringer
- Hvert prestige level bør unlocke noget synligt nyt
- Spilleren skal altid have noget at arbejde hen mod

---

# NOTER TIL CLAUDE CODE

## Før Implementation

1. **Analyser eksisterende projekt**
   - Mappestruktur
   - Eksisterende models og services
   - Arkitektur patterns (MVVM, DI setup)
   - Eksisterende UI komponenter

2. **Identificér integration points**
   - Hvordan tilføjes nye tabs til eksisterende navigation?
   - Hvor skal nye services registreres?
   - Hvordan udvides GameState med nye properties?

3. **Vurdér afhængigheder**
   - Crew system skal eksistere før Mission system
   - Police system integrerer med Mission system
   - Skills system påvirker mange andre systemer

## Under Implementation

1. **Følg eksisterende patterns**
   - Brug samme arkitektur
   - Brug samme naming conventions
   - Brug samme UI styling

2. **Inkrementel implementation**
   - Implementér én feature ad gangen
   - Test hver feature før næste påbegyndes
   - Commit efter hver fungerende feature

3. **Bevar backwards compatibility**
   - Eksisterende saves skal stadig kunne loades
   - Nye features skal have sensible defaults

---

# VISUAL MOCKUPS

## Top Bar
```
┌─────────────────────────────────────────────────────────────┐
│  💰 $1,234,567  │  📈 +$456/s  │  📉 -$120/s  │  NET: +$336/s │
└─────────────────────────────────────────────────────────────┘
```

## Bottom Navigation
```
┌─────────────────────────────────────────────────────────────┐
│   🏠        💼        🎰        📊        ⚙️              │
│  Empire   Org Crime  Casino   Skills   Settings            │
│           [🔒]       [🔒]     [🔒]                          │
└─────────────────────────────────────────────────────────────┘
```

## Mission Card
```
┌─────────────────────────────────────────────────────────────┐
│  🏦 BANK HEIST                                              │
│  ─────────────────────────────────────────────────────────  │
│  Reward: $50,000              Duration: 2 timer             │
│  Base Risk: 35%               Your Risk: 22% ✅             │
│                                                             │
│  Requirements:                                              │
│  ├── 2x Muscle ✅                                          │
│  ├── 1x Lockpicking ✅                                     │
│  └── 1x Driving ❌ (mangler Wheelman)                      │
│                                                             │
│  [ START MISSION ]                                          │
└─────────────────────────────────────────────────────────────┘
```

## Crew Overview
```
┌─────────────────────────────────────────────────────────────┐
│  👥 CREW                           Total Wages: $320/min    │
│  ─────────────────────────────────────────────────────────  │
│                                                             │
│  THUGS (5)                                                  │
│  ├── 4 Available ✅                                        │
│  ├── 1 On Mission 🎯                                       │
│  └── [ + HIRE THUG - $500 ]                                │
│                                                             │
│  WHEELMEN (2)                                               │
│  ├── 2 Available ✅                                        │
│  └── [ + HIRE WHEELMAN - $2,000 ]                          │
│                                                             │
│  🔒 IN JAIL (1)                                            │
│  └── Tony (Thug) - Bail: $5,000 [ PAY ] [ WAIT 23h ]       │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

## Skills Panel
```
┌─────────────────────────────────────────────────────────────┐
│  📊 SKILLS                         Available Points: 3      │
│  ─────────────────────────────────────────────────────────  │
│                                                             │
│  💰 INCOME                                                  │
│  ├── Efficient Operations [★★★☆☆] +15% income              │
│  └── Offline Boss [★★☆☆☆] +4 timer offline                 │
│                                                             │
│  💼 OPERATIONS                                              │
│  ├── Quick Jobs [★☆☆☆☆] -10% mission tid                   │
│  └── Crew Discount [🔒] Unlock: Prestige 3                  │
│                                                             │
│  🛡️ PROTECTION                                             │
│  ├── Police Connections [★★☆☆☆] -10% risk                  │
│  └── Low Profile [☆☆☆☆☆] Reducerer heat                    │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

## Milestone Choice Popup
```
┌─────────────────────────────────────────────────────────────┐
│                                                             │
│              🎉 MILESTONE REACHED: $1,000,000!              │
│                                                             │
│  Vælg én permanent bonus:                                   │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  💰 MONEY LAUNDERING                                │   │
│  │  Unlock ny operation der konverterer heat til cash  │   │
│  │  [ VÆLG ]                                           │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  👥 ELITE CREW                                      │   │
│  │  Unlock Hitman crew type til farlige missioner      │   │
│  │  [ VÆLG ]                                           │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  🛡️ IMMUNITY                                        │   │
│  │  Permanent -5% risk på alle missioner               │   │
│  │  [ VÆLG ]                                           │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  ⚠️ Dette valg er PERMANENT!                               │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

**Dokument slut**

*Expansion Roadmap v2.0*  
*Made Man: Idle Empire*  
*Beskrivende design-dokument til Claude Code implementation*
