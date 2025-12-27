# Task 076: Animated Cash Counter

## Metadata
- **Phase**: 7 - Visual Polish
- **Dependencies**: TASK-029 (Tema)
- **Estimated Time**: 2-3 hours
- **Status**: READY
- **Design Reference**: N/A
- **Frequency Impact**: NO

---

## Formål

Erstat instant cash-opdateringer med smooth animeret optælling for at give en mere tilfredsstillende visuel oplevelse.

**Hvorfor dette er vigtigt:**
- "Number go up" er kernen i idle games - det skal føles godt
- Animeret optælling giver dopamin-hit ved store earnings
- Industri-standard i alle succesfulde idle/clicker games
- Gør offline earnings reveal meget mere dramatisk

---

## Risici

### Potentielle Problemer
1. **Performance ved hurtige opdateringer**:
   - Edge case: Cash opdateres 10x per sekund, animationer overlapper
   - Impact: Stuttering, forkerte tal, memory leaks

2. **Læsbarhed**:
   - Tal der konstant ændrer sig kan være svære at læse
   - Impact: Frustration, spiller kan ikke se præcis beløb

3. **Animation "lag"**:
   - Animeret værdi hænger bagefter real værdi
   - Impact: UI føles unresponsive

### Mitigering
- Brug interpolation med fast target (ikke kø af animationer)
- Cancel igangværende animation når ny starter
- Vis ALTID korrekt slutværdi
- Tilpas animation speed til delta størrelse

---

## Analyse - Hvad Skal Implementeres

### 1. AnimatedNumberLabel Custom Control
**Description**: Custom Label der animerer mellem værdier
**Location**: `src/MadeMan.IdleEmpire/Controls/AnimatedNumberLabel.cs`

**Key Requirements:**
- BindableProperty for `TargetValue` (decimal)
- BindableProperty for `Duration` (ms, default 300)
- BindableProperty for `Format` (string, default "$#,##0")
- Smooth interpolation mellem current og target
- Auto-cancel previous animation ved ny target
- ALTID vis korrekt værdi når animation slutter

### 2. Integration i MainPage
**Description**: Erstat standard cash Label med AnimatedNumberLabel
**Location**: `src/MadeMan.IdleEmpire/Views/MainPage.xaml`

**Key Requirements:**
- Top bar cash display bruger AnimatedNumberLabel
- Bind til ViewModel's Cash property
- Format med tusin-separatorer og dollar sign

### 3. Smart Animation Speed
**Description**: Hurtigere animation ved små ændringer, langsommere ved store
**Location**: I AnimatedNumberLabel logic

**Key Requirements:**
- Delta < 100: Instant (ingen animation)
- Delta < 1000: 150ms
- Delta < 10000: 300ms  
- Delta > 10000: 500ms (dramatic effect)

---

## Dependencies Check

**Required Before Starting:**
- [x] MVP komplet
- [x] Cash display fungerer med binding

**Assumptions:**
- Cash property er `decimal` eller `double`
- Updates sker via INotifyPropertyChanged

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Opret AnimatedNumberLabel.cs

Path: `src/MadeMan.IdleEmpire/Controls/AnimatedNumberLabel.cs`

**Design Intent:**
```csharp
public class AnimatedNumberLabel : Label
{
    // Bindable: TargetValue - den værdi vi animerer MOD
    // Bindable: Duration - animation tid i ms
    // Bindable: Format - number format string
    
    // Private: _displayValue - den værdi vi faktisk viser
    // Private: _animationCts - CancellationTokenSource
    
    // Når TargetValue ændres:
    // 1. Cancel existing animation
    // 2. Beregn smart duration baseret på delta
    // 3. Start smooth interpolation fra _displayValue til TargetValue
    // 4. Opdater Text property løbende
    // 5. Når færdig: Sæt _displayValue = TargetValue exactly
}
```

