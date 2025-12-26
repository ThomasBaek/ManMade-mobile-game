# 🎮 MADE MAN: IDLE EMPIRE - UI/UX REDESIGN
## Claude Code Implementation Prompt v1.0

---

# DOKUMENT FORMÅL

Dette dokument beskriver **10 UI/UX forbedringer** der skal implementeres for at løfte appen fra MVP til et professionelt, engagerende spil.

**Workflow:**
1. Claude Code læser dette dokument
2. Claude Code analyserer eksisterende kodebase
3. Claude Code opretter TASK-029 til TASK-038 (eller flere hvis nødvendigt)
4. Implementation følger etableret task-workflow
5. Hver task testes og godkendes før næste påbegyndes

**KRITISK:** Følg `Task_TEMPLATE_v1_1.md` formatet for alle nye tasks.

**Informationer**
Vi skal opbygge en ny række tasks i på samme måde som vi har bygget MVP
De steder der skal bruges grafik og de rer en promt til en AI, se om du skal lave dem tilfredsstillende med SVG. Der er et App icon i /ressources/appicon og billeder i /ressources/images Disse kan bruges hvis tilfredstillende SVG ikke kan generes af CLaude code
---

# EKSISTERENDE KONTEKST

## Nuværende Status
- **MVP er KOMPLET** - Alle 28 tasks færdige
- **Arkitektur:** .NET MAUI, MVVM, CommunityToolkit.Mvvm
- **Single-page design:** Kun MainPage.xaml (skal nu udvides)
- **Framework:** .NET 10 (net10.0-android)

## Nøglefiler at Analysere
```
Views/
  └── MainPage.xaml          ← Nuværende hovedskærm
  └── Components/
      └── SkillSelectionModal.xaml
      └── SkillDisplayComponent.xaml

ViewModels/
  └── MainViewModel.cs       ← Hovedlogik
  └── SkillViewModel.cs

Services/
  └── GameEngine.cs          ← Game loop
  └── SaveManager.cs         ← Persistence

Resources/
  └── Styles/Colors.xaml     ← Farver
  └── Styles/Styles.xaml     ← Styling
```

---

# FEATURE 1: SPLASH SCREEN

## Beskrivelse
En professionel splash screen der vises i 2-3 sekunder når appen starter.

## Design Intent
- Sæt stemningen for spillet fra første sekund
- Vis branding tydeligt
- Transition smooth til gameplay

## Visuel Specifikation
```
┌─────────────────────────────────────┐
│                                     │
│                                     │
│         [MADE MAN LOGO]             │
│                                     │
│      ━━━━━━━━━━━━━━━━━━━━━━        │
│       IDLE EMPIRE                   │
│                                     │
│         Loading...                  │
│                                     │
│                                     │
│    [1930s skyline silhouette]       │
└─────────────────────────────────────┘
```

## Teknisk Approach
- Brug native Android Splash Screen API via `MauiProgram.cs`
- Alternativt: Dedikeret `SplashPage.xaml` med timer navigation
- Fade-out animation til hovedskærm

## Grafik Assets (AI Generation Required)
**Prompt til grafik AI:**
```
Create a logo for "Made Man" - a 1930s mafia-themed mobile game. 
Style: Art Deco with gold/amber accents on dark background.
Elements: Fedora hat silhouette, tommy gun outline, "MADE MAN" text in 
elegant serif font with subtle gold gradient.
Format: PNG with transparency, 512x512px for high-res, 192x192px for splash.
Color palette: #1A1A2E (dark), #D4AF37 (gold), #8B0000 (blood red accent)
```

## Acceptance Criteria
- [ ] Splash vises ved app start (2-3 sek)
- [ ] Logo og titel tydeligt synlige
- [ ] Smooth fade transition til gameplay
- [ ] Fungerer på Android (iOS senere)

---

# FEATURE 2: NAVIGATION SYSTEM (Fra EXPANSION_ROADMAP)

