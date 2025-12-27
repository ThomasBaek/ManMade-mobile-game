# Claude Code Cleanup & Review Prompt: v1.2 Pre-Release

## Your Role

You are performing a comprehensive code review and cleanup of "Made Man: Idle Empire" before version 1.2 release. This is a quality gate to ensure the codebase is professional, performant, and maintainable.

---

## CRITICAL RULES

### Principles to Enforce
- **KISS**: Is every solution the simplest possible?
- **YAGNI**: Is there any dead code or unused features?
- **SOLID**: Are responsibilities properly separated?
- **DRY**: Is there code duplication that should be refactored?

### Documentation Requirements
- ALL findings must be documented
- CLAUDE.md must be updated with current project state
- Any changes require corresponding test updates

---

## PHASE 1: FULL CODE ANALYSIS

### Step 1: Discover Project Structure

```bash
# Find all source files
echo "=== C# FILES ===" && find . -name "*.cs" -type f | grep -v "obj/" | grep -v "bin/" | sort
echo "=== XAML FILES ===" && find . -name "*.xaml" -type f | grep -v "obj/" | grep -v "bin/" | sort
echo "=== PROJECT FILES ===" && find . -name "*.csproj" -type f | sort
echo "=== TEST FILES ===" && find . -name "*Test*.cs" -type f | sort
```

### Step 2: Code Metrics Collection

```bash
# Lines of code per file
echo "=== LINES PER FILE ===" && find . -name "*.cs" -type f ! -path "*/obj/*" ! -path "*/bin/*" -exec wc -l {} + | sort -n

# Large files (potential refactoring candidates)
echo "=== FILES > 200 LINES ===" && find . -name "*.cs" -type f ! -path "*/obj/*" ! -path "*/bin/*" -exec sh -c 'lines=$(wc -l < "$1"); if [ "$lines" -gt 200 ]; then echo "$lines $1"; fi' _ {} \;
```

### Step 3: Build Verification

```bash
# Clean build
dotnet clean
dotnet build -f net10.0-android 2>&1 | tee build_output.txt

# Count warnings
echo "=== WARNINGS ===" && grep -i "warning" build_output.txt | sort | uniq -c | sort -rn
```

---

## PHASE 2: CODE QUALITY REVIEW

For EACH source file, evaluate and document:

### 2.1 Naming Conventions
- [ ] Classes: PascalCase, descriptive nouns
- [ ] Methods: PascalCase, verb phrases
- [ ] Properties: PascalCase
- [ ] Private fields: _camelCase
- [ ] Local variables: camelCase
- [ ] Constants: UPPER_SNAKE_CASE or PascalCase

### 2.2 Method Quality
- [ ] Max 30 lines per method (ideally 15-20)
- [ ] Single responsibility per method
- [ ] Clear parameter naming
- [ ] No magic numbers (use constants)
- [ ] Proper XML documentation for public methods

### 2.3 Class Quality
- [ ] Single responsibility principle
- [ ] Proper encapsulation (minimal public API)
- [ ] Dependencies injected, not created
- [ ] No god classes (too many responsibilities)

### 2.4 MVVM Pattern Compliance
- [ ] ViewModels don't reference Views
- [ ] Views only bind, no business logic
- [ ] Services are injected via DI
- [ ] ObservableProperty for bindable state
- [ ] RelayCommand for actions

### 2.5 Error Handling
- [ ] Try-catch where IO occurs
- [ ] Null checks on parameters
- [ ] Defensive programming on external data
- [ ] No swallowed exceptions

---

## PHASE 3: PERFORMANCE ANALYSIS

### 3.1 Memory Patterns

Check for:
- Event handler subscriptions without unsubscribe
- Collections that grow unbounded
- Large object allocations in loops
- String concatenation in hot paths (use StringBuilder)

```bash
# Find event subscriptions
grep -rn "+=" --include="*.cs" | grep -i "event\|handler\|changed"

# Find potential memory issues
grep -rn "new List\|new Dictionary\|\.Add(" --include="*.cs" | head -30
```

### 3.2 Game Loop Performance

