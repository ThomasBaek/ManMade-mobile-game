# Task 025: SkillViewModel

## Metadata
- **Phase**: 3 - Skill System
- **Dependencies**: TASK-024
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/extensions/SKILL_SYSTEM_SPECIFICATION.md (Section 6)
- **Requires Design Input**: NO

---

## Formål

Opret ViewModel for skill display og selection.

**Hvorfor dette er vigtigt:**
- UI skal vise aktive skills
- Modal skal præsentere skill valg
- Bindings til XAML views

---

## Analyse - Hvad Skal Implementeres

### SkillViewModel.cs
**Placering**: `ViewModels/SkillViewModel.cs`

Properties:
- `ActiveSkills` - Liste af spillerens skills
- `SelectionPool` - 3 valgmuligheder ved milestone
- `IsSelectionModalVisible` - Modal state
- `MilestoneProgress` - Progress bar (0-1)
- `NextMilestoneThreshold` - Formatted string

Commands:
- `SelectSkillCommand` - Vælg skill fra modal

---

## Implementation Guide

### Step 1: Opret SkillViewModel.cs

**Sti**: `src/MadeMan.IdleEmpire/ViewModels/SkillViewModel.cs`

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

### Step 2: Integrer i MainViewModel

Tilføj SkillViewModel som property eller som del af MainViewModel:

```csharp
// I MainViewModel.cs
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
- [ ] ObservableCollections opdaterer UI
- [ ] Commands er bindbare

---

## Acceptance Criteria

- [ ] SkillViewModel oprettet med alle properties
- [ ] SkillDisplayModel helper class
- [ ] OnMilestoneReached handler
- [ ] SelectSkillCommand implementeret
- [ ] RefreshActiveSkills opdaterer liste
- [ ] UpdateProgress beregner korrekt
- [ ] Build succeeds med 0 errors

---

## Notes

- SkillDisplayModel er en UI-venlig wrapper
- ObservableCollection for automatisk UI updates
- Event subscription for milestone notifications

---

**Task Status**: BLOCKED (venter på TASK-024)
**Last Updated**: 2024-12-25