## Beskrivelse
Implementer komplet navigationsstruktur med top bar og bottom navigation som specificeret i EXPANSION_ROADMAP_MadeMan_v2.md.

## Top Bar Specifikation

**Altid synlig øverst på skærmen**

```
┌─────────────────────────────────────────────────────────────┐
│  💰 $1,234,567  │  📈 +$456/s  │  📉 -$120/s  │ NET: +$336/s │
└─────────────────────────────────────────────────────────────┘
```

**Elementer:**
- **Cash:** Nuværende pengebeholdning (animeret ved ændring)
- **Income:** Brutto indkomst per sekund
- **Expenses:** Udgifter per sekund (til crew - placeholder for nu)
- **Net:** Netto indkomst (income - expenses)

**Design:**
- Kompakt højde (~60px)
- Mørk baggrund med subtle glow på tal
- Gold farve på positive værdier, rød på negative

## Bottom Navigation Specifikation

```
┌─────────────────────────────────────────────────────────────┐
│   🏠        💼        🎰        📊        ⚙️              │
│  Empire  Org Crime  Casino   Skills   Settings            │
│          [🔒]       [🔒]     [🔒]                          │
└─────────────────────────────────────────────────────────────┘
```

**5 Tabs:**
| Tab | Ikon | Beskrivelse | Unlock |
|-----|------|-------------|--------|
| Empire | 🏠 | Operations og prestige | Start |
| Organized Crime | 💼 | Crew og missioner | Prestige 2 |
| Casino | 🎰 | Gambling minigames | Prestige 1 |
| Skills | 📊 | Skill tree | Prestige 1 |
| Settings | ⚙️ | Indstillinger | Start |

**UX Note:** Låste tabs skal være visuelt "disabled" med lås-ikon og tooltip ved tap.

## Teknisk Implementation

### Option A: Shell Navigation (Anbefalet)
```xml
<Shell>
    <TabBar>
        <ShellContent Route="Empire" ... />
        <ShellContent Route="OrgCrime" ... />
        <!-- etc. -->
    </TabBar>
</Shell>
```

### Option B: Custom Tab Control
Mere kontrol over styling, men mere arbejde.

**Anbefaling:** Start med Shell, tilpas styling via CustomRenderers hvis nødvendigt.

## Acceptance Criteria
- [ ] Top bar viser Cash, Income, Expenses, Net
- [ ] Top bar opdateres live (via binding)
- [ ] Bottom navigation med 5 tabs
- [ ] Empire og Settings altid tilgængelige
- [ ] Andre tabs låst med visuelt feedback
- [ ] Tab unlock baseret på prestige level
- [ ] Navigation fungerer smooth

---

# FEATURE 3: WELCOME BACK / OFFLINE EARNINGS SCREEN

## Beskrivelse
En dedikeret skærm eller modal der vises ved app-åbning og fortæller spilleren hvad der skete mens de var væk.

## Design Intent
- Skab "wow"-moment ved tilbagevenden
- Vis offline earnings dramatisk
- Motivér spilleren til at fortsætte

## Visuel Specifikation

```
┌─────────────────────────────────────┐
│                                     │
│       🌙 VELKOMMEN TILBAGE, BOSS    │
│                                     │
│   Mens du var væk i 4t 23m          │
│   har dit imperium tjent:           │
│                                     │
│        💰 $127,543.00               │
│        ~~~~~~~~~~~~~~~~             │
│                                     │
│   📈 Income rate: $50/sek           │
│   ⏱️ Tid væk: 4 timer               │
│   📊 Efficiency: 50%                │
│                                     │
│   ┌─────────────────────────────┐   │
│   │      💰 INDSAML ALT         │   │
│   └─────────────────────────────┘   │
│                                     │
│         [Tryk for at fortsætte]     │
│                                     │
└─────────────────────────────────────┘
```

