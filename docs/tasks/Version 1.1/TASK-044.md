# TASK-044: Interval-Based Operation Yields - UI

## Metadata
- **Phase**: 2 - Core Features
- **Dependencies**: TASK-043 (Model & Engine)
- **Estimated Time**: 2-3 hours
- **Status**: Pending
- **Design Reference**: CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Frequency Impact**: NO

---

## Formål

Tilføj **progress bars** til operation cards og opdater visning til interval-baseret yields.

**Hvorfor dette er vigtigt:**
- **Visuel feedback**: Spilleren SER progress mod næste payout
- **Dopamin hit**: Progress bar der fylder op + beløb der "popper" ind
- **Tier differentiering**: Lange bars for high-tier operations viser tydeligt værdi

---

## Risici

### Potentielle Problemer
1. **Performance ved mange progress bars**:
   - Edge case: 5 progress bars der opdateres 2x/sekund
   - Impact: Potentiel UI lag

2. **Layout kompleksitet**:
   - Edge case: Progress bar skal passe ind i eksisterende card layout
   - Impact: Rodet UI hvis ikke designet ordentligt

### Mitigering
- Brug MAUI's built-in ProgressBar (optimeret)
- Simpelt layout: Progress bar under name/level row
- Kun opdater properties der har ændret sig (ObservableProperty)

---

## Analyse - Hvad Skal Implementeres

### 1. OperationViewModel Udvidelser
**Description**: Tilføj progress og yield display properties
**Location**: `ViewModels/OperationViewModel.cs`
**Key Requirements**:
- `Progress` property (0.0-1.0) for ProgressBar binding
- `YieldDisplay` property (fx "$20 in 3.2s")
- `TimeRemainingDisplay` property (fx "3.2s")
- Opdater `Refresh()` til at sætte disse

### 2. MainPage.xaml Layout
**Description**: Tilføj ProgressBar til operation card template
**Location**: `Views/MainPage.xaml`
**Key Requirements**:
- ProgressBar under operation info
- Vises kun når operation er unlocked
- Yield info ved siden af progress bar

### 3. IGameEngine Interface
**Description**: Eksponér operation progress data
**Location**: `Services/IGameEngine.cs`
**Key Requirements**:
- Metode til at hente progress for en operation
- Eller direkte adgang til OperationState

---

## Dependencies Check

✅ **Required Before Starting**:
- [ ] TASK-043 completed (Model & Engine changes)

⚠️ **Assumptions**:
- OperationState.AccumulatedTime eksisterer
- Operation.Interval eksisterer
- GameEngine.Tick() opdaterer AccumulatedTime

❌ **Blockers**: TASK-043 skal være færdig først

---

## Implementation Guide

