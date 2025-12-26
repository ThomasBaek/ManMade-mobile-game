# Task 038: Prestige Modal UX Forbedring

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-029, TASK-032
- **Estimated Time**: 1-1.5 timer
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 7)
- **Frequency Impact**: NO

---

## Formaal

Prestige modal skal kunne lukkes (udskydes til senere) og genaabes via prestige badge i header.

**Hvorfor dette er vigtigt:**
- Nuvaerende modal kan ikke lukkes uden at prestige
- Spilleren kan ikke fortsaette med at spare op
- Bedre kontrol over prestige timing

---

## Risici

### Potentielle Problemer
1. **Glemt prestige**:
   - Edge case: Spilleren glemmer prestige er tilgaengelig
   - Impact: Suboptimal gameplay

2. **Badge synlighed**:
   - Badge kan overses i header
   - Impact: Bruger opdager ikke mulighed

### Mitigering
- Animeret badge (pulse/glow) naar prestige tilgaengelig
- Badge altid synlig i header (dimmed naar ikke tilgaengelig)

---

## Analyse - Hvad Skal Implementeres

### Prestige Badge i Header
```
Normal (ingen prestige):     Prestige Tilgaengelig:
+----------+                 +----------+
|  P: 3    |                 |  P: 3    |  (glow animation)
+----------+                 +----------+
```

### Modal med Luk-knap
```
+--------------------------------------+
| X                                    |  <-- Luk knap
|                                      |
|        PRESTIGE AVAILABLE            |
|                                      |
|   Current bonus: 1.50x               |
|   After prestige: 1.75x              |
|                                      |
|   +------------------------------+   |
|   |      PRESTIGE NOW            |   |
|   +------------------------------+   |
|                                      |
|        [Maaske senere]               |
|                                      |
+--------------------------------------+
```

### Flow
1. TotalEarned naar threshold -> `CanPrestige = true`
2. Modal vises IKKE automatisk (kun badge gloer)
3. Spilleren tapper badge -> Modal vises
4. Spilleren kan lukke modal (X eller "Maaske senere")
5. Badge fortsaetter med at gloe

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-029 (farver)
- [x] TASK-032 (header med badge)

**Assumptions:**
- Eksisterende prestige panel kan konverteres til modal
- Header har plads til badge

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Tilfoej IsPrestigeDismissed til MainViewModel

Path: `src/MadeMan.IdleEmpire/ViewModels/MainViewModel.cs`

```csharp
[ObservableProperty]
private bool _isPrestigeModalVisible;

// Badge should glow when prestige available
public bool ShouldShowPrestigeBadgeGlow => CanPrestige;

// Modal trigger
[RelayCommand]
private void ShowPrestigeModal()
{
    if (CanPrestige)
    {
        IsPrestigeModalVisible = true;
    }
}

[RelayCommand]
private void DismissPrestigeModal()
{
    IsPrestigeModalVisible = false;
}
```

### Step 2: Opret PrestigeModal Component

Path: `src/MadeMan.IdleEmpire/Views/Components/PrestigeModal.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MadeMan.IdleEmpire.Views.Components.PrestigeModal"
             IsVisible="{Binding IsPrestigeModalVisible}">

    <!-- Dark overlay -->
    <Grid BackgroundColor="#CC000000">

        <!-- Modal Card -->
        <Frame BackgroundColor="{StaticResource Surface}"
               CornerRadius="20"
               Padding="0"
               Margin="24"
               VerticalOptions="Center"
               HorizontalOptions="Center"
               MaximumWidthRequest="350"
               BorderColor="Transparent">

            <Grid RowDefinitions="Auto,*">

                <!-- Header with close button -->
                <Grid BackgroundColor="{StaticResource Primary}"
                      Padding="16">
                    <Label Text="PRESTIGE AVAILABLE"
                           FontSize="18"
                           FontAttributes="Bold"
                           TextColor="White"
                           HorizontalOptions="Center"/>

                    <!-- Close button -->
                    <Button Text="X"
                            Command="{Binding DismissPrestigeModalCommand}"
                            BackgroundColor="Transparent"
                            TextColor="White"
                            FontAttributes="Bold"
                            FontSize="16"
                            HorizontalOptions="End"
                            VerticalOptions="Center"
                            WidthRequest="40"
                            HeightRequest="40"/>
                </Grid>

                <!-- Content -->
                <VerticalStackLayout Grid.Row="1"
                                     Padding="24"
                                     Spacing="16">

                    <!-- Star icon -->
                    <Label Text="⭐"
                           FontSize="48"
                           HorizontalOptions="Center"/>

                    <!-- Description -->
                    <Label Text="Reset your empire for a permanent bonus!"
                           FontSize="14"
                           TextColor="{StaticResource TextSecondary}"
                           HorizontalOptions="Center"
                           HorizontalTextAlignment="Center"/>

                    <!-- Bonus comparison -->
                    <Frame BackgroundColor="{StaticResource SurfaceLight}"
                           CornerRadius="8"
                           Padding="16"
                           BorderColor="Transparent">
                        <Grid ColumnDefinitions="*,Auto,*" ColumnSpacing="8">
                            <VerticalStackLayout>
                                <Label Text="Current"
                                       FontSize="11"
                                       TextColor="{StaticResource TextSecondary}"
                                       HorizontalOptions="Center"/>
                                <Label Text="{Binding CurrentPrestigeBonusDisplay}"
                                       FontSize="18"
                                       FontAttributes="Bold"
                                       TextColor="{StaticResource TextPrimary}"
                                       HorizontalOptions="Center"/>
                            </VerticalStackLayout>

                            <Label Grid.Column="1"
                                   Text="→"
                                   FontSize="24"
                                   TextColor="{StaticResource Gold}"
                                   VerticalOptions="Center"/>

                            <VerticalStackLayout Grid.Column="2">
                                <Label Text="After"
                                       FontSize="11"
                                       TextColor="{StaticResource TextSecondary}"
                                       HorizontalOptions="Center"/>
                                <Label Text="{Binding NextPrestigeBonusDisplay}"
                                       FontSize="18"
                                       FontAttributes="Bold"
                                       TextColor="{StaticResource Gold}"
                                       HorizontalOptions="Center"/>
                            </VerticalStackLayout>
                        </Grid>
                    </Frame>

                    <!-- Warning -->
                    <Label Text="This will reset all operations and cash!"
                           FontSize="11"
                           TextColor="{StaticResource Primary}"
                           HorizontalOptions="Center"
                           HorizontalTextAlignment="Center"/>

                    <!-- Prestige button -->
                    <Button Text="PRESTIGE NOW"
                            Command="{Binding PrestigeCommand}"
                            BackgroundColor="{StaticResource Gold}"
                            TextColor="{StaticResource Background}"
                            FontAttributes="Bold"
                            CornerRadius="8"
                            HeightRequest="50"/>

                    <!-- Later button -->
                    <Button Text="Maybe Later"
                            Command="{Binding DismissPrestigeModalCommand}"
                            BackgroundColor="Transparent"
                            TextColor="{StaticResource TextSecondary}"
                            FontSize="14"/>

                </VerticalStackLayout>

            </Grid>

        </Frame>
    </Grid>

</ContentView>
```

