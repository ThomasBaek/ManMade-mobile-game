# Task 035: Welcome Back / Offline Earnings Modal

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-029, TASK-032
- **Estimated Time**: 1.5-2 timer
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 3)
- **Frequency Impact**: NO

---

## Formaal

Vis en dedikeret modal ved app-aabning der fortaeller spilleren hvad der skete mens de var vaek (offline earnings).

**Hvorfor dette er vigtigt:**
- Skaber "wow"-moment ved tilbagevenden
- Viser offline earnings dramatisk
- Motiverer spilleren til at fortsaette

---

## Risici

### Potentielle Problemer
1. **Modal vises for ofte**:
   - Edge case: Kort app-skift trigger modal
   - Impact: Irriterende for bruger

2. **Beregning allerede eksisterer**:
   - GameEngine har CalculateOfflineEarnings
   - Impact: Skal integreres, ikke duplikeres

### Mitigering
- Kun vis modal hvis vaek > 1 minut
- Genbrug eksisterende beregning, gem resultat

---

## Analyse - Hvad Skal Implementeres

### Modal Layout
```
+------------------------------------------+
|                                          |
|     VELKOMMEN TILBAGE, BOSS              |
|                                          |
|   Mens du var vaek i 4t 23m              |
|   har dit imperium tjent:                |
|                                          |
|        $127,543.00                       |
|        ~~~~~~~~~~~~~~~~                  |
|                                          |
|   Income rate: $50/sek                   |
|   Tid vaek: 4 timer                      |
|   Efficiency: 50%                        |
|                                          |
|   +----------------------------------+   |
|   |         INDSAML ALT              |   |
|   +----------------------------------+   |
|                                          |
|        [Tryk for at fortsaette]          |
|                                          |
+------------------------------------------+
```

### Data Kraevet
- Offline earnings beloeb (allerede beregnet)
- Tid vaek (fra LastPlayedUtc)
- Income rate ved close
- Efficiency % (fra GameConfig)

### Flow
1. App aabnes
2. GameEngine.Initialize() beregner offline earnings
3. Hvis earnings > 0 OG tid > 1 minut -> vis modal
4. Bruger trykker "Indsaml" -> modal lukkes
5. Cash er allerede tilfojet (af GameEngine)

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-029 (farver)
- [x] Eksisterende offline earnings logic i GameEngine

**Assumptions:**
- GameEngine tilfojer allerede cash ved Initialize
- Modal viser bare hvad der skete

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Udvid GameEngine med Offline Info

Path: `src/MadeMan.IdleEmpire/Services/GameEngine.cs`

Tilfoej properties til at eksponere offline earnings info:

```csharp
// Add to GameEngine class
public double LastOfflineEarnings { get; private set; }
public TimeSpan LastOfflineTime { get; private set; }
public double LastOfflineEfficiency { get; private set; }

private void CalculateOfflineEarnings()
{
    var offlineTime = DateTime.UtcNow - State.LastPlayedUtc;
    LastOfflineTime = offlineTime;

    var maxHours = GameConfig.MaxOfflineHours + _skillService.GetOfflineMaxHoursBonus();
    var hours = Math.Min(offlineTime.TotalHours, maxHours);

    if (hours > 0.01) // More than ~36 seconds
    {
        var efficiency = GameConfig.OfflineEfficiency + _skillService.GetOfflineEfficiencyBonus();
        efficiency = Math.Min(efficiency, 1.0);
        LastOfflineEfficiency = efficiency;

        // Calculate base offline income
        double baseIncomePerSecond = 0;
        foreach (var op in GameConfig.Operations)
        {
            var opState = GetOperationState(op.Id);
            if (opState != null && opState.Level > 0)
            {
                baseIncomePerSecond += op.GetIncome(opState.Level, State.PrestigeBonus);
            }
        }

        double earnings = baseIncomePerSecond * hours * 3600 * efficiency;
        earnings *= _skillService.GetOfflineEarningsMultiplier();

        LastOfflineEarnings = earnings;
        State.Cash += earnings;
        State.TotalEarned += earnings;
    }
    else
    {
        LastOfflineEarnings = 0;
        LastOfflineEfficiency = 0;
    }

    State.LastPlayedUtc = DateTime.UtcNow;
}
```

