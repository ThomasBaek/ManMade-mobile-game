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

## Phase 3: Gameplay

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-011 | Verify Game Loop | BLOCKED | TASK-010 | 1 time |
| TASK-012 | Verify Prestige | BLOCKED | TASK-011 | 30 min |

## Phase 4: Persistence

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-013 | Test Save/Load | BLOCKED | TASK-012 | 30 min |
| TASK-014 | Test Offline Earnings | BLOCKED | TASK-013 | 30 min |

## Phase 5: Polish

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

## Total Estimat
**~12-14 timer** for fuld MVP implementation
