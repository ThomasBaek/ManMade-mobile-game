# Task 034: Splash Screen

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-029, TASK-030
- **Estimated Time**: 1-2 timer
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 1)
- **Frequency Impact**: NO

---

## Formaal

Implementer en professionel splash screen der vises i 2-3 sekunder ved app start og saetter stemningen for spillet.

**Hvorfor dette er vigtigt:**
- Foerste indtryk af spillet
- Branding og identitet
- Smooth overgang til gameplay

---

## Risici

### Potentielle Problemer
1. **For lang loading tid**:
   - Edge case: Splash blokerer for gameplay
   - Impact: Frustreret bruger

2. **Platform-specifikke issues**:
   - Android native splash vs. MAUI splash
   - Impact: Dobbelt splash eller manglende splash

### Mitigering
- Brug MAUI splash page (ikke native)
- Max 2-3 sekunder
- Start game loading i baggrunden

---

## Analyse - Hvad Skal Implementeres

### Splash Screen Layout
```
+------------------------------------------+
|                                          |
|                                          |
|          [MADE MAN LOGO]                 |
|                                          |
|        ========================          |
|           IDLE EMPIRE                    |
|                                          |
|            Loading...                    |
|                                          |
|                                          |
|       [1930s skyline silhouette]         |
+------------------------------------------+
```

### Design Elementer
- Logo: Tekst eller SVG (fra TASK-030)
- Titel: "IDLE EMPIRE" undertitel
- Loading indikator
- Moerk baggrund matching tema

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-029 (farver)
- [x] TASK-030 (ikon/logo - kan bruges)

**Assumptions:**
- Splash vises som MAUI page
- Navigation til MainPage efter delay

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Opret SplashPage

Path: `src/MadeMan.IdleEmpire/Views/SplashPage.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MadeMan.IdleEmpire.Views.SplashPage"
             BackgroundColor="{StaticResource Background}"
             Shell.NavBarIsVisible="False"
             Shell.TabBarIsVisible="False">

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <!-- Logo/Title Section -->
        <VerticalStackLayout Grid.Row="1"
                             HorizontalOptions="Center"
                             Spacing="8">

            <!-- Fedora Icon (SVG or Emoji placeholder) -->
            <Label Text="🎩"
                   FontSize="72"
                   HorizontalOptions="Center"/>

            <!-- Main Title -->
            <Label Text="MADE MAN"
                   FontSize="48"
                   FontAttributes="Bold"
                   TextColor="{StaticResource Gold}"
                   HorizontalOptions="Center"
                   FontFamily="Georgia"/>

            <!-- Divider -->
            <BoxView WidthRequest="200"
                     HeightRequest="2"
                     BackgroundColor="{StaticResource Gold}"
                     HorizontalOptions="Center"
                     Margin="0,8"/>

            <!-- Subtitle -->
            <Label Text="IDLE EMPIRE"
                   FontSize="24"
                   TextColor="{StaticResource TextSecondary}"
                   HorizontalOptions="Center"
                   CharacterSpacing="4"/>

        </VerticalStackLayout>

        <!-- Loading Indicator -->
        <VerticalStackLayout Grid.Row="2"
                             HorizontalOptions="Center"
                             Margin="0,40,0,0">

            <ActivityIndicator IsRunning="True"
                               Color="{StaticResource Gold}"
                               WidthRequest="40"
                               HeightRequest="40"/>

            <Label Text="Loading..."
                   FontSize="14"
                   TextColor="{StaticResource TextSecondary}"
                   HorizontalOptions="Center"
                   Margin="0,8,0,0"/>

        </VerticalStackLayout>

        <!-- Skyline Silhouette (placeholder) -->
        <VerticalStackLayout Grid.Row="4"
                             VerticalOptions="End"
                             Margin="0,0,0,20">

            <!-- Simple cityscape using shapes -->
            <Grid HeightRequest="60" HorizontalOptions="Fill">
                <BoxView BackgroundColor="{StaticResource Surface}"
                         HorizontalOptions="Start"
                         WidthRequest="40"
                         HeightRequest="60"
                         Margin="20,0,0,0"/>
                <BoxView BackgroundColor="{StaticResource Surface}"
                         HorizontalOptions="Start"
                         WidthRequest="30"
                         HeightRequest="40"
                         Margin="70,20,0,0"/>
                <BoxView BackgroundColor="{StaticResource Surface}"
                         HorizontalOptions="Center"
                         WidthRequest="50"
                         HeightRequest="55"
                         Margin="-60,5,0,0"/>
                <BoxView BackgroundColor="{StaticResource Surface}"
                         HorizontalOptions="Center"
                         WidthRequest="35"
                         HeightRequest="45"
                         Margin="40,15,0,0"/>
                <BoxView BackgroundColor="{StaticResource Surface}"
                         HorizontalOptions="End"
                         WidthRequest="45"
                         HeightRequest="50"
                         Margin="0,10,60,0"/>
                <BoxView BackgroundColor="{StaticResource Surface}"
                         HorizontalOptions="End"
                         WidthRequest="30"
                         HeightRequest="35"
                         Margin="0,25,20,0"/>
            </Grid>

            <!-- Copyright -->
            <Label Text="© 2024 Made Man Studios"
                   FontSize="10"
                   TextColor="{StaticResource Smoke}"
                   HorizontalOptions="Center"
                   Margin="0,8,0,0"/>

        </VerticalStackLayout>

    </Grid>

</ContentPage>
```

