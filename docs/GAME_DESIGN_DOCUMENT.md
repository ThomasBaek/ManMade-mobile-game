# 🎰 MADE MAN: IDLE EMPIRE
## Game Design Document & Spillerguide

---

# 📖 INTRODUKTION

## Hvad er Made Man: Idle Empire?

**Made Man: Idle Empire** er et idle/incremental spil hvor du bygger et kriminelt imperium fra bunden. Du starter som en nobody på havnen og arbejder dig op til at blive byens mest frygtede Godfather.

Spillet er inspireret af klassiske browser-spil som **Omerta** (2003-2015) og idle games som **Cookie Clicker** og **Adventure Capitalist**.

### Genre
- **Primær:** Idle / Incremental Game
- **Sekundær:** Tycoon / Empire Builder
- **Tema:** 1930'erne Mafia / Noir

### Platform
- Android (primær)
- iOS (sekundær)

### Målgruppe
- 18-45 år
- Fans af mafia-genren (GTA, Mafia-serien, Peaky Blinders)
- Spillere der søger casual gaming med dybde
- Nostalgiske browser-spil veteraner

---

# 🎭 LORE & BAGGRUND

## Prologen

> *New Porto, 1932.*
>
> *Forbudstiden er på sit højeste. Whiskey flyder i baggårde, korruption gennemsyrer byen fra gaderne til rådhuset, og fem mægtige familier kontrollerer alt af værdi.*
>
> *Du ankommer til havnen med en slidt kuffert, tomme lommer, og din afdøde onkels sidste ord i tankerne:*
>
> *"I New Porto er der to veje — den lige og den krogede. Den lige fører ingen steder. Den krogede? Den fører til toppen... eller graven."*
>
> *Du har valgt den krogede vej.*
>
> *The Docks — havnekvarteret — er det eneste sted i byen uden en familie i kontrol. Den forrige boss "forsvandt" under mystiske omstændigheder, og nu er der et magtvakuum. Dit magtvakuum.*
>
> *Start med at lette lommerne på fulde sømænd. Arbejd dig op til biltyveri. Åbn din første speakeasy. Og en dag... en dag vil hele byen hviske dit navn med frygt og respekt.*
>
> *Velkommen til New Porto. Velkommen til familien.*

---

## Din Karakter

Du er en **ung immigrant** uden navn, uden fortid, uden forbindelser. Alt du har er:

- **Ambition** — En brændende vilje til at nå toppen
- **Onklens arv** — Viden om hvordan "the life" fungerer
- **Ingenting at miste** — Hvilket gør dig farlig

### Din Rejse

```
KAPITEL 1: "Nobody"
Du er usynlig. Perfekt til at stjæle fra de rige.

KAPITEL 2: "Small-Timer"  
Folk begynder at kende dit ansigt. Du har et crew.

KAPITEL 3: "Made Man"
Du er officielt i familien. Respekt følger.

KAPITEL 4: "Capo"
Du kontrollerer et territorium. Andre arbejder for dig.

KAPITEL 5: "Underboss"
Du er højre hånd til magten.

KAPITEL 6: "Godfather"
Byen er din. Alle familier bøjer knæ.
```

---

## Verden: New Porto

**New Porto** er en fiktiv amerikansk storby i 1932, inspireret af New York, Chicago og Atlantic City.

### Distrikter

| Distrikt | Kontrolleret af | Specialitet |
|----------|-----------------|-------------|
| **The Docks** | DIG (start) | Smugling, tyverier |
| **Little Sicily** | Moretti-familien | Gambling, lånehajer |
| **Chinatown** | Chen-familien | Opium, import |
| **Irish Quarter** | O'Brien-familien | Bootlegging, boksning |
| **Red Borough** | Volkov-familien | Våben, afpresning |
| **Uptown** | DeLuca-familien | Politik, hvidvask |

### De Fem Familier

**Don Enzo Moretti** — *Little Sicily*
> "Gambling er ikke gambling når huset altid vinder."

Den ældste familie. Kontrollerer byens gambling-scene. Traditionel, hæderlig... på overfladen.

**Madam Mei-Lin Chen** — *Chinatown*
> "Tålmodighed er den skarpeste kniv."