### Step 3: Code-Behind

Path: `src/MadeMan.IdleEmpire/Views/Components/PrestigeModal.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views.Components;

public partial class PrestigeModal : ContentView
{
    public PrestigeModal()
    {
        InitializeComponent();
    }
}
```

### Step 4: Tilfoej Badge med Animation til Header

Modificer header i MainPage.xaml:

```xml
<!-- Prestige Badge (in header) -->
<Frame Grid.Column="1"
       BackgroundColor="{StaticResource SurfaceLight}"
       CornerRadius="4"
       Padding="8,4"
       VerticalOptions="Center"
       BorderColor="Transparent">
    <Frame.GestureRecognizers>
        <TapGestureRecognizer Command="{Binding ShowPrestigeModalCommand}"/>
    </Frame.GestureRecognizers>

    <!-- Glow effect when prestige available -->
    <Frame.Triggers>
        <DataTrigger TargetType="Frame"
                     Binding="{Binding CanPrestige}"
                     Value="True">
            <Setter Property="BackgroundColor" Value="{StaticResource Primary}"/>
        </DataTrigger>
    </Frame.Triggers>

    <HorizontalStackLayout Spacing="4">
        <Label Text="⭐"
               FontSize="12"
               IsVisible="{Binding CanPrestige}"/>
        <Label Text="P:"
               FontSize="11"
               TextColor="{StaticResource TextSecondary}"/>
        <Label Text="{Binding PrestigeCount}"
               FontSize="11"
               FontAttributes="Bold"
               TextColor="{StaticResource Gold}"/>
    </HorizontalStackLayout>
</Frame>
```

### Step 5: Tilfoej Display Properties til MainViewModel

```csharp
public string CurrentPrestigeBonusDisplay =>
    $"{_gameEngine.State.PrestigeBonus:F2}x";

public string NextPrestigeBonusDisplay =>
    $"{_gameEngine.State.PrestigeBonus + GameConfig.PrestigeBonusPerReset:F2}x";
```

### Step 6: Fjern Eksisterende Prestige Panel

Fjern det gamle prestige panel fra MainPage.xaml (Grid.Row="4" Frame).

### Step 7: Tilfoej PrestigeModal til MainPage

```xml
<!-- Prestige Modal (OVER alt andet) -->
<components:PrestigeModal Grid.RowSpan="5"/>
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Manual Test in Emulator
- [ ] Badge i header viser prestige count
- [ ] Badge aendrer farve naar prestige tilgaengelig
- [ ] Tap paa badge aabner modal
- [ ] Modal viser current vs after bonus
- [ ] "X" knap lukker modal
- [ ] "Maybe Later" lukker modal
- [ ] "Prestige Now" udforer prestige og lukker modal
- [ ] Badge fortsat synlig efter modal lukkes

---

## Acceptance Criteria

- [ ] Prestige modal har "Luk" knap
- [ ] Modal kan lukkes uden at prestige
- [ ] Badge i header indikerer tilgaengelig prestige
- [ ] Tap paa badge genaaabner modal
- [ ] Badge styling aendres naar prestige tilgaengelig
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Modal component**: Selvstaendig, genbrugelig
- **DataTrigger for glow**: Ingen custom animation code
- **Eksisterende prestige logic**: Genbrug CanPrestige og DoPrestige

### Alternativer overvejet

**Alternative 1: Notification badge**
- Lille roed badge paa ikon
- **Hvorfor fravalgt**: Ikke nok synlighed

**Alternative 2: Auto-show modal**
- Vis modal automatisk naar prestige tilgaengelig
- **Hvorfor fravalgt**: Nuvaerende opfoersel (bruger kontrollerer)

### Potentielle forbedringer (v2)
- Pulse animation paa badge
- Confetti ved prestige
- Prestige statistik (total prestiges, fastest prestige)

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Modal med simpel state
- [ ] **Laesbarhed**: Klar XAML struktur
- [ ] **DRY**: Genbrug eksisterende prestige logic
- [ ] **Performance**: Ingen tunge beregninger
- [ ] **Testbarhed**: Modal kan vises/skjules i isolation

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