## Flow
1. App åbnes → Beregn offline earnings (allerede implementeret i GameEngine)
2. Hvis offline earnings > 0 → Vis Welcome Back modal
3. Spilleren kan kun lukke ved at trykke "Indsaml" eller tab i bund navigation
4. Cash tilføjes med animation

## Teknisk Notes
- Brug eksisterende `_saveManager.CalculateOfflineEarnings()` resultat
- Gem `lastOpenTime` for at beregne tid væk
- Show modal KUN hvis væk mere end 1 minut

## Acceptance Criteria
- [ ] Modal vises ved app-åbning (hvis earnings > 0)
- [ ] Viser korrekt offline earnings beløb
- [ ] Viser tid væk
- [ ] "Indsaml" knap tilføjer beløb til cash
- [ ] Kan også lukkes via bottom navigation
- [ ] Vises IKKE hvis app var åben kort tid siden

---

# FEATURE 4: SMART OPERATION VISIBILITY

## Beskrivelse
Operations der ikke er købt endnu, skal kun vises når spilleren har råd til at købe dem (eller er tæt på).

## Nuværende Problem
- Alle 5+ operations vises altid
- Nye spillere overhelmes af locked content
- Ingen sense of discovery

## Ny Logik
```
Vis operation HVIS:
  - Allerede unlocked (IsUnlocked = true)
  - ELLER: Cash >= UnlockCost * 0.5 (inden for 50% af prisen)
  - ELLER: Næste locked operation i tier-rækkefølge
```

**Progressive Discovery:**
- Start: Kun Pickpocket synlig
- Når Cash nærmer sig Car Theft pris → Car Theft fades ind
- Osv.

## Visuel Feedback for "Coming Soon"
```
┌───────────────────────────────────┐
│ 🔒 Car Theft                      │
│ Unlock: $25 (Du har: $12)         │
│ [████████░░░░░░░░] 48%            │
└───────────────────────────────────┘
```

## Acceptance Criteria
- [ ] Låste operations kun synlige når tæt på unlock
- [ ] Progress bar viser hvor tæt spilleren er
- [ ] Smooth fade-in animation når operation bliver synlig
- [ ] Tier 1 operations prioriteres over højere tiers

---

# FEATURE 5: KOMPAKT UI LAYOUT

## Beskrivelse
Reducer størrelsen på UI elementer så der er plads til mere indhold på skærmen.

## Nuværende Problem
- Store operation cards tager meget plads
- Kun 3-4 operations synlige ad gangen
- Prestige panel er dominerende

## Målsætning
- 5-6 operations synlige uden scroll
- Mere "information density"
- Stadig læsbart og tappable (min 44px touch targets)

## Specifikke Ændringer

### Operation Cards (Før vs. Efter)
```
BEFORE:                          AFTER:
┌────────────────────────┐       ┌────────────────────────┐
│ 🎭 Pickpocket          │       │ 🎭 Pickpocket  +$1/s   │
│                        │       │ Lvl 3 │ [$50 Upgrade] │
│ Income: +$1.50/sek     │       └────────────────────────┘
│ Level: 3               │
│                        │
│ [    UPGRADE $50    ]  │
└────────────────────────┘

Height: ~120px            →      Height: ~60px
```

### Header (Kompakt)
- Reducer fra ~80px til ~50px
- Kombiner Prestige info i header

### Skill Display
- Horizontal strip med ikoner
- Ikke vertikal liste

## Acceptance Criteria
- [ ] Operation cards reduceret til ~60px højde
- [ ] Header komprimeret
- [ ] 5+ items synlige uden scroll på standard telefon
- [ ] Touch targets stadig minimum 44px

---

# FEATURE 6: DYSTERT GANGSTER TEMA

## Beskrivelse
Giv appen en mørkere, mere atmosfærisk æstetik der matcher 1930s mafia-temaet.

