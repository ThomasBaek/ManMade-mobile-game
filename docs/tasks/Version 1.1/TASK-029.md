# Task 029: Dystert Gangster Tema

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: None (Foundation task)
- **Estimated Time**: 1-2 hours
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 6)
- **Frequency Impact**: NO

---

## Formaal

Opdater farvepalet og visuel styling til et moerkere, mere atmosfaerisk design der matcher 1930s mafia-temaet.

**Hvorfor dette er vigtigt:**
- Nuvaerende design er for "generisk app" look
- Mangler stemning og karakter
- Ikke immersive nok for et gangster-tema

---

## Risici

### Potentielle Problemer
1. **Kontrast problemer**:
   - Edge case: For mork baggrund kan goere tekst svaer at laese
   - Impact: Accessibility issues, daarlig UX

2. **Inkonsekvens**:
   - Eksisterende komponenter bruger hardcodede farver
   - Impact: Blandede farver pa tvaers af UI

### Mitigering
- Test alle farvekontraster for WCAG compliance (minimum 4.5:1 ratio)
- Soeg efter hardcodede farver i XAML og erstat med StaticResource

---

## Analyse - Hvad Skal Implementeres

### 1. Opdater Colors.xaml
**Description**: Ny moerkere farvepalet
**Location**: `src/MadeMan.IdleEmpire/Resources/Styles/Colors.xaml`

**Ny Farvepalet:**
```xml
<!-- Primary Colors (OPDATERET) -->
<Color x:Key="Background">#0D0D0D</Color>      <!-- Tidligere: #1A1A2E -->
<Color x:Key="Surface">#1A1A2E</Color>          <!-- Tidligere: #16213E -->
<Color x:Key="SurfaceLight">#252540</Color>     <!-- Tidligere: #1F2B47 -->

<!-- Accent Colors (OPDATERET) -->
<Color x:Key="Gold">#D4AF37</Color>             <!-- Tidligere: #FFD700 -->
<Color x:Key="GoldDark">#8B7500</Color>         <!-- Tidligere: #B8960B -->
<Color x:Key="Primary">#8B0000</Color>          <!-- Blood Red - Tidligere: #E94560 -->
<Color x:Key="PrimaryDark">#5C0000</Color>      <!-- Tidligere: #C73E54 -->

<!-- NYE Farver -->
<Color x:Key="Smoke">#4A4A4A</Color>            <!-- Disabled, secondary -->
<Color x:Key="GoldText">#FFD700</Color>         <!-- Speciel guld tekst -->
```

### 2. Opdater Styles.xaml
**Description**: Tilpas globale styles til nyt tema
**Location**: `src/MadeMan.IdleEmpire/Resources/Styles/Styles.xaml`

**Key Requirements:**
- ContentPage default background skal vaere #0D0D0D
- Frame styles skal bruge nye Surface farver
- Button styles skal opdateres

### 3. Verificer MainPage.xaml
**Description**: Sorg for ingen hardcodede farver
**Location**: `src/MadeMan.IdleEmpire/Views/MainPage.xaml`

**Key Requirements:**
- Alle farver skal vaere StaticResource
- Test visuelt at nyt tema ser rigtigt ud

---

## Dependencies Check

**Required Before Starting:**
- [x] MVP komplet
- [x] Eksisterende farvesystem fungerer

**Assumptions:**
- Ingen andre tasks aendrer farver parallelt

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Opdater Colors.xaml

