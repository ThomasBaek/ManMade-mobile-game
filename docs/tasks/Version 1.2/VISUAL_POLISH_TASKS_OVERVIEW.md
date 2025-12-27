# Visual Polish Tasks Overview - Version 1.2

## Phase 7: Visual Polish & Premium Feel

Disse tasks tilføjer "game juice" og professionel polish til Made Man: Idle Empire.

---

## Task Oversigt

| Task | Navn | Estimat | Prioritet | Bundle |
|------|------|---------|-----------|--------|
| **TASK-075** | Button Micro-Animations | 1-2 timer | 🔴 Høj | Bundle P |
| **TASK-076** | Animated Cash Counter | 2-3 timer | 🔴 Høj | Bundle Q |
| **TASK-077** | Custom Art Deco Typography | 1 time | 🟡 Medium | Bundle Q |
| **TASK-078** | Haptic Feedback System | 30 min | 🟡 Medium | Bundle P |
| **TASK-079** | Film Grain & Vignette | 2-3 timer | 🟡 Medium | Bundle R |
| **TASK-080** | Lottie Animations | 2-3 timer | 🟡 Medium | Bundle R |
| **TASK-081** | Prestige Celebration | 3-4 timer | 🟢 Lav | Bundle S |

**Total estimeret tid:** 12-17 timer

---

## Bundle Struktur

### Bundle P: Animation Foundation (TASK-075 + TASK-078)
**Prioritet:** 🔴 Første bundle - fundament for alt andet

| Task | Beskrivelse | Dependencies |
|------|-------------|--------------|
| TASK-075 | AnimationExtensions helper class (Bounce, Shake, Pulse) | Ingen |
| TASK-078 | HapticService (Click, Success, Error, Heavy) | Ingen |

**Hvorfor først:**
- Opretter `AnimationExtensions.cs` der bruges af TASK-076 og TASK-081
- Opretter `HapticService.cs` der bruges af TASK-081
- Begge er quick wins med stor impact
- Ingen eksterne dependencies

**Deliverables:**
- `Helpers/AnimationExtensions.cs`
- `Services/HapticService.cs`
- Settings toggle for haptics

---

### Bundle Q: Enhanced UI (TASK-076 + TASK-077)
**Prioritet:** 🔴 Core gameplay feel

| Task | Beskrivelse | Dependencies |
|------|-------------|--------------|
| TASK-076 | AnimatedNumberLabel control | TASK-075 (kan bruges, men ikke påkrævet) |
| TASK-077 | Custom fonts (Bebas Neue, Inter) | Ingen |

**Hvorfor denne rækkefølge:**
- TASK-076 er kernen i idle game satisfaction ("number go up")
- TASK-077 transformerer hele UI's æstetik
- Begge kan køre parallelt

**Deliverables:**
- `Controls/AnimatedNumberLabel.cs`
- `Resources/Fonts/BebasNeue-Regular.ttf`
- `Resources/Fonts/Inter-Regular.ttf`
- Font styles i Theme.xaml

---

### Bundle R: Atmospheric Effects (TASK-079 + TASK-080)
**Prioritet:** 🟡 Visual atmosphere

| Task | Beskrivelse | Dependencies |
|------|-------------|--------------|
| TASK-079 | Film grain & vignette PNG overlays | Ingen |
| TASK-080 | Lottie animations setup | Ingen |

**Hvorfor denne rækkefølge:**
- TASK-079 er simpel (PNG overlays)
- TASK-080 kræver NuGet package og asset download
- TASK-080 forbereder til TASK-081

**NuGet Required:**
```xml
<PackageReference Include="SkiaSharp.Extended.UI.Maui" Version="3.0.0" />
```

**Deliverables:**
- `Resources/Images/overlay_vignette.png`
- `Resources/Images/overlay_grain.png`
- `Resources/Raw/anim_coin.json`
- `Resources/Raw/anim_success.json`
- `Helpers/LottieHelper.cs`
- Settings toggle for film effects

---

### Bundle S: Grand Finale (TASK-081)
**Prioritet:** 🟢 Final polish

| Task | Beskrivelse | Dependencies |
|------|-------------|--------------|
| TASK-081 | Prestige Celebration sequence | TASK-075, TASK-078, TASK-080 |

**Hvorfor sidst:**
- Kombinerer alle tidligere effekter
- Kræver AnimationExtensions (075)
- Kræver HapticService (078)
- Bruger Lottie confetti (080)
- "Cherry on top" - den store finish

**Deliverables:**
- `Services/CelebrationService.cs`
- `Resources/Raw/anim_confetti.json`
- Screen flash + confetti + text animation sequence

---

## Dependency Graph

