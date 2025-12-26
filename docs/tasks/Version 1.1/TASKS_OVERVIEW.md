# Version 1.1 - UI/UX Redesign Tasks Overview

## Summary
This document provides an overview of all UI/UX redesign tasks for Made Man: Idle Empire v1.1.

**Total Tasks:** 14 (TASK-029 to TASK-042)
**Estimated Total Time:** 17-24 hours

---

## Task List

| Task | Feature | Dependencies | Est. Time | Status |
|------|---------|--------------|-----------|--------|
| TASK-029 | Dystert Gangster Tema | None | 1-2h | READY |
| TASK-030 | App Ikon | TASK-029 | 30-60m | READY |
| TASK-031 | Bottom Navigation | TASK-029 | 2-3h | READY |
| TASK-032 | Top Bar (Cash/Income) | TASK-029, 031 | 1-2h | READY |
| TASK-033 | Settings Page | TASK-031 | 1-2h | READY |
| TASK-034 | Splash Screen | TASK-029, 030 | 1-2h | READY |
| TASK-035 | Welcome Back Modal | TASK-029, 032 | 1.5-2h | READY |
| TASK-036 | Smart Operation Visibility | TASK-029 | 1-1.5h | READY |
| TASK-037 | Kompakt UI Layout | TASK-029, 036 | 2-3h | READY |
| TASK-038 | Prestige Modal UX | TASK-029, 032 | 1-1.5h | READY |
| TASK-039 | Titel System | TASK-029, 038 | 1.5-2h | READY |
| TASK-040 | Reset Game Funktion | TASK-033 | 1-1.5h | READY |
| TASK-041 | Global Top Bar | TASK-032 | 1-1.5h | READY |
| TASK-042 | Skills Tab Redesign | TASK-041 | 2-3h | READY |

---

## Implementation Order (Recommended)

### Wave 1: Foundation (Can parallelize)
```
TASK-029: Dystert tema/farver     <- All others depend on this
TASK-030: App ikon                <- Can be done in parallel
```

### Wave 2: Navigation
```
TASK-031: Bottom Navigation       <- Requires TASK-029
TASK-032: Top Bar                 <- Requires TASK-029, TASK-031
TASK-033: Settings Page           <- Requires TASK-031
```

### Wave 3: Screens
```
TASK-034: Splash Screen           <- Requires TASK-029, TASK-030
TASK-035: Welcome Back Modal      <- Requires TASK-029, TASK-032
```

### Wave 4: Gameplay Polish
```
TASK-036: Smart Operation Vis     <- Requires TASK-029
TASK-037: Kompakt UI              <- Requires TASK-029, TASK-036
TASK-038: Prestige Modal UX       <- Requires TASK-029, TASK-032
```

### Wave 5: Features
```
TASK-039: Titel System            <- Requires TASK-029, TASK-038
TASK-040: Reset Funktion          <- Requires TASK-033
```

### Wave 6: Skills & Global UI
```
TASK-041: Global Top Bar          <- Requires TASK-032
TASK-042: Skills Tab Redesign     <- Requires TASK-041
```

---

## Dependency Graph

```
TASK-029 (Tema)
    |
    +---> TASK-030 (Ikon)
    |         |
    |         +---> TASK-034 (Splash)
    |
    +---> TASK-031 (Bottom Nav)
    |         |
    |         +---> TASK-032 (Top Bar)
    |         |         |
    |         |         +---> TASK-035 (Welcome Back)
    |         |         +---> TASK-038 (Prestige Modal)
    |         |         |         |
    |         |         |         +---> TASK-039 (Titel)
    |         |         |
    |         |         +---> TASK-041 (Global Top Bar)
    |         |                   |
    |         |                   +---> TASK-042 (Skills Tab Redesign)
    |         |
    |         +---> TASK-033 (Settings)
    |                   |
    |                   +---> TASK-040 (Reset)
    |
    +---> TASK-036 (Smart Visibility)
              |
              +---> TASK-037 (Kompakt UI)
```

---

## Key Files to Be Modified/Created

### New Files
- `Views/SplashPage.xaml` + `.cs`
- `Views/OrgCrimePage.xaml` + `.cs`
- `Views/CasinoPage.xaml` + `.cs`
- `Views/SkillsPage.xaml` + `.cs`
- `Views/SettingsPage.xaml` + `.cs`
- `Views/Components/TopBarView.xaml` + `.cs`
- `Views/Components/WelcomeBackModal.xaml` + `.cs`
- `Views/Components/PrestigeModal.xaml` + `.cs`
- `Views/Components/TitleUnlockModal.xaml` + `.cs`
- `ViewModels/SettingsViewModel.cs`
- `Models/TitleConfig.cs`

### Modified Files
- `Resources/Styles/Colors.xaml` - New color palette
- `AppShell.xaml` - Tab navigation
- `App.xaml.cs` - Splash screen startup
- `Views/MainPage.xaml` - Compact layout, modals
- `ViewModels/MainViewModel.cs` - New properties
- `ViewModels/OperationViewModel.cs` - Visibility logic
- `Services/GameEngine.cs` - Offline earnings exposure
- `MauiProgram.cs` - DI registrations

---

## Design Specifications

### New Color Palette
| Color | Hex | Usage |
|-------|-----|-------|
| Background | #0D0D0D | Main background (darker) |
| Surface | #1A1A2E | Cards, panels |
| SurfaceLight | #252540 | Elevated surfaces |
| Gold | #D4AF37 | Cash, success |
| Primary | #8B0000 | Blood red, prestige |
| Success | #4ADE80 | Positive income |
| Smoke | #4A4A4A | Disabled, locked |

### Component Sizing
| Component | Before | After |
|-----------|--------|-------|
| Operation Card | ~120px | ~60px |
| Header | ~80px | ~50-60px |
| Touch targets | varies | min 44px |

---

## Workflow Commands

```bash
# Start specific task
Start TASK-029

# Approve completed task
Approve TASK-029

# Request changes
Change TASK-029: [description]

# Get current status
Status

# Get next task
Next
```

---

## Notes

1. **SVG/Graphics**: Where AI-generated graphics are mentioned, SVG placeholders will be created. Professional graphics can be added later.

2. **Placeholder Pages**: Casino, Org Crime, and Skills pages will show "Locked" state until implemented in future versions.

3. **Sound/Music**: Settings toggles are placeholders - no audio system exists yet.

4. **Performance**: All tasks should maintain smooth 60fps performance on target devices.

---

**Document Created:** 2024-12-26
**For Version:** 1.1
**Status:** All tasks READY for implementation
