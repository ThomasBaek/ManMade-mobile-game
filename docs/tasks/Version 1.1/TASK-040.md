# Task 040: Reset Game Funktion

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-033 (Settings page)
- **Estimated Time**: 1-1.5 timer
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 9)
- **Frequency Impact**: NO

---

## Formaal

I Settings tab skal spilleren kunne nulstille hele sit spil og starte forfra med dobbelt bekraeftelse.

**Hvorfor dette er vigtigt:**
- Spillere vil nogle gange starte forfra
- Standard feature i alle spil
- Sikker implementation er kritisk

---

## Risici

### Potentielle Problemer
1. **Utilsigtet reset**:
   - Edge case: Bruger trykker ved et uheld
   - Impact: Alt progress tabt

2. **Incomplete reset**:
   - Edge case: Noget data overlever reset
   - Impact: Buggy game state

### Mitigering
- Dobbelt bekraeftelse (dialog + skriv "DELETE")
- Clear alle Preferences og GameState
- Restart app efter reset

---

## Analyse - Hvad Skal Implementeres

### Reset Flow
```
STEP 1: Tap "Reset Game" i Settings
              |
              v
STEP 2: Advarsel dialog vises
        - Viser hvad der slettes
        - [Annuller] [Fortsaet]
              |
              v (hvis Fortsaet)
STEP 3: Bekraeftelses dialog
        - "Skriv DELETE for at bekraefte"
        - Input felt
        - [Annuller] [Slet Alt]
              |
              v (hvis korrekt)
STEP 4: Alt data slettes
        - Preferences cleared
        - App genstarter
```

### Advarsel Dialog
```
+--------------------------------------+
|          ADVARSEL                    |
+--------------------------------------+
|                                      |
|   Er du sikker paa du vil           |
|   slette ALT progress?               |
|                                      |
|   Dette kan IKKE fortrydes!          |
|                                      |
|   Prestige: 5 -> 0                   |
|   Total Earned: $15M -> $0           |
|   Titel: Soldato -> Street Punk      |
|                                      |
|   [ANNULLER]       [FORTSAET]        |
|                                      |
+--------------------------------------+
```

### Bekraeftelses Dialog
```
+--------------------------------------+
|       SIDSTE CHANCE                  |
+--------------------------------------+
|                                      |
|   Skriv "DELETE" for at bekraefte:   |
|                                      |
|   +------------------------------+   |
|   |                              |   |
|   +------------------------------+   |
|                                      |
|   [ANNULLER]       [SLET ALT]        |
|                    (roed, farlig)    |
+--------------------------------------+
```

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-033 (Settings page med placeholder reset knap)

**Assumptions:**
- SaveManager har Delete() metode
- App kan genstarte via MainPage navigation

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Tilfoej Reset Logic til SettingsViewModel

Path: `src/MadeMan.IdleEmpire/ViewModels/SettingsViewModel.cs`

Erstat placeholder ResetGame command:

```csharp
private readonly SaveManager _saveManager;
private readonly IGameEngine _gameEngine;

public SettingsViewModel(SaveManager saveManager, IGameEngine gameEngine)
{
    _saveManager = saveManager;
    _gameEngine = gameEngine;

    // Load settings
    SoundEnabled = Preferences.Default.Get(SoundKey, true);
    MusicEnabled = Preferences.Default.Get(MusicKey, true);
    NotificationsEnabled = Preferences.Default.Get(NotificationsKey, true);
}

// Display values for warning
public string CurrentPrestige => $"Prestige: {_gameEngine.State.PrestigeCount}";
public string CurrentTotalEarned => $"Total Earned: ${FormatNumber(_gameEngine.State.TotalEarned)}";
public string CurrentTitle => $"Titel: {TitleConfig.GetTitle(_gameEngine.State.PrestigeCount).Name}";

[RelayCommand]
private async Task ResetGame()
{
    // Step 1: First warning
    var firstConfirm = await Application.Current.MainPage.DisplayAlert(
        "⚠️ ADVARSEL",
        $"Er du sikker paa du vil slette ALT progress?\n\n" +
        $"Dette kan IKKE fortrydes!\n\n" +
        $"{CurrentPrestige} → 0\n" +
        $"{CurrentTotalEarned} → $0\n" +
        $"{CurrentTitle} → Street Punk",
        "FORTSAET",
        "Annuller");

    if (!firstConfirm) return;

    // Step 2: Type DELETE confirmation
    var deleteConfirm = await Application.Current.MainPage.DisplayPromptAsync(
        "🚨 SIDSTE CHANCE",
        "Skriv \"DELETE\" for at bekraefte:",
        "SLET ALT",
        "Annuller",
        placeholder: "DELETE",
        maxLength: 10);

    if (deleteConfirm?.ToUpper() != "DELETE") return;

    // Step 3: Perform reset
    await PerformReset();
}

private async Task PerformReset()
{
    try
    {
        // Clear all game data
        _saveManager.Delete();

        // Clear settings
        Preferences.Default.Clear();

        // Show confirmation
        await Application.Current.MainPage.DisplayAlert(
            "✓ Reset Complete",
            "Game has been reset. App will restart.",
            "OK");

        // Restart app by navigating to splash
        Application.Current.MainPage = new Views.SplashPage();
    }
    catch (Exception ex)
    {
        await Application.Current.MainPage.DisplayAlert(
            "Error",
            $"Reset failed: {ex.Message}",
            "OK");
    }
}

private string FormatNumber(double value)
{
    if (value >= 1_000_000_000) return $"{value / 1_000_000_000:F2}B";
    if (value >= 1_000_000) return $"{value / 1_000_000:F2}M";
    if (value >= 1_000) return $"{value / 1_000:F2}K";
    return $"{value:F0}";
}
```

