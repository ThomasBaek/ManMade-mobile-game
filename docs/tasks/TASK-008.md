# Task 008: MainViewModel

## Metadata
- **Phase**: 2 - UI
- **Dependencies**: TASK-007
- **Estimated Time**: 1 time
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 590-711)
- **Requires Design Input**: NO

---

## Formål

Implementer MainViewModel - hovedcontroller for hele spillet.

**Hvorfor dette er vigtigt:**
- Game loop (timer) lever her
- Koordinerer alle OperationViewModels
- Håndterer prestige

---

## Risici

### Potentielle Problemer
1. **Timer Leak**:
   - Edge case: Timer ikke stoppet ved page leave
   - Impact: Memory leak, multiple timers

2. **UI Thread Issues**:
   - Edge case: State update fra background
   - Impact: Cross-thread exception

### Mitigering
- StopTimers() i OnDisappearing
- DispatcherTimer kører på UI thread

---

## Analyse - Hvad Skal Implementeres

### MainViewModel.cs
**Placering**: `ViewModels/MainViewModel.cs`

**Properties:**
- CashDisplay, IncomeDisplay (formateret)
- CanPrestige, PrestigeButtonText
- Operations (ObservableCollection)

**Methods:**
- OnAppearing/OnDisappearing (lifecycle)
- StartGameLoop/StopTimers
- OnGameTick (10x per sekund)
- UpdateDisplay

**Commands:**
- PrestigeCommand

---

## Implementation Guide

### Step 1: Opret MainViewModel.cs

**Sti**: `src/MadeMan.IdleEmpire/ViewModels/MainViewModel.cs`

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

### Step 2: Registrer i MauiProgram.cs

Tilføj til builder.Services i MauiProgram.cs:

```csharp
builder.Services.AddTransient<MainViewModel>();
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Forventet: 0 errors

### 2. Conceptual Test
- Game timer kører 10x/sek
- CashDisplay opdateres
- Operations collection populated

---

## Acceptance Criteria

- [ ] MainViewModel.cs oprettet
- [ ] Game loop timer virker
- [ ] Auto-save timer virker
- [ ] FormatCash formatter K/M/B korrekt
- [ ] Prestige command virker
- [ ] Registreret i DI
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Simpel timer**: DispatcherTimer, ingen Task.Run
- **Pull model**: Refresh() i stedet for events
- **No caching**: Beregner IncomePerSecond hver tick

### Kendte begrænsninger
- Beregner income 10x per sekund (OK for 5 operations)
- Ingen debounce på UI updates
- Acceptabelt for MVP

---

## Design Files Reference

- **Spec Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 590-711)
- **Related Tasks**: TASK-007, TASK-009

---

**Task Status**: BLOCKED (venter på TASK-007)
**Last Updated**: 2024-12-25
