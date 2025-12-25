# CLAUDE.md - Made Man: Idle Empire

## Project Overview
This is an **idle/incremental mobile game** built with .NET MAUI.
The player builds a criminal empire in 1930s New Porto.

**Genre:** Idle / Incremental Game (Cookie Clicker-style)
**Theme:** 1930s Mafia / Noir
**Inspiration:** Omerta (browser game), Adventure Capitalist

## Tech Stack
| Component | Technology |
|-----------|------------|
| Framework | .NET 10 MAUI |
| Language | C# |
| UI | XAML |
| Pattern | MVVM with CommunityToolkit.Mvvm |
| Storage | Preferences API (JSON) |
| Target | Android 8.0+ (API 26) |

## Project Structure
```
MadeMan.IdleEmpire/
├── Models/
│   ├── GameState.cs        # Game state (cash, prestige, operations)
│   ├── Operation.cs        # Unified crime/business model
│   └── GameConfig.cs       # All balance constants
├── ViewModels/
│   ├── MainViewModel.cs    # Main ViewModel with game loop
│   └── OperationViewModel.cs # ViewModel for each operation
├── Views/
│   └── MainPage.xaml       # ONLY page - all gameplay
├── Services/
│   ├── IGameEngine.cs      # Interface for game logic
│   ├── GameEngine.cs       # Core tick logic, unlock, upgrade
│   └── SaveManager.cs      # Save/load to Preferences
└── Resources/
    ├── Styles/
    │   └── Theme.xaml      # Colors and styles
    └── Images/
        └── *.png           # Icons
```

## Code Conventions
- **Naming:** PascalCase for public, _camelCase for private fields
- **Properties:** Use [ObservableProperty] from CommunityToolkit.Mvvm
- **Commands:** Use [RelayCommand] attribute
- **DI:** Register all services as Singleton in MauiProgram.cs
- **Async:** Use async/await for I/O operations

## Core Gameplay Loop
```
START → Cash generates automatically ($/sec)
      → Use cash for Upgrade or Unlock
      → Income increases
      → At $10,000 total: Prestige option
      → Reset with +25% permanent bonus
      → Repeat faster
```

## Key Numbers (Balance)
| Operation | Base Income | Unlock Cost |
|-----------|-------------|-------------|
| Pickpocket | $1/s | FREE (starter) |
| Car Theft | $4/s | $50 |
| Burglary | $15/s | $250 |
| Speakeasy | $50/s | $1,000 |
| Casino | $200/s | $5,000 |

**Prestige:** At $10,000 total earned → +25% permanent multiplier

## Build Commands
```bash
# Build for Android
dotnet build -f net10.0-android

# Run on emulator (emulator must be running first)
dotnet build -f net10.0-android -t:Run

# Clean build
dotnet clean && dotnet build -f net10.0-android

# Publish APK
dotnet publish -f net10.0-android -c Release
```

## Implementation Order
1. **Models** - GameState.cs, Operation.cs, GameConfig.cs
2. **Services** - SaveManager.cs, IGameEngine.cs, GameEngine.cs
3. **ViewModels** - OperationViewModel.cs, MainViewModel.cs
4. **Resources** - Theme.xaml (colors)
5. **Views** - MainPage.xaml (redesign from scratch)
6. **DI Setup** - MauiProgram.cs
7. **Lifecycle** - App.xaml.cs (save on sleep)
8. **Test** - Run on emulator

## Status Checklist
- [x] Project created
- [x] NuGet packages installed (TASK-001) ✅
- [x] Directory structure ready
- [x] Models implemented (TASK-002) ✅
- [x] Services implemented (TASK-003, TASK-004) ✅
- [x] DI & Lifecycle setup (TASK-005) ✅
- [x] Theme/colors set up (TASK-006) ✅
- [x] OperationViewModel (TASK-007) ✅
- [x] MainViewModel (TASK-008) ✅
- [x] UI implemented (TASK-009) ✅
- [x] Icons added (TASK-010) ✅
- [ ] Game loop test (TASK-011)
- [ ] Prestige test (TASK-012)
- [ ] Save/Load test (TASK-013)
- [ ] Offline earnings test (TASK-014)

**Next task:** TASK-011 (Verify Game Loop)

## Color Palette
```
Background:    #1A1A2E (dark blue)
Surface:       #16213E (slightly lighter)
Primary:       #E94560 (red accent)
Gold:          #FFD700 (cash color)
Success:       #4ADE80 (green - can afford)
Locked:        #4A5568 (gray - can't afford)
TextPrimary:   #FFFFFF
TextSecondary: #8892A0
```