Mystisk og kalkulerende. Hendes netværk strækker sig til Shanghai. Undervurder hende aldrig.

**Patrick "Knuckles" O'Brien** — *Irish Quarter*
> "Der er to ting jeg elsker: whiskey og en god slåskamp. I den rækkefølge."

Brutal men loyal. Hans bootlegging-operation er byens største. Hans knytnæver er legendariske.

**Sergei "The Bear" Volkov** — *Red Borough*
> "I Rusland lærer man at overleve. Her lærer man at dominere."

Nyankommen fra Moskva. Hans våbenhandel har gjort ham uundværlig — og farlig.

**Senator Marco DeLuca** — *Uptown*
> "Loven er blot et værktøj for dem der ved hvordan man bruger det."

Den mest magtfulde... og den mest korrupte. Han arbejder begge sider af loven.

---

# 🎮 GAMEPLAY

## Kernekonceptet

**Made Man** er et idle game. Det betyder:

1. **Passiv indkomst** — Dine operationer tjener penge automatisk
2. **Upgrades** — Brug penge til at tjene flere penge
3. **Progression** — Unlock nye, mere profitable aktiviteter
4. **Prestige** — Reset for permanente bonusser

### Den Grundlæggende Loop

```
┌─────────────────────────────────────────┐
│                                         │
│   TJENE  →  UPGRADE  →  TJENE MERE     │
│     ↑                        ↓          │
│     └────────────────────────┘          │
│                                         │
│   Når du når toppen: PRESTIGE           │
│   Start forfra med permanente bonusser  │
│   Gentag hurtigere og hurtigere...      │
│                                         │
└─────────────────────────────────────────┘
```

---

## Ressourcer

### 💵 Cash (Primær)
Din hovedressource. Bruges til alt.

- **Tjenes fra:** Alle kriminelle operationer
- **Bruges til:** Upgrades, unlocks, (fremtidigt: gambling, bestikkelse)

### ⭐ Prestige Points (Sekundær)
Permanente bonusser der overlever reset.

- **Tjenes fra:** Prestige reset
- **Bruges til:** Permanente multipliers

---

## Operationer

Operationer er dine indtægtskilder. De genererer cash automatisk hvert sekund.

### Tier 1: Gadeforbrydelser

| Operation | Beskrivelse | Base $/sek | Unlock |
|-----------|-------------|------------|--------|
| **Pickpocketing** | Lette lommer på turister og fulde sømænd | $1 | Gratis |
| **Car Theft** | Stjæl biler, sælg til dele eller eksport | $4 | $50 |
| **Burglary** | Bryd ind i de riges hjem når de er ude | $15 | $250 |

### Tier 2: Organiseret Kriminalitet

| Operation | Beskrivelse | Base $/sek | Unlock |
|-----------|-------------|------------|--------|
| **Speakeasy** | Din første illegale bar. Jazz, gin, og hemmeligheder | $50 | $1,000 |
| **Underground Casino** | Huset vinder altid. Altid. | $200 | $5,000 |

### Tier 3: Imperiet (Post-MVP)

| Operation | Beskrivelse | Base $/sek | Unlock |
|-----------|-------------|------------|--------|
| **Bootleg Distillery** | Producér din egen whiskey | $500 | $25,000 |
| **Protection Racket** | "Pæn butik... ville være ærgerligt hvis..." | $1,000 | $100,000 |
| **Money Laundering** | Gør beskidte penge rene | $2,500 | $500,000 |

---

## Upgrade System

Hver operation kan opgraderes til **level 10**.

### Upgrade Effekt
- Hver level **fordobler** operationens indkomst
- Upgrade cost stiger eksponentielt

### Eksempel: Pickpocketing

| Level | Income/sek | Upgrade Cost |
|-------|------------|--------------|
| 1 | $1.00 | - |
| 2 | $2.00 | $10 |
| 3 | $3.00 | $14 |
| 4 | $4.00 | $20 |
| 5 | $5.00 | $28 |
| ... | ... | ... |
| 10 | $10.00 | $289 |

---

## Prestige System: "The Succession"

Når du har tjent **$10,000 totalt**, kan du vælge at "Prestige".

### Hvad sker der?

1. **Alt resettes** — Cash, operationer, progress
2. **Du får bonus** — +25% permanent indkomst multiplier
3. **Genstart** — Men nu går alt hurtigere!

