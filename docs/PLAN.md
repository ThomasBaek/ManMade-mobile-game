# Made Man: Idle Empire - Implementation Plan

## Vision
Et idle/incremental mobile game hvor spilleren bygger et kriminelt imperium i 1930'ernes New Porto. Fra pickpocket til Godfather.

## Current Status
**Version 1.0 (MVP)** - COMPLETE
**Version 1.1 (Economy Rebalancing + Tests)** - COMPLETE

## Tech Stack (Locked)
| Komponent | Teknologi |
|-----------|-----------|
| Framework | .NET MAUI (.NET 10) |
| Sprog | C# |
| UI | XAML |
| Pattern | MVVM med CommunityToolkit.Mvvm |
| Storage | Preferences API (JSON) |
| Target | Android 8.0+ (API 26) |
| Testing | xUnit + Moq |

## Arkitektur Beslutninger

### Single Page Design
- AL gameplay på MainPage.xaml
- Ingen navigation mellem sider i MVP
- Simplicitet over features

### Unified Operation Model
- Crimes og Businesses bruger samme Operation klasse
- Ingen separate modeller for forskellige typer
- Nemmere at udvide senere

### Game Loop
- IDispatcherTimer til 10 ticks/sekund
- GameEngine håndterer al logik
- ViewModel er kun presentation layer

## Projekt Struktur
```
src/
├── MadeMan.IdleEmpire/              # MAUI App (net10.0-android)
│   ├── Services/                    # GameEngine, SaveManager
│   ├── ViewModels/                  # 4 ViewModels
│   ├── Views/                       # 4 pages + 5 components
│   └── Utilities/                   # NumberFormatter
│
├── MadeMan.IdleEmpire.Core/         # Shared Library (net10.0)
│   ├── Models/                      # 8 model files
│   └── Services/                    # SkillService, MilestoneService, etc.
│
tests/
└── MadeMan.IdleEmpire.Tests/        # xUnit Tests (net10.0)
    ├── SmokeTests.cs                # 7 tests
    ├── SkillTestBase.cs             # Base class
    └── Skills/                      # 142 skill tests
```

## Kode Standarder

### Navngivning
- PascalCase: Public members, classes, methods
- _camelCase: Private fields
- camelCase: Local variables, parameters

### CommunityToolkit.Mvvm
- [ObservableProperty] for alle bindable properties
- [RelayCommand] for alle commands
- Partial classes for source generators

### DI Registration
- Alle Services som Singleton
- Alle ViewModels som Transient
- Alle Pages som Transient

### Error Handling
- Try-catch i alle public methods der kan fejle
- Log til Debug.WriteLine i MVP
- Graceful degradation - app må ALDRIG crashe

## Faser

### Phase 1: Foundation (TASK-001 til TASK-005)
- Project setup & package installation
- Models implementation
- SaveManager service
- GameEngine service
- DI & Lifecycle setup

### Phase 2: UI (TASK-006 til TASK-010)
- Theme & colors
- OperationViewModel
- MainViewModel
- MainPage.xaml
- Icons

### Phase 3: Gameplay (TASK-011 til TASK-012)
- Game loop verification
- Prestige system verification

### Phase 4: Persistence (TASK-013 til TASK-014)
- Save/load testing
- Offline earnings testing

### Phase 5: Polish (TASK-015 til TASK-017)
- UI polish
- Bug fixes
- Final testing

## Balance Tal (v1.1)
| Operation | Base Income | Unlock Cost |
|-----------|-------------|-------------|
| Pickpocket | $0.5/s | GRATIS (starter) |
| Car Theft | $2/s | $75 |
| Burglary | $8/s | $400 |
| Speakeasy | $30/s | $2,000 |
| Casino | $100/s | $10,000 |

**Prestige:** Ved $25,000 total earned -> +20% permanent multiplier
**Offline Efficiency:** 40% (was 50%)

## Ikke i MVP (YAGNI)
- Heat system
- Crew members
- Territories
- PvP/Raids
- Notifications
- Ads/IAP
- Cloud save
- Achievements
- Story/narrative
- Animationer
- Lyd

## Success Metrics
- Første upgrade: < 15 sekunder
- Første unlock (Car Theft): < 3 minutter (v1.1)
- Prestige mulighed: < 30-45 minutter (v1.1)
- Build uden errors
- Kører stabilt på Android emulator
- 149 unit tests passing (v1.1)

## Test Coverage (v1.1)
| Category | Tests |
|----------|-------|
| Smoke tests | 7 |
| Income skills | 37 |
| Operation skills | 12 |
| Offline skills | 21 |
| Prestige skills | 18 |
| Skill stacking | 14 |
| Prestige reset | 7 |
| Edge cases | 16 |
| **Total** | **149** |

## Version History
| Version | Focus | Status |
|---------|-------|--------|
| 1.0 | MVP - Core gameplay | COMPLETE |
| 1.1 | Economy rebalancing + Tests | COMPLETE |
| 1.2 | Crew & Mission System | PLANNED |
