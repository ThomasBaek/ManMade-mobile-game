# Task 031: Bottom Navigation

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-029
- **Estimated Time**: 2-3 timer
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 2)
- **Frequency Impact**: NO

---

## Formaal

Implementer bottom navigation med 5 tabs som beskrevet i EXPANSION_ROADMAP for at forberede appen til fremtidige features.

**Hvorfor dette er vigtigt:**
- Nuvaerende single-page design skalerer ikke
- Forberedelse til Casino, Org Crime, Skills tabs
- Professional app struktur

---

## Risici

### Potentielle Problemer
1. **Navigation complexity**:
   - Edge case: State management mellem tabs
   - Impact: Data tab eller duplikerede views

2. **Performance**:
   - Multiple pages kan oege memory forbrug
   - Impact: Langsommere navigation

### Mitigering
- Brug Shell TabBar for native performance
- Hold placeholder pages simple
- Test pa aeldre devices

---

## Analyse - Hvad Skal Implementeres

### Navigation Struktur

| Tab | Ikon | Navn | Unlock | Indhold |
|-----|------|------|--------|---------|
| 1 | Home icon | Empire | Start | Nuvaerende MainPage |
| 2 | Briefcase | Org Crime | Prestige 2 | Placeholder (laast) |
| 3 | Dice | Casino | Prestige 1 | Placeholder (laast) |
| 4 | Chart | Skills | Prestige 1 | Placeholder (laast) |
| 5 | Gear | Settings | Start | Settings page |

### Visuel Design
```
+-------+-------+-------+-------+-------+
|  Home | Crime |Casino |Skills |  Cog  |
|  [*]  | [lock]|[lock] |[lock] |  [*]  |
+-------+-------+-------+-------+-------+
```

### Laast Tab Behavior
- Visuelt grayed out med laas-ikon
- Tap viser kort toast: "Unlock at Prestige X"
- Ikke navigerbar

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-029 (farver)
- [x] Eksisterende MainPage fungerer

**Assumptions:**
- Shell navigation er den rette tilgang
- GameState har PrestigeCount property

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Opdater AppShell.xaml

Path: `src/MadeMan.IdleEmpire/AppShell.xaml`

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
    x:Class="MadeMan.IdleEmpire.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:views="clr-namespace:MadeMan.IdleEmpire.Views"
    Shell.FlyoutBehavior="Disabled"
    Shell.NavBarIsVisible="False"
    Shell.TabBarBackgroundColor="{StaticResource Surface}"
    Shell.TabBarTitleColor="{StaticResource TextSecondary}"
    Shell.TabBarUnselectedColor="{StaticResource Smoke}"
    Shell.TabBarForegroundColor="{StaticResource Gold}">

    <TabBar>
        <ShellContent
            Title="Empire"
            Icon="icon_empire.png"
            ContentTemplate="{DataTemplate views:MainPage}"
            Route="Empire" />

        <ShellContent
            Title="Crime"
            Icon="icon_crime.png"
            ContentTemplate="{DataTemplate views:OrgCrimePage}"
            Route="OrgCrime" />

        <ShellContent
            Title="Casino"
            Icon="icon_casino.png"
            ContentTemplate="{DataTemplate views:CasinoPage}"
            Route="Casino" />

        <ShellContent
            Title="Skills"
            Icon="icon_skills.png"
            ContentTemplate="{DataTemplate views:SkillsPage}"
            Route="Skills" />

        <ShellContent
            Title="Settings"
            Icon="icon_settings.png"
            ContentTemplate="{DataTemplate views:SettingsPage}"
            Route="Settings" />
    </TabBar>

</Shell>
```

### Step 2: Opret Placeholder Pages

#### OrgCrimePage.xaml
Path: `src/MadeMan.IdleEmpire/Views/OrgCrimePage.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MadeMan.IdleEmpire.Views.OrgCrimePage"
             BackgroundColor="{StaticResource Background}"
             Shell.NavBarIsVisible="False">

    <VerticalStackLayout VerticalOptions="Center" HorizontalOptions="Center" Spacing="16">
        <Label Text="Locked"
               FontSize="24"
               FontAttributes="Bold"
               TextColor="{StaticResource Smoke}"
               HorizontalOptions="Center"/>
        <Label Text="Organized Crime"
               FontSize="32"
               FontAttributes="Bold"
               TextColor="{StaticResource TextPrimary}"
               HorizontalOptions="Center"/>
        <Label Text="Reach Prestige 2 to unlock"
               FontSize="16"
               TextColor="{StaticResource TextSecondary}"
               HorizontalOptions="Center"/>
    </VerticalStackLayout>