### Narrativ Forklaring

> *"Politiet blev for nærgående. FBI bankede på døren. Det var tid til at forsvinde. Du overdrager imperiet til din nevø og starter forfra i en ny by. Men denne gang har du erfaring... og forbindelser."*

### Prestige Strategien

| Prestige # | Total Bonus | Effekt |
|------------|-------------|--------|
| 0 | 1.00x | Baseline |
| 1 | 1.25x | Alt 25% hurtigere |
| 2 | 1.50x | Alt 50% hurtigere |
| 3 | 1.75x | Alt 75% hurtigere |
| 5 | 2.25x | Mere end dobbelt så hurtigt |
| 10 | 3.50x | Speedrun mode |

---

## Offline Progression

Spillet fortsætter med at tjene penge **mens du er væk**.

### Regler
- **Max tid:** 4 timer (8 timer med premium)
- **Effektivitet:** 50% af normal indkomst
- **Ved return:** Popup viser hvad du tjente

### Eksempel

> *Du lukker appen med $50/sek indkomst.*
> *Du vender tilbage efter 4 timer.*
> *Offline earnings: $50 × 4 × 3600 × 0.5 = $360,000*

---

# 📜 SPILLETS REGLER

## Grundregler

1. **Én aktiv spiller** — Ingen multiplayer i MVP
2. **Ingen pay-to-win** — Alt kan opnås gratis
3. **Permanent progress** — Prestige bonusser forsvinder aldrig
4. **Lokal save** — Data gemmes på din enhed

## Balancering

### Hurtig Start
- Første upgrade mulig inden **15 sekunder**
- Første unlock inden **2 minutter**
- Prestige mulighed inden **15 minutter**

### Langsigtet Engagement
- 10+ timers content før "endgame"
- Prestige giver grund til at spille igen
- Fremtidige updates tilføjer mere content

---

# 🔮 FREMTIDIGE FEATURES (Post-MVP)

## Version 1.1: Polish & Feel

### Offline Earnings Popup
```
┌─────────────────────────────────────┐
│                                     │
│   💰 VELKOMMEN TILBAGE, BOSS!       │
│                                     │
│   Mens du var væk tjente dit        │
│   imperium:                         │
│                                     │
│        $127,543.00                  │
│                                     │
│   [     INDSAML     ]               │
│                                     │
└─────────────────────────────────────┘
```

### Tap Animations
- Knapper "bouncer" ved tryk
- Cash counter "pulser" ved store beløb
- Confetti ved prestige

### Lyd Effekter
- "Cha-ching" ved køb
- Jazz-musik i baggrunden (valgfrit)
- "Level up" fanfare ved upgrades

---

## Version 1.2: Mere Content

### 5 Nye Operationer

| Operation | Tier | Base $/sek | Unlock |
|-----------|------|------------|--------|
| **Loan Sharking** | 2 | $100 | $2,500 |
| **Fight Club** | 2 | $300 | $10,000 |
| **Bootleg Distillery** | 3 | $500 | $25,000 |
| **Drug Network** | 3 | $1,000 | $75,000 |
| **Weapons Smuggling** | 3 | $2,000 | $200,000 |

### Achievement System

| Achievement | Krav | Belønning |
|-------------|------|-----------|
| **First Blood** | Unlock Pickpocketing | 🏆 Badge |
| **Wheelman** | Unlock Car Theft | +5% Car Theft income |
| **Made Man** | First prestige | Exclusive title |
| **Millionaire** | Earn $1,000,000 total | Gold cash icon |
| **Godfather** | Reach prestige 10 | Special theme |

---

## Version 1.3: Heat System 🔥

### Konceptet
Hver kriminel aktivitet genererer **Heat** — opmærksomhed fra politiet.

### Mekanik
```
HEAT METER: [████████░░] 80%

Ved 100%: RAZZIA!
- Tab 50% af nuværende cash
- Alle operationer pauset i 5 minutter
- ELLER: Brug Influence til at undgå
```

### Heat per Operation

| Operation | Heat/sek |
|-----------|----------|
| Pickpocketing | 0.1 |
| Car Theft | 0.3 |
| Burglary | 0.5 |
| Speakeasy | 0.2 |
| Casino | 0.4 |