Path: `src/MadeMan.IdleEmpire/Resources/Styles/Colors.xaml`

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">

    <!-- Made Man: Idle Empire - Dystert Gangster Tema -->

    <!-- Background Colors (Moerkere) -->
    <Color x:Key="Background">#0D0D0D</Color>
    <Color x:Key="Surface">#1A1A2E</Color>
    <Color x:Key="SurfaceLight">#252540</Color>
    <Color x:Key="Card">#252540</Color>

    <!-- Primary/Accent Colors (Blood Red) -->
    <Color x:Key="Primary">#8B0000</Color>
    <Color x:Key="PrimaryDark">#5C0000</Color>

    <!-- Gold/Cash Colors -->
    <Color x:Key="Gold">#D4AF37</Color>
    <Color x:Key="GoldDark">#8B7500</Color>
    <Color x:Key="GoldText">#FFD700</Color>

    <!-- Text Colors -->
    <Color x:Key="TextPrimary">#FFFFFF</Color>
    <Color x:Key="TextSecondary">#A0A0A0</Color>

    <!-- State Colors -->
    <Color x:Key="Success">#4ADE80</Color>
    <Color x:Key="Locked">#4A4A4A</Color>
    <Color x:Key="Smoke">#4A4A4A</Color>

    <!-- Legacy Colors (for MAUI compatibility) -->
    <Color x:Key="White">White</Color>
    <Color x:Key="Black">Black</Color>
    <Color x:Key="OffBlack">#0D0D0D</Color>

    <!-- MAUI Styles compatibility -->
    <Color x:Key="PrimaryDarkText">#FFFFFF</Color>
    <Color x:Key="SecondaryDarkText">#A0A0A0</Color>
    <Color x:Key="Secondary">#1A1A2E</Color>
    <Color x:Key="Magenta">#8B0000</Color>
    <Color x:Key="MidnightBlue">#0D0D0D</Color>
    <Color x:Key="Gray100">#E1E1E1</Color>
    <Color x:Key="Gray200">#C8C8C8</Color>
    <Color x:Key="Gray300">#ACACAC</Color>
    <Color x:Key="Gray400">#919191</Color>
    <Color x:Key="Gray500">#6E6E6E</Color>
    <Color x:Key="Gray600">#404040</Color>
    <Color x:Key="Gray900">#212121</Color>
    <Color x:Key="Gray950">#141414</Color>

    <!-- Brushes -->
    <SolidColorBrush x:Key="BackgroundBrush" Color="{StaticResource Background}"/>
    <SolidColorBrush x:Key="SurfaceBrush" Color="{StaticResource Surface}"/>
    <SolidColorBrush x:Key="SurfaceLightBrush" Color="{StaticResource SurfaceLight}"/>
    <SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource Primary}"/>
    <SolidColorBrush x:Key="GoldBrush" Color="{StaticResource Gold}"/>
    <SolidColorBrush x:Key="GoldTextBrush" Color="{StaticResource GoldText}"/>
    <SolidColorBrush x:Key="TextPrimaryBrush" Color="{StaticResource TextPrimary}"/>
    <SolidColorBrush x:Key="TextSecondaryBrush" Color="{StaticResource TextSecondary}"/>
    <SolidColorBrush x:Key="SuccessBrush" Color="{StaticResource Success}"/>
    <SolidColorBrush x:Key="LockedBrush" Color="{StaticResource Locked}"/>
    <SolidColorBrush x:Key="SmokeBrush" Color="{StaticResource Smoke}"/>

    <!-- Legacy Brushes -->
    <SolidColorBrush x:Key="WhiteBrush" Color="{StaticResource White}"/>
    <SolidColorBrush x:Key="BlackBrush" Color="{StaticResource Black}"/>
    <SolidColorBrush x:Key="Gray100Brush" Color="{StaticResource Gray100}"/>
    <SolidColorBrush x:Key="Gray200Brush" Color="{StaticResource Gray200}"/>
    <SolidColorBrush x:Key="Gray300Brush" Color="{StaticResource Gray300}"/>
    <SolidColorBrush x:Key="Gray400Brush" Color="{StaticResource Gray400}"/>
    <SolidColorBrush x:Key="Gray500Brush" Color="{StaticResource Gray500}"/>
    <SolidColorBrush x:Key="Gray600Brush" Color="{StaticResource Gray600}"/>
    <SolidColorBrush x:Key="Gray900Brush" Color="{StaticResource Gray900}"/>
    <SolidColorBrush x:Key="Gray950Brush" Color="{StaticResource Gray950}"/>

</ResourceDictionary>
```

### Step 2: Test Build

```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### Step 3: Visuelt Test

Kor app i emulator og verificer:
- Baggrund er naesten sort (#0D0D0D)
- Cards er moerk navy (#1A1A2E)
- Guld farver er dybere
- Prestige knap er blood red

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors, 0 warnings

### 2. Manual Test in Emulator
- [ ] Background er #0D0D0D (naesten sort)
- [ ] Cards er #1A1A2E (moerk navy)
- [ ] Gold farver er dybere og mere elegante
- [ ] Prestige panel er blood red (#8B0000)
- [ ] Al tekst er laesbar

---

## Acceptance Criteria

- [ ] Ny farvepalet implementeret i Colors.xaml
- [ ] Background er #0D0D0D pa alle skaerme
- [ ] Gold accenter er #D4AF37
- [ ] Primary (prestige) er blood red #8B0000
- [ ] Build succeeds med 0 errors
- [ ] Ingen hardcodede farver i XAML
- [ ] Visuelt test passed i emulator

---

## Kode Evaluering

### Simplifikations-tjek
- **Kun farveaendringer**: Ingen strukturelle aendringer
- **Bevar kompatibilitet**: Legacy farver bibeholdes
- **Ingen nye dependencies**: Kun XAML aendringer

### Alternativer overvejet

**Alternative 1: Gradient baggrunde**
- Tilfoej LinearGradientBrush til baggrund
- **Hvorfor fravalgt**: Over-engineering, performance concern pa aeldre devices

**Alternative 2: Tekstur baggrund**
- Tilfoej seamless texture pattern
- **Hvorfor fravalgt**: Kraever grafik asset, kan goeres i senere task

### Potentielle forbedringer (v2)
- Tilfoej subtle baggrundstekstur
- Implementer vignette effekt
- Gold glow animation pa vigtige tal

### Kendte begraensninger
- **Ingen teksturer**: Rent farvebaseret tema for nu
- **Statisk**: Ingen animationer i farver

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Er dette den simpleste losning? (Ja - kun farveaendringer)
- [ ] **Laesbarhed**: Kan en anden udvikler forstaa koden uden forklaring?
- [ ] **Navngivning**: Er alle navne beskrivende og konsistente?
- [ ] **DRY**: Er der nogen duplikeret kode?
- [ ] **Error handling**: Ikke relevant for styling
- [ ] **Edge cases**: Kontrast testet
- [ ] **Performance**: Ingen performance impact
- [ ] **Testbarhed**: Visuelt testbar

---

## Design Files Reference

- **Color Spec**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 6)
- **Existing Colors**: src/MadeMan.IdleEmpire/Resources/Styles/Colors.xaml

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