</ContentPage>
```

#### OrgCrimePage.xaml.cs
Path: `src/MadeMan.IdleEmpire/Views/OrgCrimePage.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views;

public partial class OrgCrimePage : ContentPage
{
    public OrgCrimePage()
    {
        InitializeComponent();
    }
}
```

#### CasinoPage.xaml
Path: `src/MadeMan.IdleEmpire/Views/CasinoPage.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MadeMan.IdleEmpire.Views.CasinoPage"
             BackgroundColor="{StaticResource Background}"
             Shell.NavBarIsVisible="False">

    <VerticalStackLayout VerticalOptions="Center" HorizontalOptions="Center" Spacing="16">
        <Label Text="Locked"
               FontSize="24"
               FontAttributes="Bold"
               TextColor="{StaticResource Smoke}"
               HorizontalOptions="Center"/>
        <Label Text="Casino"
               FontSize="32"
               FontAttributes="Bold"
               TextColor="{StaticResource TextPrimary}"
               HorizontalOptions="Center"/>
        <Label Text="Reach Prestige 1 to unlock"
               FontSize="16"
               TextColor="{StaticResource TextSecondary}"
               HorizontalOptions="Center"/>
    </VerticalStackLayout>

</ContentPage>
```

#### CasinoPage.xaml.cs
Path: `src/MadeMan.IdleEmpire/Views/CasinoPage.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views;

public partial class CasinoPage : ContentPage
{
    public CasinoPage()
    {
        InitializeComponent();
    }
}
```

#### SkillsPage.xaml
Path: `src/MadeMan.IdleEmpire/Views/SkillsPage.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MadeMan.IdleEmpire.Views.SkillsPage"
             BackgroundColor="{StaticResource Background}"
             Shell.NavBarIsVisible="False">

    <VerticalStackLayout VerticalOptions="Center" HorizontalOptions="Center" Spacing="16">
        <Label Text="Locked"
               FontSize="24"
               FontAttributes="Bold"
               TextColor="{StaticResource Smoke}"
               HorizontalOptions="Center"/>
        <Label Text="Skills"
               FontSize="32"
               FontAttributes="Bold"
               TextColor="{StaticResource TextPrimary}"
               HorizontalOptions="Center"/>
        <Label Text="Reach Prestige 1 to unlock"
               FontSize="16"
               TextColor="{StaticResource TextSecondary}"
               HorizontalOptions="Center"/>
    </VerticalStackLayout>

</ContentPage>
```

#### SkillsPage.xaml.cs
Path: `src/MadeMan.IdleEmpire/Views/SkillsPage.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views;

public partial class SkillsPage : ContentPage
{
    public SkillsPage()
    {
        InitializeComponent();
    }
}
```

#### SettingsPage.xaml
Path: `src/MadeMan.IdleEmpire/Views/SettingsPage.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MadeMan.IdleEmpire.Views.SettingsPage"
             BackgroundColor="{StaticResource Background}"
             Shell.NavBarIsVisible="False">

    <VerticalStackLayout VerticalOptions="Center" HorizontalOptions="Center" Spacing="16">
        <Label Text="Settings"
               FontSize="32"
               FontAttributes="Bold"
               TextColor="{StaticResource TextPrimary}"
               HorizontalOptions="Center"/>
        <Label Text="Coming in TASK-033"
               FontSize="16"
               TextColor="{StaticResource TextSecondary}"
               HorizontalOptions="Center"/>
    </VerticalStackLayout>

</ContentPage>
```

#### SettingsPage.xaml.cs
Path: `src/MadeMan.IdleEmpire/Views/SettingsPage.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }
}
```

### Step 3: Opret Tab Ikoner (SVG)

Opret simple SVG ikoner i `Resources/Images/`:

**icon_empire.svg:**
```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
  <path d="M12 3L4 9v12h16V9l-8-6zm0 2.5L18 10v9H6v-9l6-4.5z"/>
  <rect x="10" y="13" width="4" height="6"/>
