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
    private ObservableCollection<SkillChoiceModel> _selectionPool = new();

    [ObservableProperty]
    private bool _isSelectionModalVisible;

    [ObservableProperty]
    private double _milestoneProgress;

    [ObservableProperty]
    private string _nextMilestoneText = string.Empty;

    [ObservableProperty]
    private int _activeSkillCount;

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
            var currentLevel = _skillService.GetSkillLevel(skill.Id);
            SelectionPool.Add(new SkillChoiceModel
            {
                Id = skill.Id,
                Name = skill.Name,
                Description = skill.Description,
                EffectPerLevel = skill.EffectPerLevel,
                CurrentLevel = currentLevel,
                MaxLevel = SkillConfig.MaxSkillLevel,
                Icon = GetSkillIcon(skill),
                EffectType = skill.EffectType
            });
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
                    EffectPerLevel = definition.EffectPerLevel,
                    Icon = GetSkillIcon(definition)
                });
            }
        }
        ActiveSkillCount = ActiveSkills.Count;
    }

    public void UpdateProgress()
    {
        MilestoneProgress = _milestoneService.GetMilestoneProgress();
        var threshold = _milestoneService.GetNextMilestoneThreshold();
        NextMilestoneText = threshold < double.MaxValue
            ? $"Next: ${threshold:N0}"
            : "All milestones reached!";
    }

    private static string GetSkillIcon(SkillDefinition skill)
    {
        // Return emoji based on category for MVP (can use actual icons later)
        return skill.Category switch
        {
            SkillCategory.Income => "💰",
            SkillCategory.Operations => "🔧",
            SkillCategory.Offline => "🌙",
            SkillCategory.Prestige => "⭐",
            _ => "📦"
        };
    }
}

public class SkillDisplayModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int MaxLevel { get; set; }
    public string Description { get; set; } = string.Empty;
    public double EffectPerLevel { get; set; }
    public string Icon { get; set; } = "⭐";
    public string LevelDisplay => $"Lv {Level}/{MaxLevel}";
    public bool IsMaxed => Level >= MaxLevel;
}

public class SkillChoiceModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double EffectPerLevel { get; set; }
    public int CurrentLevel { get; set; }
    public int MaxLevel { get; set; }
    public string Icon { get; set; } = "⭐";
    public SkillEffectType EffectType { get; set; }

    // Display helpers
    public bool IsNew => CurrentLevel == 0;
    public string LevelTag => IsNew ? "NEW" : $"Lv {CurrentLevel} → {CurrentLevel + 1}";
    public string CurrentEffectDisplay => FormatEffect(CurrentLevel);
    public string NextEffectDisplay => FormatEffect(CurrentLevel + 1);

    private string FormatEffect(int level)
    {
        if (level == 0) return "-";
        var total = EffectPerLevel * level;
        return EffectType switch
        {
            SkillEffectType.Multiplier => $"+{total:F0}%",
            SkillEffectType.Reduction => $"-{total:F0}%",
            SkillEffectType.FlatBonus => $"+${total:F0}",
            SkillEffectType.Duration => $"+{total:F0}h",
            SkillEffectType.Chance => $"{total:F0}%",
            _ => $"+{total:F0}"
        };
    }
}
