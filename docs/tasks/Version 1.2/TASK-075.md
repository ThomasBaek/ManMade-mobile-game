# Task 075: Button Micro-Animations

## Metadata
- **Phase**: 7 - Visual Polish
- **Bundle**: P - Animation Foundation
- **Dependencies**: Ingen
- **Estimated Time**: 1-2 hours
- **Status**: READY
- **Priority**: 🔴 Høj
- **Frequency Impact**: NO

---

## Formål

Tilføj subtile micro-animationer til interaktive elementer for at give spillet en mere poleret og responsiv følelse.

**Hvorfor dette er vigtigt:**
- Giver instant visual feedback ved brugerinteraktion
- Får spillet til at føles mere "alive" og premium
- Industri-standard i moderne mobile spil
- Forbedrer perceived performance

---

## Risici

### Potentielle Problemer
1. **Performance degradation**:
   - Edge case: Mange samtidige animationer
   - Impact: Jank/stuttering, især på ældre devices

2. **Over-animation**:
   - For lange eller for hyppige animationer irriterer brugeren
   - Impact: Dårlig UX, føles "langsomt"

### Mitigering
- Hold animationer MEGET korte (100-150ms max)
- Brug native MAUI animations (GPU-accelerated)
- Test på low-end emulator/device
- Ingen animation på rapid-fire taps (debounce)

---

## Analyse - Hvad Skal Implementeres

### 1. AnimationExtensions Helper Class
**Description**: Reusable extension methods for common animations
**Location**: `src/MadeMan.IdleEmpire/Helpers/AnimationExtensions.cs`

**Key Requirements:**
- `BounceAsync()` - Scale ned og op (tap feedback)
- `PulseAsync()` - Subtle glow/scale pulse (attention)
- `ShakeAsync()` - Horizontal shake (error/can't afford)
- Alle metoder skal være `async Task` og cancelable
- Alle skal bruge `Easing.CubicOut` for naturlig feel

### 2. Påfør på Operation Cards
**Description**: Bounce animation når spiller køber/upgrader
**Location**: `src/MadeMan.IdleEmpire/Views/MainPage.xaml.cs` eller via Behavior

**Key Requirements:**
- Trigger ved successful køb
- IKKE ved "can't afford" (brug shake i stedet)
- Max 150ms varighed

### 3. Påfør på Prestige Button
**Description**: Subtle pulse når prestige er tilgængelig
**Location**: Prestige panel/button

**Key Requirements:**
- Continuous subtle pulse når `CanPrestige == true`
- Stop animation når modal åbnes
- Brug `CancellationToken` for cleanup

---

## Dependencies Check

**Required Before Starting:**
- [x] MVP komplet
- [x] TASK-029 tema implementeret (farver)

**Assumptions:**
- Buttons bruger standard MAUI Button eller custom controls
- MVVM pattern følges (commands på buttons)

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Opret AnimationExtensions.cs

Path: `src/MadeMan.IdleEmpire/Helpers/AnimationExtensions.cs`

**Design Intent (ikke fuld kode):**
```csharp
public static class AnimationExtensions
{
    // Bounce: Scale 1.0 → 0.92 → 1.0 over 120ms
    public static async Task BounceAsync(this VisualElement element)
    
    // Shake: Translate X -8 → +8 → 0 over 200ms (for errors)
    public static async Task ShakeAsync(this VisualElement element)
    
    // Pulse: Scale 1.0 → 1.05 → 1.0, repeat until cancelled
    public static async Task PulseAsync(this VisualElement element, CancellationToken ct)
}
```

**Animation Timing Values (optimeret til mobile):**

| Animation | Duration | Easing | Values |
|-----------|----------|--------|--------|
| Bounce down | 60ms | CubicOut | Scale 1.0 → 0.92 |
| Bounce up | 60ms | CubicOut | Scale 0.92 → 1.0 |
| Shake | 50ms x 4 | Linear | X: 0 → -8 → 8 → -4 → 0 |
| Pulse | 400ms | SinInOut | Scale 1.0 → 1.05 → 1.0 |

### Step 2: Integrer med Button Taps

**Approach A: Code-behind (simplest)**
- I `MainPage.xaml.cs`, tilføj animation før command execution

**Approach B: Custom Behavior (reusable)**
- Opret `BounceBehavior` der kan attached til enhver Button
- Anbefalet for consistency

### Step 3: Prestige Pulse Animation

- Start pulse når `CanPrestige` property ændres til `true`
- Brug `PropertyChanged` event eller `INotifyPropertyChanged`
- Husk `CancellationTokenSource` for at stoppe ved navigation

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Performance Test
- [ ] Kør app i emulator
- [ ] Tap hurtigt på flere knapper (spam click)
- [ ] Observer for jank eller frame drops
- [ ] Check memory usage ikke stiger konstant

### 3. Manual Test in Emulator
- [ ] Køb operation → bounce animation spiller
- [ ] "Can't afford" → shake animation spiller
- [ ] Prestige tilgængelig → button pulser subtilt
- [ ] Animationer føles responsive (<150ms)

---

## Acceptance Criteria

- [ ] AnimationExtensions helper class oprettet
- [ ] Bounce animation på alle interaktive køb-knapper
- [ ] Shake animation ved "can't afford" attempts
- [ ] Pulse animation på prestige button når tilgængelig
- [ ] Alle animationer er under 200ms
- [ ] Ingen performance regression (60fps maintained)
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Native MAUI animations**: Ingen eksterne libraries
- **Extension methods**: Let at genbruge, ingen kompleks arkitektur
- **Korte animationer**: Minimalt CPU/GPU overhead

### Alternativer overvejet

**Alternative 1: SkiaSharp custom animations**
- Mere kontrol, bedre visuals
- **Hvorfor fravalgt**: Over-engineering, ekstra dependency, performance concern

**Alternative 2: Lottie for alle animationer**
- Konsistent animation system
- **Hvorfor fravalgt**: Overkill for simple scale/translate, større memory footprint

### Potentielle forbedringer (v2)
- Konfigurerbar animation speed i Settings
- Reduktion/disable for accessibility
- Flere animation varianter (slide, fade)

### Kendte begrænsninger
- **Ingen particle effects**: Gemmes til separat task
- **Ingen custom easing curves**: Bruger built-in easings

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Er dette den simpleste løsning? (Ja - native MAUI)
- [ ] **Læsbarhed**: Extension methods med klare navne
- [ ] **Navngivning**: BounceAsync, ShakeAsync, PulseAsync
- [ ] **Funktioner**: Hver animation er én metode
- [ ] **DRY**: Reusable helpers, ikke copy-paste
- [ ] **Error handling**: CancellationToken support
- [ ] **Edge cases**: Rapid tapping handled
- [ ] **Performance**: GPU-accelerated, short duration
- [ ] **Testbarhed**: Kan testes visuelt i emulator

---

## Notes

- Test på både hurtig og langsom emulator
- Overvej at tilføje haptic feedback i samme tap handler (se TASK-078)
- Animation timing kan finjusteres efter user testing

---

**Task Status**: READY
**Version**: 1.2
**Last Updated**: 2024-12-27
**Implemented By**: Pending