### Step 2: Opret SplashPage Code-Behind

Path: `src/MadeMan.IdleEmpire/Views/SplashPage.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Wait for splash duration
        await Task.Delay(2500);

        // Navigate to main app
        Application.Current.MainPage = new AppShell();
    }
}
```

### Step 3: Opdater App.xaml.cs

Path: `src/MadeMan.IdleEmpire/App.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Start with splash page
        return new Window(new Views.SplashPage());
    }
}
```

### Step 4: Tilfoej Fade Animation (Optional)

For smooth overgang, tilfoej fade-out animation i SplashPage:

```csharp
protected override async void OnAppearing()
{
    base.OnAppearing();

    // Wait for splash duration
    await Task.Delay(2000);

    // Fade out
    await this.FadeTo(0, 500);

    // Navigate to main app
    Application.Current.MainPage = new AppShell();
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
- [ ] Splash vises ved app start
- [ ] Logo og titel er synlige
- [ ] Loading indikator animerer
- [ ] Efter ~2.5 sek navigeres til main app
- [ ] Transition er smooth (fade eller instant)

---

## Acceptance Criteria

- [ ] Splash screen vises ved app start
- [ ] Logo og "MADE MAN" titel vises
- [ ] "IDLE EMPIRE" undertitel vises
- [ ] Loading indikator synlig
- [ ] Auto-navigation efter 2-3 sekunder
- [ ] Smooth transition til gameplay
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **MAUI ContentPage**: Standard MAUI approach
- **Emoji placeholder**: Ingen ekstern grafik kraevet
- **Simple timer**: Task.Delay er simpelt

### Alternativer overvejet

**Alternative 1: Native Android Splash**
- Brug Android-specifik splash screen API
- **Hvorfor fravalgt**: Platform-specifik, mere kompleks

**Alternative 2: Lottie Animation**
- Animated splash med Lottie
- **Hvorfor fravalgt**: Ekstra dependency, over-engineering

### Potentielle forbedringer (v2)
- Professionel logo grafik
- Lottie animation
- Progress bar for faktisk loading
- Version nummer

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Simpel ContentPage med timer
- [ ] **Laesbarhed**: Klar struktur
- [ ] **Performance**: Minimal overhead
- [ ] **Testbarhed**: Visuelt verificerbar

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