## Nuværende Problem
- For "generisk app" look
- Mangler stemning og karakter
- Ikke immersive

## Ny Farvepalet
```
Primary Colors:
  - Background:     #0D0D0D (næsten sort)
  - Surface:        #1A1A2E (mørk navy)
  - Card:           #252540 (lidt lysere)
  
Accent Colors:
  - Gold:           #D4AF37 (cash, success)
  - Blood Red:      #8B0000 (danger, prestige)
  - Smoke:          #4A4A4A (disabled, secondary)
  
Text:
  - Primary:        #FFFFFF (hvid)
  - Secondary:      #A0A0A0 (grå)
  - Gold Text:      #FFD700 (speciel)
```

## Visuelle Elementer

### Teksturer (AI Generation)
**Prompt til grafik AI:**
```
Seamless tileable texture for mobile game background.
Style: Dark worn leather or vintage paper with subtle grain.
Color: Very dark (#0D0D0D to #1A1A2E range)
Subtle elements: Faint cigar smoke wisps, art deco patterns at very low opacity
Format: PNG, 512x512, seamless tileable
```

### Ikoner (AI Generation)
**Prompt til grafik AI:**
```
Icon set for 1930s mafia mobile game. Art Deco style, gold/amber on dark.
Icons needed:
1. Fedora hat (operations/empire)
2. Tommy gun (organized crime)  
3. Poker chips (casino)
4. Rising graph with dollar (skills/income)
5. Gear with cigar (settings)
Format: SVG preferred, 64x64px PNG fallback, gold (#D4AF37) on transparent
```

## Effekter
- Subtle vignette på kanter
- Gold glow på vigtige tal
- Smoke particle effect i baggrund (valgfrit, performance først)

## Acceptance Criteria
- [ ] Ny farvepalet implementeret i Colors.xaml
- [ ] Mørk baggrund på alle skærme
- [ ] Gold accenter på cash og success states
- [ ] Konsistent styling på alle komponenter

---

# FEATURE 7: PRESTIGE MODAL FORBEDRING

## Beskrivelse
Prestige modal skal kunne lukkes (udskydes til senere) og genåbnes via prestige level badge.

## Nuværende Problem
- Modal kan ikke lukkes uden at prestige
- Spilleren kan ikke fortsætte med at spare op
- Ingen indikation af tilgængelig prestige når lukket

## Ny Flow
```
1. TotalEarned når threshold → Prestige modal vises
2. Spilleren kan:
   a) Trykke "Prestige Nu" → Normal prestige flow
   b) Trykke "Senere" (X) → Modal lukkes
3. Når lukket:
   - Badge på prestige level blinker/glows
   - Tap på badge genåbner modal
```

## UI Ændringer

### Prestige Badge (I header eller tab)
```
Normal:          Prestige Tilgængelig:
┌─────────┐      ┌─────────┐
│  P: 3   │  →   │ ✨ P: 3 │  (glow animation)
└─────────┘      └─────────┘
```

### Modal med Luk-knap
```
┌─────────────────────────────────────┐
│ ✕                                   │  ← Luk knap (øverst højre)
│                                     │
│       ⭐ PRESTIGE AVAILABLE         │
│                                     │
│   Reset for +25% permanent bonus    │
│                                     │
│   Current bonus: 1.50x              │
│   After prestige: 1.75x             │
│                                     │
│   ┌─────────────────────────────┐   │
│   │      PRESTIGE NOW           │   │
│   └─────────────────────────────┘   │
│                                     │
│        [Måske senere]               │
│                                     │
└─────────────────────────────────────┘
```

## Teknisk Notes
- Tilføj `IsPrestigeAvailableDismissed` flag til GameState
- Reset flag ved næste threshold eller ved prestige
- Badge animation via MAUI Animation API

