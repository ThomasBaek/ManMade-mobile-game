# Task 077: Custom Art Deco Typography

## Metadata
- **Phase**: 7 - Visual Polish
- **Bundle**: Q - Enhanced UI
- **Dependencies**: Ingen
- **Estimated Time**: 1 hour
- **Status**: READY
- **Priority**: 🟡 Medium
- **Frequency Impact**: NO

---

## Formål

Tilføj periode-korrekte Art Deco fonts for at forstærke 1930s mafia-atmosfæren.

**Hvorfor dette er vigtigt:**
- Typography er en af de stærkeste visuelle signaler for tema
- Standard system fonts føles "generiske"
- Art Deco fonts giver instant 1930s recognition
- Billig måde at løfte hele spillets æstetik

---

## Risici

### Potentielle Problemer
1. **Font læsbarhed**:
   - Edge case: Dekorative fonts er svære at læse i små størrelser
   - Impact: Dårlig UX, accessibility issues

2. **App størrelse**:
   - Fonts kan være 200KB-1MB+
   - Impact: Større download, længere startup

3. **Licens issues**:
   - Ikke alle fonts er gratis til kommerciel brug
   - Impact: Juridiske problemer

### Mitigering
- Brug dekorativ font KUN til headers/titler
- Behold læsbar font til body text og tal
- Vælg fonts fra Google Fonts (gratis kommercielt)
- Brug kun de weights vi rent faktisk bruger

---

## Analyse - Hvad Skal Implementeres

### 1. Font Selection
**Anbefalede Google Fonts (gratis, kommercielt):**

| Font | Brug | Stil |
|------|------|------|
| **Bebas Neue** | Headers, titler | Kraftig, bold, Art Deco |
| **Cinzel** | Alternative headers | Elegant, klassisk serif |
| **Playfair Display** | Subtitles | 1930s avis-stil |
| **Inter** eller **Roboto** | Body, tal | Læsbar, moderne |

**Anbefaling:** Start med **Bebas Neue** til headers + **Inter** til body.

### 2. Font Files
**Location**: `src/MadeMan.IdleEmpire/Resources/Fonts/`

**Filer at downloade:**
- `BebasNeue-Regular.ttf`
- `Inter-Regular.ttf`
- `Inter-SemiBold.ttf`

### 3. Font Registration
**Location**: `src/MadeMan.IdleEmpire/MauiProgram.cs`

**Key Requirements:**
- Registrer fonts i `ConfigureFonts()`
- Brug beskrivende alias navne

### 4. Style Updates
**Location**: `src/MadeMan.IdleEmpire/Resources/Styles/Styles.xaml`

**Key Requirements:**
- Definer font families som StaticResource
- Opdater Label styles til at bruge nye fonts
- Header style bruger Bebas Neue
- Body style bruger Inter

---

## Dependencies Check

**Required Before Starting:**
- [x] MVP komplet
- [x] Styles.xaml eksisterer

**Assumptions:**
- MAUI font system fungerer standard

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Download Fonts

**Fra Google Fonts:**
1. Gå til https://fonts.google.com/specimen/Bebas+Neue
2. Download TTF fil
3. Gå til https://fonts.google.com/specimen/Inter
4. Download Regular og SemiBold weights

### Step 2: Tilføj til projekt

Path: `src/MadeMan.IdleEmpire/Resources/Fonts/`

Placer filer:
- `BebasNeue-Regular.ttf`
- `Inter-Regular.ttf`
- `Inter-SemiBold.ttf`

### Step 3: Registrer i MauiProgram.cs

**Tilføj i ConfigureFonts:**
```csharp
.ConfigureFonts(fonts =>
{
    fonts.AddFont("BebasNeue-Regular.ttf", "BebasNeue");
    fonts.AddFont("Inter-Regular.ttf", "Inter");
    fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
    // Behold eksisterende fonts
    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
});
```

### Step 4: Opdater Styles.xaml

