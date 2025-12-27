# Task 078: Haptic Feedback System

## Metadata
- **Phase**: 7 - Visual Polish
- **Bundle**: P - Animation Foundation
- **Dependencies**: Ingen
- **Estimated Time**: 30 min - 1 hour
- **Status**: READY
- **Priority**: 🟡 Medium (Quick Win!)
- **Frequency Impact**: NO

---

## Formål

Tilføj taktil feedback (vibration) ved vigtige brugerinteraktioner for en mere immersiv oplevelse.

**Hvorfor dette er vigtigt:**
- Haptics giver fysisk bekræftelse på handlinger
- Standard i moderne mobile apps og spil
- Forbedrer "game feel" markant
- Især vigtigt for køb, prestige, og fejl

---

## Risici

### Potentielle Problemer
1. **Irritation ved overuse**:
   - Edge case: Vibration ved hver cash tick
   - Impact: Bruger slår det fra, dårlig oplevelse

2. **Battery drain**:
   - Konstant haptics bruger batteri
   - Impact: Negativ app review

3. **Device support**:
   - Ikke alle devices har haptic motor
   - Impact: Crash eller no-op

### Mitigering
- Kun haptics ved BETYDELIGE handlinger
- Altid wrap i try-catch
- Tilføj toggle i Settings (default: ON)
- Test at det fejler gracefully på devices uden haptics

---

## Analyse - Hvad Skal Implementeres

### 1. HapticService
**Description**: Centraliseret service til haptic feedback
**Location**: `src/MadeMan.IdleEmpire/Services/HapticService.cs`

**Key Requirements:**
- Singleton service
- Check `IsEnabled` setting før vibration
- Metoder til forskellige feedback types:
  - `Click()` - let tap (køb, navigation)
  - `Success()` - medium (upgrade købt)
  - `Error()` - kort shake pattern (can't afford)
  - `Heavy()` - kraftig (prestige)

### 2. MAUI Haptics API
**Description**: Brug built-in HapticFeedback class
**Location**: Native MAUI

**Available Types:**
```csharp
HapticFeedback.Default.Perform(HapticFeedbackType.Click);
HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
```

### 3. Integration Points
**Description**: Hvor haptics skal trigges
**Locations**: ViewModels og code-behind

| Action | Haptic Type | When |
|--------|-------------|------|
| Køb operation | Click | Successful purchase |
| Upgrade | Success (LongPress) | Successful upgrade |
| Can't afford | Error pattern | Insufficient funds |
| Prestige | Heavy (LongPress x2) | Prestige confirmed |
| Navigation tap | Click | Tab change |

### 4. Settings Toggle
**Description**: Bruger kan disable haptics
**Location**: Settings page og Preferences

---

## Dependencies Check

**Required Before Starting:**
- [x] MVP komplet
- [x] Settings page eksisterer (TASK-033)

**Assumptions:**
- MAUI HapticFeedback API er tilgængelig
- Android target har vibration permission

**Blockers:**
- Ingen

---

## Implementation Guide

### Step 1: Opret HapticService

Path: `src/MadeMan.IdleEmpire/Services/HapticService.cs`

**Design Intent:**
```csharp
public interface IHapticService
{
    bool IsEnabled { get; set; }
    void Click();      // Light tap
    void Success();    // Medium feedback
    void Error();      // Error pattern
    void Heavy();      // Strong feedback
}

public class HapticService : IHapticService
{
    // IsEnabled reads from Preferences
    // Each method checks IsEnabled, then calls HapticFeedback.Perform()
    // All wrapped in try-catch for devices without haptics
}
```

### Step 2: Registrer i DI

Path: `src/MadeMan.IdleEmpire/MauiProgram.cs`

```csharp
builder.Services.AddSingleton<IHapticService, HapticService>();
```

### Step 3: Integrer i ViewModels

**I MainViewModel eller relevant ViewModel:**
```csharp
private readonly IHapticService _haptics;

// I constructor: inject IHapticService

// Ved køb:
_haptics.Click();

// Ved upgrade success:
_haptics.Success();

// Ved can't afford:
_haptics.Error();
```

### Step 4: Settings Toggle

**I SettingsViewModel:**
- Bind til HapticService.IsEnabled
- Gem preference ved ændring

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Manual Test on Physical Device
> ⚠️ Haptics virker ikke i emulator - test på fysisk device

- [ ] Køb operation → kort vibration
- [ ] Upgrade → lidt længere vibration
- [ ] Can't afford → kort "error" vibration
- [ ] Prestige → kraftig vibration
- [ ] Settings toggle → disabler al vibration

### 3. Graceful Failure Test
- [ ] Test på emulator (ingen crash)
- [ ] Test med haptics disabled

---

## Acceptance Criteria

- [ ] HapticService oprettet med Click/Success/Error/Heavy
- [ ] Haptics trigger ved køb, upgrade, errors
- [ ] Settings toggle fungerer (on/off)
- [ ] Ingen crash på devices uden haptic motor
- [ ] Ingen crash i emulator
- [ ] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Native MAUI API**: Ingen third-party libraries
- **Simple service**: 4 metoder, minimal logic
- **Preference-based**: Standard settings pattern

### Alternativer overvejet

**Alternative 1: Custom vibration patterns**
- Definer præcise ms-patterns for hver type
- **Hvorfor fravalgt**: Platform-specifik API, komplekst

**Alternative 2: Haptics på alt**
- Vibrer ved hver UI interaction
- **Hvorfor fravalgt**: Irriterende, battery drain

### Potentielle forbedringer (v2)
- Intensity slider i settings (let/medium/kraftig)
- Custom patterns for specifikke events
- Sync med animations

### Kendte begrænsninger
- **Kun 2 intensiteter**: MAUI tilbyder kun Click og LongPress
- **Ingen emulator test**: Kræver fysisk device

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Wrapper omkring native API
- [ ] **Læsbarhed**: Klare metode navne (Click, Success, Error)
- [ ] **Error handling**: Try-catch på alle haptic calls
- [ ] **Performance**: Ingen performance impact
- [ ] **Testbarhed**: Kan mockes i unit tests

---

## Android Permission Note

Haptics kræver typisk IKKE eksplicit permission i AndroidManifest, da MAUI håndterer det. Men verificer at dette er tilfældet:

```xml
<!-- Normalt IKKE nødvendigt, men check om der er issues -->
<uses-permission android:name="android.permission.VIBRATE" />
```

---

**Task Status**: READY
**Version**: 1.2
**Last Updated**: 2024-12-27
**Implemented By**: Pending
