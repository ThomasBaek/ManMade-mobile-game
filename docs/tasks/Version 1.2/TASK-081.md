# Task 081: Prestige Celebration Effects

## Metadata
- **Phase**: 7 - Visual Polish
- **Bundle**: S - Grand Finale
- **Dependencies**: TASK-075, TASK-078, TASK-080 (alle anbefalet)
- **Estimated Time**: 3-4 hours
- **Status**: READY
- **Priority**: 🟢 Lav (men stor impact!)
- **Frequency Impact**: NO

---

## Formål

Skab en memorable "celebration moment" når spilleren prestiger, med particle effects og visuel fanfare.

**Hvorfor dette er vigtigt:**
- Prestige er spillets største milestone - det SKAL føles epic
- Celebration effects giver dopamin rush og motivation til at prestige igen
- Differentierer fra konkurrenter
- "Shareworthy moment" potentiale

---

## Risici

### Potentielle Problemer
1. **Performance ved mange partikler**:
   - Edge case: 500+ partikler på ældre device
   - Impact: Frame drops, crash

2. **Overdone effects**:
   - For mange/for lange effekter føles cheap
   - Impact: "Casino game" vibes, negative reviews

3. **Implementerings kompleksitet**:
   - Custom particle system er komplekst
   - Impact: Bugs, lang development tid

### Mitigering
- Brug Lottie animation i stedet for custom particles (TASK-080)
- ELLER brug simpel SkiaSharp particle system med max 100 partikler
- Hold celebration under 3 sekunder
- Test på low-end device tidligt
- Start simpelt, tilføj mere hvis performance tillader

---

## Analyse - Hvad Skal Implementeres

### Implementation Options (vælg én)

#### Option A: Lottie Confetti (ANBEFALET)
**Pros**: Simpel, performant, professionel
**Cons**: Mindre kontrol, fixed design

**Approach:**
- Download confetti Lottie animation
- Play fullscreen ved prestige
- Kombiner med screen flash og haptic

#### Option B: Simple SkiaSharp Particles
**Pros**: Fuld kontrol, kan tilpasses
**Cons**: Mere komplekst, performance risiko

**Approach:**
- Custom SKCanvasView med particle loop
- 50-100 partikler max
- Simple physics (gravity, fade)

#### Option C: Hybrid (Anbefalet for best results)
**Approach:**
- Lottie confetti som base
- Simple screen flash (native MAUI)
- Haptic feedback burst
- Sound effect (fremtidig)

### Celebration Sequence

```
T+0ms:    Screen flash (white overlay, quick fade)
T+50ms:   Haptic burst (Heavy)
T+100ms:  Confetti animation starts
T+100ms:  "PRESTIGE COMPLETE" text animation
T+500ms:  Bonus text appears ("+50% income!")
T+2000ms: Confetti fades
T+2500ms: Return to normal gameplay
```

### Components at Implementere

| Component | Type | Location |
|-----------|------|----------|
| Screen Flash | MAUI Animation | Fullscreen overlay |
| Confetti | Lottie eller Particles | Fullscreen overlay |
| Text Animation | MAUI Animation | Center screen |
| Haptic Burst | HapticService | N/A |

---

## Dependencies Check

**Required Before Starting:**
- [x] MVP komplet
- [x] Prestige system fungerer

**Recommended:**
- [ ] TASK-075 (Animation helpers)
- [ ] TASK-078 (Haptic service)
- [ ] TASK-080 (Lottie setup)

**Assumptions:**
- Prestige modal/flow eksisterer
- Kan injecte celebration sequence

**Blockers:**
- Ingen hard blockers, men bedre med TASK-080 først

---

## Implementation Guide

### Step 1: Screen Flash Effect

**Simpel MAUI animation:**
```csharp
// I celebration sequence
var flashOverlay = new BoxView { Color = Colors.White, Opacity = 0 };
// Add to layout

await flashOverlay.FadeTo(0.8, 50, Easing.CubicIn);
await flashOverlay.FadeTo(0, 200, Easing.CubicOut);
```

### Step 2: Confetti Effect

**Option A: Lottie (hvis TASK-080 er komplet)**
```csharp
confettiAnimation.IsVisible = true;
confettiAnimation.Progress = 0;
await Task.Delay(2500); // Animation duration
confettiAnimation.IsVisible = false;
```

**Option B: Simple particles (standalone)**
- Se "Particle System Implementation" sektion nedenfor

### Step 3: Text Animation

