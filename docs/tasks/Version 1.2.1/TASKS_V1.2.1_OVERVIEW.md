# Version 1.2.1 - UX Improvements & Performance

## Overview
Quality-of-life improvements, stats tracking, documentation, and performance optimization.

---

## Task Summary

| Task | Name | Bundle | Estimate | Priority | Status |
|------|------|--------|----------|----------|--------|
| TASK-082 | Zero Income Start | T | 30 min | High | ✅ |
| TASK-083 | Operations Locked UI | T | 45 min | High | ✅ |
| TASK-084 | Stats Page | U | 2 hours | Medium | ✅ |
| TASK-085 | Prestige Progress Visibility | V | 45 min | Medium | ✅ |
| TASK-086 | Skills Guide Page | U | 1 hour | Medium | ✅ |
| TASK-087 | How to Play Page | U | 2-3 hours | Low | ✅ |
| TASK-088 | Smooth Countdown Animation | V | 1-2 hours | Medium | ✅ |
| TASK-089 | Remove $ from TopBar | T | 15 min | Low | ✅ |
| TASK-090 | Tab Switching Performance | W | 2-3 hours | High | |

**Total Estimate:** 10-14 hours

---

## Bundle Structure

### Bundle T: Quick UX Fixes (TASK-082, TASK-083, TASK-089) ✅ COMPLETED
**Priority:** High - Immediate polish
**Estimate:** 1.5 hours

| Task | Description | Impact |
|------|-------------|--------|
| TASK-082 | Start with $5 and 0 income, require manual first purchase | Onboarding |
| TASK-083 | Better locked operation messaging, hide progress until purchased | Clarity |
| TASK-089 | Remove redundant $ from cash display (icon sufficient) | Clean UI |

**Files affected:**
- `GameEngine.cs` - Initial state logic
- `OperationViewModel.cs` - Locked state display
- `TopBar.xaml` - Remove $ prefix

---

### Bundle U: Stats & Documentation (TASK-084, TASK-086, TASK-087) ✅ COMPLETED
**Priority:** Medium - New features
**Estimate:** 5-6 hours
**Completed:** 2024-12-27

| Task | Description | Impact |
|------|-------------|--------|
| TASK-084 | Stats page in settings (money earned/spent, prestige count, etc.) | Progression feel |
| TASK-086 | Skill descriptions with info icons, always show 0% | Clarity |
| TASK-087 | Help page with game mechanics documentation | Onboarding |

**New files:**
- `Models/GameStats.cs` - Persistent stats model
- `Views/Components/StatsSection.xaml` - Stats UI
- `Views/HelpPage.xaml` - Documentation page

**Test impact:**
- New tests for stats persistence
- Stats should NOT reset on prestige

---

### Bundle V: UI Refinements (TASK-085, TASK-088) ✅ COMPLETED
**Priority:** Medium - Visual polish
**Estimate:** 2-3 hours
**Completed:** 2024-12-27

| Task | Description | Impact |
|------|-------------|--------|
| TASK-085 | Prestige progress only on Empire tab, title in top bar | Less clutter |
| TASK-088 | Smooth countdown animation (interpolated, not stepped) | Premium feel |

**Technical considerations:**
- TASK-088 may need custom timer or animation approach
- Current countdown updates once per tick (2x/sec) - needs interpolation

---

### Bundle W: Performance (TASK-090)
**Priority:** High - User experience
**Estimate:** 2-3 hours

| Task | Description | Impact |
|------|-------------|--------|
| TASK-090 | Investigate & fix tab switching lag | Responsiveness |

**Investigation areas:**
- Progress bar rendering cost
- Binding evaluation on tab switch
- OperationViewModel.Refresh() overhead
- Potential virtualization needs

---

## Dependency Graph

```
Bundle T (Quick Fixes)          Bundle W (Performance)
┌─────────────────────┐         ┌─────────────────────┐
│  TASK-082 (Start)   │         │  TASK-090 (Tabs)    │
│  TASK-083 (Locked)  │         └─────────────────────┘
│  TASK-089 (TopBar)  │                   │
└─────────────────────┘                   │
          │                               │
          ▼                               ▼
Bundle U (Stats & Docs)         Bundle V (UI Refinements)
┌─────────────────────┐         ┌─────────────────────┐
│  TASK-084 (Stats)   │         │  TASK-085 (Prestige)│
│  TASK-086 (Skills)  │         │  TASK-088 (Countdown│
│  TASK-087 (Help)    │         └─────────────────────┘
└─────────────────────┘
```

**Recommended order:** T → W → V → U

---

## Implementation Considerations

### Stats Persistence (TASK-084)
Stats must survive prestige reset. Options:
1. **Separate SaveManager key** - `GameStats` saved independently
2. **Flag in GameState** - `PrestigeResetsStats = false`

**Recommended:** Separate stats object with own save key.

### Countdown Animation (TASK-088)
Current: Updates every 500ms (2 ticks/sec)
Target: Smooth 60fps countdown

Options:
1. **Lerp-based display** - Interpolate between ticks
2. **Separate UI timer** - 60fps timer just for display
3. **Animation API** - Use MAUI animation for countdown bar

**Recommended:** Lerp-based display with cached target values.

### Tab Performance (TASK-090)
Investigation checklist:
- [ ] Profile with dotnet-trace or VS profiler
- [ ] Measure OperationViewModel.Refresh() cost
- [ ] Check binding complexity in MainPage.xaml
- [ ] Test with BindableLayout vs ListView
- [ ] Consider UI virtualization

---

## Test Requirements

### New Tests Needed
| Task | Tests |
|------|-------|
| TASK-082 | `InitialState_HasZeroIncome`, `FirstPurchase_ActivatesIncome` |
| TASK-084 | `Stats_PersistAcrossPrestige`, `Stats_TrackEarnings`, `Stats_TrackSpending` |
| TASK-086 | `Skill_AlwaysShowsPercentage` |

### Existing Tests to Update
| Task | Affected Tests |
|------|----------------|
| TASK-082 | Tests assuming non-zero initial income |
| TASK-084 | Integration tests with new stats tracking |

---

## Settings Additions

| Setting | Location | Default |
|---------|----------|---------|
| (none new) | - | - |

Note: Stats page is view-only, no toggle needed.

---

## Files Changed Summary

| Bundle | New Files | Modified Files |
|--------|-----------|----------------|
| T | 0 | 4 |
| U | 4-5 | 3-4 |
| V | 0 | 3-4 |
| W | 0 | 2-3 |

---

## Completion Checklist

- [x] **Bundle T Complete** - Quick UX fixes
- [x] **Bundle U Complete** - Stats & documentation
- [x] **Bundle V Complete** - UI refinements
- [ ] **Bundle W Complete** - Performance investigation
- [x] **All Tests Pass** - 149+ tests passing
- [x] **Build Clean** - 0 errors, minimal warnings
- [x] **Prestige threshold restored** - $25,000 (from debug $250)

---

**Document Version:** 1.0
**Created:** 2024-12-27
**Task Range:** TASK-082 to TASK-090
**Total Tasks:** 9
**Bundles:** 4 (T, U, V, W)
