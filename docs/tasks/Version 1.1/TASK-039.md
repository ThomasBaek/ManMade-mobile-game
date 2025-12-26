# Task 039: Titel System

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-029, TASK-038
- **Estimated Time**: 1.5-2 timer
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 8)
- **Frequency Impact**: NO

---

## Formaal

Implementer et titel-system hvor spilleren starter med laveste titel og avancerer gennem prestige levels til "Godfather".

**Hvorfor dette er vigtigt:**
- Giver sense of progression
- Tematisk pasende for mafia-spil
- Ekstra motivation til prestige

---

## Risici

### Potentielle Problemer
1. **Titel oploeves som meningsloes**:
   - Edge case: Bare kosmetisk uden gameplay impact
   - Impact: Spilleren ignorerer titler

2. **For hurtig/langsom progression**:
   - Titler kommer for hurtigt eller for langsomt
   - Impact: Mindre tilfredsstillende

### Mitigering
- Velplacerede thresholds (se tabel)
- Overvej lille bonus per titel (optional)
- Unlock celebration

---

## Analyse - Hvad Skal Implementeres

### Titel Progression Tabel

| Prestige Level | Titel | Beskrivelse |
|----------------|-------|-------------|
| 0 | Street Punk | Du er ingen... endnu |
| 1 | Pickpocket | Du stjaeler til dagen og vejen |
| 2 | Thug | Folk begynder at frygte dig |
| 3 | Enforcer | Du driver penge ind for andre |
| 5 | Soldato | Du er nu en del af familien |
| 7 | Caporegime | Du leder dit eget crew |
| 10 | Underboss | Kun een over dig nu |
| 15 | Consigliere | Raadgiver til de maegtige |
| 20+ | Godfather | Du ER familien |

### UI Integration

**Header med Titel:**
```
+----------------------------------------+
| ENFORCER                    P: 3       |
| "Du driver penge ind for andre"        |
+----------------------------------------+
```

**Titel Unlock Popup:**
```
+--------------------------------------+
|                                      |
|       TITLE UNLOCKED!                |
|                                      |
|          SOLDATO                     |
|                                      |
|   "Du er nu en del af familien"      |
|                                      |
|         [FORTSAT]                    |
|                                      |
+--------------------------------------+
```

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-029 (farver)
- [x] TASK-038 (prestige modal)

**Assumptions:**
- Titel er ren kosmetik for MVP
- Popup vises ved prestige naar ny titel

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Opret TitleConfig Model

Path: `src/MadeMan.IdleEmpire/Models/TitleConfig.cs`

```csharp
namespace MadeMan.IdleEmpire.Models;

public static class TitleConfig
{
    public static readonly TitleLevel[] Titles = new[]
    {
        new TitleLevel(0, "Street Punk", "Du er ingen... endnu"),
        new TitleLevel(1, "Pickpocket", "Du stjaeler til dagen og vejen"),
        new TitleLevel(2, "Thug", "Folk begynder at frygte dig"),
        new TitleLevel(3, "Enforcer", "Du driver penge ind for andre"),
        new TitleLevel(5, "Soldato", "Du er nu en del af familien"),
        new TitleLevel(7, "Caporegime", "Du leder dit eget crew"),
        new TitleLevel(10, "Underboss", "Kun een over dig nu"),
        new TitleLevel(15, "Consigliere", "Raadgiver til de maegtige"),
        new TitleLevel(20, "Godfather", "Du ER familien")
    };

    public static TitleLevel GetTitle(int prestigeLevel)
    {
        TitleLevel result = Titles[0];
        foreach (var title in Titles)
        {
            if (prestigeLevel >= title.RequiredPrestige)
            {
                result = title;
            }
            else
            {
                break;
            }
        }
        return result;
    }

    public static TitleLevel? GetNextTitle(int prestigeLevel)
    {
        foreach (var title in Titles)
        {
            if (title.RequiredPrestige > prestigeLevel)
            {
                return title;
            }
        }
        return null; // Already max title
    }
}

public record TitleLevel(int RequiredPrestige, string Name, string Description);
```

### Step 2: Tilfoej Titel Properties til MainViewModel

Path: `src/MadeMan.IdleEmpire/ViewModels/MainViewModel.cs`

```csharp
// Current title
public string CurrentTitle => TitleConfig.GetTitle(_gameEngine.State.PrestigeCount).Name;
public string CurrentTitleDescription => TitleConfig.GetTitle(_gameEngine.State.PrestigeCount).Description;

// Next title info
public string? NextTitleName => TitleConfig.GetNextTitle(_gameEngine.State.PrestigeCount)?.Name;
public int? NextTitlePrestige => TitleConfig.GetNextTitle(_gameEngine.State.PrestigeCount)?.RequiredPrestige;

// Title unlock popup
[ObservableProperty]
private bool _showTitleUnlockPopup;

[ObservableProperty]
private string _unlockedTitleName = "";

[ObservableProperty]
private string _unlockedTitleDescription = "";

[RelayCommand]
private void DismissTitlePopup()
{
    ShowTitleUnlockPopup = false;
}
```

### Step 3: Modificer DoPrestige til at Checke for Ny Titel

