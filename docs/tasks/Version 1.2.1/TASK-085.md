# TASK-085: Prestige Progress Visibility

## Metadata
- **Phase**: Version 1.2.1
- **Bundle**: V (UI Refinements)
- **Dependencies**: None
- **Estimated Time**: 45 min
- **Status**: READY
- **Priority**: Medium

---

## Purpose

1. Only show prestige progress on Empire tab (not Skills/Settings)
2. Consider moving player title to top bar for constant visibility

**Why this matters:**
- Less clutter on non-Empire tabs
- Title is a status symbol, should be always visible
- Prestige progress only relevant when viewing operations

---

## Current Behavior

- Prestige progress bar shows in TopBar on all tabs
- Player title shown... (check where)

## Target Behavior

- Prestige progress bar: Empire tab only
- Player title: Always visible in TopBar (new line above cash?)

---

## Technical Analysis

### Option A: Conditional Visibility via Binding
Pass current page context to TopBar:
```xml
<components:TopBar ShowPrestigeProgress="{Binding IsEmpireTab}"/>
```

### Option B: Shell-based Detection
TopBar detects current page from Shell:
```csharp
var currentPage = Shell.Current.CurrentPage;
ShowPrestigeProgress = currentPage is MainPage;
```

### Option C: Event-based
MainPage.OnAppearing sets flag, other pages clear it.

**Recommended:** Option B - self-contained in TopBar

---

## UI Design

### TopBar Layout (Proposed)

```
┌─────────────────────────────────────────────┐
│  Street Punk                    [⚙️]        │  ← Title line (NEW)
├─────────────────────────────────────────────┤
│  💰 1,234    +$5.50/s                       │  ← Cash line (existing)
├─────────────────────────────────────────────┤
│  [████████░░░░░░░░]  $12K / $25K  (48%)     │  ← Prestige (Empire only)
└─────────────────────────────────────────────┘
```

### Alternative: Title Badge
```
┌─────────────────────────────────────────────┐
│  💰 1,234    [Street Punk]    +$5.50/s     │
├─────────────────────────────────────────────┤
│  [████████░░░░░░░░]  48% to Prestige        │  ← Empire only
└─────────────────────────────────────────────┘
```

---

## Implementation Steps

### Step 1: Add Title to TopBar

**TopBar.xaml:**
```xml
<!-- Title row -->
<Label Text="{Binding CurrentTitle}"
       FontFamily="BebasNeue"
       FontSize="14"
       TextColor="{StaticResource Gold}"/>
```

### Step 2: Conditional Prestige Progress

**TopBar.xaml.cs:**
```csharp
protected override void OnAppearing()
{
    base.OnAppearing();
    UpdatePrestigeVisibility();
}

private void UpdatePrestigeVisibility()
{
    var currentRoute = Shell.Current?.CurrentState?.Location?.ToString();
    PrestigeSection.IsVisible = currentRoute?.Contains("MainPage") ?? true;
}
```

Or bind to a property on MainViewModel.

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Navigation Tests
- [ ] Empire tab: Title + Cash + Prestige progress visible
- [ ] Skills tab: Title + Cash visible, NO prestige progress
- [ ] Settings tab: Title + Cash visible, NO prestige progress
- [ ] Navigating back to Empire restores prestige progress

---

## Acceptance Criteria

- [ ] Prestige progress only visible on Empire tab
- [ ] Player title visible on all tabs
- [ ] Smooth transition when switching tabs
- [ ] No layout jumps when progress hides/shows
- [ ] Build succeeds

---

## Code Evaluation

### Simplicity Check
- Simple IsVisible binding or code-behind
- Title is just one new Label in TopBar
- No complex state management

### Performance Impact
- None significant

---

**Task Status**: READY
