using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;
using System.Collections.ObjectModel;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class SkillViewModel : ObservableObject, IDisposable
{
    private readonly ISkillService _skillService;
    private readonly IMilestoneService _milestoneService;
    private bool _disposed;

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

    [ObservableProperty]
    private bool _hasAnySkills;

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
        HasAnySkills = ActiveSkillCount > 0;
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Unsubscribe from event to prevent memory leak
            _milestoneService.OnMilestoneReached -= OnMilestoneReached;
        }

        _disposed = true;
    }
}