## Acceptance Criteria
- [ ] Prestige modal har "Luk" knap
- [ ] Modal forsvinder ved luk
- [ ] Badge i header indikerer tilgængelig prestige
- [ ] Tap på badge genåbner modal
- [ ] Badge animation (pulse/glow)

---

# FEATURE 8: TITEL SYSTEM

## Beskrivelse
Spilleren starter med laveste titel og avancerer gennem prestige levels til "Godfather".

## Titler og Progression

| Prestige Level | Titel | Beskrivelse |
|----------------|-------|-------------|
| 0 | Street Punk | Du er ingen... endnu |
| 1 | Pickpocket | Du stjæler til dagen og vejen |
| 2 | Thug | Folk begynder at frygte dig |
| 3 | Enforcer | Du driver penge ind for andre |
| 5 | Soldato | Du er nu en del af familien |
| 7 | Caporegime | Du leder dit eget crew |
| 10 | Underboss | Kun én over dig nu |
| 15 | Consigliere | Rådgiver til de mægtige |
| 20+ | Godfather | Du ER familien |

## UI Integration
- Vis titel i header ved siden af prestige level
- Titel ændres med animation ved prestige
- Achievement-lignende popup ved ny titel

```
┌─────────────────────────────────────┐
│ 🎩 ENFORCER                 P: 3   │
│ "Du driver penge ind for andre"    │
└─────────────────────────────────────┘
```

## Ved Prestige (Ny Titel)
```
┌─────────────────────────────────────┐
│                                     │
│        🎉 TITLE UNLOCKED!           │
│                                     │
│         ⭐ SOLDATO ⭐                │
│                                     │
│   "Du er nu en del af familien"    │
│                                     │
│         [FORTSÆT]                   │
│                                     │
└─────────────────────────────────────┘
```

## Acceptance Criteria
- [ ] Titel vises i header
- [ ] Titel matcher prestige level
- [ ] Popup ved ny titel unlock
- [ ] Alle 9 titler implementeret
- [ ] Progression føles meningsfuld

---

# FEATURE 9: RESET FUNKTION (SETTINGS TAB)

## Beskrivelse
I Settings tab skal spilleren kunne nulstille hele sit spil og starte forfra.

## Sikkerhedsflow
Reset er permanent og farligt → Kræv bekræftelse!

```
STEP 1: Tap "Reset Game" i Settings

STEP 2: Advarsel vises
┌─────────────────────────────────────┐
│            ⚠️ ADVARSEL              │
│                                     │
│   Er du sikker på du vil           │
│   slette ALT progress?              │
│                                     │
│   Dette kan IKKE fortrydes!         │
│                                     │
│   Prestige: 5 → 0                   │
│   Total Earned: $15M → $0          │
│   Titel: Soldato → Street Punk      │
│                                     │
│  [ANNULLER]       [JA, SLET ALT]   │
│                   (rød, farlig)     │
└─────────────────────────────────────┘

STEP 3: Hvis bekræftet → Skriv "DELETE" for at bekræfte

STEP 4: Alt slettes, app genstarter med tutorial
```

## Settings Page Layout
```
┌─────────────────────────────────────┐
│           ⚙️ SETTINGS               │
├─────────────────────────────────────┤
│                                     │
│  Sound Effects          [ON/OFF]    │
│  ──────────────────────────────     │
│  Music                  [ON/OFF]    │
│  ──────────────────────────────     │
│  Notifications          [ON/OFF]    │
│  ──────────────────────────────     │
│                                     │
│  ═══════════════════════════════    │
│                                     │
│  Version: 1.0.0                     │
│  ──────────────────────────────     │
│  Credits                      >     │
│  ──────────────────────────────     │
│                                     │
│  ═══════════════════════════════    │
│                                     │
│  ┌─────────────────────────────┐    │
│  │   🗑️ RESET GAME             │    │
│  │   (Slet alt progress)       │    │
│  └─────────────────────────────┘    │
│           (rød farve)               │
│                                     │
└─────────────────────────────────────┘
```

