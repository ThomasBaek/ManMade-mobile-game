# TASK-058: Add Moq Package for Mocking

## Bundle
**Bundle L: Test Infrastructure**

## Description
Add Moq package to enable mocking of dependencies in tests.

## Changes Required

**Command to run:**
```bash
cd E:/Arbejde/ManMade-mobile-game/tests/MadeMan.IdleEmpire.Tests
dotnet add package Moq
```

## Acceptance Criteria
- [ ] Moq package added to test project
- [ ] `dotnet build` succeeds
- [ ] Can use `using Moq;` in test files

## Dependencies
TASK-057

## Estimate
5 min
