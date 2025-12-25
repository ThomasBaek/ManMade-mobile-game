# Task 007: OperationViewModel

## Metadata
- **Phase**: 2 - UI
- **Dependencies**: TASK-006
- **Estimated Time**: 45 min
- **Status**: COMPLETED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (line 714-783)
- **Requires Design Input**: NO

---

## Purpose

Implement OperationViewModel - ViewModel for each individual operation in UI.

**Why this is important:**
- Separates presentation from game logic
- Handles display formatting
- Button states and colors

---

## Risks

### Potential Issues
1. **Stale Data**:
   - Edge case: ViewModel not updated after state change
   - Impact: UI shows incorrect data

### Mitigation
- Refresh() method called from MainViewModel
- ObservableProperty for reactive UI

---

## Analysis - What to Implement

### OperationViewModel.cs
**Location**: `ViewModels/OperationViewModel.cs`

**Properties:**
- Name, Icon (from Operation)
- LevelDisplay, IncomeDisplay (formatted)
- ButtonText, ButtonColor (state-dependent)
- IsUnlocked (binding helper)

**Commands:**
- TapCommand - Unlock or upgrade

---

## Implementation Guide

### Step 1: Create OperationViewModel.cs

**Path**: `src/MadeMan.IdleEmpire/ViewModels/OperationViewModel.cs`

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class OperationViewModel : ObservableObject
{
    private readonly Operation _operation;
    private readonly IGameEngine _engine;

    public string Name => _operation.Name;
    public string Icon => _operation.Icon;

    [ObservableProperty]
    private string levelDisplay = string.Empty;

    [ObservableProperty]
    private string incomeDisplay = string.Empty;

    [ObservableProperty]
    private string buttonText = string.Empty;

    [ObservableProperty]
    private Color buttonColor = Colors.Gray;

    [ObservableProperty]
    private bool isUnlocked;

    public OperationViewModel(Operation operation, IGameEngine engine)
    {
        _operation = operation;
        _engine = engine;
        Refresh();
    }

    public void Refresh()
    {
        var state = _engine.State.Operations.FirstOrDefault(o => o.Id == _operation.Id);
        if (state == null) return;

        IsUnlocked = state.Level > 0;

        if (!IsUnlocked)
        {
            LevelDisplay = "LOCKED";
            IncomeDisplay = string.Empty;
            ButtonText = $"${_operation.UnlockCost:F0}";
            ButtonColor = _engine.CanUnlock(_operation.Id)
                ? Color.FromArgb("#4ADE80")
                : Color.FromArgb("#4A5568");
        }
        else
        {
            LevelDisplay = $"Lvl {state.Level}";
            var income = _operation.GetIncome(state.Level, _engine.State.PrestigeBonus);
            IncomeDisplay = $"${income:F2}/s";

            var upgradeCost = _operation.GetUpgradeCost(state.Level);
            ButtonText = $"${upgradeCost:F0}";
            ButtonColor = _engine.CanUpgrade(_operation.Id)
                ? Color.FromArgb("#E94560")
                : Color.FromArgb("#4A5568");
        }
    }

    [RelayCommand]
    private void Tap()
    {
        _engine.UnlockOrUpgrade(_operation.Id);
        Refresh();
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

### 2. Conceptual Test
- Locked operation: LevelDisplay = "LOCKED"
- Unlocked operation: LevelDisplay = "Lvl X"
- Button color changes based on affordability

---

## Acceptance Criteria

- [x] OperationViewModel.cs created
- [x] [ObservableProperty] on all bindable properties
- [x] [RelayCommand] on Tap
- [x] Refresh() updates all properties
- [x] Build succeeds with 0 errors

---

## Code Evaluation

### Simplification Check
- **No computed bindings**: Everything updates via Refresh()
- **Simple color logic**: Hardcoded hex values
- **Direct engine access**: No abstraction layer

### Known Limitations
- Colors are hardcoded (not from resources)
- Acceptable for MVP

---

## Design Files Reference

- **Spec Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (line 714-783)
- **Related Tasks**: TASK-006, TASK-008

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Commit**: 01bd4f0
