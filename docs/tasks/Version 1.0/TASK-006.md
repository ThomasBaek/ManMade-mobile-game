# Task 006: Implement Theme

## Metadata
- **Phase**: 2 - UI
- **Dependencies**: TASK-005
- **Estimated Time**: 30 min
- **Status**: COMPLETED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (line 405-418)
- **Requires Design Input**: NO

---

## Purpose

Update Colors.xaml with game-specific colors and create Theme.xaml.

**Why this is important:**
- Consistent visual expression
- Dark mafia aesthetic
- Easy to change colors in one place

---

## Risks

### Potential Issues
1. **StaticResource Not Found**:
   - Edge case: Typo in resource key
   - Impact: Runtime exception

### Mitigation
- Use copy-paste for resource keys
- Test all colors in UI

---

## Analysis - What to Implement

### 1. Update Colors.xaml
**Location**: `Resources/Styles/Colors.xaml`
- Replace default MAUI colors with game palette

### 2. Create Theme.xaml (optional)
Can be included in Colors.xaml for simplicity

---

## Implementation Guide

### Step 1: Update Colors.xaml

**Path**: `src/MadeMan.IdleEmpire/Resources/Styles/Colors.xaml`

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
Expected: 0 errors

### 2. Visual Test (in TASK-009)
- Background displays as #1A1A2E
- Primary displays as #E94560

---

## Acceptance Criteria

- [x] Colors.xaml updated with game colors
- [x] All 11 game-specific colors defined
- [x] Brushes for main colors
- [x] Build succeeds with 0 errors

---

## Code Quality Checklist

- [x] **Naming**: Descriptive color keys
- [x] **Consistency**: All colors use hex format

---

## Color Reference

| Key | Hex | Usage |
|-----|-----|-------|
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

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Commit**: f6cb1c7
