# Task 026: Skill Selection Modal

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-025
- **Estimated Time**: 45 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 6)
- **Requires Design Input**: NO

---

## Formål

Opret modal UI for skill selection ved milestones.

**Hvorfor dette er vigtigt:**
- Spilleren skal kunne vælge mellem 3 skills
- Tydelig præsentation af skill info
- Blokerer gameplay indtil valg er truffet

---

## Analyse - Hvad Skal Implementeres

### SkillSelectionModal.xaml
**Placering**: `Views/Components/SkillSelectionModal.xaml`

UI Elementer:
- Overlay baggrund (semi-transparent)
- Modal container
- Titel "Choose a Skill"
- 3 skill cards med:
  - Ikon
  - Navn
  - Level (ny eller upgrade)
  - Beskrivelse
  - Effect preview
- Tap gesture for selection

---

## Implementation Guide

### Step 1: Opret SkillSelectionModal.xaml

**Sti**: `src/MadeMan.IdleEmpire/Views/Components/SkillSelectionModal.xaml`

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:MadeMan.IdleEmpire.ViewModels"
             x:Class="MadeMan.IdleEmpire.Views.Components.SkillSelectionModal"
             IsVisible="{Binding SkillVM.IsSelectionModalVisible}">

    <!-- Overlay -->
    <Grid BackgroundColor="#CC000000">

        <!-- Modal Container -->
        <Frame VerticalOptions="Center"
               HorizontalOptions="Center"
               Margin="20"
               Padding="20"
               BackgroundColor="{StaticResource Surface}"
               CornerRadius="16"
               HasShadow="True">

            <VerticalStackLayout Spacing="16">

                <!-- Title -->
                <Label Text="🎯 Milestone Reached!"
                       FontSize="24"
                       FontAttributes="Bold"
                       TextColor="{StaticResource Gold}"
                       HorizontalOptions="Center"/>

                <Label Text="Choose a skill to unlock or upgrade:"
                       FontSize="14"
                       TextColor="{StaticResource TextSecondary}"
                       HorizontalOptions="Center"/>

                <!-- Skill Options -->
                <CollectionView ItemsSource="{Binding SkillVM.SelectionPool}"
                                SelectionMode="None">
                    <CollectionView.ItemTemplate>
                        <DataTemplate>
                            <Frame Margin="0,4"
                                   Padding="12"
                                   BackgroundColor="{StaticResource Background}"
                                   CornerRadius="8"
                                   BorderColor="{StaticResource Primary}">
                                <Frame.GestureRecognizers>
                                    <TapGestureRecognizer
                                        Command="{Binding Source={RelativeSource AncestorType={x:Type vm:MainViewModel}}, Path=SkillVM.SelectSkillCommand}"
                                        CommandParameter="{Binding Id}"/>
                                </Frame.GestureRecognizers>

                                <Grid ColumnDefinitions="48,*" ColumnSpacing="12">

                                    <!-- Icon -->
                                    <Frame Grid.Column="0"
                                           WidthRequest="48"
                                           HeightRequest="48"
                                           CornerRadius="24"
                                           BackgroundColor="{StaticResource Surface}"
                                           Padding="0">
                                        <Label Text="⭐"
                                               FontSize="24"
                                               HorizontalOptions="Center"
                                               VerticalOptions="Center"/>
                                    </Frame>

                                    <!-- Info -->
                                    <VerticalStackLayout Grid.Column="1" Spacing="2">
                                        <Label Text="{Binding Name}"
                                               FontSize="16"
                                               FontAttributes="Bold"
                                               TextColor="{StaticResource TextPrimary}"/>
                                        <Label Text="{Binding Description}"
                                               FontSize="12"
                                               TextColor="{StaticResource TextSecondary}"/>
                                        <Label FontSize="11"
                                               TextColor="{StaticResource Gold}">
                                            <Label.FormattedText>
                                                <FormattedString>
                                                    <Span Text="+"/>
                                                    <Span Text="{Binding EffectPerLevel}"/>
                                                    <Span Text="% per level"/>
                                                </FormattedString>
                                            </Label.FormattedText>
                                        </Label>
                                    </VerticalStackLayout>

                                </Grid>
                            </Frame>
                        </DataTemplate>
                    </CollectionView.ItemTemplate>
                </CollectionView>

            </VerticalStackLayout>
        </Frame>
    </Grid>
</ContentView>
```

### Step 2: Opret code-behind

**Sti**: `src/MadeMan.IdleEmpire/Views/Components/SkillSelectionModal.xaml.cs`

```csharp
namespace MadeMan.IdleEmpire.Views.Components;

public partial class SkillSelectionModal : ContentView
{
    public SkillSelectionModal()
    {
        InitializeComponent();
    }
}
```

### Step 3: Integrer i MainPage.xaml

Tilføj modal som overlay i MainPage:

```xml
<!-- I MainPage.xaml, efter hovedindhold -->
<views:SkillSelectionModal />
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Visual Test
- [ ] Modal vises centreret
- [ ] Overlay dækker baggrund
- [ ] Skill cards er tappable
- [ ] Info vises korrekt

---

## Acceptance Criteria

- [ ] SkillSelectionModal.xaml oprettet
- [ ] Code-behind oprettet
- [ ] Binding til SkillVM.SelectionPool
- [ ] Binding til IsSelectionModalVisible
- [ ] TapGestureRecognizer på hver skill
- [ ] SelectSkillCommand kaldes med skill ID
- [ ] Integreret i MainPage
- [ ] Build succeeds med 0 errors

---

## UI Specifikationer

- **Overlay**: #CC000000 (80% sort)
- **Modal Background**: Surface farve
- **Corner Radius**: 16dp
- **Card Spacing**: 8dp
- **Padding**: 20dp

---

**Task Status**: BLOCKED (venter på TASK-025)
**Last Updated**: 2024-12-25
