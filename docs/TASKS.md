# Made Man: Idle Empire - Tasks Overview

## Aktuel Task
**TASK-006**: Implement Theme - READY

---

## Task Status Legend
- READY - Klar til implementation
- BLOCKED - Venter på dependencies
- IN_PROGRESS - Under arbejde
- COMPLETED - Færdig og godkendt

---

## Phase 1: Foundation

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-001 | Install CommunityToolkit.Mvvm | COMPLETED | None | 15 min |
| TASK-002 | Implement Models | COMPLETED | TASK-001 | 45 min |
| TASK-003 | Implement SaveManager | COMPLETED | TASK-002 | 30 min |
| TASK-004 | Implement GameEngine | COMPLETED | TASK-003 | 1 time |
| TASK-005 | Setup DI & Lifecycle | COMPLETED | TASK-004 | 30 min |

## Phase 2: UI

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-006 | Implement Theme | READY | TASK-005 | 30 min |
| TASK-007 | OperationViewModel | BLOCKED | TASK-006 | 45 min |
| TASK-008 | MainViewModel | BLOCKED | TASK-007 | 1 time |
| TASK-009 | MainPage UI | BLOCKED | TASK-008 | 1.5 time |
| TASK-010 | Add Icons | BLOCKED | TASK-009 | 30 min |

## Phase 3: Skill System (NEW)

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-018 | Skill Models | BLOCKED | TASK-002 | 30 min |
| TASK-019 | Skill Config (18 Skills) | BLOCKED | TASK-018 | 45 min |
| TASK-020 | Skill Calculator | BLOCKED | TASK-019 | 30 min |
| TASK-021 | Skill Service | BLOCKED | TASK-020 | 45 min |
| TASK-022 | Milestone Service | BLOCKED | TASK-021 | 30 min |
| TASK-023 | Integrate Skills into GameEngine | BLOCKED | TASK-021, TASK-022 | 45 min |
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

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-011 | Verify Game Loop | BLOCKED | TASK-010, TASK-028 | 1 time |
| TASK-012 | Verify Prestige | BLOCKED | TASK-011 | 30 min |

## Phase 5: Persistence

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-013 | Test Save/Load | BLOCKED | TASK-012 | 30 min |
| TASK-014 | Test Offline Earnings | BLOCKED | TASK-013 | 30 min |

## Phase 6: Polish

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-015 | UI Polish | BLOCKED | TASK-014 | 1 time |
| TASK-016 | Bug Fixes | BLOCKED | TASK-015 | Variable |
| TASK-017 | Final Testing | BLOCKED | TASK-016 | 1 time |

---

## Completed Tasks

| Task | Navn | Completed | Commit |
|------|------|-----------|--------|
| TASK-001 | Install CommunityToolkit.Mvvm | 2024-12-25 | ae0df75 |
| TASK-002 | Implement Models | 2024-12-25 | ddc7d58 |
| TASK-003 | Implement SaveManager | 2024-12-25 | fe72620 |
| TASK-004 | Implement GameEngine | 2024-12-25 | 7f52788 |
| TASK-005 | Setup DI & Lifecycle | 2024-12-25 | a5bd9e0 |

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

## Total Estimat
**~18-20 timer** for fuld MVP implementation (inkl. Skill System)
