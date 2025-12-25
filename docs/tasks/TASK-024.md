# Task 024: Update DI for Skills

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-023
- **Estimated Time**: 15 min
- **Status**: BLOCKED
- **Design Reference**: N/A
- **Requires Design Input**: NO

---

## Purpose

Register skill services in DI container.

**Why this is important:**
- All services must be available via DI
- GameEngine requires skill services
- ViewModels must be able to access skill info

---

## Analysis - What to Implement

### MauiProgram.cs (extend)

Add registrations:
- `ISkillService` → `SkillService`
- `IMilestoneService` → `MilestoneService`

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

        // Core Services
        builder.Services.AddSingleton<SaveManager>();

        // Skill Services
        builder.Services.AddSingleton<ISkillService, SkillService>();
        builder.Services.AddSingleton<IMilestoneService, MilestoneService>();

        // Game Engine (depends on skill services)
        builder.Services.AddSingleton<IGameEngine, GameEngine>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. DI Verification
- [ ] App starts without DI exceptions
- [ ] GameEngine gets skill services injected
- [ ] Services are singletons (same instance)

---

## Acceptance Criteria

- [ ] ISkillService registered
- [ ] IMilestoneService registered
- [ ] Services registered BEFORE GameEngine
- [ ] Build succeeds with 0 errors
- [ ] App starts without DI errors

---

## Notes

- Order is important: Skill services must be registered before GameEngine
- All services are Singletons to share state

---

**Task Status**: BLOCKED (waiting for TASK-023)
**Last Updated**: 2024-12-25
