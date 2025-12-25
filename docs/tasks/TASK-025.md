# Task 025: SkillViewModel

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-024
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 6)
- **Requires Design Input**: NO

---

## Purpose

Create ViewModel for skill display and selection.

**Why this is important:**
- UI must display active skills
- Modal must present skill choices
- Bindings to XAML views

---

## Analysis - What to Implement

### SkillViewModel.cs
**Location**: `ViewModels/SkillViewModel.cs`

Properties:
- `ActiveSkills` - List of player's skills
- `SelectionPool` - 3 choices at milestone
- `IsSelectionModalVisible` - Modal state
- `MilestoneProgress` - Progress bar (0-1)
- `NextMilestoneThreshold` - Formatted string

Commands:
- `SelectSkillCommand` - Select skill from modal

---

## Implementation Guide

### Step 1: Create SkillViewModel.cs

**Path**: `src/MadeMan.IdleEmpire/ViewModels/SkillViewModel.cs`

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;
using System.Collections.ObjectModel;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class SkillViewModel : ObservableObject
{
    private readonly ISkillService _skillService;
    private readonly IMilestoneService _milestoneService;

    [ObservableProperty]
    private ObservableCollection<SkillDisplayModel> _activeSkills = new();

    [ObservableProperty]
    private ObservableCollection<SkillDefinition> _selectionPool = new();

    [ObservableProperty]
    private bool _isSelectionModalVisible;

    [ObservableProperty]
    private double _milestoneProgress;

    [ObservableProperty]
    private string _nextMilestoneText = string.Empty;

    public SkillViewModel(ISkillService skillService, IMilestoneService milestoneService)
    {
        _skillService = skillService;
        _milestoneService = milestoneService;

        // Subscribe to milestone events
        _milestoneService.OnMilestoneReached += OnMilestoneReached;
    }

    private void OnMilestoneReached(List<SkillDefinition> pool)
    {
        SelectionPool.Clear();
        foreach (var skill in pool)
        {
            SelectionPool.Add(skill);
        }
        IsSelectionModalVisible = true;
    }

    [RelayCommand]
    private void SelectSkill(string skillId)
    {
        _milestoneService.CompleteMilestone(skillId);
        IsSelectionModalVisible = false;
        RefreshActiveSkills();
    }

    public void RefreshActiveSkills()
    {
        ActiveSkills.Clear();
        foreach (var skill in _skillService.GetActiveSkills())
        {
            var definition = SkillConfig.Skills.FirstOrDefault(s => s.Id == skill.Id);
            if (definition != null)
            {
                ActiveSkills.Add(new SkillDisplayModel
                {
                    Id = skill.Id,
                    Name = definition.Name,
                    Level = skill.Level,
                    MaxLevel = SkillConfig.MaxSkillLevel,
                    Description = definition.Description,
                    Icon = definition.Icon
                });
            }
        }
    }

    public void UpdateProgress()
    {
        MilestoneProgress = _milestoneService.GetMilestoneProgress();
        var threshold = _milestoneService.GetNextMilestoneThreshold();
        NextMilestoneText = threshold < double.MaxValue
            ? $"Next: ${threshold:N0}"
            : "Max milestones reached";
    }
}

public class SkillDisplayModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int MaxLevel { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string LevelDisplay => $"Lv {Level}/{MaxLevel}";
}
```

### Step 2: Integrate in MainViewModel

Add SkillViewModel as property or as part of MainViewModel:

```csharp
// In MainViewModel.cs
public SkillViewModel SkillVM { get; }

public MainViewModel(IGameEngine engine, SkillViewModel skillVM)
{
    _engine = engine;
    SkillVM = skillVM;
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. ViewModel Verification
- [ ] SkillViewModel compiles
- [ ] ObservableCollections update UI
- [ ] Commands are bindable

---

## Acceptance Criteria

- [ ] SkillViewModel created with all properties
- [ ] SkillDisplayModel helper class
- [ ] OnMilestoneReached handler
- [ ] SelectSkillCommand implemented
- [ ] RefreshActiveSkills updates list
- [ ] UpdateProgress calculates correctly
- [ ] Build succeeds with 0 errors

---

## Notes

- SkillDisplayModel is a UI-friendly wrapper
- ObservableCollection for automatic UI updates
- Event subscription for milestone notifications

---

**Task Status**: BLOCKED (waiting for TASK-024)
**Last Updated**: 2024-12-25
