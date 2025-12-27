# TASK-083: Operations Locked UI Improvements

## Metadata
- **Phase**: Version 1.2.1
- **Bundle**: T (Quick UX Fixes)
- **Dependencies**: None
- **Estimated Time**: 45 min
- **Status**: READY
- **Priority**: High

---

## Purpose

Improve the locked operation display:
1. Replace "LOCKED" text with more engaging messaging
2. Hide progress bar until operation is purchased
3. Clearer visual distinction between locked/unlocked states

**Why this matters:**
- "LOCKED" feels negative/punishing
- Progress bar for locked items is confusing
- Better UX creates anticipation, not frustration

---

## Current Behavior

- Shows "LOCKED" text somewhere
- Progress bar visible even for locked operations
- Unlock progress bar shows progress toward unlock cost

## Target Behavior

- Show operation name with teaser (e.g., "Car Theft - $75 to unlock")
- NO progress bar until operation is active (level ≥ 1)
- Unlock cost clearly visible on button
- Maybe show potential income: "Earns $2/s"

---

## Technical Analysis

### Current UI Structure (MainPage.xaml)
```xml
<!-- Progress Bar (unlocked: yield progress) -->
<ProgressBar IsVisible="{Binding IsUnlocked}" ... />

<!-- Progress Bar (locked: unlock progress) -->
<Grid IsVisible="{Binding IsUnlocked, Converter={StaticResource InverseBoolConverter}}">
    <!-- Shows progress toward unlock -->
</Grid>
```

### Proposed Changes

1. **Remove locked progress bar entirely**
2. **Update locked state info display**
3. **Add potential income teaser**

---

## Implementation Guide

### Step 1: Update MainPage.xaml

**Remove or hide the locked progress bar:**
```xml
<!-- Progress Bar (unlocked: yield progress) - KEEP -->
<ProgressBar Progress="{Binding Progress}"
             ProgressColor="{StaticResource Gold}"
             IsVisible="{Binding IsUnlocked}"/>

<!-- REMOVE the locked progress bar Grid entirely -->
<!-- Or change to show potential income instead -->
```

**Add potential income teaser for locked operations:**
```xml
<!-- Teaser for locked operations -->
<Label Text="{Binding PotentialIncomeDisplay}"
       IsVisible="{Binding IsUnlocked, Converter={StaticResource InverseBoolConverter}}"
       FontSize="11"
       TextColor="{StaticResource TextSecondary}"/>
```

### Step 2: Update OperationViewModel.cs

**Add potential income display:**
```csharp
public string PotentialIncomeDisplay =>
    IsUnlocked ? "" : $"Unlocks: {NumberFormatter.FormatCurrency(Config.BaseIncome)}/s";
```

**Or more enticing:**
```csharp
public string PotentialIncomeDisplay =>
    IsUnlocked ? "" : $"Earns {NumberFormatter.FormatCurrency(Config.BaseIncome)}/s";
```

### Step 3: Update Level Display for Locked

Current shows "Lv.0" or similar for locked. Consider:
```csharp
public string LevelDisplay =>
    Level == 0 ? "" : $"Lv.{Level}";
    // Or show nothing for locked operations
```

---

## Visual Design

### Locked Operation Card

```
┌─────────────────────────────────────────────┐
│ [🚗]  Car Theft                    [$75]   │
│       Earns $2/s                           │
└─────────────────────────────────────────────┘
```

### Unlocked Operation Card (for comparison)

```
┌─────────────────────────────────────────────┐
│ [🚗]  Car Theft  Lv.3             [$150]   │
│       ████████████░░░░░░░  +$6/s   3.2s    │
└─────────────────────────────────────────────┘
```

### Key Differences
- Locked: No level badge, no progress bar
- Locked: Shows potential income as teaser
- Locked: Button shows unlock cost in gray

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Visual Tests
- [ ] Locked operations show no progress bar
- [ ] Locked operations show potential income
- [ ] Locked button shows unlock cost
- [ ] Unlocked operations unchanged
- [ ] Level 0 Pickpocket (TASK-082) displays correctly

### 3. Edge Cases
- [ ] Just-unlocked operation transitions smoothly
- [ ] All 5 operation types display correctly

---

## Acceptance Criteria

- [ ] Locked operations don't show progress bar
- [ ] Locked operations show potential income teaser
- [ ] No "LOCKED" text displayed
- [ ] Visual distinction between locked/unlocked is clear
- [ ] Build succeeds with 0 errors

---

## Code Evaluation

### Simplicity Check
- Remove code (locked progress bar) rather than add
- Simple property for potential income display
- Minimal XAML changes

### Performance Impact
- Slightly better (fewer bindings evaluated)

---

**Task Status**: READY