### Step 1: Udvid OperationViewModel
Path: `ViewModels/OperationViewModel.cs`

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

    public string IconEmoji => _operation.Id switch
    {
        "pickpocket" => "\U0001F91A",
        "cartheft" => "\U0001F697",
        "burglary" => "\U0001F3E0",
        "speakeasy" => "\U0001F37A",
        "casino" => "\U0001F3B0",
        _ => "\U0001F4B0"
    };

    [ObservableProperty]
    private string _levelDisplay = string.Empty;

    [ObservableProperty]
    private string _incomeDisplay = string.Empty;

    [ObservableProperty]
    private string _buttonText = string.Empty;

    [ObservableProperty]
    private Color _buttonColor = Colors.Gray;

    [ObservableProperty]
    private bool _isUnlocked;

    // NEW: Progress bar support
    [ObservableProperty]
    private double _progress; // 0.0 - 1.0

    [ObservableProperty]
    private string _yieldDisplay = string.Empty; // "$20"

    [ObservableProperty]
    private string _timeRemainingDisplay = string.Empty; // "3.2s"

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

            // Reset progress display for locked operations
            Progress = 0;
            YieldDisplay = string.Empty;
            TimeRemainingDisplay = string.Empty;
        }
        else
        {
            LevelDisplay = $"Lvl {state.Level}";

            // Calculate yield with all multipliers
            var yield = _operation.GetYield(state.Level, _engine.State.PrestigeBonus);
            // Note: Full multiplier calculation is in GameEngine, this is base display
            YieldDisplay = $"${FormatNumber(yield)}";

            // Effective income per second for reference
            var incomePerSec = _operation.GetIncomePerSecond(state.Level, _engine.State.PrestigeBonus);
            IncomeDisplay = $"${FormatNumber(incomePerSec)}/s";

            // Progress calculation
            Progress = Math.Min(state.AccumulatedTime / _operation.Interval, 1.0);
            var timeRemaining = Math.Max(_operation.Interval - state.AccumulatedTime, 0);
            TimeRemainingDisplay = $"{timeRemaining:F1}s";

            // Upgrade button
            var upgradeCost = _operation.GetUpgradeCost(state.Level);
            ButtonText = $"${FormatNumber(upgradeCost)}";
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

    private static string FormatNumber(double value)
    {
        if (value >= 1_000_000) return $"{value / 1_000_000:F1}M";
        if (value >= 1_000) return $"{value / 1_000:F1}K";
        return $"{value:F0}";
    }
}
```

### Step 2: Opdater MainPage.xaml Operation Card
Path: `Views/MainPage.xaml`

```xml
<DataTemplate x:DataType="vm:OperationViewModel">
    <Frame BackgroundColor="{StaticResource Surface}"
           CornerRadius="12"
           Padding="16"
           BorderColor="Transparent">
        <Frame.GestureRecognizers>
            <TapGestureRecognizer Command="{Binding TapCommand}"/>
        </Frame.GestureRecognizers>

        <Grid ColumnDefinitions="50,*,Auto" RowDefinitions="Auto,Auto,Auto">

            <!-- Icon -->
            <Frame Grid.RowSpan="3"
                   BackgroundColor="{StaticResource SurfaceLight}"
                   CornerRadius="8"
                   Padding="0"
                   HeightRequest="50"
                   WidthRequest="50"
                   BorderColor="Transparent"
                   VerticalOptions="Start">
                <Label Text="{Binding IconEmoji}"
                       FontSize="28"
                       HorizontalOptions="Center"
                       VerticalOptions="Center"/>
            </Frame>

            <!-- Name + Level -->
            <HorizontalStackLayout Grid.Column="1" Spacing="8" Margin="12,0,0,0">
                <Label Text="{Binding Name}"
                       FontSize="16"
                       FontAttributes="Bold"
                       TextColor="{StaticResource TextPrimary}"/>
                <Label Text="{Binding LevelDisplay}"
                       FontSize="14"
                       TextColor="{StaticResource TextSecondary}"/>
            </HorizontalStackLayout>

            <!-- Progress Bar Row (only when unlocked) -->
            <Grid Grid.Column="1" Grid.Row="1"
                  ColumnDefinitions="*,Auto"
                  Margin="12,6,0,0"
                  IsVisible="{Binding IsUnlocked}">

                <!-- Progress Bar -->
                <ProgressBar Progress="{Binding Progress}"
                             ProgressColor="{StaticResource Gold}"
                             BackgroundColor="{StaticResource SurfaceLight}"
                             HeightRequest="8"
                             VerticalOptions="Center"/>

                <!-- Yield + Time -->
                <HorizontalStackLayout Grid.Column="1" Spacing="4" Margin="8,0,0,0">
                    <Label Text="{Binding YieldDisplay}"
                           FontSize="12"
                           FontAttributes="Bold"
                           TextColor="{StaticResource Gold}"/>
                    <Label Text="{Binding TimeRemainingDisplay}"
                           FontSize="12"
                           TextColor="{StaticResource TextSecondary}"/>
                </HorizontalStackLayout>
            </Grid>

            <!-- Effective Income (small text) -->
            <Label Grid.Column="1" Grid.Row="2"
                   Text="{Binding IncomeDisplay}"
                   FontSize="11"
                   TextColor="{StaticResource TextSecondary}"
                   IsVisible="{Binding IsUnlocked}"
                   Margin="12,2,0,0"/>

            <!-- Action Button -->
            <Button Grid.Column="2" Grid.RowSpan="3"
                    Text="{Binding ButtonText}"
                    Command="{Binding TapCommand}"
                    BackgroundColor="{Binding ButtonColor}"
                    TextColor="White"
                    CornerRadius="8"
                    Padding="16,8"
                    FontSize="14"
                    FontAttributes="Bold"
                    VerticalOptions="Center"/>
        </Grid>
    </Frame>
