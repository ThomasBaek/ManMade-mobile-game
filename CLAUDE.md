# CLAUDE.md - Made Man: Idle Empire

## Projekt Oversigt
Dette er et **idle/incremental mobile game** bygget med .NET MAUI.
Spilleren bygger et kriminelt imperium i 1930'ernes New Porto.

**Genre:** Idle / Incremental Game (Cookie Clicker-stil)
**Tema:** 1930'er Mafia / Noir
**Inspiration:** Omerta (browser game), Adventure Capitalist

## Tech Stack
| Komponent | Teknologi |
|-----------|-----------|
| Framework | .NET 10 MAUI |
| Sprog | C# |
| UI | XAML |
| Pattern | MVVM med CommunityToolkit.Mvvm |
| Storage | Preferences API (JSON) |
| Target | Android 8.0+ (API 26) |

## Projekt Struktur
```
MadeMan.IdleEmpire/
├── Models/
│   ├── GameState.cs        # Spillets tilstand (cash, prestige, operations)
│   ├── Operation.cs        # Unified crime/business model
│   └── GameConfig.cs       # Alle balance-konstanter
├── ViewModels/
│   ├── MainViewModel.cs    # Hoved ViewModel med game loop
│   └── OperationViewModel.cs # ViewModel for hver operation
├── Views/
│   └── MainPage.xaml       # ENESTE side - alt gameplay
├── Services/
│   ├── IGameEngine.cs      # Interface for game logic
│   ├── GameEngine.cs       # Core tick logic, unlock, upgrade
│   └── SaveManager.cs      # Gem/hent til Preferences
└── Resources/
    ├── Styles/
    │   └── Theme.xaml      # Farver og styles
    └── Images/
        └── *.png           # Ikoner
```

## Kode Konventioner
- **Navngivning:** PascalCase for public, _camelCase for private fields
- **Properties:** Brug [ObservableProperty] fra CommunityToolkit.Mvvm
- **Commands:** Brug [RelayCommand] attribut
- **DI:** Registrer alle services som Singleton i MauiProgram.cs
- **Async:** Brug async/await for I/O operationer

## Core Gameplay Loop
```
START → Cash genereres automatisk ($/sek)
      → Brug cash til Upgrade eller Unlock
      → Income stiger
      → Ved $10,000 total: Prestige mulighed
      → Reset med +25% permanent bonus
      → Gentag hurtigere
```

## Vigtige Tal (Balance)
| Operation | Base Income | Unlock Cost |
|-----------|-------------|-------------|
| Pickpocket | $1/s | GRATIS (starter) |
| Car Theft | $4/s | $50 |
| Burglary | $15/s | $250 |
| Speakeasy | $50/s | $1,000 |
| Casino | $200/s | $5,000 |

**Prestige:** Ved $10,000 total earned → +25% permanent multiplier

## Build Kommandoer
```bash
# Build til Android
dotnet build -f net10.0-android

# Kør på emulator (emulator skal køre først)
dotnet build -f net10.0-android -t:Run

# Clean build
dotnet clean && dotnet build -f net10.0-android

# Publish APK
dotnet publish -f net10.0-android -c Release
```

## Implementerings Rækkefølge
1. **Models** - GameState.cs, Operation.cs, GameConfig.cs
2. **Services** - SaveManager.cs, IGameEngine.cs, GameEngine.cs
3. **ViewModels** - OperationViewModel.cs, MainViewModel.cs
4. **Resources** - Theme.xaml (farver)
5. **Views** - MainPage.xaml (redesign fra scratch)
6. **DI Setup** - MauiProgram.cs
7. **Lifecycle** - App.xaml.cs (save on sleep)
8. **Test** - Kør på emulator

## Status Checklist
- [x] Projekt oprettet
- [x] NuGet packages installeret (TASK-001) ✅
- [x] Mappestruktur klar
- [x] Models implementeret (TASK-002) ✅
- [x] Services implementeret (TASK-003, TASK-004) ✅
- [ ] DI & Lifecycle setup (TASK-005)
- [ ] Theme/farver sat op (TASK-006)
- [ ] ViewModels implementeret (TASK-007, TASK-008)
- [ ] UI implementeret (TASK-009)
- [ ] Ikoner tilføjet (TASK-010)
- [ ] Game loop test (TASK-011)
- [ ] Prestige test (TASK-012)
- [ ] Save/Load test (TASK-013)
- [ ] Offline earnings test (TASK-014)

