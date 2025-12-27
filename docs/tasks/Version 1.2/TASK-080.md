# Task 080: Lottie Coin & Success Animations

## Metadata
- **Phase**: 7 - Visual Polish
- **Dependencies**: TASK-029 (Tema)
- **Estimated Time**: 2-3 hours
- **Status**: READY
- **Design Reference**: N/A
- **Frequency Impact**: NO

---

## Formål

Tilføj professionelle Lottie-animationer for key moments som køb, upgrade success, og coin effects.

**Hvorfor dette er vigtigt:**
- Lottie animationer er industri-standard for polerede apps
- Giver "wow" moments ved vigtige handlinger
- Professionelt look uden at kræve animation skills
- Mange gratis animationer tilgængelige

---

## Risici

### Potentielle Problemer
1. **Package compatibility**:
   - Edge case: Lottie NuGet package ikke kompatibel med .NET 9/10
   - Impact: Build fejler eller runtime crash

2. **Memory usage**:
   - Mange Lottie views kan bruge betydelig hukommelse
   - Impact: App slowdown, OOM på ældre devices

3. **Animation file size**:
   - Lottie JSON filer kan være store
   - Impact: Større app bundle

### Mitigering
- Brug `SkiaSharp.Extended.UI.Maui` (velvedligeholdt package)
- Load animations on-demand, dispose efter brug
- Vælg simple, optimerede animationer (<50KB)
- Cache genbrugte animationer
- Test på low-memory emulator

---

## Analyse - Hvad Skal Implementeres

### 1. NuGet Package Installation
**Package**: `SkiaSharp.Extended.UI.Maui`
**Location**: Project file

### 2. Lottie Animations at Tilføje

| Animation | Trigger | Placement | File |
|-----------|---------|-----------|------|
| Coin/Money | Successful purchase | Over købt item | `coin_collect.json` |
| Checkmark | Upgrade complete | Modal/Toast | `success_check.json` |
| Sparkle/Shine | Prestige available | Around prestige button | `sparkle.json` |
| Confetti | Prestige complete | Fullscreen overlay | `confetti.json` |

### 3. Find/Download Animations
**Source**: https://lottiefiles.com (gratis med attribution)

**Søgeord:**
- "coin collect" / "money"
- "success checkmark"
- "sparkle gold"
- "confetti celebration"

**Kriterier for valg:**
- Under 50KB filstørrelse
- Simple (ikke for mange layers)
- Farvepalet der matcher tema (guld, rød)
- Gratis for kommerciel brug

### 4. LottieAnimationView Integration
**Description**: Wrapper component for at vise animationer
**Location**: Custom control eller direkte i XAML

---

## Dependencies Check

**Required Before Starting:**
- [x] MVP komplet
- [x] TASK-029 tema implementeret

**Assumptions:**
- SkiaSharp.Extended.UI.Maui er kompatibelt med .NET 10
- Animationer kan customizes farver (ikke alle kan)

**Blockers:**
- Verificer NuGet package kompatibilitet først

---

## Implementation Guide

### Step 1: Tilføj NuGet Package

```bash
cd src/MadeMan.IdleEmpire
dotnet add package SkiaSharp.Extended.UI.Maui
```

### Step 2: Registrer i MauiProgram.cs

```csharp
builder
    .UseMauiApp<App>()
    .UseSkiaSharp()  // Tilføj denne linje
    // ... rest of configuration
```

### Step 3: Download Lottie Files

**Anbefalede animationer fra LottieFiles.com:**

1. **Coin collect** (søg: "coin spin gold")
   - Gem som: `Resources/Raw/anim_coin.json`

2. **Success checkmark** (søg: "success check green")
   - Gem som: `Resources/Raw/anim_success.json`

3. **Confetti** (søg: "confetti celebration")
   - Gem som: `Resources/Raw/anim_confetti.json`

**Vigtigt**: Vælg animationer under 50KB, med simple designs.

### Step 4: Opret Animation Helper

Path: `src/MadeMan.IdleEmpire/Helpers/LottieHelper.cs`

**Design Intent:**
```csharp
public static class LottieHelper
{
    // Play one-shot animation at position
    // Auto-dispose after completion
    // Returns Task that completes when animation ends
    
    public static Task PlayCoinAsync(View anchor)
    public static Task PlaySuccessAsync(View anchor)  
    public static Task PlayConfettiAsync(Layout container)
}
```

