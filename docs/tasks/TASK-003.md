# Task 003: Implement SaveManager

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: TASK-002
- **Estimated Time**: 30 min
- **Status**: COMPLETED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Formål

Implementer SaveManager service til at gemme og hente GameState via Preferences API.

**Hvorfor dette er vigtigt:**
- Spillere skal kunne lukke appen og vende tilbage
- Offline earnings kræver viden om sidste session
- Data persistence er kritisk for idle games

---

## Risici

### Potentielle Problemer
1. **JSON Serialization Fejl**:
   - Edge case: Korrupt data i Preferences
   - Impact: Spillet crasher ved load

2. **Data Loss**:
   - Edge case: App crasher under save
   - Impact: Spilleren mister progress

### Mitigering
- Try-catch around al I/O
- Return null ved fejl (fallback til nyt spil)
- Brug System.Text.Json for simpel serialisering

---

## Analyse - Hvad Skal Implementeres

### SaveManager.cs
**Placering**: `Services/SaveManager.cs`

**Funktioner:**
- `Save(GameState state)` - Gem til Preferences
- `Load()` - Hent fra Preferences, return null hvis fejl
- `Delete()` - Slet gemt data (til testing/reset)

---

## Dependencies Check

**Krævet Før Start**:
- [x] TASK-002 completed (GameState model eksisterer)

**Antagelser**:
- Preferences API er tilgængelig (MAUI standard)
- System.Text.Json er tilgængelig

**Blockers**: TASK-002 skal være færdig

---

## Implementation Guide

### Step 1: Opret SaveManager.cs

**Sti**: `src/MadeMan.IdleEmpire/Services/SaveManager.cs`

```csharp
using System.Text.Json;
using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public class SaveManager
{
    private const string SaveKey = "gamestate_v1";

    public void Save(GameState state)
    {
        try
        {
            state.LastPlayedUtc = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(state);
            Preferences.Default.Set(SaveKey, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Save failed: {ex.Message}");
        }
    }

    public GameState? Load()
    {
        try
        {
            var json = Preferences.Default.Get(SaveKey, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            return JsonSerializer.Deserialize<GameState>(json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Load failed: {ex.Message}");
            return null;
        }
    }

    public void Delete()
    {
        try
        {
            Preferences.Default.Remove(SaveKey);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Delete failed: {ex.Message}");
        }
    }
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Forventet: 0 errors

### 2. Conceptual Test
- Save kaldes med valid GameState -> Ingen exception
- Load på tom data -> Returns null
- Load på valid data -> Returns GameState

---

## Acceptance Criteria

- [x] SaveManager.cs oprettet
- [x] Save metode serialiserer GameState til JSON
- [x] Load metode deserialiserer JSON til GameState
- [x] Error handling på alle operationer
- [x] Build succeeds med 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Preferences API**: Simplere end SQLite for MVP
- **Ingen async**: Preferences er synkront (OK for små data)
- **Enkelt save key**: Ingen versioning kompleksitet

### Alternativer overvejet

**Alternativ: SQLite database**
```csharp
// SQLite ville kræve mere setup
using SQLite;
public class SaveService { ... }
```
**Hvorfor fravalgt**: Over-engineering for MVP, Preferences er nok

### Kendte begrænsninger
- Kun ét save slot
- Ingen backup
- Acceptabelt for MVP

---

## Kode Kvalitet Checklist

- [x] **KISS**: Simpleste persistence løsning
- [x] **Error handling**: Try-catch på alle operationer
- [x] **Logging**: Debug output for fejlfinding

---

## Design Files Reference

- **Spec Reference**: docs/MVP_Specification_MAUI.md (linje 397-429)
- **Related Tasks**: TASK-002, TASK-004

---

## Notes

- Versioned save key (gamestate_v1) for fremtidig migration
- Preferences API gemmer i app-specific storage

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Completed**: 2024-12-25