**Tilføj font resources og styles:**
```xml
<!-- Font Family Resources -->
<x:String x:Key="HeaderFont">BebasNeue</x:String>
<x:String x:Key="BodyFont">Inter</x:String>
<x:String x:Key="BodyFontBold">InterSemiBold</x:String>

<!-- Header Style -->
<Style x:Key="HeaderLabel" TargetType="Label">
    <Setter Property="FontFamily" Value="{StaticResource HeaderFont}"/>
    <Setter Property="FontSize" Value="28"/>
    <Setter Property="TextColor" Value="{StaticResource Gold}"/>
</Style>

<!-- Title Style -->
<Style x:Key="TitleLabel" TargetType="Label">
    <Setter Property="FontFamily" Value="{StaticResource HeaderFont}"/>
    <Setter Property="FontSize" Value="20"/>
    <Setter Property="TextColor" Value="{StaticResource TextPrimary}"/>
</Style>
```

### Step 5: Anvend i UI

Opdater relevante Labels i XAML:
- App titel: `Style="{StaticResource HeaderLabel}"`
- Section headers: `Style="{StaticResource TitleLabel}"`
- Operation names: `FontFamily="{StaticResource BodyFontBold}"`

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Font Loading Test
- [ ] App starter uden crash
- [ ] Ingen "font not found" warnings i output

### 3. Manual Test in Emulator
- [ ] "MADE MAN" header vises i Bebas Neue
- [ ] Section headers har Art Deco font
- [ ] Body text er læsbar (Inter)
- [ ] Tal vises korrekt (ingen missing glyphs)
- [ ] Små tekst størrelser er stadig læsbare

---

## Acceptance Criteria

- [ ] Font filer tilføjet til Resources/Fonts
- [ ] Fonts registreret i MauiProgram.cs
- [ ] Header style bruger Bebas Neue (eller valgt Art Deco font)
- [ ] Body text bruger læsbar font
- [ ] Alle tal vises korrekt
- [ ] Fonts loader hurtigt (ingen mærkbar delay)
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **2-3 fonts max**: Ikke over-engineer med mange varianter
- **Standard MAUI approach**: Ingen custom font loaders
- **Google Fonts**: Gratis, velkendte, veldokumenterede

### Alternativer overvejet

**Alternative 1: Custom hand-made font**
- Unik til spillet
- **Hvorfor fravalgt**: Kræver font designer, dyrt, tidskrævende

**Alternative 2: System fonts only**
- Ingen ekstra bytes
- **Hvorfor fravalgt**: Mangler karakter, føles generisk

**Alternative 3: Mange font weights**
- Regular, Bold, Light, Italic, etc.
- **Hvorfor fravalgt**: Unødvendig app size, kompleksitet

### Potentielle forbedringer (v2)
- Custom "Made Man" logotype font
- Animated text effects
- Gold gradient på headers (SkiaSharp)

### Kendte begrænsninger
- **Kun latinske tegn**: Fonts understøtter måske ikke alle sprog
- **Ingen fallback**: Hvis font fejler, bruges system default

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Minimal antal fonts
- [ ] **Læsbarhed**: Klar font naming (HeaderFont, BodyFont)
- [ ] **Navngivning**: Beskrivende style names
- [ ] **DRY**: Styles genbruges via StaticResource
- [ ] **Performance**: Fonts loaded én gang ved startup
- [ ] **Testbarhed**: Visuelt verificerbar

---

## Font License Notes

**Bebas Neue**: SIL Open Font License - ✅ Gratis kommercielt
**Inter**: SIL Open Font License - ✅ Gratis kommercielt
**Cinzel**: SIL Open Font License - ✅ Gratis kommercielt
**Playfair Display**: SIL Open Font License - ✅ Gratis kommercielt

Alle anbefalede fonts er lovlige til kommerciel brug uden attribution krav i app'en.

---

**Task Status**: READY
**Version**: 1.2
**Last Updated**: 2024-12-27
**Implemented By**: Pending
