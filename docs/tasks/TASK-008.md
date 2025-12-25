# Task 008: MainViewModel

## Metadata
- **Phase**: 2 - UI
- **Dependencies**: TASK-007
- **Estimated Time**: 1 hour
- **Status**: COMPLETED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (line 590-711)
- **Requires Design Input**: NO

---

## Purpose

Implement MainViewModel - main controller for the entire game.

**Why this is important:**
- Game loop (timer) lives here
- Coordinates all OperationViewModels
- Handles prestige

---

## Risks

### Potential Issues
1. **Timer Leak**:
   - Edge case: Timer not stopped on page leave
   - Impact: Memory leak, multiple timers

2. **UI Thread Issues**:
   - Edge case: State update from background
   - Impact: Cross-thread exception

### Mitigation
- StopTimers() in OnDisappearing
- DispatcherTimer runs on UI thread

---

## Analysis - What to Implement

### MainViewModel.cs
**Location**: `ViewModels/MainViewModel.cs`

**Properties:**
- CashDisplay, IncomeDisplay (formatted)
- CanPrestige, PrestigeButtonText
- Operations (ObservableCollection)

**Methods:**
- OnAppearing/OnDisappearing (lifecycle)
- StartGameLoop/StopTimers
- OnGameTick (10x per second)
- UpdateDisplay

**Commands:**
- PrestigeCommand

---

## Implementation Guide

### Step 1: Create MainViewModel.cs

**Path**: `src/MadeMan.IdleEmpire/ViewModels/MainViewModel.cs`

```csharp
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IGameEngine _engine;
    private readonly SaveManager _saveManager;
    private IDispatcherTimer? _gameTimer;
    private IDispatcherTimer? _saveTimer;
    private DateTime _lastTick;

    [ObservableProperty]
    private string cashDisplay = "$0";

    [ObservableProperty]
    private string incomeDisplay = "+$0/s";

    [ObservableProperty]
    private bool canPrestige;

    [ObservableProperty]
    private string prestigeButtonText = "BECOME A MADE MAN";

    public ObservableCollection<OperationViewModel> Operations { get; } = new();

    public MainViewModel(IGameEngine engine, SaveManager saveManager)
    {
        _engine = engine;
        _saveManager = saveManager;
    }

    public void OnAppearing()
    {
        _engine.Initialize();
        BuildOperationViewModels();
        StartGameLoop();
        StartAutoSave();
    }

    public void OnDisappearing()
    {
        StopTimers();
        _saveManager.Save(_engine.State);
    }

    private void BuildOperationViewModels()
    {
        Operations.Clear();
        foreach (var op in GameConfig.Operations)
        {
            Operations.Add(new OperationViewModel(op, _engine));
        }
    }

    private void StartGameLoop()
    {
        _lastTick = DateTime.UtcNow;
        _gameTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_gameTimer != null)
        {
            _gameTimer.Interval = TimeSpan.FromMilliseconds(1000.0 / GameConfig.TicksPerSecond);
            _gameTimer.Tick += OnGameTick;
            _gameTimer.Start();
        }
    }

    private void OnGameTick(object? sender, EventArgs e)
    {
        var now = DateTime.UtcNow;
        var delta = (now - _lastTick).TotalSeconds;
        _lastTick = now;

        _engine.Tick(delta);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        CashDisplay = FormatCash(_engine.State.Cash);
        IncomeDisplay = $"+{FormatCash(_engine.IncomePerSecond)}/s";
        CanPrestige = _engine.CanPrestige();

        if (CanPrestige)
        {
            var bonus = GameConfig.PrestigeBonusPerReset * 100;
            PrestigeButtonText = $"RESET FOR +{bonus:0}% BONUS";
        }

        foreach (var op in Operations)
        {
            op.Refresh();
        }
    }

    [RelayCommand]
    private void Prestige()
    {
        if (!_engine.CanPrestige()) return;

        _engine.DoPrestige();
        BuildOperationViewModels();
        UpdateDisplay();
    }

    private void StartAutoSave()
    {
        _saveTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_saveTimer != null)
        {
            _saveTimer.Interval = TimeSpan.FromSeconds(GameConfig.AutoSaveIntervalSeconds);
            _saveTimer.Tick += (s, e) => _saveManager.Save(_engine.State);
            _saveTimer.Start();
        }
    }

    private void StopTimers()
    {
        _gameTimer?.Stop();
        _saveTimer?.Stop();
        _gameTimer = null;
        _saveTimer = null;
    }

    private static string FormatCash(double value)
    {
        if (value >= 1_000_000_000) return $"${value / 1_000_000_000:F2}B";
        if (value >= 1_000_000) return $"${value / 1_000_000:F2}M";
        if (value >= 1_000) return $"${value / 1_000:F2}K";
        return $"${value:F2}";
    }
}
```

### Step 2: Register in MauiProgram.cs

Add to builder.Services in MauiProgram.cs:

```csharp
builder.Services.AddTransient<MainViewModel>();
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Conceptual Test
- Game timer runs 10x/sec
- CashDisplay updates
- Operations collection populated

---

## Acceptance Criteria

- [x] MainViewModel.cs created
- [x] Game loop timer works
- [x] Auto-save timer works
- [x] FormatCash formats K/M/B correctly
- [x] Prestige command works
- [x] Registered in DI
- [x] Build succeeds with 0 errors

---

## Code Evaluation

### Simplification Check
- **Simple timer**: DispatcherTimer, no Task.Run
- **Pull model**: Refresh() instead of events
- **No caching**: Calculates IncomePerSecond every tick

### Known Limitations
- Calculates income 10x per second (OK for 5 operations)
- No debounce on UI updates
- Acceptable for MVP

---

## Design Files Reference

- **Spec Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (line 590-711)
- **Related Tasks**: TASK-007, TASK-009

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Commit**: dd8cf82