## Teknisk Implementation
- Clear alle Preferences
- Nulstil GameState til default
- Navigate til splash/tutorial

## Acceptance Criteria
- [ ] Settings page oprettet
- [ ] Reset knap synlig (rød, farlig)
- [ ] Bekræftelsesdialog med preview af tab
- [ ] Sekundær bekræftelse (skriv DELETE)
- [ ] Komplet reset af alt data
- [ ] App genstarter efter reset

---

# FEATURE 10: APP IKON

## Beskrivelse
Design og implementer et professionelt app-ikon der repræsenterer spillet.

## Design Retning
- 1930s mafia æstetik
- Genkendelig på lille størrelse
- Skiller sig ud i app store

## Koncept
**Primær:** Fedora hat med guldkant på mørk baggrund
**Alternativ:** Dollar-tegn i art deco stil

## AI Generation Prompt
```
Mobile app icon for "Made Man" - a 1930s mafia idle game.
Main element: Classic fedora hat viewed from front angle, slightly tilted
Style: Minimalist art deco, strong silhouette
Colors: 
  - Background: Dark navy (#1A1A2E) with subtle gradient
  - Hat: Black with gold (#D4AF37) trim/band
  - Subtle shadow underneath
Shape: Rounded square (standard app icon)
Sizes needed: 1024x1024 (master), 512x512, 192x192, 144x144, 96x96, 72x72, 48x48
Format: PNG with no transparency (filled background)
Additional: Add subtle texture/grain for vintage feel
```

## Implementering
- Placer ikoner i `Resources/AppIcon/`
- Opdater `.csproj` med korrekte MauiIcon referencer
- Test på fysisk enhed (emulator viser ikke altid korrekt)

## Acceptance Criteria
- [ ] Ikon designet i alle størrelser
- [ ] Implementeret i MAUI projekt
- [ ] Synlig på emulator
- [ ] Fungerer på fysisk enhed
- [ ] Matcher app æstetik

---

# IMPLEMENTATION PRIORITERING

## Anbefalet Rækkefølge (Dependencies)

```
WAVE 1: Foundation (Kan paralleliseres delvist)
├── TASK-029: Dystert tema/farver     ← Alt andet afhænger af dette
├── TASK-030: App ikon                ← Kan laves parallelt

WAVE 2: Navigation
├── TASK-031: Bottom Navigation       ← Kræver TASK-029
├── TASK-032: Top Bar                 ← Kræver TASK-029
├── TASK-033: Settings Page           ← Kræver TASK-031

WAVE 3: Screens
├── TASK-034: Splash Screen           ← Kræver TASK-029, TASK-030
├── TASK-035: Welcome Back Modal      ← Kræver TASK-032

WAVE 4: Gameplay Polish
├── TASK-036: Smart Operation Vis     ← Kræver TASK-032
├── TASK-037: Kompakt UI              ← Kræver TASK-036
├── TASK-038: Prestige Modal UX       ← Kræver TASK-032

WAVE 5: Features
├── TASK-039: Titel System            ← Kræver TASK-038
├── TASK-040: Reset Funktion          ← Kræver TASK-033
```

## Estimeret Tid
| Task | Feature | Estimat |
|------|---------|---------|
| TASK-029 | Dystert tema | 1-2 timer |
| TASK-030 | App ikon | 30 min (+ AI generation) |
| TASK-031 | Bottom Navigation | 2-3 timer |
| TASK-032 | Top Bar | 1 time |
| TASK-033 | Settings Page | 1 time |
| TASK-034 | Splash Screen | 1 time |
| TASK-035 | Welcome Back Modal | 1.5 timer |
| TASK-036 | Smart Operation Vis | 1 time |
| TASK-037 | Kompakt UI | 2 timer |
| TASK-038 | Prestige Modal UX | 1 time |
| TASK-039 | Titel System | 1.5 timer |
| TASK-040 | Reset Funktion | 1 time |