### Influence (Ny Ressource)
- Bruges til at **reducere heat**
- Tjenes via **politiske operationer**
- Kan bruges til at **undgå razzia**

---

## Version 1.4: Crew System 👥

### Konceptet
Rekruttér **loyale soldater** der booster dine operationer.

### Crew Members

| Medlem | Specialitet | Effekt |
|--------|-------------|--------|
| **Vinnie "Fingers"** | Pickpocket | +50% Pickpocket income |
| **Big Tony** | Enforcer | -20% Heat fra alle operationer |
| **Slick Eddie** | Con Man | +25% til alle Tier 2 operationer |
| **Lucia** | Accountant | +10% til ALL income |

### Recruitment
- Crew members **unlockes** ved milestones
- Koster **cash** at rekruttere
- Kan **opgraderes** for bedre effekt

---

## Version 2.0: Casino Feature 🎰

### Konceptet
Brug dine penge i dit eget kasino — **gamble for at vinde (eller tabe) stort!**

### Spillene

#### 🎡 Roulette
```
┌─────────────────────────────────────┐
│                                     │
│         🎡 ROULETTE                 │
│                                     │
│   Indsats: $1,000                  │
│                                     │
│   [RØD 2x]  [SORT 2x]  [GRØN 14x] │
│                                     │
│   [  TAL 1-18 2x  ]  [TAL 19-36 2x]│
│                                     │
│   [         SPIN         ]          │
│                                     │
└─────────────────────────────────────┘
```

**Odds:**
- Rød/Sort: 48.6% chance, 2x payout
- Grøn (0): 2.7% chance, 14x payout
- Specifikt tal: 2.7% chance, 35x payout

#### 🃏 Blackjack
Klassisk 21 — slå dealeren uden at gå over.

**Regler:**
- Standard Vegas regler
- Blackjack betaler 3:2
- Dealer står på 17

#### 🎰 Slots
Pull the lever og håb på det bedste!

**Symboler:**
- 🍒🍒🍒 = 5x
- 🍋🍋🍋 = 10x
- 💎💎💎 = 50x
- 7️⃣7️⃣7️⃣ = 100x

### Gambling Limits
- **Min bet:** $100
- **Max bet:** 10% af nuværende cash
- **Daglig tab limit:** $50,000 (kan slås fra)

### Risiko vs. Belønning
> ⚠️ **ADVARSEL:** Gambling er designet til at være **underholdning**, ikke en effektiv måde at tjene penge på. Over tid vil huset altid vinde. Brug det for sjov, ikke som strategi!

---

## Version 2.1: Territories 🗺️

### Konceptet
Udvid dit imperium ved at **overtage nye distrikter**.

### Territory Map
```
┌─────────────────────────────────────┐
│                                     │
│   [THE DOCKS]  ←──  [INDUSTRIAL]   │
│       ↓                   ↓         │
│   [WATERFRONT]      [DOWNTOWN]      │
│       ↓                   ↓         │
│   [CHINATOWN]  ───→ [UPTOWN]       │
│                                     │
│   🟢 = Kontrolleret af dig          │
│   🔴 = Rival familie                │
│   ⚪ = Neutralt                     │
│                                     │
└─────────────────────────────────────┘
```

### Overtag Mekanik
1. **Scout** territoriet (koster tid)
2. **Forbered** et angreb (koster cash)
3. **Angrib** (skill-baseret minigame)
4. **Kontrollér** (ny income stream)

### Territory Bonusser

| Territorium | Bonus |
|-------------|-------|
| Industrial | +25% til alle operationer |
| Downtown | -30% Heat generation |
| Chinatown | Unlock: Opium operation |
| Uptown | Unlock: Money Laundering |

---

## Version 2.2: PvP Raids ⚔️

### Konceptet
**Asynkron PvP** — raid andre spilleres territorier mens de er offline.

### Raid Mekanik
1. **Vælg mål** fra leaderboard
2. **Send crew** på raid (de er utilgængelige i X timer)
3. **Resultat** beregnes baseret på crew power vs. forsvar
4. **Loot** eller **tab** — risiko er reel!