Review:
- Timer interval (not too frequent)
- Work done per tick (minimize)
- UI updates (batch if possible)
- Async operations (properly awaited)

### 3.3 Persistence Performance

Check:
- Save frequency (not too often)
- JSON serialization size
- Deserialization on startup

```bash
# Find save operations
grep -rn "Save\|Preferences\|Serialize" --include="*.cs"
```

### 3.4 XAML Performance

Review:
- Nested layouts (flatten where possible)
- CollectionView virtualization
- Image sizes and caching
- Binding mode (OneWay vs TwoWay)

---

## PHASE 4: TEST COVERAGE ANALYSIS

### 4.1 Existing Tests Inventory

```bash
# List all test files
find . -name "*Test*.cs" -o -name "*Tests.cs" | xargs wc -l 2>/dev/null

# List test methods
grep -rn "\[Fact\]\|\[Theory\]\|\[Test\]" --include="*.cs"
```

### 4.2 Test Coverage Gaps

For each core component, verify test exists:

**Models**
- [ ] GameState serialization/deserialization
- [ ] Operation calculations
- [ ] SkillDefinition effects

**Services**
- [ ] GameEngine.CalculateIncome()
- [ ] GameEngine.ProcessUpgrade()
- [ ] GameEngine.ProcessPrestige()
- [ ] SaveManager.Save/Load roundtrip
- [ ] Offline earnings calculation

**ViewModels**
- [ ] Property change notifications
- [ ] Command execution

### 4.3 Test Quality Check

For existing tests:
- [ ] Tests are isolated (no shared state)
- [ ] Tests have clear arrange/act/assert
- [ ] Edge cases covered
- [ ] Negative cases tested

---

## PHASE 5: DEAD CODE & CLEANUP

### 5.1 Unused Code Detection

```bash
# Find potentially unused private methods (manual verification needed)
grep -rn "private.*(" --include="*.cs" | grep -v "get;\|set;\|=>"

# Find TODO/FIXME comments
grep -rn "TODO\|FIXME\|HACK\|XXX" --include="*.cs" --include="*.xaml"

# Find commented-out code
grep -rn "^\s*//" --include="*.cs" | head -50
```

### 5.2 Remove/Fix

- [ ] Remove unused usings
- [ ] Remove commented-out code
- [ ] Address or remove TODO comments
- [ ] Remove unused private methods
- [ ] Remove unused files

---

## PHASE 6: UPDATE CLAUDE.md

After analysis, update CLAUDE.md with:

```markdown
# CLAUDE.md - Made Man: Idle Empire

## Last Updated
[DATE OF THIS REVIEW]

## Project Overview
Idle/incremental mobile game built with .NET MAUI.
Player builds a criminal empire in 1930s New Porto.

## Version
**Current**: 1.2-dev
**Last Release**: 1.0 MVP

## Tech Stack
- Framework: .NET 10 MAUI
- Pattern: MVVM with CommunityToolkit.Mvvm
- Target: Android 8.0+ (API 26)
- Storage: Preferences API (JSON)

## Build Commands
```bash
# Development build
dotnet build src/MadeMan.IdleEmpire -f net10.0-android

# Run on emulator
dotnet build src/MadeMan.IdleEmpire -f net10.0-android -t:Run

# Clean build
dotnet clean && dotnet build -f net10.0-android