**Total estimat:** ~14-17 timer

---

# GRAFIK AI INSTRUKTIONER

## Workflow for Claude Code

Når Claude Code støder på grafik der kræves:

1. **Identificer behovet** - Hvad skal laves?
2. **Skriv AI prompt** - Brug template fra dette dokument
3. **Output prompt til bruger** - I markdown format
4. **Marker som blocker** - Task kan ikke fuldføres uden asset
5. **Fortsæt med andet** - Parallelt arbejde

## Eksempel Output til Bruger
```markdown
## ⚠️ GRAFIK ASSET KRÆVET

Følgende grafik skal genereres før task kan fuldføres:

### Asset: Splash Screen Logo
**AI Prompt (kopier til DALL-E/Midjourney):**
```
Create a logo for "Made Man" - a 1930s mafia-themed mobile game...
[fuld prompt]
```

**Specifikationer:**
- Filnavn: `splash_logo.png`
- Størrelse: 512x512px
- Format: PNG med transparency
- Placering: `Resources/Images/`

**Status:** ⏸️ Venter på asset
```

---

# NOTER TIL CLAUDE CODE

## Før Implementation

1. **Analyser eksisterende kodebase**
   - Bekræft arkitektur stadig matcher docs
   - Find eksisterende styling patterns
   - Identificer integration points

2. **Opret alle TASK filer først**
   - TASK-029 til TASK-040
   - Med korrekte dependencies
   - Følg Task_TEMPLATE_v1_1.md

3. **Start med TASK-029 (Tema)**
   - Alt andet afhænger af dette
   - Mindre risiko for rework

## Under Implementation

1. **Test HVER ændring visuelt**
   - Kør i emulator ofte
   - Screenshot før/efter

2. **Commit ofte**
   - Efter hver fungerende feature
   - Beskrivende commit messages

3. **Opdater docs**
   - STATE.md efter hver task
   - TASKS.md med status

## Kvalitetskrav

- [ ] Alle farver fra ny palette
- [ ] Konsistent spacing/padding
- [ ] Ingen hardcoded værdier (brug StaticResource)
- [ ] Fungerer på forskellige skærmstørrelser
- [ ] Performance: Ingen lag ved animation

---

# KRITISKE FORBEDRINGER JEG ANBEFALER

Som en del af min analyse har jeg identificeret følgende forbedringer:

## 1. Feature 2 Udvidelse: Tab Content Pages
**Problem:** Du beskriver navigation, men hvad er på hver tab?
**Løsning:** Tilføj placeholder pages for Casino, Skills, Org Crime så navigation er funktionel selv om content kommer senere.

## 2. Feature 3 Enhancement: First-Time User Experience
**Problem:** Welcome Back virker kun for returbrugere.
**Løsning:** Tilføj kort tutorial/intro for helt nye spillere (kan være optional).

## 3. Feature 5 Responsivitet
**Problem:** "Kompakt UI" er defineret i pixels.
**Løsning:** Brug relative sizing (em, %) så det skalerer på tablets.

## 4. Feature 8 Gameplay Impact
**Problem:** Titler er kun kosmetiske.
**Løsning:** Overvej at titler giver små bonusser (f.eks. +1% income per titel) for mere meningsfuldhed.

## 5. Manglende Feature: Haptic Feedback
**Problem:** Mobile spil føles bedre med vibration.
**Løsning:** Tilføj kort vibration ved upgrade, prestige, og milestone (kan være i polish).

---

# START KOMMANDO

Når du er klar, sig til mig:

```
Klar til UI Redesign - Start TASK-029
```

Jeg starter så med:
1. Analysere nuværende kodebase
2. Oprette alle TASK filer (029-040)
3. Begynde implementation af TASK-029

---

**Document Version:** 1.0
**Last Updated:** 2024-12-26
**For:** Claude Code Implementation
