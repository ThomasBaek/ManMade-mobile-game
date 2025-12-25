# Task Template v1.1
## Standard Structure for All Task

**Use this template for creating new command files.**
**All Task MUST include code quality sections.**

---

## Metadata Template

```markdown
# Command XXX: [Command Name]

## Metadata
- **Phase**: [1-5] - [Phase Name]
- **Dependencies**: [Command IDs or "None"]
- **Estimated Time**: [X-Y hours]
- **Status**: Pending | In Progress | Completed
- **Design Reference**: [Path to design file or "N/A"]
- **Frequency Impact**: YES/NO - Does this command involve frequency features?
```

---

## Required Sections

### 1. Formål (Purpose)
```markdown
## Formål

[Klar beskrivelse af hvad denne command skal opnå]

**Hvorfor dette er vigtigt:**
- [Reason 1]
- [Reason 2]
- [Connection to user value]
```

### 2. Risici (Risks)
```markdown
## Risici

### Potentielle Problemer
1. **[Problem category]**:
   - Edge case: [Description]
   - Impact: [What could go wrong]

2. **[Another problem]**:
   - Description
   - Impact

### Mitigering
- [How to prevent/handle problem 1]
- [How to prevent/handle problem 2]
```

### 3. Analyse - Hvad Skal Implementeres
```markdown
## Analyse - Hvad Skal Implementeres

### [Component/Feature 1]
**Description**: [What it is]
**Location**: [File path]
**Key Requirements**:
- [Requirement 1]
- [Requirement 2]

### [Component/Feature 2]
[Same structure]

**Business Rules** (if applicable):
```csharp
// Example code showing business logic
```
```

### 4. Dependencies Check
```markdown
## Dependencies Check

✅ **Required Before Starting**:
- [x] Dependency 1 completed
- [x] Dependency 2 completed

⚠️ **Assumptions**:
- [Assumption about existing code]
- [Assumption about design]

❌ **Blockers** (if any):
- [What would prevent starting this command]
```

### 5. Implementation Guide
```markdown
## Implementation Guide

### Step 1: [First Task]
Path: `[file path]`

```csharp
// Complete code example
```

**Explanation**: [Why this code]

### Step 2: [Second Task]
[Same structure]

### Step N: Register/Integrate
[DI registration, routing, etc.]
```

### 6. Verification Steps
```markdown
## Verification Steps

### 1. Build Test
```bash
dotnet build src/Stribe/Stribe.csproj
```
Expected: 0 errors

### 2. Unit Tests (if applicable)
```csharp
[Fact]
public async Task [TestName]()
{
    // Test code
}
```

### 3. Manual Test in Emulator
- [ ] Test scenario 1
- [ ] Test scenario 2
- [ ] Edge case testing
```

### 7. Acceptance Criteria
```markdown
## Acceptance Criteria

- [ ] [Criterion 1]
- [ ] [Criterion 2]
- [ ] [Frequency feature working (if applicable)]
- [ ] Build succeeds with 0 errors
- [ ] No new warnings introduced
- [ ] Manual testing passed
```

### 8. Kode Evaluering (REQUIRED)
```markdown
## Kode Evaluering

### Simplifikations-tjek
Denne implementation følger KISS princippet ved at:
- **[Simplification 1]**: [Description]
- **[Simplification 2]**: [Description]
- **Ingen [X]**: [What complexity was avoided]

### Alternativer overvejet

**Alternative 1: [Name]**
```csharp
// Alternative approach code
```
**Hvorfor fravalgt**: [Reason - usually "too complex" or "over-engineering"]

**Alternative 2: [Name]**
[Same structure]

### Potentielle forbedringer (v2)
- [Enhancement 1] - Not needed for MVP
- [Enhancement 2] - Can add later if needed
- [Enhancement 3] - User feedback might request this

### Kendte begrænsninger
- **[Limitation 1]**: [Description and why it's acceptable for MVP]
- **[Limitation 2]**: [Description]
```