**Animation Algorithm (vigtig for smoothness):**
```
Brug Lerp (Linear Interpolation) med easing:
- t = elapsed / duration (0.0 til 1.0)
- easedT = EaseOutQuad(t) = 1 - (1-t)² 
- displayValue = startValue + (targetValue - startValue) * easedT

Opdater med ~60fps (hvert 16ms) via Device.StartTimer eller Task.Delay
```

### Step 2: Opdater MainPage.xaml

**Approach:**
```xml
<!-- Før -->
<Label Text="{Binding CashDisplay}" ... />

<!-- Efter -->
<controls:AnimatedNumberLabel 
    TargetValue="{Binding Cash}"
    Format="$#,##0"
    Duration="300"
    ... />
```

### Step 3: Test Performance

- Sæt game tick til hurtig (100ms)
- Observer at animationer ikke stacker
- Verify final value er altid korrekt

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Animation Smoothness Test
- [ ] Start app, observer cash counter
- [ ] Tal tæller smoothly op (ikke instant jumps)
- [ ] Ved hurtige opdateringer: ingen stutter
- [ ] Ved store jumps (offline earnings): dramatisk optælling

### 3. Accuracy Test
- [ ] Pause spillet, notér cash
- [ ] Resume, vent på animation
- [ ] Verify endelig værdi er matematisk korrekt
- [ ] Ingen "drifting" over tid

### 4. Manual Test in Emulator
- [ ] Normal gameplay: smooth counting
- [ ] Køb upgrade: cash falder animeret
- [ ] Offline earnings: dramatisk optælling
- [ ] Prestige reset: instant til 0 (ingen animation)

---

## Acceptance Criteria

- [ ] AnimatedNumberLabel custom control oprettet
- [ ] Cash display bruger animeret counter
- [ ] Smooth animation ved værdiændringer
- [ ] Smart duration baseret på delta størrelse
- [ ] Ingen performance issues ved hurtige opdateringer
- [ ] Korrekt slutværdi ALTID
- [ ] Works med tusin-separatorer og formatering
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Custom control**: Selvcontained, ingen global state
- **Native rendering**: Bruger standard Label underneath
- **Simple math**: Linear interpolation med easing

### Alternativer overvejet

**Alternative 1: SkiaSharp rendered numbers**
- Mere visuel kontrol, custom fonts
- **Hvorfor fravalgt**: Over-engineering, performance overhead

**Alternative 2: Pre-baked Lottie animation**
- Smooth, designer-controlled
- **Hvorfor fravalgt**: Kan ikke vise arbitrære tal

**Alternative 3: String character-by-character animation**
- Hver digit animerer separat (slot machine effect)
- **Hvorfor fravalgt**: Komplekst, kan tilføjes i v2

### Potentielle forbedringer (v2)
- Slot machine digit animation
- Color flash ved store earnings
- Particle effects ved milestones
- Sound effect synced med animation

### Kendte begrænsninger
- **Ingen color change**: Tal ændrer ikke farve baseret på værdi
- **Simpel easing**: Kun ease-out, ingen bounce

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Custom control med minimal kompleksitet
- [ ] **Læsbarhed**: Klar separation af animation logic
- [ ] **Navngivning**: AnimatedNumberLabel, TargetValue, etc.
- [ ] **Funktioner**: Animation loop er isoleret
- [ ] **DRY**: Reusable for alle tal i app
- [ ] **Error handling**: CancellationToken, null checks
- [ ] **Edge cases**: Negative delta, zero, very large numbers
- [ ] **Performance**: 60fps target, minimal allocations
- [ ] **Testbarhed**: Kan unit testes (animation logic)

---

## Notes

- Overvej at tilføje subtle "glow" effect ved store gains
- Kan genbruges til andre tal (income rate, prestige bonus)
- Animation skal IKKE spille ved første load (initial state)

---

**Task Status**: READY
**Version**: 1.2
**Last Updated**: 2024-12-27
**Implemented By**: Pending
