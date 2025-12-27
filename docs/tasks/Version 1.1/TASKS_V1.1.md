# Made Man: Idle Empire - Version 1.1 Tasks

## Overview
**Focus:** Economy Rebalancing & Skill System Testing

This version slows down game progression and adds comprehensive tests for the skill system.

---

## Bundle Summary

| Bundle | Name | Tasks | Focus |
|--------|------|-------|-------|
| **J** | Economy Core | TASK-046 to TASK-050 | Operations, prestige, offline |
| **K** | Skills & Milestones | TASK-051 to TASK-055 | Skill effects, milestone thresholds |
| **L** | Test Infrastructure | TASK-056 to TASK-059 | xUnit setup, base classes |
| **M** | Income & Operation Tests | TASK-060 to TASK-064 | 10 skill test classes |
| **N** | Offline & Prestige Tests | TASK-065 to TASK-069 | 8 skill test classes |
| **O** | Integration Tests | TASK-070 to TASK-074 | Stacking, reset, edge cases |

---

## Bundle J: Economy Rebalancing - Core Operations

| Task | Name | Status | Dependencies |
|------|------|--------|--------------|
| TASK-046 | Update Operation Income Values | READY | None |
| TASK-047 | Update Operation Unlock Costs | READY | TASK-046 |
| TASK-048 | Update Upgrade Cost Multiplier | READY | TASK-047 |
| TASK-049 | Update Prestige Threshold/Bonus | READY | TASK-048 |
| TASK-050 | Update Offline Efficiency | READY | TASK-049 |

**Files:** `GameConfig.cs`, `GameEngine.cs`

---

## Bundle K: Economy Rebalancing - Skills & Milestones

| Task | Name | Status | Dependencies |
|------|------|--------|--------------|
| TASK-051 | Update Income Skill Effects | READY | Bundle J |
| TASK-052 | Update Operation Skill Effects | READY | TASK-051 |
| TASK-053 | Update Offline Skill Effects | READY | TASK-052 |
| TASK-054 | Update Prestige Skill Effects | READY | TASK-053 |
| TASK-055 | Update Milestone Thresholds | READY | TASK-054 |

**Files:** `SkillConfig.cs`, `MilestoneService.cs`

---

## Bundle L: Test Infrastructure

| Task | Name | Status | Dependencies |
|------|------|--------|--------------|
| TASK-056 | Create xUnit Test Project | READY | None |
| TASK-057 | Add Project Reference | READY | TASK-056 |
| TASK-058 | Add Moq Package | READY | TASK-057 |
| TASK-059 | Create SkillTestBase Class | READY | TASK-058 |

**Files:** New `tests/MadeMan.IdleEmpire.Tests/` project

---

## Bundle M: Skill Tests - Income & Operations

| Task | Name | Status | Dependencies |
|------|------|--------|--------------|
| TASK-060 | Test CashFlow Skill | READY | TASK-059, Bundle K |
| TASK-061 | Test StreetSmarts & BusinessAcumen | READY | TASK-060 |
| TASK-062 | Test LuckyBreak Skill | READY | TASK-061 |
| TASK-063 | Test TheSkim & CompoundInterest | READY | TASK-062 |
| TASK-064 | Test Operation-Specific Skills | READY | TASK-063 |

**Files:** `IncomeSkillsTests.cs`, `OperationSkillsTests.cs`

---

## Bundle N: Skill Tests - Offline & Prestige

| Task | Name | Status | Dependencies |
|------|------|--------|--------------|
| TASK-065 | Test NightOwl Skill | READY | TASK-059, Bundle K |
| TASK-066 | Test ExtendedShift Skill | READY | TASK-065 |
| TASK-067 | Test PassiveIncome & GodfathersCut | READY | TASK-066 |
| TASK-068 | Test OldConnections Skill | READY | TASK-067 |
| TASK-069 | Test Reputation, FastLearner, EarlyBird | READY | TASK-068 |

**Files:** `OfflineSkillsTests.cs`, `PrestigeSkillsTests.cs`

---

## Bundle O: Skill Tests - Integration & Edge Cases

| Task | Name | Status | Dependencies |
|------|------|--------|--------------|
| TASK-070 | Test Income Skills Stacking | READY | Bundle M, N |
| TASK-071 | Test Cost Reduction Stacking | READY | TASK-070 |
| TASK-072 | Test Max Skill Limit | READY | TASK-071 |
| TASK-073 | Test Prestige Reset | READY | TASK-072 |
| TASK-074 | Test Edge Cases | READY | TASK-073 |

**Files:** `SkillStackingTests.cs`, `PrestigeResetTests.cs`, `SkillEdgeCaseTests.cs`

---

## Recommended Execution Order

```
Phase 1: Rebalancing (can run in parallel)
├── Bundle J (TASK-046 → TASK-050)
└── Bundle L (TASK-056 → TASK-059)

Phase 2: Skill Updates
└── Bundle K (TASK-051 → TASK-055)

Phase 3: Tests (sequential)
├── Bundle M (TASK-060 → TASK-064)
├── Bundle N (TASK-065 → TASK-069)
└── Bundle O (TASK-070 → TASK-074)
```

---

## Verification Checklist

### After Bundle J+K (Rebalancing)
- [ ] Pickpocket = $0.5/s
- [ ] Car Theft unlock = $75
- [ ] Prestige threshold = $25,000
- [ ] First milestone at $2,500
- [ ] First prestige takes ~30-45 min

### After Bundle L+M+N+O (Testing)
- [ ] All 18 skills have tests
- [ ] `dotnet test` shows all green
- [ ] No test failures
- [ ] Edge cases covered

---

## Total Tasks: 29
**Estimated Time:** 8-10 hours
