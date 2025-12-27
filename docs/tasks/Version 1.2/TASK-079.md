# Task 079: Film Grain & Vignette Overlay

## Metadata
- **Phase**: 7 - Visual Polish
- **Dependencies**: TASK-029 (Tema)
- **Estimated Time**: 2-3 hours
- **Status**: READY
- **Design Reference**: N/A
- **Frequency Impact**: NO

---

## Formål

Tilføj subtile visuelle overlays der giver spillet en autentisk 1930s film-æstetik.

**Hvorfor dette er vigtigt:**
- Film grain og vignette er instant "old movie" signaler
- Skaber atmosfære og immersion
- Differentierer fra andre idle games
- Relativt simpelt at implementere med stor visuel impact

---

## Risici

### Potentielle Problemer
1. **Performance impact**:
   - Edge case: Animated grain på hele skærmen
   - Impact: Frame drops, battery drain

2. **Distraction**:
   - For kraftig effekt gør UI svært at bruge
   - Impact: Frustration, accessibility issues

3. **Device variation**:
   - Effekter ser forskellige ud på forskellige skærme
   - Impact: Inkonsistent oplevelse

### Mitigering
- Start med MEGET subtile effekter (5-10% opacity)
- Grain skal være STATISK (ikke animeret) for performance
- Vignette som simpel gradient, ikke shader
- Toggle i Settings for brugere der ikke vil have det
- Test på flere skærmstørrelser

---

## Analyse - Hvad Skal Implementeres

### 1. Vignette Overlay (Simpel)
**Description**: Radial gradient der mørkner kanterne
**Location**: Overlay på top-level layout

**Approach Options:**
- **Option A (Anbefalet)**: PNG med transparent center, mørke kanter
- **Option B**: RadialGradientBrush i XAML
- **Option C**: SkiaSharp custom drawing

**Anbefaling**: Option A - Pre-rendered PNG er mest performant

### 2. Film Grain Texture (Simpel)
**Description**: Subtil støj-tekstur over UI
**Location**: Overlay på top-level layout

**Approach:**
- Seamless tileable grain texture (PNG)
- Meget lav opacity (5-8%)
- STATISK - ingen animation for performance
- Blend mode: Overlay eller Soft Light simuleret via opacity

### 3. Asset Creation
**Description**: Opret/find passende textures

**Vignette:**
- 512x512 eller 1024x1024 PNG
- Transparent center, radial fade til svag sort (50% i hjørner)
- Stretch to fill screen

**Grain:**
- 256x256 tileable grain texture
- Grayscale noise
- Tile across screen

### 4. Performance Optimization
**Description**: Sikr at overlays ikke påvirker performance

**Key Requirements:**
- Overlays som `Image` elements, IKKE custom rendering
- `IsInputTransparent="True"` så de ikke blokerer touch
- Caching via `CacheMode` hvis tilgængeligt

---

## Dependencies Check

**Required Before Starting:**
- [x] MVP komplet
- [x] TASK-029 tema (mørk baggrund)

**Assumptions:**
- MAUI kan vise semi-transparent PNG overlays
- Touch passerer gennem transparent areas

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Opret/Anskaf Texture Assets

**Vignette texture prompt til AI image generator:**
```
Create a vignette overlay texture, 1024x1024 pixels.
- Center should be completely transparent
- Edges fade to semi-transparent black (50% opacity at corners)
- Radial gradient from center to edges
- Smooth, no banding
- PNG format with alpha channel
- Black color only (no hue)
```

**Grain texture prompt:**
```
Create a seamless tileable film grain texture, 256x256 pixels.
- Grayscale noise pattern
- Very subtle, fine grain
- Seamlessly tileable in all directions
- PNG format, grayscale
- Medium-low contrast
```

**Alternativ**: Download gratis textures fra:
- textures.com (grain)
- pngtree.com (vignette)

### Step 2: Tilføj Assets til Projekt

Path: `src/MadeMan.IdleEmpire/Resources/Images/`

Filer:
- `overlay_vignette.png`
- `overlay_grain.png`

### Step 3: Implementer i Layout