### 9. Kode Kvalitet Checklist (REQUIRED)
```markdown
## Kode Kvalitet Checklist

- [ ] **KISS**: Er dette den simpleste løsning?
- [ ] **Læsbarhed**: Kan en anden udvikler forstå koden uden forklaring?
- [ ] **Navngivning**: Er alle navne beskrivende og konsistente?
- [ ] **Funktioner**: Er alle funktioner korte og fokuserede? (max 20-30 linjer)
- [ ] **DRY**: Er der nogen duplikeret kode?
- [ ] **Error handling**: Er fejl håndteret korrekt?
- [ ] **Edge cases**: Er edge cases identificeret og håndteret?
- [ ] **Performance**: Er der åbenlyse performance problemer?
- [ ] **Testbarhed**: Kan koden nemt testes?
```

### 10. Design Files Reference
```markdown
## Design Files Reference

- **Screen Spec**: [Path to .md file]
- **Component Spec**: [Path to .md file]
- **Related**: [Other relevant design files]
```

### 11. Notes (Optional)
```markdown
## Notes

- [Implementation note]
- [Design decision]
- [Future consideration]
```

---

## Frequency Features Integration

### For Commands Involving DailyTargetCount or ActiveDays

Add this section to Analyse:

```markdown
### Frequency Feature Integration

**This command involves frequency features:**

**DailyTargetCount handling**:
- Component shows progress ring when `habit.DailyTargetCount > 1`
- Toggle logic increments count up to target, then resets
- [Specific handling for this component]

**ActiveDays handling**:
- Filter habits where `IsActiveOnDay(currentDate) == true`
- Week progress bar only shows active day segments
- [Specific handling for this component]

**Code Example**:
```csharp
// Show how to handle frequency in this command
public bool ShouldShowHabit(Habit habit, DateTime date)
{
    return habit.IsActiveOnDay(date);
}

public bool IsCompleted(Completion completion, Habit habit)
{
    return completion != null && completion.Count >= habit.DailyTargetCount;
}
```
```

---

## Command Status Indicators

```markdown
**Command Status**: ⏸️ Ready to implement | 🔄 In Progress | ✅ Completed
**Last Updated**: [Date]
**Implemented By**: [Name or "Pending"]
```

---

## Example: Minimal Command (Simple)

For simple commands (like adding a helper method), you can use abbreviated sections:

```markdown
# Command 008: Error Handling & Logging

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: None
- **Estimated Time**: 1-2 hours
- **Status**: Pending
- **Frequency Impact**: NO

## Formål
Add global error handler and logging setup for debugging.

## Analyse
Create ErrorLogger class with file logging and console output.

## Implementation Guide
```csharp
public static class ErrorLogger
{
    public static void Log(Exception ex) { /* implementation */ }
}
```

## Kode Evaluering
### Simplifikations-tjek
- Simple file-based logging (no complex frameworks)
- Console output for debugging
- No external dependencies

### Alternativer overvejet
- **Serilog**: Too complex for MVP
- **ApplicationInsights**: Cloud dependency not needed

## Kode Kvalitet Checklist
- [x] KISS: File + console logging only
- [x] DRY: Single logger class
[... rest of checklist]

**Command Status**: ⏸️ Ready to implement
```

---

## Example: Complex Command (Detailed)

For complex commands (like HabitService, HomeViewModel), use full template with all sections expanded. See `005-habit-service.md` as reference.

---

## Checklist for Command Completion

Before marking a command as completed:

1. ✅ All code implemented as specified
2. ✅ Build succeeds (0 errors)
3. ✅ Manual testing completed
4. ✅ Kode Evaluering section filled out
5. ✅ Kode Kvalitet Checklist all checked
6. ✅ Acceptance criteria met
7. ✅ _state.json updated
8. ✅ Git commit created

---

**Template Version**: 1.1
**Last Updated**: 2025-12-23
**Usage**: Copy this template for each new command file
