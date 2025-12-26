# Task 003: Implement SaveManager

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: TASK-002
- **Estimated Time**: 30 min
- **Status**: COMPLETED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Purpose

Implement SaveManager service to save and load GameState via Preferences API.

**Why this is important:**
- Players must be able to close the app and return
- Offline earnings require knowledge of last session
- Data persistence is critical for idle games

---

## Risks

### Potential Problems
1. **JSON Serialization Errors**:
   - Edge case: Corrupt data in Preferences
   - Impact: Game crashes on load

2. **Data Loss**:
   - Edge case: App crashes during save
   - Impact: Player loses progress

### Mitigation
- Try-catch around all I/O
- Return null on error (fallback to new game)
- Use System.Text.Json for simple serialization

---

## Analysis - What Needs to be Implemented

### SaveManager.cs
**Location**: `Services/SaveManager.cs`

**Functions:**
- `Save(GameState state)` - Save to Preferences
- `Load()` - Load from Preferences, return null on error
- `Delete()` - Delete saved data (for testing/reset)

---

## Dependencies Check

**Required Before Start**:
- [x] TASK-002 completed (GameState model exists)

**Assumptions**:
- Preferences API is available (MAUI standard)
- System.Text.Json is available

**Blockers**: TASK-002 must be complete

---

## Implementation Guide

### Step 1: Create SaveManager.cs

**Path**: `src/MadeMan.IdleEmpire/Services/SaveManager.cs`

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
Expected: 0 errors

### 2. Conceptual Test
- Save called with valid GameState -> No exception
- Load on empty data -> Returns null
- Load on valid data -> Returns GameState

---

## Acceptance Criteria

- [x] SaveManager.cs created
- [x] Save method serializes GameState to JSON
- [x] Load method deserializes JSON to GameState
- [x] Error handling on all operations
- [x] Build succeeds with 0 errors

---

## Code Evaluation

### Simplification Check
- **Preferences API**: Simpler than SQLite for MVP
- **No async**: Preferences is synchronous (OK for small data)
- **Single save key**: No versioning complexity

### Alternatives Considered

**Alternative: SQLite database**
```csharp
// SQLite would require more setup
using SQLite;
public class SaveService { ... }
```
**Why rejected**: Over-engineering for MVP, Preferences is enough

### Known Limitations
- Only one save slot
- No backup
- Acceptable for MVP

---

## Code Quality Checklist

- [x] **KISS**: Simplest persistence solution
- [x] **Error handling**: Try-catch on all operations
- [x] **Logging**: Debug output for troubleshooting

---

## Design Files Reference

- **Spec Reference**: docs/MVP_Specification_MAUI.md (lines 397-429)
- **Related Tasks**: TASK-002, TASK-004

---

## Notes

- Versioned save key (gamestate_v1) for future migration
- Preferences API saves to app-specific storage

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Completed**: 2024-12-25
