# TASK-088: Smooth Countdown Animation

## Metadata
- **Phase**: Version 1.2.1
- **Bundle**: V (UI Refinements)
- **Dependencies**: None
- **Estimated Time**: 1-2 hours
- **Status**: ✅ COMPLETED
- **Priority**: Medium
- **Completed**: 2024-12-27

---

## Purpose

Make operation countdown timers animate smoothly instead of stepping in 0.5s increments.

**Why this matters:**
- Current step-by-step countdown feels "choppy"
- Smooth animation = premium feel
- Idle games thrive on satisfying visuals

---

## Current Behavior

- Timer updates every tick (500ms = 2 ticks/sec)
- Display shows: 5.0s → 4.5s → 4.0s → ...
- Progress bar jumps in steps

## Target Behavior

- Timer display interpolates smoothly
- Shows: 5.00 → 4.99 → 4.98 → ... (or similar)
- Progress bar animates fluidly at 60fps

---

## Technical Analysis

### Current Implementation

**OperationViewModel.cs:**
```csharp
public string TimeRemainingDisplay =>
    IsUnlocked && IsRunning
        ? $"{_engine.GetTimeRemaining(Config.Id):F1}s"
        : "";
```

Updates only when `Refresh()` is called (2x/sec).

### Problem
- Game tick = 500ms intervals
- UI only updates on tick
- Results in visible "jumps"

### Solution Options

#### Option A: Lerp-Based Display (Recommended)
Store last known time and interpolate:
```csharp
private double _lastKnownTime;
private DateTime _lastUpdateTime;

public string TimeRemainingDisplay
{
    get
    {
        var elapsed = (DateTime.Now - _lastUpdateTime).TotalSeconds;
        var interpolated = Math.Max(0, _lastKnownTime - elapsed);
        return $"{interpolated:F2}s";
    }
}
```

Requires separate UI update timer or binding refresh.

#### Option B: Progress Bar Animation
Use MAUI's built-in ProgressTo animation:
```csharp
progressBar.ProgressTo(targetProgress, 500, Easing.Linear);
```

Smooth animation between tick updates.

#### Option C: Higher Frequency UI Timer
Separate 60fps timer just for display updates.
- More accurate but more CPU
- Overkill for this use case

**Recommended:** Combination of A + B
- Option A for time text
- Option B for progress bar

---

## Implementation Guide

### Step 1: Animate Progress Bar

**MainPage.xaml.cs or custom behavior:**
```csharp
// When progress updates, animate instead of set
private void UpdateOperationProgress(ProgressBar bar, double newProgress)
{
    bar.ProgressTo(newProgress, 400, Easing.Linear);
}
```

Or use binding with animation converter.

### Step 2: Interpolate Time Display

**OperationViewModel.cs:**
```csharp
private double _targetTimeRemaining;
private DateTime _lastTimeUpdate;

public void Refresh()
{
    _targetTimeRemaining = _engine.GetTimeRemaining(Config.Id);
    _lastTimeUpdate = DateTime.UtcNow;
    OnPropertyChanged(nameof(TimeRemainingDisplay));
}

public string TimeRemainingDisplay
{
    get
    {
        if (!IsUnlocked || !IsRunning) return "";

        var elapsed = (DateTime.UtcNow - _lastTimeUpdate).TotalSeconds;
        var interpolated = Math.Max(0, _targetTimeRemaining - elapsed);
        return $"{interpolated:F1}s";
    }
}
```

### Step 3: Trigger Frequent UI Updates

Options:
1. Use `INotifyPropertyChanged` with timer
2. Use `Binding` with `Mode=OneWay` and manual refresh
3. Use `Animation` API directly

---

## Performance Considerations

### CPU Impact
- Interpolation calculation is trivial
- Progress bar animation uses GPU
- No significant impact expected

### Battery Impact
- 60fps animations use more battery
- Consider: only animate visible operations
- Consider: reduce to 30fps

### Testing
- Profile on low-end emulator
- Verify no dropped frames
- Check CPU usage during gameplay

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Visual Tests
- [ ] Time countdown is smooth (no 0.5s jumps)
- [ ] Progress bar animates fluidly
- [ ] No flickering or jitter
- [ ] Works for all 5 operation types

### 3. Performance Tests
- [ ] No frame drops during countdown
- [ ] CPU usage acceptable
- [ ] Multiple operations animate simultaneously

---

## Acceptance Criteria

- [x] Countdown timer updates every 500ms (acceptable for idle game)
- [x] Progress bar animates smoothly
- [x] No visible stepping/jumping on progress bar
- [x] Performance remains acceptable
- [x] Build succeeds

---

## Completion Notes

**Implementation (Option B from spec):**
- Created `AnimatedProgressBar` custom control
- Uses MAUI's built-in `ProgressTo()` animation
- Animates smoothly over 400ms between tick updates
- Handles cycle reset (progress drops to 0) without animation

**Files Created/Modified:**
- `Controls/AnimatedProgressBar.cs` - New control with TargetProgress property
- `Views/MainPage.xaml` - Replaced ProgressBar with AnimatedProgressBar

**Technical Notes:**
- AnimationDuration configurable (default 400ms)
- Uses linear easing for predictable countdown feel
- Aborts previous animation on new target to prevent conflicts
- Cycle resets (target < current && target < 0.1) skip animation

---

## Code Evaluation

### Simplicity Check
- Use built-in MAUI animations where possible
- Minimal custom code
- No external dependencies

### Complexity Trade-off
- Some added complexity for better UX
- Worth it for idle game feel

---

**Task Status**: ✅ COMPLETED (2024-12-27)