### Step 5: Integrer i UI

**Eksempel: Coin animation ved køb**
```xml
<!-- I relevant XAML -->
<skia:SKLottieView x:Name="CoinAnimation"
                   Source="anim_coin.json"
                   RepeatCount="0"
                   IsVisible="False"
                   WidthRequest="80"
                   HeightRequest="80"/>
```

**I code-behind eller ViewModel:**
```csharp
// Ved successful køb:
CoinAnimation.IsVisible = true;
CoinAnimation.Progress = 0;
await Task.Delay(1000); // Animation duration
CoinAnimation.IsVisible = false;
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Package Compatibility Test
- [ ] App starter uden crash
- [ ] Ingen runtime exceptions relateret til SkiaSharp

### 3. Animation Quality Test
- [ ] Coin animation spiller ved køb
- [ ] Success animation spiller ved upgrade
- [ ] Confetti animation spiller ved prestige
- [ ] Animationer er smooth (ingen stutter)

### 4. Performance Test
- [ ] Trigger mange animationer hurtigt
- [ ] Observer memory usage
- [ ] Ingen memory leak (memory falder efter animation)

### 5. Manual Test in Emulator
- [ ] Alle animationer vises korrekt
- [ ] Timing føles rigtigt
- [ ] Farver passer til tema

---

## Acceptance Criteria

- [ ] SkiaSharp.Extended.UI.Maui package installeret
- [ ] Minimum 2 Lottie animationer implementeret
- [ ] Coin/money animation ved purchase
- [ ] Success animation ved upgrade
- [ ] Animationer er smooth og performer godt
- [ ] Memory cleanup efter animation completion
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Pre-made animationer**: Ingen custom animation work
- **Standard package**: Velvedligeholdt, dokumenteret
- **On-demand loading**: Ikke alle loaded på startup

### Alternativer overvejet

**Alternative 1: Custom frame-by-frame animation**
- Mere kontrol
- **Hvorfor fravalgt**: Enormt arbejde, kræver animator

**Alternative 2: MAUI built-in animations only**
- Ingen dependencies
- **Hvorfor fravalgt**: Begrænsede effekter, ikke så poleret

**Alternative 3: Video overlays**
- Rigere effekter
- **Hvorfor fravalgt**: Stor filstørrelse, kompleks playback

### Potentielle forbedringer (v2)
- Flere animation varianter
- Color customization af animationer
- Particle effects kombineret med Lottie
- Sound sync med animationer

### Kendte begrænsninger
- **Afhængig af package updates**: Kan have breaking changes
- **Farve-lock**: Nogle Lottie files kan ikke recolores

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Wrapper helpers, ikke kompleks arkitektur
- [ ] **Læsbarhed**: Klare metode navne (PlayCoinAsync, etc.)
- [ ] **Memory**: Dispose efter brug
- [ ] **Error handling**: Graceful failure hvis animation mangler
- [ ] **Performance**: On-demand loading
- [ ] **Testbarhed**: Kan disable animationer for testing

---

## Lottie File Recommendations

**Specifikt anbefalede fra LottieFiles.com:**

1. **Coin Spin**: https://lottiefiles.com/animations/coin-spin-[id]
   - Look for gold/yellow colored
   - Duration: ~1 second
   - Size: <30KB

2. **Check Mark**: https://lottiefiles.com/animations/check-[id]
   - Look for circular with check
   - Green or gold color
   - Duration: ~0.5 second

3. **Confetti**: https://lottiefiles.com/animations/confetti-[id]
   - Multi-colored particles
   - Duration: ~2 seconds
   - Size: <50KB (confetti tends to be larger)

**Tip**: Download JSON, ikke dotLottie format.

---

## License Note

De fleste LottieFiles animationer er gratis til kommerciel brug men kræver attribution. Check hver animations licens:
- **Lottie Simple License**: Gratis, kræver attribution i app credits
- **CC-BY**: Gratis med attribution
- **Public Domain**: Gratis uden krav

Tilføj attribution i Settings → About eller Credits sektion.

---

**Task Status**: READY
**Version**: 1.2
**Last Updated**: 2024-12-27
**Implemented By**: Pending
