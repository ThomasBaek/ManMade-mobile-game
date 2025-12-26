# Task 033: Settings Page

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-031
- **Estimated Time**: 1-2 timer
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 9)
- **Frequency Impact**: NO

---

## Formaal

Implementer en Settings page med basale indstillinger og forberedelse til reset funktion (TASK-040).

**Hvorfor dette er vigtigt:**
- Spillere forventer settings i alle apps
- Forberedelse til reset game funktion
- Version info og credits

---

## Risici

### Potentielle Problemer
1. **Settings persistence**:
   - Edge case: Settings skal gemmes separat fra GameState
   - Impact: Settings tab ved app genstart

2. **Toggle state**:
   - Sound/Music toggles skal have effekt
   - Impact: Toggles der ikke virker er forvirrende

### Mitigering
- Brug Preferences API for settings
- Implementer toggles som placeholders (no-op for nu)

---

## Analyse - Hvad Skal Implementeres

### Settings Layout
```
+------------------------------------------+
|           SETTINGS                        |
+------------------------------------------+
|                                          |
|  Sound Effects              [ON/OFF]     |
|  ----------------------------------------|
|  Music                      [ON/OFF]     |
|  ----------------------------------------|
|  Notifications              [ON/OFF]     |
|                                          |
|  ========================================|
|                                          |
|  Version: 1.1.0                          |
|  ----------------------------------------|
|  Credits                           >     |
|                                          |
|  ========================================|
|                                          |
|  +------------------------------------+  |
|  |     RESET GAME (Placeholder)       |  |
|  |     (Implemented in TASK-040)      |  |
|  +------------------------------------+  |
|                                          |
+------------------------------------------+
```

### Settings Model
```csharp
public class AppSettings
{
    public bool SoundEnabled { get; set; } = true;
    public bool MusicEnabled { get; set; } = true;
    public bool NotificationsEnabled { get; set; } = true;
}
```

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-031 (navigation med Settings tab)

**Assumptions:**
- Settings gemmes i Preferences API
- Sound/Music er placeholders (ingen lyd endnu)

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Opret SettingsViewModel