# Run tests
dotnet test tests/MadeMan.IdleEmpire.Tests
```

## Architecture

### Folder Structure
```
src/MadeMan.IdleEmpire/
├── Models/           # Data classes
├── Services/         # Business logic
├── ViewModels/       # MVVM ViewModels
├── Views/            # XAML pages & components
├── Resources/        # Styles, images, fonts
└── MauiProgram.cs    # DI configuration
```

### Key Files
[LIST THE MOST IMPORTANT FILES AND THEIR PURPOSE]

### Data Flow
1. GameEngine manages game state
2. SaveManager persists to Preferences
3. ViewModels expose state to Views
4. Views bind and display

## Coding Standards

### Principles
- **KISS**: Simplest solution that works
- **YAGNI**: Only what's needed now
- **SOLID**: Proper responsibility separation
- **DRY**: No code duplication

### Naming
- Classes: PascalCase nouns
- Methods: PascalCase verbs
- Properties: PascalCase
- Private fields: _camelCase
- Constants: PascalCase

### Method Guidelines
- Max 30 lines (prefer 15-20)
- Single responsibility
- XML docs for public API

### MVVM Rules
- ViewModels never reference Views
- Views only bind, no logic
- All services via DI
- [ObservableProperty] for state
- [RelayCommand] for actions

## Testing

### Test Project
tests/MadeMan.IdleEmpire.Tests/

### Running Tests
```bash
dotnet test tests/MadeMan.IdleEmpire.Tests
```

### Test Coverage Areas
[LIST WHAT IS TESTED]

### Adding New Tests
1. Create test file matching source file name
2. Use xUnit [Fact] or [Theory]
3. Follow Arrange-Act-Assert pattern
4. Test edge cases and failures

## Feature Flags / Configuration

### GameConfig.cs
[DOCUMENT KEY CONFIGURATION VALUES]

## Common Tasks

### Adding a New Operation
1. Add to GameConfig.Operations list
2. [OTHER STEPS]

### Adding a New Skill
1. Add SkillDefinition to SkillConfig
2. [OTHER STEPS]

### Modifying Game Balance
1. Update values in GameConfig
2. Run balance tests
3. Test in emulator

## Known Issues / Technical Debt
[LIST ANY ISSUES FOUND]

## Performance Considerations
[LIST PERFORMANCE NOTES]

## Changelog for v1.2
[DOCUMENT WHAT CHANGED IN THIS VERSION]
```

---

## PHASE 7: IMPROVEMENT RECOMMENDATIONS

Document in a new file `docs/REVIEW_FINDINGS.md`:

```markdown
# Code Review Findings - v1.2

## Date: [DATE]

## Summary
- Files reviewed: X
- Issues found: X
- Critical: X
- Performance: X
- Style: X

## Critical Issues
[MUST FIX BEFORE RELEASE]

## Performance Improvements
### High Priority
[SIGNIFICANT IMPACT]

### Medium Priority
[NICE TO HAVE]

### Low Priority
[FUTURE CONSIDERATION]

## Code Quality Issues
[STYLE, NAMING, STRUCTURE]

## Test Gaps
[MISSING TESTS]

## Recommended Refactoring
[OPTIONAL BUT BENEFICIAL]
```

---

## PHASE 8: EXECUTION CHECKLIST

### Before Starting
- [ ] Read this entire prompt
- [ ] Understand the project structure

### Analysis Phase
- [ ] Complete Phase 1 (structure discovery)
- [ ] Complete Phase 2 (code quality)
- [ ] Complete Phase 3 (performance)
- [ ] Complete Phase 4 (test coverage)
- [ ] Complete Phase 5 (dead code)

### Documentation Phase
- [ ] Update CLAUDE.md with current state
- [ ] Create REVIEW_FINDINGS.md
- [ ] Update any affected task files

### Cleanup Phase (if issues found)
- [ ] Fix critical issues
- [ ] Add missing tests
- [ ] Remove dead code
- [ ] Address performance issues

### Verification
- [ ] Build passes with 0 errors
- [ ] All tests pass
- [ ] Manual smoke test on emulator

---

## OUTPUT FORMAT

Provide findings in this structure:

### 1. Executive Summary
- Overall code health rating (1-10)
- Key strengths
- Key concerns
- Recommended immediate actions

### 2. Detailed Findings by Category
- Code quality issues (with file:line references)
- Performance concerns (with impact estimate)
- Missing tests (with priority)
- Dead code to remove

### 3. CLAUDE.md Draft
- Complete updated CLAUDE.md content

### 4. Action Items
Prioritized list:
1. [CRITICAL] ...
2. [HIGH] ...
3. [MEDIUM] ...
4. [LOW] ...

---

## START NOW

Begin with Phase 1: Full Code Analysis. Report back with findings before making any changes.

**Command to start**: Run the discovery commands and report what you find.
