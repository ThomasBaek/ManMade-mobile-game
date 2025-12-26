# Task 032: Top Bar (Cash/Income Display)

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-029, TASK-031
- **Estimated Time**: 1-2 timer
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 2)
- **Frequency Impact**: NO

---

## Formaal

Implementer en kompakt top bar der altid viser Cash, Income, Expenses og Net income - synlig paa alle tabs.

**Hvorfor dette er vigtigt:**
- Spilleren skal altid kunne se sin okonomi
- Nuvaerende header er for stor og kun paa MainPage
- Expenses placeholder for fremtidige crew costs

---

## Risici

### Potentielle Problemer
1. **Data binding paa tvaers af pages**:
   - Edge case: Top bar skal opdateres fra alle tabs
   - Impact: Stale data eller binding fejl

2. **Layout issues**:
   - Safe area pa forskellige devices
   - Impact: Top bar skjult af notch/status bar

### Mitigering
- Brug Shell.TitleView for konsistent top bar
- Eller: Implementer som global component med shared ViewModel
- Test paa devices med notch

---

## Analyse - Hvad Skal Implementeres

### Top Bar Layout
```
+----------------------------------------------------------+
|  $1,234,567  |  +$456/s  |  -$120/s  | NET: +$336/s      |
+----------------------------------------------------------+
```

### Elementer
| Element | Beskrivelse | Farve |
|---------|-------------|-------|
| Cash | Nuvaerende beholdning | Gold (#D4AF37) |
| Income | Brutto per sekund | Success (#4ADE80) |
| Expenses | Udgifter (placeholder: $0) | Primary (#8B0000) |
| Net | Income - Expenses | Afhaengig af fortegn |

### Design Specs
- Hoejde: ~50-60px
- Baggrund: Surface (#1A1A2E)
- Padding: Safe area + 8px

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-029 (farver)
- [x] TASK-031 (Shell navigation)

**Assumptions:**
- MainViewModel har Cash og Income properties
- Expenses er 0 for nu (ingen crew system)

**Blockers:**
- Ingen

---

## Implementation Guide

### Approach A: Shell TitleView (Anbefalet)

Shell TitleView vises automatisk paa alle pages.

### Step 1: Opret TopBarView Component

Path: `src/MadeMan.IdleEmpire/Views/Components/TopBarView.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:MadeMan.IdleEmpire.ViewModels"
             x:Class="MadeMan.IdleEmpire.Views.Components.TopBarView"
             x:DataType="vm:MainViewModel"
             BackgroundColor="{StaticResource Surface}"
             HeightRequest="50">

    <Grid ColumnDefinitions="*,Auto,Auto,Auto"
          Padding="12,0"
          VerticalOptions="Center">

        <!-- Cash -->
        <HorizontalStackLayout Spacing="4">
            <Label Text="$"
                   FontSize="14"
                   TextColor="{StaticResource Gold}"
                   VerticalOptions="Center"/>
            <Label Text="{Binding CashDisplay}"
                   FontSize="16"
                   FontAttributes="Bold"
                   TextColor="{StaticResource Gold}"
                   VerticalOptions="Center"/>
        </HorizontalStackLayout>

        <!-- Income -->
        <HorizontalStackLayout Grid.Column="1" Spacing="4" Margin="12,0">
            <Label Text="+"
                   FontSize="12"
                   TextColor="{StaticResource Success}"
                   VerticalOptions="Center"/>
            <Label Text="{Binding IncomeDisplay}"
                   FontSize="12"
                   TextColor="{StaticResource Success}"
                   VerticalOptions="Center"/>
        </HorizontalStackLayout>

        <!-- Expenses (placeholder) -->
        <HorizontalStackLayout Grid.Column="2" Spacing="4" Margin="12,0">
            <Label Text="-$0/s"
                   FontSize="12"
                   TextColor="{StaticResource Smoke}"
                   VerticalOptions="Center"/>
        </HorizontalStackLayout>

        <!-- Net -->
        <Frame Grid.Column="3"
               BackgroundColor="{StaticResource SurfaceLight}"
               CornerRadius="4"
               Padding="8,4"
               BorderColor="Transparent">
            <Label Text="{Binding NetIncomeDisplay}"
                   FontSize="12"
                   FontAttributes="Bold"
                   TextColor="{StaticResource Success}"
                   VerticalOptions="Center"/>
        </Frame>

    </Grid>

</ContentView>
```

Path: `src/MadeMan.IdleEmpire/Views/Components/TopBarView.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views.Components;

public partial class TopBarView : ContentView
{
    public TopBarView()
    {
        InitializeComponent();
    }
}
```

### Step 2: Opdater MainViewModel med Net Income

Path: `src/MadeMan.IdleEmpire/ViewModels/MainViewModel.cs`

Tilfoej property:
```csharp
public string NetIncomeDisplay => $"NET: {IncomeDisplay}";
```

Eller hvis expenses implementeres:
```csharp
private double _expenses = 0; // Placeholder

public double NetIncome => _gameEngine.IncomePerSecond - _expenses;

public string NetIncomeDisplay
{
    get
    {
        var net = NetIncome;
        var prefix = net >= 0 ? "+" : "";
        return $"NET: {prefix}${FormatNumber(net)}/s";
    }
}
```

### Step 3: Tilfoej TopBar til AppShell

Path: `src/MadeMan.IdleEmpire/AppShell.xaml`

Modificer Shell til at inkludere header:
```xml
<Shell ...
       Shell.NavBarIsVisible="True">

    <Shell.TitleView>
        <views:TopBarView BindingContext="{Binding Source={x:Reference shell}, Path=BindingContext}"/>
    </Shell.TitleView>

    <!-- TabBar som før -->
</Shell>
```

### Alternative: Custom Layout

Hvis Shell.TitleView ikke virker som forventet, kan TopBar tilfojes direkte til hver page eller via en custom Shell template.

### Step 4: Fjern gammel header fra MainPage

MainPage har allerede en Cash header - den skal enten fjernes eller konverteres til kompakt format.

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Manual Test in Emulator
- [ ] Top bar synlig paa alle tabs
- [ ] Cash opdateres live
- [ ] Income vises korrekt
- [ ] Expenses viser $0 (placeholder)
- [ ] Net beregnes korrekt
- [ ] Kompakt hoejde (~50px)

---

## Acceptance Criteria

- [ ] Top bar viser Cash, Income, Expenses, Net
- [ ] Top bar synlig paa alle tabs
- [ ] Live opdatering fra game engine
- [ ] Kompakt design (~50px hoejde)
- [ ] Korrekte farver (gold, green, red)
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Reuse MainViewModel**: Brug eksisterende bindings
- **Shell.TitleView**: Native lösning for global header
- **Placeholder expenses**: Ingen ny logik for nu

### Alternativer overvejet

**Alternative 1: Custom Header paa hver page**
- Kopier header til alle pages
- **Hvorfor fravalgt**: DRY violation, maintenance nightmare

**Alternative 2: AbsoluteLayout overlay**
- Floating header over content
- **Hvorfor fravalgt**: Kompleks, potentielle z-index issues

### Potentielle forbedringer (v2)
- Animeret cash opdatering (count-up effect)
- Expense tracking fra crew system
- Prestige badge i top bar

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Shell.TitleView er simplest
- [ ] **Laesbarhed**: TopBarView er selvstaendig component
- [ ] **Navngivning**: Klare property navne
- [ ] **DRY**: En komponent, genbrugt paa alle tabs
- [ ] **Performance**: Binding er effektivt
- [ ] **Testbarhed**: Visuelt verificerbar

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
