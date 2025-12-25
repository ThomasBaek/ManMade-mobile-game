# Task 001: Install CommunityToolkit.Mvvm

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: None
- **Estimated Time**: 15 min
- **Status**: READY
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Purpose

Install CommunityToolkit.Mvvm NuGet package for MVVM support.

**Why this is important:**
- CommunityToolkit.Mvvm is necessary for [ObservableProperty] and [RelayCommand]
- Significantly reduces boilerplate code
- Standard pattern for .NET MAUI apps

---

## Risks

### Potential Problems
1. **Package Version Conflict**:
   - Edge case: Incompatible version with .NET 10
   - Impact: Compile errors

### Mitigation
- Use latest stable version (8.2.2 or higher)
- Test build after installation

---

## Analysis - What Needs to be Implemented

### 1. Add NuGet Package
**Location**: `src/MadeMan.IdleEmpire/MadeMan.IdleEmpire.csproj`

**Changes:**
- Add CommunityToolkit.Mvvm package reference

### 2. Verify Build
**Command**: `dotnet build -f net10.0-android`

---

## Dependencies Check

**Required Before Start**:
- [x] Project exists and builds (verified 2024-12-25)

**Assumptions**:
- NuGet feeds are accessible

**Blockers**: None

---

## Implementation Guide

### Step 1: Add CommunityToolkit.Mvvm Package

**Path**: `src/MadeMan.IdleEmpire/MadeMan.IdleEmpire.csproj`

Add in ItemGroup with other PackageReference:

```xml
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
```

### Step 2: Restore and Build

```bash
cd src/MadeMan.IdleEmpire
dotnet restore
dotnet build -f net10.0-android
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Package Verification
```bash
dotnet list src/MadeMan.IdleEmpire package
```
Expected: CommunityToolkit.Mvvm 8.2.2 (or higher) listed

---

## Acceptance Criteria

- [x] CommunityToolkit.Mvvm package installed
- [x] Build succeeds with 0 errors
- [x] No new warnings introduced

---

## Code Evaluation

### Simplification Check
- **Minimal change**: Only one line added to .csproj
- **No new code**: Only package reference

### Alternatives Considered
None - CommunityToolkit.Mvvm is standard for MAUI MVVM

### Known Limitations
- None

---

## Code Quality Checklist

- [x] **KISS**: Simplest solution
- [x] **Readability**: Standard .csproj format
- [x] **DRY**: No duplication

---

## Design Files Reference

- **Spec Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Related Tasks**: TASK-002

---

## Notes

- CommunityToolkit.Mvvm 8.2.2 is compatible with .NET 10
- Source generators require partial classes

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Completed**: 2024-12-25