</svg>
```

**icon_crime.svg:**
```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
  <path d="M20 6h-8l-2-2H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2z"/>
</svg>
```

**icon_casino.svg:**
```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
  <circle cx="12" cy="12" r="10"/>
  <circle cx="8" cy="10" r="1.5" fill="#1A1A2E"/>
  <circle cx="16" cy="10" r="1.5" fill="#1A1A2E"/>
  <circle cx="12" cy="14" r="1.5" fill="#1A1A2E"/>
</svg>
```

**icon_skills.svg:**
```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
  <path d="M3 13h2v8H3v-8zm4-4h2v12H7V9zm4-4h2v16h-2V5zm4 8h2v8h-2v-8zm4-4h2v12h-2V9z"/>
</svg>
```

**icon_settings.svg:**
```svg
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
  <path d="M19.14 12.94c.04-.31.06-.63.06-.94 0-.31-.02-.63-.06-.94l2.03-1.58c.18-.14.23-.41.12-.61l-1.92-3.32c-.12-.22-.37-.29-.59-.22l-2.39.96c-.5-.38-1.03-.7-1.62-.94l-.36-2.54c-.04-.24-.24-.41-.48-.41h-3.84c-.24 0-.43.17-.47.41l-.36 2.54c-.59.24-1.13.57-1.62.94l-2.39-.96c-.22-.08-.47 0-.59.22L2.74 8.87c-.12.21-.08.47.12.61l2.03 1.58c-.04.31-.06.63-.06.94s.02.63.06.94l-2.03 1.58c-.18.14-.23.41-.12.61l1.92 3.32c.12.22.37.29.59.22l2.39-.96c.5.38 1.03.7 1.62.94l.36 2.54c.05.24.24.41.48.41h3.84c.24 0 .44-.17.47-.41l.36-2.54c.59-.24 1.13-.56 1.62-.94l2.39.96c.22.08.47 0 .59-.22l1.92-3.32c.12-.22.07-.47-.12-.61l-2.01-1.58zM12 15.6c-1.98 0-3.6-1.62-3.6-3.6s1.62-3.6 3.6-3.6 3.6 1.62 3.6 3.6-1.62 3.6-3.6 3.6z"/>
</svg>
```

### Step 4: Registrer Pages i DI (hvis noedvendigt)

Path: `src/MadeMan.IdleEmpire/MauiProgram.cs`

Tilfoej til service registration:
```csharp
// Pages
builder.Services.AddTransient<OrgCrimePage>();
builder.Services.AddTransient<CasinoPage>();
builder.Services.AddTransient<SkillsPage>();
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
- [ ] 5 tabs vises i bunden
- [ ] Empire tab viser nuvaerende gameplay
- [ ] Crime/Casino/Skills viser "Locked" placeholder
- [ ] Settings tab viser settings placeholder
- [ ] Tab switching fungerer smooth
- [ ] Korrekte ikoner vises

---

## Acceptance Criteria

- [ ] Bottom navigation med 5 tabs
- [ ] Empire og Settings altid tilgaengelige
- [ ] Placeholder pages for locked tabs
- [ ] Tab styling matcher tema (moerk, guld accenter)
- [ ] Build succeeds med 0 errors
- [ ] Navigation fungerer smooth

---

## Kode Evaluering

### Simplifikations-tjek
- **Shell TabBar**: Brug MAUI standard navigation
- **Placeholder pages**: Minimalt indhold for nu
- **Ingen state logic**: Bare UI struktur

### Alternativer overvejet

**Alternative 1: Custom Tab Control**
- Byg egen tab navigation fra scratch
- **Hvorfor fravalgt**: Meget mere arbejde, Shell er optimeret

**Alternative 2: Flyout Navigation**
- Brug side-menu i stedet for tabs
- **Hvorfor fravalgt**: Tabs er bedre for idle games

### Potentielle forbedringer (v2)
- Tab unlock animation
- Badge pa tabs (notifikationer)
- Tab state persistence

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Shell TabBar er simpleste losning
- [ ] **Laesbarhed**: Placeholder pages er klare
- [ ] **Navngivning**: Konsistente navne
- [ ] **DRY**: Placeholder pages folger samme pattern
- [ ] **Performance**: Shell er optimeret for mobile
- [ ] **Testbarhed**: Visuelt verificerbar

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