</DataTemplate>
```

### Step 3: Sikre Progress Opdateres i MainViewModel
Path: `ViewModels/MainViewModel.cs`

```csharp
private void UpdateDisplay()
{
    CashDisplay = FormatCash(_engine.State.Cash);
    IncomeDisplay = $"+{FormatCash(_engine.IncomePerSecond)}/s";
    // ... existing code ...

    // Refresh all operations (includes progress)
    foreach (var op in Operations)
    {
        op.Refresh();
    }

    // ... rest of existing code ...
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Manual Test in Emulator
- [ ] Progress bars vises på alle unlocked operations
- [ ] Progress bars opdateres smoothly (2x/sekund)
- [ ] Yield beløb vises korrekt ($1, $20, $300, etc.)
- [ ] Time remaining tæller ned
- [ ] Ved 100% progress, resetter bar og cash stiger
- [ ] Locked operations viser ingen progress bar
- [ ] Effective income/s vises under progress bar
- [ ] Layout ser pænt ud på forskellige skærmstørrelser

---

## Acceptance Criteria

- [ ] ProgressBar vises på hver unlocked operation
- [ ] Progress property opdateres korrekt (0.0-1.0)
- [ ] YieldDisplay viser beløb for næste payout
- [ ] TimeRemainingDisplay viser tid til næste yield
- [ ] IncomeDisplay viser effective income/s
- [ ] Locked operations skjuler progress elementer
- [ ] Build succeeds with 0 errors
- [ ] UI er responsiv og smooth

---

## Kode Evaluering

### Simplifikations-tjek
Denne implementation følger KISS princippet ved at:
- **MAUI ProgressBar**: Bruger built-in component, ingen custom rendering
- **Simple bindings**: ObservableProperty + DataTemplate binding
- **Minimal layout changes**: Tilføjer én row til eksisterende Grid

### Alternativer overvejet

**Alternative 1: Custom Progress Bar Control**
```csharp
public class AnimatedProgressBar : View { ... }
```
**Hvorfor fravalgt**: Over-engineering, MAUI's ProgressBar er sufficient

**Alternative 2: Canvas-baseret rendering**
```csharp
// Custom drawing of progress
```
**Hvorfor fravalgt**: Komplekst, unødvendigt for simple progress bars

### Potentielle forbedringer (v2)
- Pulse animation når yield popper
- Farveændring baseret på operation tier
- "Pop" tekst animation med beløb

### Kendte begrænsninger
- **Ingen yield animation**: Progress bar resetter abrupt (polish later)
- **Simpel number formatting**: Kunne forbedres med locale support

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Er dette den simpleste løsning?
- [ ] **Læsbarhed**: Kan en anden udvikler forstå koden uden forklaring?
- [ ] **Navngivning**: Er alle navne beskrivende og konsistente?
- [ ] **Funktioner**: Er alle funktioner korte og fokuserede?
- [ ] **DRY**: Er der nogen duplikeret kode?
- [ ] **Error handling**: Er fejl håndteret korrekt?
- [ ] **Edge cases**: Er edge cases identificeret og håndteret?
- [ ] **Performance**: Er der åbenlyse performance problemer?
- [ ] **Testbarhed**: Kan koden nemt testes?

---

## Design Files Reference

- **Game Design**: PLAN.md
- **Implementation Guide**: CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Depends on**: TASK-043 (Model & Engine)

---

## Notes

- ProgressBar HeightRequest=8 giver en tynd, elegant bar
- Gold farve matcher tema for income/money elementer
- TimeRemainingDisplay med 1 decimal (3.2s) giver god feedback

---

**Task Status**: ⏸️ Blocked by TASK-043
**Last Updated**: 2024-12-26
**Implemented By**: Pending
