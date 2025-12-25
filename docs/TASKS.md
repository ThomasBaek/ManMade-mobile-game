# Made Man: Idle Empire - Tasks Overview

## Current Task
**Bundle C**: TASK-023 + TASK-024 (Integrate Skills into GameEngine + DI) - READY

---

## Task Status Legend
- READY - Ready for implementation
- BLOCKED - Waiting for dependencies
- IN_PROGRESS - Work in progress
- COMPLETED - Done and approved

---

## Phase 1: Foundation

| Task | Name | Status | Dependencies | Estimate |
|------|------|--------|--------------|----------|
| TASK-001 | Install CommunityToolkit.Mvvm | COMPLETED | None | 15 min |
| TASK-002 | Implement Models | COMPLETED | TASK-001 | 45 min |
| TASK-003 | Implement SaveManager | COMPLETED | TASK-002 | 30 min |
| TASK-004 | Implement GameEngine | COMPLETED | TASK-003 | 1 hour |
| TASK-005 | Setup DI & Lifecycle | COMPLETED | TASK-004 | 30 min |

## Phase 2: UI

| Task | Name | Status | Dependencies | Estimate |
|------|------|--------|--------------|----------|
| TASK-006 | Implement Theme | COMPLETED | TASK-005 | 30 min |
| TASK-007 | OperationViewModel | COMPLETED | TASK-006 | 45 min |
| TASK-008 | MainViewModel | COMPLETED | TASK-007 | 1 hour |
| TASK-009 | MainPage UI | COMPLETED | TASK-008 | 1.5 hours |
| TASK-010 | Add Icons | COMPLETED | TASK-009 | 30 min |

## Phase 3: Skill System (NEW)

| Task | Name | Status | Dependencies | Estimate |
|------|------|--------|--------------|----------|
| TASK-018 | Skill Models | COMPLETED | TASK-002 | 30 min |
| TASK-019 | Skill Config (18 Skills) | COMPLETED | TASK-018 | 45 min |
| TASK-020 | Skill Calculator | COMPLETED | TASK-019 | 30 min |
| TASK-021 | Skill Service | COMPLETED | TASK-020 | 45 min |
| TASK-022 | Milestone Service | COMPLETED | TASK-021 | 30 min |
| TASK-023 | Integrate Skills into GameEngine | READY | TASK-021, TASK-022 | 45 min |
| TASK-024 | Update DI for Skills | BLOCKED | TASK-023 | 15 min |
| TASK-025 | SkillViewModel | BLOCKED | TASK-024 | 30 min |
| TASK-026 | Skill Selection Modal | BLOCKED | TASK-025 | 45 min |
| TASK-027 | Skill Display Component | BLOCKED | TASK-025 | 30 min |
| TASK-028 | Verify Skill Effects | BLOCKED | TASK-026, TASK-027 | 30 min |

**Skill System Summary:**
- 18 skills across 4 categories (Income, Operations, Offline, Prestige)
- Max 5 skills, max level 5 per skill
- 10 milestones based on TotalEarned thresholds
- Skills reset on prestige

## Phase 4: Gameplay (Updated dependencies)

| Task | Name | Status | Dependencies | Estimate |
|------|------|--------|--------------|----------|
| TASK-011 | Verify Game Loop | COMPLETED | TASK-010 | 1 hour |
| TASK-012 | Verify Prestige | COMPLETED | TASK-011 | 30 min |

## Phase 5: Persistence

| Task | Name | Status | Dependencies | Estimate |
|------|------|--------|--------------|----------|
| TASK-013 | Test Save/Load | COMPLETED | TASK-012 | 30 min |
| TASK-014 | Test Offline Earnings | COMPLETED | TASK-013 | 30 min |

## Phase 6: Polish

| Task | Name | Status | Dependencies | Estimate |
|------|------|--------|--------------|----------|
| TASK-015 | UI Polish | BLOCKED | TASK-014 | 1 hour |
| TASK-016 | Bug Fixes | BLOCKED | TASK-015 | Variable |
| TASK-017 | Final Testing | BLOCKED | TASK-016 | 1 hour |

---

## Completed Tasks

| Task | Name | Completed | Commit |
|------|------|-----------|--------|
| TASK-001 | Install CommunityToolkit.Mvvm | 2024-12-25 | ae0df75 |
| TASK-002 | Implement Models | 2024-12-25 | ddc7d58 |
| TASK-003 | Implement SaveManager | 2024-12-25 | fe72620 |
| TASK-004 | Implement GameEngine | 2024-12-25 | 7f52788 |
| TASK-005 | Setup DI & Lifecycle | 2024-12-25 | a5bd9e0 |
| TASK-006 | Implement Theme | 2024-12-25 | f6cb1c7 |
| TASK-007 | OperationViewModel | 2024-12-25 | 01bd4f0 |
| TASK-008 | MainViewModel | 2024-12-25 | dd8cf82 |
| TASK-009 | MainPage UI | 2024-12-25 | 8dd4516 |
| TASK-010 | Add Icons | 2024-12-25 | 8dd4516 |
| TASK-011 | Verify Game Loop | 2024-12-25 | a6782d5 |
| TASK-012 | Verify Prestige | 2024-12-25 | a6782d5 |
| TASK-013 | Test Save/Load | 2024-12-25 | a6782d5 |
| TASK-014 | Test Offline Earnings | 2024-12-25 | a6782d5 |
| TASK-018 | Skill Models | 2024-12-25 | 79e430e |
| TASK-019 | Skill Config (18 Skills) | 2024-12-25 | 79e430e |
| TASK-020 | Skill Calculator | 2024-12-25 | 79e430e |
| TASK-021 | Skill Service | 2024-12-25 | 79e430e |
| TASK-022 | Milestone Service | 2024-12-25 | 79e430e |

---

## Dependency Graph

```
Phase 1 (Foundation)
TASK-001 → TASK-002 → TASK-003 → TASK-004 → TASK-005
                ↓
Phase 2 (UI)    ↓
           TASK-006 → TASK-007 → TASK-008 → TASK-009 → TASK-010
                                                           ↓
Phase 3 (Skills)                                           ↓
TASK-018 → TASK-019 → TASK-020 → TASK-021 → TASK-022      ↓
                                     ↓         ↓           ↓
                                  TASK-023 ←───┘           ↓
                                     ↓                     ↓
                                  TASK-024                 ↓
                                     ↓                     ↓
                                  TASK-025                 ↓
                                   ↓   ↓                   ↓
                            TASK-026  TASK-027             ↓
                                   ↓   ↓                   ↓
                                  TASK-028 ←───────────────┘
                                     ↓
Phase 4 (Gameplay)                   ↓
                                  TASK-011 → TASK-012
                                                ↓
Phase 5 (Persistence)                           ↓
                                  TASK-013 → TASK-014
                                                ↓
Phase 6 (Polish)                                ↓
                        TASK-015 → TASK-016 → TASK-017
```

---

## Total Estimate
**~18-20 hours** for full MVP implementation (incl. Skill System)