### Forsvar
- Invester i **sikkerhed** for at beskytte dit imperium
- Stærkere forsvar = færre succesfulde raids mod dig
- Modtag **notifikation** når du bliver raidet

### Fair Play
- Kan kun raide spillere inden for ±20% af dit power level
- Max 3 raids per dag
- "Revenge" feature: Raid tilbage!

---

## Version 3.0: Story Mode 📚

### Konceptet
Oplev **den fulde historie** om din rejse fra nobody til Godfather.

### Story Kapitler

**Kapitel 1: Ankomsten**
> Lær spillets basics gennem en narrativ tutorial.

**Kapitel 2: Første Blod**
> Din første konfrontation med en rival. Valg med konsekvenser.

**Kapitel 3: The Moretti Problem**
> Don Moretti vil have dig ud. Allier dig med Chen eller O'Brien?

**Kapitel 4: Forræderen**
> Nogen i dit crew arbejder for fjenden. Find dem.

**Kapitel 5: The Commission**
> De fem familier mødes. Dit valg: Join eller udfordr?

**Epilog: The Godfather**
> Du sidder i tronen. Men hvem kan du stole på?

### Story Valg
- Valg påvirker **hvilke alliances** du får
- Valg åbner **unikke operationer**
- Multiple endings baseret på dine beslutninger

---

## Version 3.1: Events & Seasons 🎄

### Konceptet
**Tidsbegrænsede events** med unikke belønninger.

### Event Eksempler

**🎃 Halloween: "The Witch of Chinatown"**
- Special enemy: Madam Chen's hekseri
- Unique reward: Spooky speakeasy skin

**🎄 Jul: "A Bloody Christmas"**
- Snow-covered New Porto
- Gift boxes med random loot
- Santa hat til din avatar

**🍀 St. Patrick's: "O'Brien's Revenge"**
- Double income fra bootlegging
- Green beer i din speakeasy
- Leprechaun crew member

### Season Pass
- Gratis track med basic rewards
- Premium track ($4.99) med exclusive cosmetics
- 50 tiers, 30 dage til at complete

---

# 💰 MONETIZATION (Etisk)

## Principper

1. **INGEN pay-to-win** — Alt gameplay er gratis
2. **Respekt for tid** — Ingen manipulative timers
3. **Værdi for penge** — Premium features er det værd
4. **Gennemsigtighed** — Ingen skjulte costs

## Premium Features

### "Full Experience" Bundle — $9.99
- ✅ Fjern alle reklamer
- ✅ 8 timers offline earnings (vs. 4)
- ✅ Exclusive "Founder's Fedora" cosmetic
- ✅ 500 Gold Coins
- ✅ Supporter badge

### Cosmetics (Gold Coins)
- Speakeasy skins: 100-300 coins
- Avatar items: 50-200 coins
- Cash counter themes: 100 coins

### Gold Coins
- Køb: 100 coins = $0.99
- Gratis: Daily login, achievements, prestige

---

# 📊 SUCCESS METRICS

## Spillermål

| Milestone | Forventet Tid |
|-----------|---------------|
| Første upgrade | 15 sekunder |
| Første unlock | 2 minutter |
| Alle Tier 1 unlocked | 10 minutter |
| Første prestige | 15-20 minutter |
| Alle operationer unlocked | 2-3 timer |
| Prestige 5 | 1 dag |
| Prestige 10 | 1 uge |
| "Endgame" | 2-4 uger |

## Retention Targets

| Metric | Mål |
|--------|-----|
| Day 1 Retention | >40% |
| Day 7 Retention | >20% |
| Day 30 Retention | >10% |
| Average Session | 8-15 min |
| Sessions per Day | 3-5 |

---

# 🎬 KONKLUSION

**Made Man: Idle Empire** er mere end et idle game — det er en **fantasy om magt og progression**.

Spilleren starter med ingenting og bygger et imperium. Hver upgrade føles meningsfuld. Hvert unlock åbner nye muligheder. Og når du når toppen, starter du forfra — men stærkere.

Det er essensen af idle games: **Simpelt at forstå, tilfredsstillende at mestre, umuligt at lægge fra sig.**

> *"I New Porto er der to veje — den lige og den krogede."*
>
> *Du valgte rigtigt.*

---

*Game Design Document v1.0*
*Made Man: Idle Empire*
*© 2024*