```
Bundle P (Foundation)
┌─────────────────────────────────────┐
│  TASK-075 (Animations)              │
│  TASK-078 (Haptics)                 │
└─────────────────────────────────────┘
              │
              ▼
Bundle Q (UI)                 Bundle R (Atmosphere)
┌─────────────────────┐       ┌─────────────────────┐
│  TASK-076 (Counter) │       │  TASK-079 (Film)    │
│  TASK-077 (Fonts)   │       │  TASK-080 (Lottie)  │
└─────────────────────┘       └─────────────────────┘
              │                         │
              └──────────┬──────────────┘
                         ▼
              Bundle S (Finale)
              ┌─────────────────────┐
              │  TASK-081 (Prestige)│
              └─────────────────────┘
```

---

## Implementation Order

```
Start
  │
  ├─► Bundle P: Animation Foundation
  │     ├─ TASK-078 (Haptics) ──────► 30 min, quick win
  │     └─ TASK-075 (Animations) ───► 1-2 timer
  │
  ├─► Bundle Q + R (kan køres parallelt)
  │     │
  │     ├─ Bundle Q: UI
  │     │    ├─ TASK-077 (Fonts) ────► 1 time
  │     │    └─ TASK-076 (Counter) ──► 2-3 timer
  │     │
  │     └─ Bundle R: Atmosphere
  │          ├─ TASK-079 (Film) ─────► 2-3 timer
  │          └─ TASK-080 (Lottie) ───► 2-3 timer
  │
  └─► Bundle S: Grand Finale
        └─ TASK-081 (Celebration) ──► 3-4 timer

End
```

---

## Performance Considerations

### Alle tasks følger disse principper:

1. **60 FPS target** - Ingen effekt må droppe frames
2. **Memory conscious** - Cleanup efter brug, object pooling
3. **GPU over CPU** - Brug native MAUI animations hvor muligt
4. **Graceful degradation** - Settings toggles for alle effekter
5. **Low-end device testing** - Test på langsom emulator

### Performance Checkpoints:

| Task | Critical Performance Check |
|------|---------------------------|
| 075 | Rapid-fire tapping må ikke stalle |
| 076 | Hurtige cash updates må ikke stacke |
| 077 | Font loading må ikke delay startup |
| 078 | Haptics må ikke delay UI |
| 079 | Overlays må ikke påvirke touch response |
| 080 | Lottie animations må ikke spike memory |
| 081 | Celebration må være smooth 60fps |

---

## Asset Requirements

### Fonts (TASK-077)
- `BebasNeue-Regular.ttf` - Fra Google Fonts (gratis)
- `Inter-Regular.ttf` - Fra Google Fonts (gratis)
- `Inter-SemiBold.ttf` - Fra Google Fonts (gratis)

### Textures (TASK-079)
- `overlay_vignette.png` - 1024x1024, radial transparency
- `overlay_grain.png` - 256x256, tileable noise

### Lottie Files (TASK-080 + TASK-081)
- `anim_coin.json` - Coin/money animation
- `anim_success.json` - Checkmark success
- `anim_confetti.json` - Celebration confetti

**Source:** https://lottiefiles.com (gratis med attribution)

---

## Settings Toggles

Alle visuelle effekter skal have toggles i Settings:

| Setting | Default | Affects Tasks |
|---------|---------|---------------|
| Enable Animations | ON | 075, 076 |
| Enable Haptics | ON | 078 |
| Enable Film Effects | ON | 079 |
| Enable Celebrations | ON | 081 |

---

## Quick Wins vs. Big Impact

### Quick Wins (lav indsats, høj effekt):
- **TASK-078 Haptics** - 30 min, instant premium feel
- **TASK-077 Fonts** - 1 time, transformerer hele UI

### Big Impact (mere arbejde, wow factor):
- **TASK-076 Cash Counter** - Kernen i idle game satisfaction
- **TASK-081 Celebration** - "Shareworthy" moment

---

## Version 1.2 Completion Checklist

- [ ] **Bundle P Complete** - Animation + Haptic foundation
- [ ] **Bundle Q Complete** - Counter + Typography
- [ ] **Bundle R Complete** - Film effects + Lottie
- [ ] **Bundle S Complete** - Prestige celebration
- [ ] **All Settings Toggles** - User can disable effects
- [ ] **Performance Verified** - 60fps on all animations
- [ ] **Build Passes** - 0 errors, 0 warnings

---

**Document Version**: 2.0
**Created**: 2024-12-27
**Updated**: 2024-12-27
**Task Range**: TASK-075 to TASK-081
**Total Tasks**: 7
**Bundles**: 4 (P, Q, R, S)
