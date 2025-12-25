# Task 005: Setup DI & Lifecycle

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: TASK-004
- **Estimated Time**: 30 min
- **Status**: COMPLETED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Formål

Konfigurer Dependency Injection i MauiProgram.cs og lifecycle hooks i App.xaml.cs.

**Hvorfor dette er vigtigt:**
- Services skal være tilgængelige via DI
- App skal gemme ved baggrund/lukning
- Korrekt lifetime management

---

## Risici

### Potentielle Problemer
1. **Circular Dependencies**:
   - Edge case: Services der afhænger af hinanden
   - Impact: DI container fejler

2. **Missing Registration**:
   - Edge case: Service ikke registreret
   - Impact: Runtime exception

### Mitigering
- Simpel dependency chain: SaveManager -> GameEngine
- Verificer alle registrations før build

---

## Analyse - Hvad Skal Implementeres

### 1. MauiProgram.cs
**Ændringer:**
- Registrer SaveManager som Singleton
- Registrer GameEngine som Singleton
- (Fremtid: Registrer ViewModels og Pages)

### 2. App.xaml.cs
**Ændringer:**
- Inject IGameEngine og SaveManager
- OnSleep: Gem game state
- OnResume: Beregn offline earnings (via Initialize)

---

## Dependencies Check

**Krævet Før Start**:
- [x] TASK-004 completed (GameEngine)

**Antagelser**:
- Standard MAUI DI container

**Blockers**: TASK-004 skal være færdig

---

## Implementation Guide

### Step 1: Opdater MauiProgram.cs

**Sti**: `src/MadeMan.IdleEmpire/MauiProgram.cs`

```csharp
using Microsoft.Extensions.Logging;
using MadeMan.IdleEmpire.Services;

namespace MadeMan.IdleEmpire;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<SaveManager>();
        builder.Services.AddSingleton<IGameEngine, GameEngine>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
```

### Step 2: Opdater App.xaml.cs

**Sti**: `src/MadeMan.IdleEmpire/App.xaml.cs`

```csharp
using MadeMan.IdleEmpire.Services;

namespace MadeMan.IdleEmpire;

public partial class App : Application
{
    private readonly IGameEngine _gameEngine;
    private readonly SaveManager _saveManager;

    public App(IGameEngine gameEngine, SaveManager saveManager)
    {
        InitializeComponent();
        _gameEngine = gameEngine;
        _saveManager = saveManager;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    protected override void OnSleep()
    {
        base.OnSleep();
        _saveManager.Save(_gameEngine.State);
    }

    protected override void OnResume()
    {
        base.OnResume();
        // Offline earnings beregnes automatisk i Initialize
        // som kaldes fra MainViewModel
    }
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Forventet: 0 errors

### 2. DI Verification
- App starter uden DI exceptions
- IGameEngine kan injectes

---

## Acceptance Criteria

- [x] SaveManager registreret som Singleton
- [x] GameEngine registreret som Singleton (via IGameEngine)
- [x] App.xaml.cs modtager dependencies via constructor
- [x] OnSleep gemmer game state
- [x] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Minimal DI**: Kun nødvendige services
- **Singleton pattern**: Game state delt på tværs af app
- **Ingen factory pattern**: Overkill for MVP

### Alternativer overvejet

**Alternativ: Scoped services**
```csharp
builder.Services.AddScoped<IGameEngine, GameEngine>();
```
**Hvorfor fravalgt**: Game state skal persistere, Singleton er korrekt

### Kendte begrænsninger
- Ingen scope per page
- Acceptabelt for single-page app

---

## Kode Kvalitet Checklist

- [x] **KISS**: Minimal DI configuration
- [x] **Lifecycle**: Save on sleep korrekt
- [x] **Dependency Chain**: SaveManager -> GameEngine

---

## Design Files Reference

- **Spec Reference**: docs/MVP_Specification_MAUI.md (linje 823-889)
- **Related Tasks**: TASK-004, TASK-006

---

## Notes

- ViewModels registreres i senere tasks
- MainPage registreres når UI er klar

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Completed**: 2024-12-25