**I AppShell.xaml eller MainPage.xaml som top-level overlay:**

```xml
<!-- Content goes here -->

<!-- Vignette Overlay (bag alt interaktivt) -->
<Image Source="overlay_vignette.png"
       Aspect="AspectFill"
       Opacity="0.4"
       InputTransparent="True"
       IsHitTestVisible="False"
       AbsoluteLayout.LayoutBounds="0,0,1,1"
       AbsoluteLayout.LayoutFlags="All"/>

<!-- Grain Overlay -->
<Image Source="overlay_grain.png"
       Aspect="Fill"
       Opacity="0.06"
       InputTransparent="True"
       IsHitTestVisible="False"
       AbsoluteLayout.LayoutBounds="0,0,1,1"
       AbsoluteLayout.LayoutFlags="All"/>
```

### Step 4: Settings Toggle

**Tilføj i SettingsViewModel:**
- `EnableFilmEffects` boolean preference
- Bind overlay `IsVisible` til denne setting

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Visual Quality Test
- [ ] Vignette synlig men subtil (mørkere hjørner)
- [ ] Grain synlig men MEGET subtil
- [ ] Ingen banding eller artifacts
- [ ] Effekter skalerer til skærmstørrelse

### 3. Performance Test
- [ ] Start app, observer FPS
- [ ] Scroll og interact - ingen stutter
- [ ] Sammenlign med/uden overlays
- [ ] Acceptabel: <5% FPS impact

### 4. Interaction Test
- [ ] Alle buttons stadig klikbare
- [ ] Scroll fungerer normalt
- [ ] Touch events passerer gennem overlays

### 5. Manual Test in Emulator
- [ ] Overlays vises korrekt
- [ ] Settings toggle fungerer
- [ ] Ingen visual glitches

---

## Acceptance Criteria

- [ ] Vignette overlay vises med subtil mørkhed i kanter
- [ ] Film grain texture vises over UI
- [ ] Effekter er MEGET subtile (ikke distraherende)
- [ ] Touch/interaction ikke påvirket
- [ ] Settings toggle til at disable effekter
- [ ] Ingen mærkbar performance impact
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Pre-rendered PNG**: Ingen runtime rendering
- **Simple Image elements**: Standard MAUI, ingen custom controls
- **Static overlays**: Ingen animation overhead

### Alternativer overvejet

**Alternative 1: SkiaSharp shader effects**
- Programmatic grain og vignette
- **Hvorfor fravalgt**: Performance overhead, over-engineering

**Alternative 2: Animated grain**
- Grain texture animerer/moves
- **Hvorfor fravalgt**: Significant performance impact, unødvendigt

**Alternative 3: Kun vignette, ingen grain**
- Simplere
- **Hvorfor fravalgt**: Grain tilføjer meget atmosfære for minimal indsats

### Potentielle forbedringer (v2)
- Animated grain (subtle drift)
- Intensity slider i settings
- Screen scratches / dust particles
- Occasional film flicker effect

### Kendte begrænsninger
- **Statisk grain**: Ingen animation for performance
- **Fixed intensity**: Ikke user-adjustable i v1

---

## Kode Kvalitet Checklist

- [ ] **KISS**: PNG overlays, ingen kompleks rendering
- [ ] **Læsbarhed**: Klare asset navne
- [ ] **Performance**: Static images, minimal overhead
- [ ] **Testbarhed**: Toggle on/off for A/B testing
- [ ] **Accessibility**: Kan disables for brugere med visual sensitivitet

---

## Design Notes

**Vignette intensity guide:**
- 20% opacity: Næsten usynlig
- 40% opacity: Subtil, professionel (ANBEFALET start)
- 60% opacity: Tydelig, dramatisk
- 80%+ opacity: For kraftig, distraherende

**Grain intensity guide:**
- 3% opacity: Næsten usynlig
- 6% opacity: Subtil texture (ANBEFALET)
- 10% opacity: Tydelig grain
- 15%+ opacity: For kraftig, looks like compression artifact

---

**Task Status**: READY
**Version**: 1.2
**Last Updated**: 2024-12-27
**Implemented By**: Pending