### Step 2: Opdater DI Registration

Path: `src/MadeMan.IdleEmpire/MauiProgram.cs`

Sorg for at SettingsViewModel faar SaveManager og GameEngine:

```csharp
builder.Services.AddTransient<SettingsViewModel>();
// SaveManager og IGameEngine er allerede registreret som Singleton
```

### Step 3: Opdater SettingsPage Constructor

Path: `src/MadeMan.IdleEmpire/Views/SettingsPage.xaml.cs`

```csharp
using MadeMan.IdleEmpire.ViewModels;

namespace MadeMan.IdleEmpire.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
```

### Step 4: Opdater Reset Knap Styling

Path: `src/MadeMan.IdleEmpire/Views/SettingsPage.xaml`

Modificer danger zone section:

```xml
<!-- Danger Zone -->
<Frame BackgroundColor="{StaticResource Surface}"
       CornerRadius="12"
       Padding="16"
       BorderColor="{StaticResource Primary}"
       Margin="0,24,0,0">

    <VerticalStackLayout Spacing="12">

        <Label Text="DANGER ZONE"
               FontSize="12"
               FontAttributes="Bold"
               TextColor="{StaticResource Primary}"
               HorizontalOptions="Center"/>

        <BoxView HeightRequest="1"
                 BackgroundColor="{StaticResource Primary}"
                 Opacity="0.3"/>

        <Button Text="🗑️ RESET GAME"
                Command="{Binding ResetGameCommand}"
                BackgroundColor="{StaticResource Primary}"
                TextColor="White"
                CornerRadius="8"
                FontAttributes="Bold"
                HeightRequest="50"/>

        <Label Text="This will delete ALL progress permanently!"
               FontSize="11"
               TextColor="{StaticResource TextSecondary}"
               HorizontalOptions="Center"
               HorizontalTextAlignment="Center"/>

    </VerticalStackLayout>

</Frame>
```

### Step 5: Tilfoej IGameEngine Injection

Hvis SettingsViewModel mangler adgang til GameEngine:

```csharp
public SettingsViewModel(SaveManager saveManager, IGameEngine gameEngine)
{
    _saveManager = saveManager;
    _gameEngine = gameEngine;
    // ...
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

**Positive flow:**
- [ ] Tap Reset Game
- [ ] Foerste dialog viser stats
- [ ] Tap "Fortsaet"
- [ ] Anden dialog beder om "DELETE"
- [ ] Skriv "DELETE" og tap "Slet Alt"
- [ ] App genstarter med fresh state
- [ ] Prestige er 0, ingen operations unlocked

**Negative flows:**
- [ ] Tap "Annuller" i foerste dialog -> intet sker
- [ ] Tap "Annuller" i anden dialog -> intet sker
- [ ] Skriv forkert tekst -> intet sker
- [ ] Tom input -> intet sker

---

## Acceptance Criteria

- [ ] Reset knap synlig i Settings (roed, farlig)
- [ ] Foerste bekraeftelsesdialog med preview af tab
- [ ] Sekundaer bekraeftelse (skriv DELETE)
- [ ] Komplet reset af alt data
- [ ] App genstarter efter reset
- [ ] Kan afbryde paa begge trin
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Standard dialogs**: Brug DisplayAlert og DisplayPromptAsync
- **Preferences.Clear()**: En-linje reset
- **Navigation reset**: MainPage = new SplashPage()

### Alternativer overvejet

**Alternative 1: Confirm checkbox**
- Checkbox "I understand" i stedet for DELETE
- **Hvorfor fravalgt**: For nemt at trykke ved uheld

**Alternative 2: Timer delay**
- Tvungen ventetid foer reset er mulig
- **Hvorfor fravalgt**: Irriterende UX

### Potentielle forbedringer (v2)
- Cloud backup foer reset
- "Export progress" funktion
- Soft reset (kun cash, behold prestige)

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Standard MAUI dialogs
- [ ] **Laesbarhed**: Klar flow i ResetGame method
- [ ] **Error handling**: Try-catch omkring reset
- [ ] **Security**: Dobbelt bekraeftelse
- [ ] **Testbarhed**: Kan simuleres med mock SaveManager

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