### Step 2: Opret WelcomeBackModal Component

Path: `src/MadeMan.IdleEmpire/Views/Components/WelcomeBackModal.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MadeMan.IdleEmpire.Views.Components.WelcomeBackModal"
             IsVisible="{Binding IsVisible, Source={x:Reference thisModal}}"
             x:Name="thisModal">

    <!-- Dark overlay -->
    <Grid BackgroundColor="#CC000000">
        <Grid.GestureRecognizers>
            <TapGestureRecognizer Tapped="OnOverlayTapped"/>
        </Grid.GestureRecognizers>

        <!-- Modal Card -->
        <Frame BackgroundColor="{StaticResource Surface}"
               CornerRadius="20"
               Padding="24"
               Margin="24"
               VerticalOptions="Center"
               HorizontalOptions="Center"
               MaximumWidthRequest="400"
               BorderColor="Transparent">

            <VerticalStackLayout Spacing="16">

                <!-- Header -->
                <Label Text="VELKOMMEN TILBAGE"
                       FontSize="20"
                       FontAttributes="Bold"
                       TextColor="{StaticResource Gold}"
                       HorizontalOptions="Center"/>

                <Label Text="BOSS"
                       FontSize="28"
                       FontAttributes="Bold"
                       TextColor="{StaticResource TextPrimary}"
                       HorizontalOptions="Center"
                       Margin="0,-8,0,0"/>

                <!-- Time Away -->
                <Label x:Name="TimeAwayLabel"
                       FontSize="14"
                       TextColor="{StaticResource TextSecondary}"
                       HorizontalOptions="Center"/>

                <Label Text="har dit imperium tjent:"
                       FontSize="14"
                       TextColor="{StaticResource TextSecondary}"
                       HorizontalOptions="Center"/>

                <!-- Earnings Amount -->
                <Label x:Name="EarningsLabel"
                       FontSize="36"
                       FontAttributes="Bold"
                       TextColor="{StaticResource Gold}"
                       HorizontalOptions="Center"/>

                <BoxView HeightRequest="1"
                         BackgroundColor="{StaticResource Gold}"
                         Opacity="0.3"
                         Margin="20,0"/>

                <!-- Stats -->
                <Grid ColumnDefinitions="*,*" RowDefinitions="Auto,Auto" ColumnSpacing="16" RowSpacing="8">
                    <Label Text="Tid vaek:"
                           FontSize="12"
                           TextColor="{StaticResource TextSecondary}"/>
                    <Label x:Name="TimeStatLabel"
                           Grid.Column="1"
                           FontSize="12"
                           TextColor="{StaticResource TextPrimary}"
                           HorizontalOptions="End"/>

                    <Label Grid.Row="1"
                           Text="Efficiency:"
                           FontSize="12"
                           TextColor="{StaticResource TextSecondary}"/>
                    <Label x:Name="EfficiencyLabel"
                           Grid.Row="1"
                           Grid.Column="1"
                           FontSize="12"
                           TextColor="{StaticResource TextPrimary}"
                           HorizontalOptions="End"/>
                </Grid>

                <!-- Collect Button -->
                <Button Text="INDSAML ALT"
                        BackgroundColor="{StaticResource Gold}"
                        TextColor="{StaticResource Background}"
                        FontAttributes="Bold"
                        CornerRadius="8"
                        HeightRequest="50"
                        Margin="0,8,0,0"
                        Clicked="OnCollectClicked"/>

            </VerticalStackLayout>

        </Frame>
    </Grid>

</ContentView>
```

### Step 3: WelcomeBackModal Code-Behind

