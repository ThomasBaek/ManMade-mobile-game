# TASK-089: Remove $ from TopBar

## Metadata
- **Phase**: Version 1.2.1
- **Bundle**: T (Quick UX Fixes)
- **Dependencies**: None
- **Estimated Time**: 15 min
- **Status**: READY
- **Priority**: Low

---

## Purpose

Remove the "$" prefix from cash display in TopBar since there's already a money bag icon.

**Why this matters:**
- Redundant information (icon + symbol)
- Cleaner, more minimal design
- More space for large numbers

---

## Current Behavior

Display: `💰 $1,234.56`

## Target Behavior

Display: `💰 1,234.56` or `💰 1,234`

---

## Technical Analysis

### Location
**File:** `Utilities/NumberFormatter.cs`

```csharp
public static string FormatCurrency(double value)
{
    // Currently returns "$1.23K"
    // Should return "1.23K"
}
```

### Impact Analysis

`FormatCurrency` is used in multiple places:
- TopBar cash display ← CHANGE
- Operation upgrade costs ← KEEP $
- Prestige threshold display ← KEEP $
- Welcome back modal ← KEEP $

### Options

#### Option A: New Method (Recommended)
Create `FormatCurrencyNoSymbol()`:
```csharp
public static string FormatCurrencyNoSymbol(double value)
{
    if (value >= 1_000_000_000) return $"{value / 1_000_000_000:F2}B";
    if (value >= 1_000_000) return $"{value / 1_000_000:F2}M";
    if (value >= 1_000) return $"{value / 1_000:F2}K";
    return $"{value:F0}";
}
```

#### Option B: Parameter Flag
```csharp
public static string FormatCurrency(double value, bool includeSymbol = true)
```

#### Option C: Separate TopBar Formatter
TopBar has its own formatting.

**Recommended:** Option A - clear intent, no breaking changes

---

## Implementation Steps

1. Add `FormatCurrencyNoSymbol()` to `NumberFormatter.cs`
2. Update `MainViewModel.UpdateDisplay()` to use new method for `CashDisplay`
3. Verify other usages still show `$`

---

## Files Changed

| File | Change |
|------|--------|
| `Utilities/NumberFormatter.cs` | Add new method |
| `ViewModels/MainViewModel.cs` | Use new method for CashDisplay |

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Visual Tests
- [ ] TopBar shows "1,234" not "$1,234"
- [ ] Money icon still visible
- [ ] Operation costs still show "$"
- [ ] Prestige threshold still shows "$"

---

## Acceptance Criteria

- [ ] TopBar cash display has no "$" prefix
- [ ] Money icon provides context
- [ ] Other currency displays unchanged
- [ ] Build succeeds

---

## Code Evaluation

### Simplicity Check
- One new method
- One call site change
- No breaking changes

### Risk
- Very low - additive change only

---

**Task Status**: READY
