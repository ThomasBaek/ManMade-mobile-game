# Made Man: Idle Empire - Implementation Plan

## Vision
Et idle/incremental mobile game hvor spilleren bygger et kriminelt imperium i 1930'ernes New Porto. Fra pickpocket til Godfather.

## Tech Stack (Locked)
| Komponent | Teknologi |
|-----------|-----------|
| Framework | .NET MAUI (.NET 10) |
| Sprog | C# |
| UI | XAML |
| Pattern | MVVM med CommunityToolkit.Mvvm |
| Storage | Preferences API (JSON) |
| Target | Android 8.0+ (API 26) |

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
MadeMan.IdleEmpire/
├── Models/
│   ├── GameState.cs
│   ├── Operation.cs
│   └── GameConfig.cs
├── ViewModels/
│   ├── MainViewModel.cs
│   └── OperationViewModel.cs
├── Views/
│   └── MainPage.xaml
├── Services/
│   ├── IGameEngine.cs
│   ├── GameEngine.cs
│   └── SaveManager.cs
└── Resources/
    ├── Styles/Theme.xaml
    └── Images/*.png
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

## Balance Tal
| Operation | Base Income | Unlock Cost |
|-----------|-------------|-------------|
| Pickpocket | $1/s | GRATIS (starter) |
| Car Theft | $4/s | $50 |
| Burglary | $15/s | $250 |
| Speakeasy | $50/s | $1,000 |
| Casino | $200/s | $5,000 |

**Prestige:** Ved $10,000 total earned -> +25% permanent multiplier

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
- Første unlock (Car Theft): < 2 minutter
- Prestige mulighed: < 15 minutter
- Build uden errors
- Kører stabilt på Android emulator