Path: `src/MadeMan.IdleEmpire/ViewModels/SettingsViewModel.cs`

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private const string SoundKey = "settings_sound";
    private const string MusicKey = "settings_music";
    private const string NotificationsKey = "settings_notifications";

    public string Version => "1.1.0";

    [ObservableProperty]
    private bool _soundEnabled;

    [ObservableProperty]
    private bool _musicEnabled;

    [ObservableProperty]
    private bool _notificationsEnabled;

    public SettingsViewModel()
    {
        // Load settings from Preferences
        SoundEnabled = Preferences.Default.Get(SoundKey, true);
        MusicEnabled = Preferences.Default.Get(MusicKey, true);
        NotificationsEnabled = Preferences.Default.Get(NotificationsKey, true);
    }

    partial void OnSoundEnabledChanged(bool value)
    {
        Preferences.Default.Set(SoundKey, value);
        // TODO: Actually toggle sound when implemented
    }

    partial void OnMusicEnabledChanged(bool value)
    {
        Preferences.Default.Set(MusicKey, value);
        // TODO: Actually toggle music when implemented
    }

    partial void OnNotificationsEnabledChanged(bool value)
    {
        Preferences.Default.Set(NotificationsKey, value);
        // TODO: Actually toggle notifications when implemented
    }

    [RelayCommand]
    private async Task ShowCredits()
    {
        await Application.Current.MainPage.DisplayAlert(
            "Credits",
            "Made Man: Idle Empire\n\nDeveloped with .NET MAUI\n\n© 2024",
            "OK");
    }

    [RelayCommand]
    private async Task ResetGame()
    {
        // Placeholder - will be implemented in TASK-040
        await Application.Current.MainPage.DisplayAlert(
            "Coming Soon",
            "Reset Game will be available in a future update.",
            "OK");
    }
}
```

### Step 2: Opdater SettingsPage.xaml

Path: `src/MadeMan.IdleEmpire/Views/SettingsPage.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:MadeMan.IdleEmpire.ViewModels"
             x:Class="MadeMan.IdleEmpire.Views.SettingsPage"
             x:DataType="vm:SettingsViewModel"
             BackgroundColor="{StaticResource Background}"
             Shell.NavBarIsVisible="False">

    <ScrollView>
        <VerticalStackLayout Padding="16" Spacing="0">

            <!-- Title -->
            <Label Text="Settings"
                   FontSize="28"
                   FontAttributes="Bold"
                   TextColor="{StaticResource TextPrimary}"
                   HorizontalOptions="Center"
                   Margin="0,16,0,24"/>

            <!-- Sound Settings -->
            <Frame BackgroundColor="{StaticResource Surface}"
                   CornerRadius="12"
                   Padding="16"
                   BorderColor="Transparent"
                   Margin="0,0,0,16">
                <VerticalStackLayout Spacing="0">

                    <!-- Sound Effects -->
                    <Grid ColumnDefinitions="*,Auto" HeightRequest="48">
                        <Label Text="Sound Effects"
                               FontSize="16"
                               TextColor="{StaticResource TextPrimary}"
                               VerticalOptions="Center"/>
                        <Switch Grid.Column="1"
                                IsToggled="{Binding SoundEnabled}"
                                OnColor="{StaticResource Gold}"
                                ThumbColor="{StaticResource TextPrimary}"/>
                    </Grid>

                    <BoxView HeightRequest="1"
                             BackgroundColor="{StaticResource SurfaceLight}"
                             Margin="0,8"/>

                    <!-- Music -->
                    <Grid ColumnDefinitions="*,Auto" HeightRequest="48">
                        <Label Text="Music"
                               FontSize="16"
                               TextColor="{StaticResource TextPrimary}"
                               VerticalOptions="Center"/>
                        <Switch Grid.Column="1"
                                IsToggled="{Binding MusicEnabled}"
                                OnColor="{StaticResource Gold}"
                                ThumbColor="{StaticResource TextPrimary}"/>
                    </Grid>

                    <BoxView HeightRequest="1"
                             BackgroundColor="{StaticResource SurfaceLight}"
                             Margin="0,8"/>

                    <!-- Notifications -->
                    <Grid ColumnDefinitions="*,Auto" HeightRequest="48">
                        <Label Text="Notifications"
                               FontSize="16"
                               TextColor="{StaticResource TextPrimary}"
                               VerticalOptions="Center"/>
                        <Switch Grid.Column="1"
                                IsToggled="{Binding NotificationsEnabled}"
                                OnColor="{StaticResource Gold}"
                                ThumbColor="{StaticResource TextPrimary}"/>
                    </Grid>

                </VerticalStackLayout>
            </Frame>

            <!-- Info Section -->
            <Frame BackgroundColor="{StaticResource Surface}"
                   CornerRadius="12"
                   Padding="16"
                   BorderColor="Transparent"
                   Margin="0,0,0,16">
                <VerticalStackLayout Spacing="0">

                    <!-- Version -->
                    <Grid ColumnDefinitions="*,Auto" HeightRequest="48">
                        <Label Text="Version"
                               FontSize="16"
                               TextColor="{StaticResource TextPrimary}"
                               VerticalOptions="Center"/>
                        <Label Grid.Column="1"
                               Text="{Binding Version}"
                               FontSize="16"
                               TextColor="{StaticResource TextSecondary}"
                               VerticalOptions="Center"/>
                    </Grid>

                    <BoxView HeightRequest="1"
                             BackgroundColor="{StaticResource SurfaceLight}"
                             Margin="0,8"/>

                    <!-- Credits -->
                    <Grid ColumnDefinitions="*,Auto" HeightRequest="48">
                        <Grid.GestureRecognizers>
                            <TapGestureRecognizer Command="{Binding ShowCreditsCommand}"/>
                        </Grid.GestureRecognizers>
                        <Label Text="Credits"
                               FontSize="16"
                               TextColor="{StaticResource TextPrimary}"
                               VerticalOptions="Center"/>
                        <Label Grid.Column="1"
                               Text=">"
                               FontSize="16"
                               TextColor="{StaticResource TextSecondary}"
                               VerticalOptions="Center"/>
                    </Grid>

                </VerticalStackLayout>
            </Frame>

            <!-- Danger Zone -->
            <Frame BackgroundColor="{StaticResource Surface}"
                   CornerRadius="12"
                   Padding="16"
                   BorderColor="Transparent"
                   Margin="0,24,0,0">

                <Button Text="Reset Game"
                        Command="{Binding ResetGameCommand}"
                        BackgroundColor="{StaticResource Primary}"
                        TextColor="White"
                        CornerRadius="8"
                        FontAttributes="Bold"
                        HeightRequest="50"/>

            </Frame>

            <!-- Footer note -->
            <Label Text="This will delete all progress!"
                   FontSize="12"
                   TextColor="{StaticResource TextSecondary}"
                   HorizontalOptions="Center"
                   Margin="0,8,0,0"/>

        </VerticalStackLayout>
    </ScrollView>

</ContentPage>
```

### Step 3: Opdater SettingsPage.xaml.cs

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

### Step 4: Registrer i DI

Path: `src/MadeMan.IdleEmpire/MauiProgram.cs`

```csharp
// ViewModels
builder.Services.AddTransient<SettingsViewModel>();

// Pages
builder.Services.AddTransient<SettingsPage>();
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Manual Test in Emulator
- [ ] Settings tab viser korrekt layout
- [ ] Toggles fungerer og gemmer state
- [ ] Version nummer vises
- [ ] Credits dialog vises ved tap
- [ ] Reset knap viser placeholder dialog

---

## Acceptance Criteria

- [ ] Settings page med toggles
- [ ] Settings persisteres via Preferences
- [ ] Version og Credits info
- [ ] Reset knap (placeholder for TASK-040)
- [ ] Konsistent styling med tema
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Preferences API**: Simpel persistence
- **Placeholder toggles**: Ingen lyd-system endnu
- **Standard MVVM**: Velkendt pattern

### Alternativer overvejet

**Alternative 1: JSON Settings fil**
- Gem settings som JSON
- **Hvorfor fravalgt**: Preferences API er simplere for key-value

**Alternative 2: Database storage**
- SQLite for settings
- **Hvorfor fravalgt**: Over-engineering for 3 booleans

### Potentielle forbedringer (v2)
- Faktisk lyd-implementering
- Push notifications setup
- Sprog-valg
- Theme toggle (light/dark)

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Preferences API er simpelt
- [ ] **Laesbarhed**: Klar separation af concerns
- [ ] **Navngivning**: Beskrivende property navne
- [ ] **DRY**: Ingen duplikation
- [ ] **Error handling**: Preferences har fallback values
- [ ] **Testbarhed**: ViewModel kan unit testes

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
