# Task 027: Skill Display Component

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-025
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 6)
- **Requires Design Input**: NO

---

## Formål

Vis spillerens aktive skills og milestone progress i UI.

**Hvorfor dette er vigtigt:**
- Spilleren skal se sine skills
- Progress mod næste milestone
- Visuelt feedback for skill levels

---

## Analyse - Hvad Skal Implementeres

### SkillsPanel.xaml
**Placering**: `Views/Components/SkillsPanel.xaml`

UI Elementer:
- Milestone progress bar
- "Next milestone: $X" text
- Grid af aktive skills (max 5)
- Hver skill viser: ikon, navn, level

---

## Implementation Guide

### Step 1: Opret SkillsPanel.xaml

**Sti**: `src/MadeMan.IdleEmpire/Views/Components/SkillsPanel.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MadeMan.IdleEmpire.Views.Components.SkillsPanel">

    <Frame BackgroundColor="{StaticResource Surface}"
           CornerRadius="12"
           Padding="12"
           Margin="0,8">

        <VerticalStackLayout Spacing="8">

            <!-- Header -->
            <Grid ColumnDefinitions="*,Auto">
                <Label Text="Skills"
                       FontSize="16"
                       FontAttributes="Bold"
                       TextColor="{StaticResource TextPrimary}"/>
                <Label Grid.Column="1"
                       Text="{Binding SkillVM.NextMilestoneText}"
                       FontSize="12"
                       TextColor="{StaticResource TextSecondary}"/>
            </Grid>

            <!-- Progress Bar -->
            <Frame BackgroundColor="{StaticResource Background}"
                   CornerRadius="4"
                   Padding="0"
                   HeightRequest="8">
                <BoxView BackgroundColor="{StaticResource Gold}"
                         HorizontalOptions="Start"
                         WidthRequest="{Binding SkillVM.MilestoneProgress,
                             Converter={StaticResource ProgressToWidthConverter}}"/>
            </Frame>

            <!-- Active Skills -->
            <FlexLayout Direction="Row"
                        Wrap="Wrap"
                        JustifyContent="Start"
                        AlignItems="Start"
                        BindableLayout.ItemsSource="{Binding SkillVM.ActiveSkills}">
                <BindableLayout.ItemTemplate>
                    <DataTemplate>
                        <Frame Margin="4"
                               Padding="8"
                               BackgroundColor="{StaticResource Background}"
                               CornerRadius="8"
                               WidthRequest="100">
                            <VerticalStackLayout Spacing="2">

                                <!-- Icon placeholder -->
                                <Label Text="⭐"
                                       FontSize="20"
                                       HorizontalOptions="Center"/>

                                <!-- Name -->
                                <Label Text="{Binding Name}"
                                       FontSize="10"
                                       FontAttributes="Bold"
                                       HorizontalTextAlignment="Center"
                                       TextColor="{StaticResource TextPrimary}"
                                       LineBreakMode="TailTruncation"/>

                                <!-- Level -->
                                <Label Text="{Binding LevelDisplay}"
                                       FontSize="10"
                                       HorizontalTextAlignment="Center"
                                       TextColor="{StaticResource Gold}"/>

                            </VerticalStackLayout>
                        </Frame>
                    </DataTemplate>
                </BindableLayout.ItemTemplate>
            </FlexLayout>

            <!-- Empty State -->
            <Label Text="No skills yet. Earn more to unlock!"
                   FontSize="12"
                   TextColor="{StaticResource TextSecondary}"
                   HorizontalOptions="Center"
                   IsVisible="{Binding SkillVM.ActiveSkills.Count, Converter={StaticResource ZeroToBoolConverter}}"/>

        </VerticalStackLayout>
    </Frame>
</ContentView>
```

### Step 2: Opret code-behind

**Sti**: `src/MadeMan.IdleEmpire/Views/Components/SkillsPanel.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views.Components;

public partial class SkillsPanel : ContentView
{
    public SkillsPanel()
    {
        InitializeComponent();
    }
}
```

### Step 3: Opret Converters (hvis ikke findes)

**Sti**: `src/MadeMan.IdleEmpire/Converters/ProgressToWidthConverter.cs`

```csharp
using System.Globalization;

namespace MadeMan.IdleEmpire.Converters;

public class ProgressToWidthConverter : IValueConverter
{
    public double MaxWidth { get; set; } = 300; // Default, override in XAML

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double progress)
        {
            return progress * MaxWidth;
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

### Step 4: Integrer i MainPage.xaml

Tilføj SkillsPanel i hovedlayout:

```xml
<!-- Efter operations list, før prestige knap -->
<views:SkillsPanel />
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Visual Test
- [ ] Panel vises korrekt
- [ ] Progress bar opdateres
- [ ] Skills vises i grid
- [ ] Empty state vises uden skills

---

## Acceptance Criteria

- [ ] SkillsPanel.xaml oprettet
- [ ] Code-behind oprettet
- [ ] Binding til ActiveSkills
- [ ] Binding til MilestoneProgress
- [ ] Progress bar visualiserer progress
- [ ] Skill cards viser level
- [ ] Empty state tekst
- [ ] Integreret i MainPage
- [ ] Build succeeds med 0 errors

---

## UI Specifikationer

- **Panel Background**: Surface farve
- **Progress Bar Height**: 8dp
- **Progress Bar Color**: Gold
- **Skill Card Size**: 100x80dp
- **Corner Radius**: 8dp

---

**Task Status**: BLOCKED (venter på TASK-025)
**Last Updated**: 2024-12-25
