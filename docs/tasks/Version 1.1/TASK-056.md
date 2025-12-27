# TASK-056: Create xUnit Test Project

## Bundle
**Bundle L: Test Infrastructure**

## Description
Create a new xUnit test project for the MadeMan.IdleEmpire solution.

## Changes Required

**Commands to run:**
```bash
cd E:/Arbejde/ManMade-mobile-game
dotnet new xunit -n MadeMan.IdleEmpire.Tests -o tests/MadeMan.IdleEmpire.Tests
```

**Expected structure:**
```
tests/
└── MadeMan.IdleEmpire.Tests/
    ├── MadeMan.IdleEmpire.Tests.csproj
    └── UnitTest1.cs (can be deleted)
```

## Acceptance Criteria
- [ ] Test project created in `tests/` folder
- [ ] Project uses xUnit framework
- [ ] `dotnet build` succeeds for test project

## Dependencies
None (can run in parallel with Bundle J/K)

## Estimate
10 min
