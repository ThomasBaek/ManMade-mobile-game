# Task 007: OperationViewModel

## Metadata
- **Phase**: 2 - UI
- **Dependencies**: TASK-006
- **Estimated Time**: 45 min
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 714-783)
- **Requires Design Input**: NO

---

## Formål

Implementer OperationViewModel - ViewModel for hver enkelt operation i UI.

**Hvorfor dette er vigtigt:**
- Separerer presentation fra game logic
- Håndterer display formatting
- Knap states og farver

---

## Risici

### Potentielle Problemer
1. **Stale Data**:
   - Edge case: ViewModel ikke opdateret efter state change
   - Impact: UI viser forkert data

### Mitigering
- Refresh() metode kaldes fra MainViewModel
- ObservableProperty for reaktiv UI

---

## Analyse - Hvad Skal Implementeres

### OperationViewModel.cs
**Placering**: `ViewModels/OperationViewModel.cs`

**Properties:**
- Name, Icon (fra Operation)
- LevelDisplay, IncomeDisplay (formateret)
- ButtonText, ButtonColor (state-dependent)
- IsUnlocked (binding helper)

**Commands:**
- TapCommand - Unlock eller upgrade

---

## Implementation Guide

### Step 1: Opret OperationViewModel.cs

**Sti**: `src/MadeMan.IdleEmpire/ViewModels/OperationViewModel.cs`

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
Forventet: 0 errors

### 2. Conceptual Test
- Locked operation: LevelDisplay = "LOCKED"
- Unlocked operation: LevelDisplay = "Lvl X"
- Button color changes based on affordability

---

## Acceptance Criteria

- [ ] OperationViewModel.cs oprettet
- [ ] [ObservableProperty] på alle bindable properties
- [ ] [RelayCommand] på Tap
- [ ] Refresh() opdaterer alle properties
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Ingen computed bindings**: Alt opdateres via Refresh()
- **Simple color logic**: Hardcoded hex values
- **Direkte engine access**: Ingen abstraktion lag

### Kendte begrænsninger
- Farver er hardcoded (ikke fra resources)
- Acceptabelt for MVP

---

## Design Files Reference

- **Spec Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 714-783)
- **Related Tasks**: TASK-006, TASK-008

---

**Task Status**: BLOCKED (venter på TASK-006)
**Last Updated**: 2024-12-25