**"PRESTIGE COMPLETE" animeret ind:**
```csharp
prestigeLabel.Scale = 0;
prestigeLabel.Opacity = 0;
prestigeLabel.IsVisible = true;

await Task.WhenAll(
    prestigeLabel.ScaleTo(1, 400, Easing.BounceOut),
    prestigeLabel.FadeTo(1, 300)
);
```

### Step 4: Orchestrate Sequence

Path: `src/MadeMan.IdleEmpire/Services/CelebrationService.cs`

**Design Intent:**
```csharp
public interface ICelebrationService
{
    Task PlayPrestigeCelebrationAsync();
}

public class CelebrationService : ICelebrationService
{
    // Coordonerer alle celebration elements
    // Timing, sequencing, cleanup
}
```

---

## Particle System Implementation (Kun hvis Option B vælges)

**⚠️ Kun implementer dette hvis Lottie ikke bruges**

Path: `src/MadeMan.IdleEmpire/Controls/ParticleView.cs`

**Design Principles:**
- Max 100 partikler
- Simple struct for partikel data (position, velocity, life, color)
- Fixed update rate (60fps target)
- Object pooling - ingen allocations under animation
- Auto-stop når alle partikler er døde

**Partikel Properties:**
```csharp
struct Particle
{
    float X, Y;           // Position
    float VelX, VelY;     // Velocity
    float Life;           // 0-1, fades out
    Color Color;          // Gold, Red, White
    float Size;           // Radius
    float Rotation;       // For visual variety
}
```

**Physics (simpel):**
- Gravity: VelY += 0.3 per frame
- Fade: Life -= 0.01 per frame
- Remove when Life <= 0 or Y > screen height

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Performance Test
- [ ] Trigger prestige celebration
- [ ] Observe frame rate (skal være smooth)
- [ ] Ingen memory spike der ikke falder igen
- [ ] Trigger celebration 5x i træk - stadig performant?

### 3. Visual Quality Test
- [ ] Flash er hurtig og ikke blinding
- [ ] Confetti er synlig men ikke overwhelming
- [ ] Text animation er smooth
- [ ] Timing føles "right"

### 4. Manual Test in Emulator
- [ ] Prestige → celebration spiller
- [ ] Celebration stopper efter ~2-3 sekunder
- [ ] Kan interagere med UI efter celebration
- [ ] Haptic feedback mærkes (test på device)

---

## Acceptance Criteria

- [ ] Screen flash ved prestige start
- [ ] Confetti/particle effect vises
- [ ] "Prestige Complete" text animerer ind
- [ ] Bonus info vises tydeligt
- [ ] Celebration varer 2-3 sekunder
- [ ] Smooth performance (60fps)
- [ ] Cleanup efter celebration (ingen lingering elements)
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Lottie preferred**: Undgå custom particle system hvis muligt
- **Simple sequence**: Linear flow, ingen kompleks state machine
- **Reuse**: Brug eksisterende animation helpers

### Alternativer overvejet

**Alternative 1: Unity-style particle system**
- Fuldt featured particle editor
- **Hvorfor fravalgt**: Massivt overkill, wrong tool

**Alternative 2: Video overlay**
- Pre-rendered celebration video
- **Hvorfor fravalgt**: Stor filstørrelse, ingen customization

**Alternative 3: Ingen celebration, bare modal**
- Simpelt
- **Hvorfor fravalgt**: Missed opportunity for delight

### Potentielle forbedringer (v2)
- Different celebrations per prestige level
- Sound effects synchronized
- Screenshot/share prompt efter celebration
- Custom particle colors baseret på earned bonus

### Kendte begrænsninger
- **Ingen lyd**: Sound system ikke implementeret endnu
- **Fixed celebration**: Samme hver gang i v1

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Prefer Lottie over custom particles
- [ ] **Læsbarhed**: Clear sequence of events
- [ ] **Performance**: Test on low-end device
- [ ] **Memory**: No leaks, proper cleanup
- [ ] **Error handling**: Graceful failure if assets missing
- [ ] **Testbarhed**: Can trigger celebration manually for testing

---

## Design Notes

**Celebration Duration:**
- Under 2 sec: Feels rushed
- 2-3 sec: Sweet spot
- Over 4 sec: Feels slow, annoying on repeat

**Visual Intensity:**
- Flash: Very brief (50-100ms), not full white (80% opacity)
- Confetti: Dense at start, thins out
- Text: Big but not screen-filling

**Color Palette for Particles:**
- Gold (#D4AF37) - Primary, 50%
- White (#FFFFFF) - Secondary, 30%
- Blood Red (#8B0000) - Accent, 20%

---

**Task Status**: READY
**Version**: 1.2
**Last Updated**: 2024-12-27
**Implemented By**: Pending