## Troubleshooting

### Build fails
```bash
dotnet clean
dotnet restore
dotnet build -f net10.0-android
```

### Emulator not found
```bash
# List devices
adb devices

# If empty, start emulator from Android Studio
# or: emulator -list-avds && emulator -avd <name>
```

### Hot Reload not working
Restart the app completely: `dotnet build -f net10.0-android -t:Run`

## Design Documents
See other markdown files in the project for:
- **GAME_DESIGN_DOCUMENT.md** - Lore, rules, future features
- **CLAUDE_CODE_IMPLEMENTATION_GUIDE.md** - Detailed code spec
- **MVP_Specification_MAUI.md** - Original technical spec

---

## Task Workflow (Claude Code)

### Commands

| Command | Description |
|---------|-------------|
| `Start TASK-XXX` | Start specific task |
| `Approve TASK-XXX` | Approve and commit completed task |
| `Change TASK-XXX: [description]` | Request changes |
| `Status` | Get overview of current state |
| `Next` | What's the next task? |

### Task Flow

**1. User says:** `Start TASK-XXX`

**2. Claude implements:**
- Reads task specification
- Implements the code
- Asks questions if necessary

**3. Claude tests BEFORE completion:**
- Runs `dotnet build -f net10.0-android`
- Verifies 0 errors, 0 warnings
- Code review against task specification
- Checks that all acceptance criteria are met

**4. Claude presents for review (STOPS HERE - NO COMMIT):**
- What is implemented (files, methods)
- Test results (build output)
- Code review against spec
- **What user should test** (concrete test instructions)
- Waits for approval

**5. User tests:**
- Runs tests/build themselves
- Code review in IDE
- Emulator test (if UI)

**6. User approves:** `Approve TASK-XXX`

**7. ONLY NOW Claude commits:**
- Git commit with task ID
- Updates TASK-XXX.md (status: COMPLETED)
- Updates TASKS.md (next task READY)
- Updates STATE.md
- Reports next task ready

### Project Management Files
```
docs/
├── PLAN.md           # Architecture + roadmap
├── TASKS.md          # Task overview with status
├── STATE.md          # Current project status
├── NOTES.md          # Scratchpad for notes
└── tasks/            # Individual task files
    ├── TASK-001.md
    ├── TASK-002.md
    └── ...
```

---

## Code Principles

### KISS
- Simplest solution that works
- No over-engineering

### YAGNI
- Only what's needed NOW
- No speculative features

### SOLID
- Single Responsibility
- Open-Closed
- Liskov Substitution
- Interface Segregation
- Dependency Inversion

---

## Git Commit Convention
```
[TASK-XXX]: Short description

- Detail 1
- Detail 2
```

---

## Quality Requirements Before Task Approval
1. `dotnet build -f net10.0-android` succeeds
2. No new warnings
3. Manual test in emulator (if UI)
4. All acceptance criteria met
5. Code evaluation filled in task file

---

## Testing Instructions Per Task

### After Each Task - Claude Must Provide:

1. **Build Verification**
   - Command to run: `dotnet build -f net10.0-android`
   - Expected result: 0 errors, 0 warnings

2. **Code Review Points**
   - Which files to review in IDE
   - Key methods/properties to verify
   - What patterns to look for

3. **Emulator Testing** (if UI task)
   - How to start: `dotnet build -f net10.0-android -t:Run`
   - Prerequisite: Android emulator must be running
   - What to look for in the UI
   - Expected behaviors to verify

4. **Functional Testing**
   - Step-by-step test scenarios
   - Expected outcomes for each step
   - Edge cases to check

### Testing Commands Reference
```bash
# Start emulator (from Android Studio or):
"$LOCALAPPDATA/Android/Sdk/emulator/emulator.exe" -avd <emulator_name>

# Check emulator is running:
"$LOCALAPPDATA/Android/Sdk/platform-tools/adb.exe" devices

# Build and deploy:
dotnet build -f net10.0-android -t:Run

# View logs (for debugging):
adb logcat -s DOTNET
```

### Task-Specific Testing Notes

| Task | Testable in Emulator? | What to Test |
|------|----------------------|--------------|
| TASK-001 to TASK-006 | No | Build only - backend/setup |
| TASK-007, TASK-008 | No | Build only - no UI binding yet |
| TASK-009+ | Yes | UI visible and interactive |

---

## Contact / Notes
- Primary goal: Working MVP on Android
- Secondary: iOS support (later)
- Focus: Simple but engaging gameplay
- No server/backend in MVP
