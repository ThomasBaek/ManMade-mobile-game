# Visual Polish Tasks Overview (TASK-041 til TASK-047)

## Phase 7: Visual Polish & Premium Feel

Disse tasks tilføjer "game juice" og professionel polish til Made Man: Idle Empire.

---

## Task Oversigt

| Task | Navn | Tid | Prioritet | Dependencies |
|------|------|-----|-----------|--------------|
| **TASK-041** | Button Micro-Animations | 1-2 timer | 🔴 Høj | TASK-029 |
| **TASK-042** | Animated Cash Counter | 2-3 timer | 🔴 Høj | TASK-029 |
| **TASK-043** | Custom Art Deco Typography | 1 time | 🟡 Medium | TASK-029 |
| **TASK-044** | Haptic Feedback System | 30 min | 🟡 Medium | None |
| **TASK-045** | Film Grain & Vignette | 2-3 timer | 🟡 Medium | TASK-029 |
| **TASK-046** | Lottie Animations | 2-3 timer | 🟡 Medium | TASK-029 |
| **TASK-047** | Prestige Celebration | 3-4 timer | 🟢 Lav | TASK-041, TASK-046 |

**Total estimeret tid:** 12-17 timer

---

## Anbefalet Implementation Order

```
1. TASK-041 (Bounce animations)     ─┐
                                     ├─► Core polish
2. TASK-042 (Cash counter)          ─┘

3. TASK-043 (Fonts)                 ─┐
                                     ├─► Tema enhancement
4. TASK-044 (Haptics)               ─┘

5. TASK-045 (Film effects)          ─┐
                                     ├─► Atmosphere
6. TASK-046 (Lottie)                ─┘

7. TASK-047 (Celebration)           ─► Grand finale (bruger 041+046)
```

---

## Dependency Graph

```
TASK-029 (Tema) ────────────────────────────────────┐
        │                                           │
        ├──► TASK-041 (Animations) ─────────────────┤
        │            │                              │
        │            └──► TASK-047 (Celebration) ◄──┤
        │                        ▲                  │
        ├──► TASK-042 (Cash Counter)                │
        │                                           │
        ├──► TASK-043 (Fonts)                       │
        │                                           │
        ├──► TASK-045 (Film Effects)                │
        │                                           │
        └──► TASK-046 (Lottie) ─────────────────────┘
                     │
                     └──► TASK-047 (Celebration)

TASK-044 (Haptics) ── No dependencies
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
| 041 | Rapid-fire tapping må ikke stalle |
| 042 | Hurtige cash updates må ikke stacke |
| 043 | Font loading må ikke delay startup |
| 044 | Haptics må ikke delay UI |
| 045 | Overlays må ikke påvirke touch response |
| 046 | Lottie animations må ikke spike memory |
| 047 | Celebration må være smooth 60fps |

---

## NuGet Packages Required

```xml
<!-- For TASK-046 og TASK-047 (optional) -->
<PackageReference Include="SkiaSharp.Extended.UI.Maui" Version="3.0.0" />
```

---

## Asset Requirements

### Fonts (TASK-043)
- `BebasNeue-Regular.ttf` - Fra Google Fonts
- `Inter-Regular.ttf` - Fra Google Fonts
- `Inter-SemiBold.ttf` - Fra Google Fonts

### Textures (TASK-045)
- `overlay_vignette.png` - 1024x1024, radial transparency
- `overlay_grain.png` - 256x256, tileable noise

### Lottie Files (TASK-046)
- `anim_coin.json` - Coin/money animation
- `anim_success.json` - Checkmark success
- `anim_confetti.json` - Celebration confetti

**Source:** https://lottiefiles.com (gratis med attribution)

---

## Settings Toggles

Alle visuelle effekter skal have toggles i Settings:

| Setting | Default | Affects |
|---------|---------|---------|
| Enable Animations | ON | TASK-041, 042 |
| Enable Haptics | ON | TASK-044 |
| Enable Film Effects | ON | TASK-045 |
| Enable Celebrations | ON | TASK-047 |

---

## Quick Wins vs. Big Impact

### Quick Wins (lav indsats, høj effekt):
- **TASK-044 Haptics** - 30 min, instant premium feel
- **TASK-043 Fonts** - 1 time, transformerer hele UI

### Big Impact (mere arbejde, wow factor):
- **TASK-042 Cash Counter** - Kernen i idle game satisfaction
- **TASK-047 Celebration** - "Shareworthy" moment

---

## Notes for Claude Code

- Tasks er designet til at kunne implementeres uafhængigt
- Hver task har verification steps - følg dem
- Performance testing er OBLIGATORISK før completion
- Start med TASK-041 + TASK-042 for hurtigst synligt resultat
- Test på emulator OG fysisk device (haptics virker ikke i emulator)

---

**Document Version**: 1.0
**Created**: 2024-12-27
**Task Range**: TASK-041 to TASK-047
**Total Tasks**: 7
