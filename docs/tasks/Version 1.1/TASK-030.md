# Task 030: App Ikon

## Metadata
- **Phase**: 6 - UI/UX Redesign
- **Dependencies**: TASK-029 (for farve konsistens)
- **Estimated Time**: 30-60 minutter
- **Status**: READY
- **Design Reference**: docs/tasks/Version 1.1/CLAUDE_CODE_UI_REDESIGN_PROMPT.md (Feature 10)
- **Frequency Impact**: NO

---

## Formaal

Design og implementer et professionelt app-ikon der repraesenterer spillet med 1930s mafia aestetik.

**Hvorfor dette er vigtigt:**
- App ikon er foerste indtryk
- Skal skille sig ud i app store
- Repraesenterer brandidentitet

---

## Risici

### Potentielle Problemer
1. **Ikon vises ikke korrekt**:
   - Edge case: Forkert stoerrelse eller format
   - Impact: Generisk eller manglende ikon

2. **Platform-specifikke krav**:
   - Android kraever adaptive icons
   - iOS kraever specifikke stoerrrelser

### Mitigering
- Foelg MAUI ikon krav noeje
- Test pa fysisk enhed (emulator viser ikke altid korrekt)

---

## Analyse - Hvad Skal Implementeres

### Ikon Design Koncept
**Primaer:** Fedora hat med guldkant pa moerk baggrund

**Design Specifikationer:**
- Main element: Klassisk fedora hat set forfra, let tilted
- Style: Minimalistisk art deco, staerk silhuet
- Farver:
  - Background: Dark navy (#1A1A2E) med subtle gradient
  - Hat: Sort med guld (#D4AF37) trim/band
  - Subtle skygge underneden
- Shape: Rounded square (standard app ikon)

### Stoorrelser
| Stoerrelse | Brug |
|------------|------|
| 1024x1024 | Master/source |
| 512x512 | Hoj oploesning |
| 192x192 | Android adaptive |
| 144x144 | Android |
| 96x96 | Android |
| 72x72 | Android |
| 48x48 | Android |

### SVG Alternativ
Da Claude Code ikke kan generere billeder, kan vi:
1. Bruge eksisterende `icon.png` i Resources/AppIcon/
2. Oprette et simpelt SVG ikon som placeholder
3. Dokumentere krav for professionelt ikon

---

## Dependencies Check

**Required Before Starting:**
- [x] TASK-029 (farver defineret)

**Assumptions:**
- Der eksisterer allerede en icon.png fil

**Blockers:**
- Ingen (kan bruge eksisterende eller placeholder)

---

## Implementation Guide

### Step 1: Verificer eksisterende ikon

Check om der er et eksisterende ikon:
```bash
ls src/MadeMan.IdleEmpire/Resources/AppIcon/
```

### Step 2: Opdater .csproj med ikon reference

Path: `src/MadeMan.IdleEmpire/MadeMan.IdleEmpire.csproj`

Verificer at MauiIcon er korrekt konfigureret:
```xml
<!-- App Icon -->
<MauiIcon Include="Resources\AppIcon\appicon.svg"
          ForegroundFile="Resources\AppIcon\appiconfg.svg"
          Color="#1A1A2E" />
```

Eller for PNG:
```xml
<MauiIcon Include="Resources\AppIcon\icon.png" />
```

### Step 3: Opret simpelt SVG ikon (hvis nødvendigt)

Hvis der ikke findes et tilfredsstillende ikon, kan vi oprette en simpel SVG:

Path: `src/MadeMan.IdleEmpire/Resources/AppIcon/appicon.svg`

```svg
<?xml version="1.0" encoding="UTF-8"?>
<svg width="512" height="512" viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg">
  <!-- Background -->
  <rect width="512" height="512" rx="64" fill="#1A1A2E"/>

  <!-- Fedora Hat Silhouette -->
  <g transform="translate(256, 280)">
    <!-- Hat brim -->
    <ellipse cx="0" cy="40" rx="180" ry="35" fill="#0D0D0D"/>

    <!-- Hat crown -->
    <path d="M-120,40 C-120,-20 -80,-80 0,-80 C80,-80 120,-20 120,40 L100,40 C100,-10 70,-60 0,-60 C-70,-60 -100,-10 -100,40 Z" fill="#0D0D0D"/>

    <!-- Hat band (gold) -->
    <rect x="-100" y="10" width="200" height="20" rx="3" fill="#D4AF37"/>

    <!-- Hat crease -->
    <path d="M-60,-40 Q0,-60 60,-40" stroke="#1A1A2E" stroke-width="8" fill="none"/>
  </g>

  <!-- Subtle shadow -->
  <ellipse cx="256" cy="400" rx="120" ry="20" fill="#000000" opacity="0.3"/>
</svg>
```

### Step 4: Test Build

```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Manual Test
- [ ] Byg APK og installer pa enhed
- [ ] Verificer ikon vises i app drawer
- [ ] Ikon er genkendeligt og matcher tema

---

## Acceptance Criteria

- [ ] Ikon defineret i projekt
- [ ] Build succeeds
- [ ] Ikon synlig pa emulator/enhed
- [ ] Matcher app aestetik (moerke farver, guld accent)

---

## Kode Evaluering

### Simplifikations-tjek
- **Brug eksisterende asset**: Hvis icon.png findes, brug den
- **SVG placeholder**: Simpelt design der virker
- **Ingen eksterne dependencies**: Kun MAUI standard

### Alternativer overvejet

**Alternative 1: Professionel grafiker**
- Hyr designer til ikon
- **Hvorfor fravalgt**: Ud af scope for Claude Code

**Alternative 2: AI genereret billede**
- Brug DALL-E eller Midjourney
- **Hvorfor fravalgt**: Claude Code kan ikke generere billeder

### Potentielle forbedringer (v2)
- Professionelt designet ikon
- Animated ikon til splash
- Adaptive ikon med separate layers

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Simpleste fungerende ikon
- [ ] **Laesbarhed**: SVG er struktureret og kommenteret
- [ ] **Performance**: Ikon optimeret til mobile
- [ ] **Testbarhed**: Visuelt verificerbar

---

## Grafik Asset Note

**Hvis professionelt ikon oenskes, brug denne prompt til AI grafik generator:**

```
Mobile app icon for "Made Man" - a 1930s mafia idle game.
Main element: Classic fedora hat viewed from front angle, slightly tilted
Style: Minimalist art deco, strong silhouette
Colors:
  - Background: Dark navy (#1A1A2E) with subtle gradient
  - Hat: Black with gold (#D4AF37) trim/band
  - Subtle shadow underneath
Shape: Rounded square (standard app icon)
Sizes needed: 1024x1024 (master), 512x512, 192x192, 144x144, 96x96, 72x72, 48x48
Format: PNG with no transparency (filled background)
Additional: Add subtle texture/grain for vintage feel
```

---

**Task Status**: READY
**Last Updated**: 2024-12-26
**Implemented By**: Pending
