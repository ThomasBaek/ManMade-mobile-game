# Task 036: Smart Operation Visibility

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-029
- **Estimated Time**: 1-1.5 timer
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 4)
- **Frequency Impact**: NO

---

## Formaal

Operations der ikke er koebt endnu, skal kun vises naar spilleren har raad til at koebe dem (eller er taet paa).

**Hvorfor dette er vigtigt:**
- Nye spillere overhelmes af locked content
- Skaber sense of discovery
- Cleaner UI med faerre elementer

---

## Risici

### Potentielle Problemer
1. **Manglende feedback**:
   - Edge case: Bruger ved ikke der er flere operations
   - Impact: Forvirring om game progression

2. **Flickering**:
   - Operation dukker op/forsvinder ved cash fluctuation
   - Impact: Distraherende UI

### Mitigering
- Brug 50% threshold (vises naar 50% af pris)
- Altid vis naeste laaste operation
- Smooth fade-in animation

---

## Analyse - Hvad Skal Implementeres

### Visibility Logic
```
Vis operation HVIS:
  1. Allerede unlocked (Level > 0)
  OR
  2. Cash >= UnlockCost * 0.5 (50% af pris)
  OR
  3. Er naeste laaste operation i tier-raekkefoelge
```

### Progress Indikator for Locked
```
+-----------------------------------+
| LOCKED Car Theft                  |
| Unlock: $50 (Du har: $25)         |
| [████████░░░░░░░░] 50%            |
+-----------------------------------+
```

### UI States
1. **Unlocked**: Normal display
2. **Affordable**: Unlock knap aktiv, fuldt synlig
3. **Almost**: 50-99% af pris, grayed med progress
4. **Hidden**: Under 50% af pris (kun hvis ikke "naeste")

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-029 (farver)
- [x] Eksisterende OperationViewModel

**Assumptions:**
- Operations er sorteret efter tier/unlock cost
- OperationViewModel har adgang til Cash

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Tilfoej Visibility Properties til OperationViewModel

Path: `src/MadeMan.IdleEmpire/ViewModels/OperationViewModel.cs`

```csharp
// Add these properties

/// <summary>
/// Should this operation be visible in the list?
/// </summary>
public bool ShouldShow
{
    get
    {
        // Always show if unlocked
        if (IsUnlocked) return true;

        // Always show first locked operation
        if (IsNextToUnlock) return true;

        // Show if within 50% of unlock cost
        var progress = UnlockProgress;
        return progress >= 0.5;
    }
}

/// <summary>
/// Is this the next operation to be unlocked?
/// </summary>
[ObservableProperty]
private bool _isNextToUnlock;

/// <summary>
/// Progress towards unlock (0.0 to 1.0)
/// </summary>
public double UnlockProgress
{
    get
    {
        if (IsUnlocked) return 1.0;
        if (_operation.UnlockCost <= 0) return 1.0;
        return Math.Min(1.0, _cash / _operation.UnlockCost);
    }
}

/// <summary>
/// Progress bar width (for binding)
/// </summary>
public double ProgressBarWidth => UnlockProgress * 200; // Adjust width as needed

/// <summary>
/// Progress text for locked operations
/// </summary>
public string ProgressText => $"${_cash:N0} / ${_operation.UnlockCost:N0} ({UnlockProgress:P0})";
```

### Step 2: Opdater MainViewModel til at sætte IsNextToUnlock

Path: `src/MadeMan.IdleEmpire/ViewModels/MainViewModel.cs`

```csharp
private void UpdateOperationVisibility()
{
    // Find first locked operation
    OperationViewModel? firstLocked = null;
    foreach (var op in Operations)
    {
        op.IsNextToUnlock = false;

        if (!op.IsUnlocked && firstLocked == null)
        {
            firstLocked = op;
        }
    }

    // Mark first locked as "next to unlock"
    if (firstLocked != null)
    {
        firstLocked.IsNextToUnlock = true;
    }

    // Notify visibility changes
    foreach (var op in Operations)
    {
        op.NotifyVisibilityChanged();
    }
}

// Call this in Tick or when cash changes significantly
```

### Step 3: Opdater MainPage.xaml med Visibility Binding

Path: `src/MadeMan.IdleEmpire/Views/MainPage.xaml`

Modificer operation template:
```xml
<DataTemplate x:DataType="vm:OperationViewModel">
    <Frame BackgroundColor="{StaticResource Surface}"
           CornerRadius="12"
           Padding="16"
           BorderColor="Transparent"
           IsVisible="{Binding ShouldShow}"
           Opacity="{Binding Opacity}">

        <!-- Existing content -->

        <!-- ADD: Progress bar for locked operations -->
        <Grid Grid.Row="2" Grid.ColumnSpan="3"
              IsVisible="{Binding IsLocked}"
              Margin="0,8,0,0">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="Auto"/>
            </Grid.ColumnDefinitions>

            <!-- Progress bar background -->
            <Frame BackgroundColor="{StaticResource SurfaceLight}"
                   CornerRadius="4"
                   HeightRequest="8"
                   Padding="0"
                   BorderColor="Transparent">
                <!-- Progress bar fill -->
                <BoxView BackgroundColor="{StaticResource Gold}"
                         CornerRadius="4"
                         HorizontalOptions="Start"
                         WidthRequest="{Binding ProgressBarWidth}"/>
            </Frame>

            <!-- Progress text -->
            <Label Grid.Column="1"
                   Text="{Binding ProgressText}"
                   FontSize="10"
                   TextColor="{StaticResource TextSecondary}"
                   Margin="8,0,0,0"/>
        </Grid>

    </Frame>
</DataTemplate>
```

### Step 4: Tilfoej Opacity for "Almost" State

```csharp
// In OperationViewModel
public double Opacity
{
    get
    {
        if (IsUnlocked) return 1.0;
        if (CanUnlock) return 1.0;
        return 0.7; // Slightly dimmed for locked
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

### 2. Manual Test in Emulator
- [ ] Kun Pickpocket synlig ved start
- [ ] Car Theft vises naar cash naar $25+ (50%)
- [ ] Progress bar viser korrekt %
- [ ] Locked operations er dimmed
- [ ] Naeste operation er altid synlig

---

## Acceptance Criteria

- [ ] Laaste operations kun synlige naar taet paa unlock
- [ ] Progress bar viser hvor taet spilleren er
- [ ] Naeste locked operation altid synlig
- [ ] Dimmed styling paa locked operations
- [ ] Build succeeds med 0 errors
- [ ] Smooth UX (ingen flickering)

---

## Kode Evaluering

### Simplifikations-tjek
- **Simpel threshold**: 50% af pris
- **Eksisterende binding**: Bygger paa MVVM pattern
- **Ingen animation**: Fade-in kan tilfojes senere

### Alternativer overvejet

**Alternative 1: Show all with locks**
- Vis alle operations med laas-ikon
- **Hvorfor fravalgt**: Overheadmer nye spillere

**Alternative 2: Category-based unlock**
- Vis hele kategorier ad gangen
- **Hvorfor fravalgt**: Ikke passende for nuvaerende game design

### Potentielle forbedringer (v2)
- Fade-in animation naar operation bliver synlig
- "Coming soon" teaser for naeste tier
- Unlock celebration animation

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Simpel threshold logik
- [ ] **Laesbarhed**: Klare property navne
- [ ] **DRY**: Genbrug eksisterende OperationViewModel
- [ ] **Performance**: Beregning kun naar cash aendres
- [ ] **Testbarhed**: Kan unit testes

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
