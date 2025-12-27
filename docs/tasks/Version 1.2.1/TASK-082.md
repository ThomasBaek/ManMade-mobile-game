# TASK-082: Zero Income Start

## Metadata
- **Phase**: Version 1.2.1
- **Bundle**: T (Quick UX Fixes)
- **Dependencies**: None
- **Estimated Time**: 30 min
- **Status**: READY
- **Priority**: High

---

## Purpose

When starting fresh (new game or after prestige), player should have:
- $5 starting cash
- $0/s income (no passive earning)
- Must manually tap "BUY" on Pickpocket to start earning

**Why this matters:**
- Creates intentional game start moment
- Player actively engages rather than watching numbers
- Clear cause-and-effect: "I bought this, now I earn"

---

## Current Behavior

- Player starts with $5 cash
- Pickpocket is auto-unlocked at level 1
- Income immediately shows $0.5/s
- Cash increases passively without player action

## Target Behavior

- Player starts with $5 cash
- Pickpocket shows "BUY $0" or "START" button
- Income shows $0/s until first purchase
- After first tap, Pickpocket activates and income begins

---

## Technical Analysis

### Option A: Level 0 Start (Recommended)
Pickpocket starts at level 0 instead of level 1.
- Level 0 = unlocked but not active
- First upgrade goes 0→1 and costs $0
- Simple, minimal code change

### Option B: Explicit "Started" Flag
Add `HasStartedPlaying` flag to GameState.
- More complex
- Another field to save/load
- Not recommended

### Implementation (Option A)

**File:** `Services/GameEngine.cs`

Change initial Pickpocket level from 1 to 0:
```csharp
// In CreateFreshState() or Initialize()
// Pickpocket starts at level 0, not 1
operations["pickpocket"] = new OperationState { Level = 0 };
```

**File:** `Models/GameConfig.cs`

Ensure Pickpocket unlock cost is $0:
```csharp
// Already the case, but verify
new OperationConfig("pickpocket", ..., UnlockCost: 0)
```

**File:** `ViewModels/OperationViewModel.cs`

Handle level 0 display:
```csharp
public string ButtonText => IsUnlocked
    ? (Level == 0 ? "START" : $"${UpgradeCost:N0}")
    : $"${UnlockCost:N0}";
```

---

## UI Changes

### Operation Button States

| State | Button Text | Button Color |
|-------|-------------|--------------|
| Locked | "$75" (unlock cost) | Smoke |
| Level 0 (new!) | "START" or "BUY" | Success |
| Level 1+ | "$X" (upgrade cost) | Success/Smoke |

### TopBar Income Display

When total income is 0:
- Show "$0/s" or "+$0/s" (already works)
- Consider subtle styling to indicate "not started"

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Functional Tests
- [ ] Fresh start shows $5 cash, $0/s income
- [ ] Pickpocket button shows "START" or "BUY $0"
- [ ] Tapping Pickpocket changes to level 1
- [ ] Income updates to $0.5/s after purchase
- [ ] Other operations still locked correctly

### 3. Prestige Test
- [ ] After prestige, same behavior (start at $0/s)

---

## Test Impact

### Existing Tests to Update
Tests that assume Pickpocket starts at level 1:
- Any test checking initial income > 0
- Integration tests with immediate earning

### New Tests
```csharp
[Fact]
public void FreshState_HasZeroIncome()
{
    var engine = CreateEngine();
    engine.Initialize();
    Assert.Equal(0, engine.IncomePerSecond);
}

[Fact]
public void FirstPurchase_ActivatesIncome()
{
    var engine = CreateEngine();
    engine.Initialize();
    engine.TryUpgrade("pickpocket"); // 0 → 1
    Assert.True(engine.IncomePerSecond > 0);
}
```

---

## Acceptance Criteria

- [ ] Fresh game starts with $5 and $0/s income
- [ ] Pickpocket shows "START" button at level 0
- [ ] First tap activates income
- [ ] Prestige resets to same state
- [ ] All existing tests pass (or updated)
- [ ] Build succeeds with 0 errors

---

## Code Evaluation

### Simplicity Check
- Minimal change: just start level at 0 instead of 1
- Reuses existing upgrade logic
- No new state flags needed

### Performance Impact
- None

### Edge Cases
- What if player has exactly $0 and Pickpocket costs $0? ✓ Should work
- Rapid tapping on START? ✓ Goes to level 1, normal behavior

---

**Task Status**: READY
