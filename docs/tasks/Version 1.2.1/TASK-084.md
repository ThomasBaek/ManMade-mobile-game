# TASK-084: Stats Page

## Metadata
- **Phase**: Version 1.2.1
- **Bundle**: U (Stats & Documentation)
- **Dependencies**: None
- **Estimated Time**: 2 hours
- **Status**: READY
- **Priority**: Medium

---

## Purpose

Add a Stats section in Settings showing lifetime statistics that persist across prestige.

**Why this matters:**
- Progression feel even after reset
- "Wow I've earned $10M total!"
- Engagement metric for player

---

## Stats to Track

### Core Stats (Must Have)
| Stat | Description | Resets on Prestige? |
|------|-------------|---------------------|
| Total Money Earned | Lifetime earnings | NO |
| Total Money Spent | Lifetime spending | NO |
| Prestige Count | Number of prestiges | NO (already exists) |
| Time Played | Total game time | NO |
| Highest Cash | Max cash at one time | NO |

### Extended Stats (Nice to Have)
| Stat | Description |
|------|-------------|
| Operations Upgraded | Total upgrade purchases |
| Milestones Reached | Unique milestones unlocked |
| Skills Chosen | Total skill selections |
| Offline Earnings | Total from offline time |
| Taps | Total button taps |

---

## Technical Design

### New Model: GameStats.cs

**Location:** `src/MadeMan.IdleEmpire.Core/Models/GameStats.cs`

```csharp
namespace MadeMan.IdleEmpire.Models;

public class GameStats
{
    public double TotalEarned { get; set; }
    public double TotalSpent { get; set; }
    public double HighestCash { get; set; }
    public TimeSpan TimePlayed { get; set; }
    public int TotalUpgrades { get; set; }
    public int TotalTaps { get; set; }
    public DateTime FirstPlayDate { get; set; }
}
```

### SaveManager Changes

**Separate save key for stats:**
```csharp
private const string StatsKey = "game_stats";

public GameStats LoadStats() { ... }
public void SaveStats(GameStats stats) { ... }
```

### GameEngine Integration

Track stats during gameplay:
```csharp
// In ProcessUpgrade/ProcessUnlock:
_stats.TotalSpent += cost;
_stats.TotalUpgrades++;

// In Tick:
_stats.TotalEarned += earnedThisTick;
_stats.HighestCash = Math.Max(_stats.HighestCash, State.Cash);
_stats.TimePlayed += deltaTime;
```

---

## UI Design

### Location in Settings
Add new section in SettingsPage.xaml:

```
┌─ SETTINGS ──────────────────────────────────┐
│                                             │
│  ═══════════ STATISTICS ═══════════         │
│                                             │
│  Total Earned        $1,234,567             │
│  Total Spent           $987,654             │
│  Highest Cash          $345,678             │
│  Time Played           12h 34m              │
│  Prestiges                   5              │
│  Operations Upgraded       127              │
│                                             │
│  ─────────────────────────────────          │
│  First Played: Dec 25, 2024                 │
│                                             │
└─────────────────────────────────────────────┘
```

### Component: StatsSection.xaml

Create reusable stats display component with key-value pairs.

---

## Implementation Steps

1. Create `GameStats.cs` model
2. Add `LoadStats()`/`SaveStats()` to SaveManager
3. Integrate tracking into GameEngine
4. Create `StatsSection.xaml` component
5. Add to SettingsPage
6. Write tests for persistence

---

## Test Requirements

### New Tests
```csharp
[Fact]
public void Stats_PersistAcrossPrestige()
{
    var engine = CreateEngine();
    engine.TryUpgrade("pickpocket"); // Spend money
    var earnedBefore = engine.Stats.TotalEarned;
    engine.DoPrestige();
    Assert.Equal(earnedBefore, engine.Stats.TotalEarned);
}

[Fact]
public void Stats_TrackSpending()
{
    var engine = CreateEngine();
    var initialSpent = engine.Stats.TotalSpent;
    engine.TryUpgrade("pickpocket");
    Assert.True(engine.Stats.TotalSpent > initialSpent);
}
```

---

## Acceptance Criteria

- [ ] GameStats model created
- [ ] Stats save/load independently from game state
- [ ] Stats do NOT reset on prestige
- [ ] Stats section visible in Settings
- [ ] All tracked stats display correctly
- [ ] All tests pass
- [ ] Build succeeds

---

## Code Evaluation

### Simplicity Check
- Separate concerns: stats vs game state
- Simple tracking in existing methods
- No complex calculations

### Performance Impact
- Minimal: few extra assignments per tick
- Stats save can be less frequent than game state

---

**Task Status**: READY
