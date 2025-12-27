# TASK-057: Add Project Reference to Test Project

## Bundle
**Bundle L: Test Infrastructure**

## Description
Add reference from test project to main project so tests can access game classes.

## Changes Required

**Command to run:**
```bash
cd E:/Arbejde/ManMade-mobile-game
dotnet add tests/MadeMan.IdleEmpire.Tests reference src/MadeMan.IdleEmpire
```

**Note:** May need to handle MAUI-specific references. If build fails, consider:
- Creating a shared library project for testable code
- Or mocking MAUI dependencies

## Acceptance Criteria
- [ ] Test project references main project
- [ ] `dotnet build tests/MadeMan.IdleEmpire.Tests` succeeds
- [ ] Can use `using MadeMan.IdleEmpire.Models;` in test files

## Dependencies
TASK-056

## Estimate
15 min
