# Task 005: Setup DI & Lifecycle

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: TASK-004
- **Estimated Time**: 30 min
- **Status**: COMPLETED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Purpose

Configure Dependency Injection in MauiProgram.cs and lifecycle hooks in App.xaml.cs.

**Why this is important:**
- Services must be accessible via DI
- App must save on background/close
- Correct lifetime management

---

## Risks

### Potential Problems
1. **Circular Dependencies**:
   - Edge case: Services that depend on each other
   - Impact: DI container fails

2. **Missing Registration**:
   - Edge case: Service not registered
   - Impact: Runtime exception

### Mitigation
- Simple dependency chain: SaveManager -> GameEngine
- Verify all registrations before build

---

## Analysis - What Needs to be Implemented

### 1. MauiProgram.cs
**Changes:**
- Register SaveManager as Singleton
- Register GameEngine as Singleton
- (Future: Register ViewModels and Pages)

### 2. App.xaml.cs
**Changes:**
- Inject IGameEngine and SaveManager
- OnSleep: Save game state
- OnResume: Calculate offline earnings (via Initialize)

---

## Dependencies Check

**Required Before Start**:
- [x] TASK-004 completed (GameEngine)

**Assumptions**:
- Standard MAUI DI container

**Blockers**: TASK-004 must be complete

---

## Implementation Guide

### Step 1: Update MauiProgram.cs

**Path**: `src/MadeMan.IdleEmpire/MauiProgram.cs`

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

### Step 2: Update App.xaml.cs

**Path**: `src/MadeMan.IdleEmpire/App.xaml.cs`

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
        // Offline earnings calculated automatically in Initialize
        // which is called from MainViewModel
    }
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. DI Verification
- App starts without DI exceptions
- IGameEngine can be injected

---

## Acceptance Criteria

- [x] SaveManager registered as Singleton
- [x] GameEngine registered as Singleton (via IGameEngine)
- [x] App.xaml.cs receives dependencies via constructor
- [x] OnSleep saves game state
- [x] Build succeeds with 0 errors

---

## Code Evaluation

### Simplification Check
- **Minimal DI**: Only necessary services
- **Singleton pattern**: Game state shared across app
- **No factory pattern**: Overkill for MVP

### Alternatives Considered

**Alternative: Scoped services**
```csharp
builder.Services.AddScoped<IGameEngine, GameEngine>();
```
**Why rejected**: Game state must persist, Singleton is correct

### Known Limitations
- No scope per page
- Acceptable for single-page app

---

## Code Quality Checklist

- [x] **KISS**: Minimal DI configuration
- [x] **Lifecycle**: Save on sleep correct
- [x] **Dependency Chain**: SaveManager -> GameEngine

---

## Design Files Reference

- **Spec Reference**: docs/MVP_Specification_MAUI.md (lines 823-889)
- **Related Tasks**: TASK-004, TASK-006

---

## Notes

- ViewModels registered in later tasks
- MainPage registered when UI is ready

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Completed**: 2024-12-25
