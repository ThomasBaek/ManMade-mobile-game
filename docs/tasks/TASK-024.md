# Task 024: Update DI for Skills

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-023
- **Estimated Time**: 15 min
- **Status**: BLOCKED
- **Design Reference**: N/A
- **Requires Design Input**: NO

---

## Formål

Registrer skill services i DI container.

**Hvorfor dette er vigtigt:**
- Alle services skal være tilgængelige via DI
- GameEngine kræver skill services
- ViewModels skal kunne tilgå skill info

---

## Analyse - Hvad Skal Implementeres

### MauiProgram.cs (udvid)

Tilføj registreringer:
- `ISkillService` → `SkillService`
- `IMilestoneService` → `MilestoneService`

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
- [ ] App starter uden DI exceptions
- [ ] GameEngine får skill services injected
- [ ] Services er singletons (samme instans)

---

## Acceptance Criteria

- [ ] ISkillService registreret
- [ ] IMilestoneService registreret
- [ ] Services registreret FØR GameEngine
- [ ] Build succeeds med 0 errors
- [ ] App starter uden DI fejl

---

## Notes

- Rækkefølge er vigtig: Skill services skal registreres før GameEngine
- Alle services er Singletons for at dele state

---

**Task Status**: BLOCKED (venter på TASK-023)
**Last Updated**: 2024-12-25
