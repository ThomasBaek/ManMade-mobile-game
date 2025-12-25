# Task 006: Implement Theme

## Metadata
- **Phase**: 2 - UI
- **Dependencies**: TASK-005
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 405-418)
- **Requires Design Input**: NO

---

## Formål

Opdater Colors.xaml med game-specific farver og opret Theme.xaml.

**Hvorfor dette er vigtigt:**
- Konsistent visuelt udtryk
- Mørk mafia-æstetik
- Nemt at ændre farver ét sted

---

## Risici

### Potentielle Problemer
1. **StaticResource Not Found**:
   - Edge case: Typo i resource key
   - Impact: Runtime exception

### Mitigering
- Brug copy-paste for resource keys
- Test alle farver i UI

---

## Analyse - Hvad Skal Implementeres

### 1. Opdater Colors.xaml
**Placering**: `Resources/Styles/Colors.xaml`
- Erstat default MAUI farver med game palette

### 2. Opret Theme.xaml (valgfrit)
Kan inkluderes i Colors.xaml for simplicitet

---

## Implementation Guide

### Step 1: Opdater Colors.xaml

**Sti**: `src/MadeMan.IdleEmpire/Resources/Styles/Colors.xaml`

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<ResourceDictionary
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">

    <!-- Made Man: Idle Empire Theme -->

    <!-- Background Colors -->
    <Color x:Key="Background">#1A1A2E</Color>
    <Color x:Key="Surface">#16213E</Color>
    <Color x:Key="SurfaceLight">#1F2B47</Color>

    <!-- Primary/Accent Colors -->
    <Color x:Key="Primary">#E94560</Color>
    <Color x:Key="PrimaryDark">#C73E54</Color>

    <!-- Gold/Cash Colors -->
    <Color x:Key="Gold">#FFD700</Color>
    <Color x:Key="GoldDark">#B8960B</Color>

    <!-- Text Colors -->
    <Color x:Key="TextPrimary">#FFFFFF</Color>
    <Color x:Key="TextSecondary">#8892A0</Color>

    <!-- State Colors -->
    <Color x:Key="Success">#4ADE80</Color>
    <Color x:Key="Locked">#4A5568</Color>

    <!-- Legacy Colors (for compatibility) -->
    <Color x:Key="White">White</Color>
    <Color x:Key="Black">Black</Color>
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
    <SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource Primary}"/>
    <SolidColorBrush x:Key="GoldBrush" Color="{StaticResource Gold}"/>

</ResourceDictionary>
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Forventet: 0 errors

### 2. Visual Test (i TASK-009)
- Background vises som #1A1A2E
- Primary vises som #E94560

---

## Acceptance Criteria

- [ ] Colors.xaml opdateret med game farver
- [ ] Alle 11 game-specific farver defineret
- [ ] Brushes for hovedfarver
- [ ] Build succeeds med 0 errors

---

## Kode Kvalitet Checklist

- [ ] **Navngivning**: Beskrivende color keys
- [ ] **Konsistens**: Alle farver bruger hex format

---

## Color Reference

| Key | Hex | Brug |
|-----|-----|------|
| Background | #1A1A2E | Page background |
| Surface | #16213E | Cards, panels |
| SurfaceLight | #1F2B47 | Elevated elements |
| Primary | #E94560 | Buttons, accents |
| Gold | #FFD700 | Cash, income |
| Success | #4ADE80 | Can afford |
| Locked | #4A5568 | Can't afford |
| TextPrimary | #FFFFFF | Main text |
| TextSecondary | #8892A0 | Labels |

---

**Task Status**: BLOCKED (venter på TASK-005)
**Last Updated**: 2024-12-25