```csharp
[RelayCommand]
private void Prestige()
{
    if (!CanPrestige) return;

    // Remember current title
    var oldTitle = TitleConfig.GetTitle(_gameEngine.State.PrestigeCount);

    // Do prestige
    _gameEngine.DoPrestige();
    IsPrestigeModalVisible = false;

    // Check for new title
    var newTitle = TitleConfig.GetTitle(_gameEngine.State.PrestigeCount);
    if (newTitle != oldTitle)
    {
        UnlockedTitleName = newTitle.Name;
        UnlockedTitleDescription = newTitle.Description;
        ShowTitleUnlockPopup = true;
    }

    // Update all properties
    OnPropertyChanged(nameof(CurrentTitle));
    OnPropertyChanged(nameof(CurrentTitleDescription));
    // ... other property notifications
}
```

### Step 4: Opret TitleUnlockModal Component

Path: `src/MadeMan.IdleEmpire/Views/Components/TitleUnlockModal.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MadeMan.IdleEmpire.Views.Components.TitleUnlockModal"
             IsVisible="{Binding ShowTitleUnlockPopup}">

    <!-- Dark overlay -->
    <Grid BackgroundColor="#CC000000">

        <!-- Modal Card -->
        <Frame BackgroundColor="{StaticResource Surface}"
               CornerRadius="20"
               Padding="32"
               Margin="32"
               VerticalOptions="Center"
               HorizontalOptions="Center"
               MaximumWidthRequest="320"
               BorderColor="{StaticResource Gold}">

            <VerticalStackLayout Spacing="16">

                <!-- Celebration icon -->
                <Label Text="🎉"
                       FontSize="48"
                       HorizontalOptions="Center"/>

                <!-- Header -->
                <Label Text="TITLE UNLOCKED!"
                       FontSize="16"
                       FontAttributes="Bold"
                       TextColor="{StaticResource Gold}"
                       HorizontalOptions="Center"/>

                <!-- Star decorations -->
                <HorizontalStackLayout HorizontalOptions="Center" Spacing="8">
                    <Label Text="⭐" FontSize="24"/>
                    <Label Text="{Binding UnlockedTitleName}"
                           FontSize="28"
                           FontAttributes="Bold"
                           TextColor="{StaticResource TextPrimary}"/>
                    <Label Text="⭐" FontSize="24"/>
                </HorizontalStackLayout>

                <!-- Description -->
                <Label Text="{Binding UnlockedTitleDescription}"
                       FontSize="14"
                       TextColor="{StaticResource TextSecondary}"
                       HorizontalOptions="Center"
                       HorizontalTextAlignment="Center"
                       FontAttributes="Italic"/>

                <!-- Continue button -->
                <Button Text="CONTINUE"
                        Command="{Binding DismissTitlePopupCommand}"
                        BackgroundColor="{StaticResource Gold}"
                        TextColor="{StaticResource Background}"
                        FontAttributes="Bold"
                        CornerRadius="8"
                        HeightRequest="50"
                        Margin="0,16,0,0"/>

            </VerticalStackLayout>

        </Frame>
    </Grid>

</ContentView>
```

### Step 5: Code-Behind

Path: `src/MadeMan.IdleEmpire/Views/Components/TitleUnlockModal.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views.Components;

public partial class TitleUnlockModal : ContentView
{
    public TitleUnlockModal()
    {
        InitializeComponent();
    }
}
```

### Step 6: Tilfoej Titel Display til Header

Modificer header i MainPage.xaml:

```xml
<!-- Title display in header -->
<VerticalStackLayout Spacing="2">
    <HorizontalStackLayout Spacing="4">
        <Label Text="🎩"
               FontSize="14"/>
        <Label Text="{Binding CurrentTitle}"
               FontSize="14"
               FontAttributes="Bold"
               TextColor="{StaticResource Gold}"/>
    </HorizontalStackLayout>
    <Label Text="{Binding CurrentTitleDescription}"
           FontSize="10"
           TextColor="{StaticResource TextSecondary}"
           LineBreakMode="TailTruncation"
           MaximumWidthRequest="200"/>
</VerticalStackLayout>
```

### Step 7: Tilfoej Modal til MainPage

```xml
<!-- Title Unlock Modal (OVER alt andet) -->
<components:TitleUnlockModal Grid.RowSpan="5"/>
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Manual Test in Emulator
- [ ] Titel vises i header
- [ ] Starter som "Street Punk" ved prestige 0
- [ ] Prestige 1 -> "Pickpocket" med popup
- [ ] Prestige 2 -> "Thug" med popup
- [ ] Description matcher titel
- [ ] Popup kan lukkes med "Continue"

---

## Acceptance Criteria

- [ ] Titel vises i header
- [ ] Titel matcher prestige level
- [ ] Popup ved ny titel unlock
- [ ] Alle 9 titler implementeret
- [ ] Description vises for hver titel
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Static config**: Titler defineret i TitleConfig
- **Simpel lookup**: Linear search er fin for 9 items
- **Ingen database**: Alt i memory

### Alternativer overvejet

**Alternative 1: Titler med bonusser**
- Hver titel giver +1% income bonus
- **Hvorfor fravalgt**: Komplicerer balance, kan tilfojes senere

**Alternative 2: Achievement system**
- Titler som achievements med badges
- **Hvorfor fravalgt**: Over-engineering for MVP

### Potentielle forbedringer (v2)
- Titel bonusser (+X% income)
- Achievement badges
- Title progress bar til naeste
- Share titel paa sociale medier

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Static lookup tabel
- [ ] **Laesbarhed**: Record type for TitleLevel
- [ ] **DRY**: Centrale config
- [ ] **Performance**: O(n) lookup, n=9
- [ ] **Testbarhed**: TitleConfig kan unit testes

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
