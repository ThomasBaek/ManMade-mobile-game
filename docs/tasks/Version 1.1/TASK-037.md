# Task 037: Kompakt UI Layout

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-029, TASK-036
- **Estimated Time**: 2-3 timer
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 5)
- **Frequency Impact**: NO

---

## Formaal

Reducer stoerrelsen paa UI elementer saa der er plads til mere indhold paa skaermen uden scroll.

**Hvorfor dette er vigtigt:**
- Nuvaerende operation cards er for store
- Kun 3-4 operations synlige ad gangen
- Prestige panel dominerer for meget

---

## Risici

### Potentielle Problemer
1. **Touch targets for smaa**:
   - Edge case: Knapper under 44px
   - Impact: Svaert at trykke, accessibility issues

2. **Laesbarhed**:
   - Mindre font sizes kan vaere svaere at laese
   - Impact: Daarlig UX paa mindre telefoner

### Mitigering
- Minimum 44px touch targets (Apple/Google guideline)
- Minimum 12px font size
- Test paa flere skaermstoerrrelser

---

## Analyse - Hvad Skal Implementeres

### Operation Card Komprimering

**Foer (ca. 120px hoejde):**
```
+--------------------------------+
| Icon  | Pickpocket             |
|       |                        |
|       | Income: +$1.50/sek     |
|       | Level: 3               |
|       |                        |
|       | [    UPGRADE $50    ]  |
+--------------------------------+
```

**Efter (ca. 60px hoejde):**
```
+--------------------------------+
| Icon | Pickpocket  Lvl 3 | +$1/s | [UPGRADE $50] |
+--------------------------------+
```

### Header Komprimering
- Cash display: Mere kompakt
- Prestige badge: Integreret i header
- Mindre vertikal padding

### Maalsaetninger
- 5-6 operations synlige uden scroll
- Header under 60px
- Touch targets minimum 44px

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-029 (farver)
- [x] TASK-036 (visibility logic)

**Assumptions:**
- Nuvaerende MainPage layout kan modificeres
- Font sizes og padding er fleksible

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Kompakt Operation Card Template

Path: `src/MadeMan.IdleEmpire/Views/MainPage.xaml`

Erstat eksisterende operation template med kompakt version:

```xml
<DataTemplate x:DataType="vm:OperationViewModel">
    <Frame BackgroundColor="{StaticResource Surface}"
           CornerRadius="8"
           Padding="12,8"
           BorderColor="Transparent"
           IsVisible="{Binding ShouldShow}"
           HeightRequest="56">
        <Frame.GestureRecognizers>
            <TapGestureRecognizer Command="{Binding TapCommand}"/>
        </Frame.GestureRecognizers>

        <Grid ColumnDefinitions="40,*,Auto,Auto" ColumnSpacing="8">

            <!-- Icon (smaller) -->
            <Frame BackgroundColor="{StaticResource SurfaceLight}"
                   CornerRadius="6"
                   Padding="0"
                   HeightRequest="40"
                   WidthRequest="40"
                   BorderColor="Transparent">
                <Label Text="{Binding IconEmoji}"
                       FontSize="20"
                       HorizontalOptions="Center"
                       VerticalOptions="Center"/>
            </Frame>

            <!-- Name + Level (combined) -->
            <VerticalStackLayout Grid.Column="1"
                                 VerticalOptions="Center"
                                 Spacing="2">
                <HorizontalStackLayout Spacing="6">
                    <Label Text="{Binding Name}"
                           FontSize="14"
                           FontAttributes="Bold"
                           TextColor="{StaticResource TextPrimary}"
                           LineBreakMode="TailTruncation"/>
                    <Label Text="{Binding LevelDisplay}"
                           FontSize="12"
                           TextColor="{StaticResource TextSecondary}"
                           IsVisible="{Binding IsUnlocked}"/>
                </HorizontalStackLayout>

                <!-- Progress bar for locked -->
                <Grid HeightRequest="4"
                      IsVisible="{Binding IsLocked}">
                    <BoxView BackgroundColor="{StaticResource SurfaceLight}"
                             CornerRadius="2"/>
                    <BoxView BackgroundColor="{StaticResource Gold}"
                             CornerRadius="2"
                             HorizontalOptions="Start"
                             WidthRequest="{Binding ProgressBarWidth}"/>
                </Grid>
            </VerticalStackLayout>

            <!-- Income (compact) -->
            <Label Grid.Column="2"
                   Text="{Binding IncomeDisplay}"
                   FontSize="12"
                   TextColor="{StaticResource Gold}"
                   IsVisible="{Binding IsUnlocked}"
                   VerticalOptions="Center"/>

            <!-- Action Button (compact) -->
            <Button Grid.Column="3"
                    Text="{Binding ButtonText}"
                    Command="{Binding TapCommand}"
                    BackgroundColor="{Binding ButtonColor}"
                    TextColor="White"
                    CornerRadius="6"
                    Padding="12,0"
                    FontSize="11"
                    FontAttributes="Bold"
                    HeightRequest="36"
                    MinimumWidthRequest="70"
                    VerticalOptions="Center"/>
        </Grid>
    </Frame>
</DataTemplate>
```

### Step 2: Kompakt Header

Reducer header section:

```xml
<!-- === CASH HEADER (Compact) === -->
<Frame Grid.Row="1"
       BackgroundColor="{StaticResource Surface}"
       CornerRadius="12"
       Padding="12"
       Margin="0,0,0,8"
       BorderColor="Transparent">

    <Grid ColumnDefinitions="*,Auto" RowDefinitions="Auto,Auto" RowSpacing="4">

        <!-- Cash + Income row -->
        <HorizontalStackLayout Spacing="8">
            <Label Text="{Binding CashDisplay}"
                   FontSize="24"
                   FontAttributes="Bold"
                   TextColor="{StaticResource Gold}"/>
            <Frame BackgroundColor="{StaticResource SurfaceLight}"
                   CornerRadius="4"
                   Padding="6,2"
                   VerticalOptions="Center"
                   BorderColor="Transparent">
                <Label Text="{Binding IncomeDisplay}"
                       FontSize="11"
                       TextColor="{StaticResource Success}"/>
            </Frame>
        </HorizontalStackLayout>

        <!-- Prestige badge (moved to header) -->
        <HorizontalStackLayout Grid.Column="1"
                               Spacing="4"
                               IsVisible="{Binding HasPrestiged}"
                               VerticalOptions="Center">
            <Label Text="P:"
                   FontSize="11"
                   TextColor="{StaticResource TextSecondary}"/>
            <Label Text="{Binding PrestigeCount}"
                   FontSize="11"
                   FontAttributes="Bold"
                   TextColor="{StaticResource Gold}"/>
        </HorizontalStackLayout>

        <!-- Progress bar row -->
        <Grid Grid.Row="1" Grid.ColumnSpan="2" HeightRequest="4" Margin="0,4,0,0">
            <BoxView BackgroundColor="{StaticResource SurfaceLight}"
                     CornerRadius="2"/>
            <BoxView BackgroundColor="{StaticResource Primary}"
                     CornerRadius="2"
                     HorizontalOptions="Start"
                     WidthRequest="{Binding PrestigeProgress, Converter={StaticResource ProgressToWidth}}"/>
        </Grid>
    </Grid>
</Frame>
```

### Step 3: Reducer Operation List Spacing

```xml
<!-- === OPERATIONS LIST === -->
<ScrollView Grid.Row="3">
    <VerticalStackLayout Spacing="8"
                         BindableLayout.ItemsSource="{Binding Operations}">
        <!-- Template as above -->
    </VerticalStackLayout>
</ScrollView>
```

### Step 4: Kompakt Prestige Panel

```xml
<!-- === PRESTIGE PANEL (Compact) === -->
<Frame Grid.Row="4"
       BackgroundColor="{StaticResource Primary}"
       CornerRadius="12"
       Padding="12"
       Margin="0,8,0,0"
       IsVisible="{Binding CanPrestige}"
       BorderColor="Transparent">

    <Grid ColumnDefinitions="*,Auto" ColumnSpacing="12">
        <VerticalStackLayout VerticalOptions="Center">
            <Label Text="PRESTIGE READY"
                   FontSize="12"
                   FontAttributes="Bold"
                   TextColor="White"/>
            <Label Text="{Binding PrestigeButtonText}"
                   FontSize="10"
                   TextColor="White"
                   Opacity="0.8"/>
        </VerticalStackLayout>

        <Button Grid.Column="1"
                Text="GO"
                Command="{Binding PrestigeCommand}"
                BackgroundColor="White"
                TextColor="{StaticResource Primary}"
                CornerRadius="6"
                FontAttributes="Bold"
                HeightRequest="36"
                WidthRequest="60"/>
    </Grid>
</Frame>
```

### Step 5: Opdater OperationViewModel ButtonText

Kortere knaptekst:

```csharp
public string ButtonText
{
    get
    {
        if (!IsUnlocked)
            return $"${FormatCompact(_unlockCost)}";

        return $"${FormatCompact(_upgradeCost)}";
    }
}

private string FormatCompact(double value)
{
    if (value >= 1_000_000) return $"{value/1_000_000:F1}M";
    if (value >= 1_000) return $"{value/1_000:F1}K";
    return $"{value:F0}";
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
- [ ] Operation cards er ca. 60px hoeje
- [ ] 5+ operations synlige uden scroll
- [ ] Alle knapper er tappable (min 44px)
- [ ] Tekst er laesbar
- [ ] Header er kompakt

---

## Acceptance Criteria

- [ ] Operation cards reduceret til ~60px hoejde
- [ ] Header komprimeret
- [ ] 5+ items synlige uden scroll paa standard telefon
- [ ] Touch targets minimum 44px
- [ ] Tekst laesbar (min 11px font)
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Kun layout aendringer**: Ingen ny logik
- **Responsive units**: Brug HeightRequest, ikke fixed px
- **Eksisterende bindings**: Genbrug alle data bindings

### Alternativer overvejet

**Alternative 1: CollectionView med virtualization**
- Brug CollectionView for bedre scroll performance
- **Hvorfor fravalgt**: BindableLayout er simpelt nok for 5-6 items

**Alternative 2: Adaptive layout**
- Forskellige layouts til forskellige skaermstoerrrelser
- **Hvorfor fravalgt**: Over-engineering for MVP

### Potentielle forbedringer (v2)
- Swipe actions paa operation cards
- Expand/collapse for detaljer
- Landscape mode support

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Kun layout aendringer
- [ ] **Laesbarhed**: Klar XAML struktur
- [ ] **Performance**: Ingen ekstra beregninger
- [ ] **Accessibility**: Touch targets >= 44px
- [ ] **Testbarhed**: Visuelt verificerbar

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