**Næste task:** TASK-005 (Setup DI & Lifecycle)

## Farve Palette
```
Background:    #1A1A2E (mørk blå)
Surface:       #16213E (lidt lysere)
Primary:       #E94560 (rød accent)
Gold:          #FFD700 (cash farve)
Success:       #4ADE80 (grøn - kan købe)
Locked:        #4A5568 (grå - kan ikke købe)
TextPrimary:   #FFFFFF
TextSecondary: #8892A0
```

## Fejlfinding

### Build fejler
```bash
dotnet clean
dotnet restore
dotnet build -f net10.0-android
```

### Emulator ikke fundet
```bash
# List devices
adb devices

# Hvis tom, start emulator fra Android Studio
# eller: emulator -list-avds && emulator -avd <navn>
```

### Hot Reload virker ikke
Genstart appen helt: `dotnet build -f net8.0-android -t:Run`

## Design Dokumenter
Se projektets andre markdown filer for:
- **GAME_DESIGN_DOCUMENT.md** - Lore, regler, fremtidige features
- **CLAUDE_CODE_IMPLEMENTATION_GUIDE.md** - Detaljeret kode spec
- **MVP_Specification_MAUI.md** - Original teknisk spec

---

## Task Workflow (Claude Code)

### Kommandoer

| Kommando | Beskrivelse |
|----------|-------------|
| `Start TASK-XXX` | Start specifik task |
| `Godkend TASK-XXX` | Godkend og commit færdig task |
| `Aendr TASK-XXX: [beskrivelse]` | Bed om ændringer |
| `Status` | Få overblik over nuværende state |
| `Naeste` | Hvad er næste task? |

### Task Flow

**1. Bruger siger:** `Start TASK-XXX`

**2. Claude implementerer:**
- Læser task specifikation
- Implementerer koden
- Stiller spørgsmål hvis nødvendigt

**3. Claude tester INDEN færdigmelding:**
- Kører `dotnet build -f net10.0-android`
- Verificerer 0 errors, 0 warnings
- Code review mod task specifikation
- Tjekker at alle acceptance criteria er opfyldt

**4. Claude præsenterer for review (STOPPER HER - INGEN COMMIT):**
- Hvad er implementeret (filer, metoder)
- Test resultater (build output)
- Code review mod spec
- **Hvad bruger skal teste** (konkrete test instruktioner)
- Venter på godkendelse

**5. Bruger tester:**
- Kører tests/build selv
- Code review i IDE
- Emulator test (hvis UI)

**6. Bruger godkender:** `Godkend TASK-XXX`

**7. FØRST NU committer Claude:**
- Git commit med task ID
- Opdaterer TASK-XXX.md (status: COMPLETED)
- Opdaterer TASKS.md (næste task READY)
- Opdaterer STATE.md
- Melder næste task klar

### Projekt Management Filer
```
docs/
├── PLAN.md           # Arkitektur + roadmap
├── TASKS.md          # Task oversigt med status
├── STATE.md          # Nuværende projekt status
├── NOTES.md          # Scratchpad til noter
└── tasks/            # Individuelle task-filer
    ├── TASK-001.md
    ├── TASK-002.md
    └── ...
```

---

## Code Principper

### KISS
- Simpleste løsning der virker
- Ingen over-engineering

### YAGNI
- Kun det nødvendige NU
- Ingen spekulative features

### SOLID
- Single Responsibility
- Open-Closed
- Liskov Substitution
- Interface Segregation
- Dependency Inversion

---

## Git Commit Convention
```
[TASK-XXX]: Kort beskrivelse

- Detalje 1
- Detalje 2
```

---

## Kvalitetskrav Før Task Godkendelse
1. `dotnet build -f net10.0-android` succeeds
2. Ingen nye warnings
3. Manuel test i emulator (hvis UI)
4. Alle acceptance criteria opfyldt
5. Kode evaluering udfyldt i task fil

---

## Kontakt / Notes
- Primært mål: Fungerende MVP på Android
- Sekundært: iOS support (senere)
- Fokus: Simpelt men engagerende gameplay
- Ingen server/backend i MVP