Path: `src/MadeMan.IdleEmpire/Views/Components/WelcomeBackModal.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views.Components;

public partial class WelcomeBackModal : ContentView
{
    public static readonly BindableProperty IsVisibleProperty =
        BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(WelcomeBackModal), false);

    public new bool IsVisible
    {
        get => (bool)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    public WelcomeBackModal()
    {
        InitializeComponent();
    }

    public void Show(double earnings, TimeSpan timeAway, double efficiency)
    {
        // Format time away
        var hours = (int)timeAway.TotalHours;
        var minutes = timeAway.Minutes;
        var timeText = hours > 0
            ? $"Mens du var vaek i {hours}t {minutes}m"
            : $"Mens du var vaek i {minutes} minutter";
        TimeAwayLabel.Text = timeText;

        // Format earnings
        EarningsLabel.Text = $"${FormatNumber(earnings)}";

        // Stats
        TimeStatLabel.Text = hours > 0 ? $"{hours}t {minutes}m" : $"{minutes}m";
        EfficiencyLabel.Text = $"{(int)(efficiency * 100)}%";

        IsVisible = true;
    }

    private void OnCollectClicked(object sender, EventArgs e)
    {
        IsVisible = false;
    }

    private void OnOverlayTapped(object sender, TappedEventArgs e)
    {
        // Allow closing by tapping outside (optional)
        // IsVisible = false;
    }

    private string FormatNumber(double value)
    {
        if (value >= 1_000_000_000) return $"{value / 1_000_000_000:F2}B";
        if (value >= 1_000_000) return $"{value / 1_000_000:F2}M";
        if (value >= 1_000) return $"{value / 1_000:F2}K";
        return $"{value:F2}";
    }
}
```

### Step 4: Integrer i MainPage

Path: `src/MadeMan.IdleEmpire/Views/MainPage.xaml`

Tilfoej modal til bunden af Grid:
```xml
<!-- Skill Selection Modal -->
<components:SkillSelectionModal Grid.RowSpan="5"/>

<!-- Welcome Back Modal (skal vaere OVER skill modal) -->
<components:WelcomeBackModal x:Name="WelcomeBackModal" Grid.RowSpan="5"/>
```

### Step 5: Trigger Modal i MainPage.xaml.cs

Path: `src/MadeMan.IdleEmpire/Views/MainPage.xaml.cs`

```csharp
protected override void OnAppearing()
{
    base.OnAppearing();

    // Check for offline earnings to display
    ShowWelcomeBackIfNeeded();
}

private void ShowWelcomeBackIfNeeded()
{
    var engine = _viewModel.GameEngine; // Need to expose or inject

    // Only show if away for more than 1 minute and earned something
    if (engine.LastOfflineTime.TotalMinutes >= 1 && engine.LastOfflineEarnings > 0)
    {
        WelcomeBackModal.Show(
            engine.LastOfflineEarnings,
            engine.LastOfflineTime,
            engine.LastOfflineEfficiency);
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
- [ ] Luk app og vent > 1 minut
- [ ] Genaabn app
- [ ] Welcome Back modal vises
- [ ] Korrekt beloeb vises
- [ ] Tid vaek vises korrekt
- [ ] "Indsaml" knap lukker modal
- [ ] Cash er allerede tilfojet

---

## Acceptance Criteria

- [ ] Modal vises ved app-aabning (hvis earnings > 0)
- [ ] Viser korrekt offline earnings beloeb
- [ ] Viser tid vaek formateret
- [ ] Viser efficiency %
- [ ] "Indsaml" knap lukker modal
- [ ] Vises IKKE hvis vaek < 1 minut
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Genbrug logic**: Bruger eksisterende CalculateOfflineEarnings
- **Modal component**: Selvstaendig, genbrugelig
- **Ingen ny persistens**: Bruger eksisterende GameState

### Alternativer overvejet

**Alternative 1: Toast notification**
- Vis earnings som toast i stedet for modal
- **Hvorfor fravalgt**: Mindre dramatisk, kan misse info

**Alternative 2: Dedicated page**
- Separat Welcome Back page
- **Hvorfor fravalgt**: Over-engineering, modal er tilstraekkeligt

### Potentielle forbedringer (v2)
- Count-up animation paa beloeb
- Confetti effect
- Sammenligning med forrige session

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Simpel modal med binding
- [ ] **Laesbarhed**: Klar separation
- [ ] **DRY**: Formattering genbrugt
- [ ] **Performance**: Ingen tung beregning i UI
- [ ] **Testbarhed**: Modal kan testes isoleret

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
