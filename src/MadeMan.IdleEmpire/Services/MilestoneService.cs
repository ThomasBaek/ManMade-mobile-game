using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public class MilestoneService : IMilestoneService
{
    private readonly IGameStateProvider _stateProvider;
    private readonly ISkillService _skillService;

    // Prevent re-triggering while modal is open
    private bool _pendingSelection;
    private List<SkillDefinition>? _cachedPool;

    public event Action<List<SkillDefinition>>? OnMilestoneReached;

    public MilestoneService(IGameStateProvider stateProvider, ISkillService skillService)
    {
        _stateProvider = stateProvider;
        _skillService = skillService;
    }

    private GameState State => _stateProvider.State;

    public bool CheckForMilestone()
    {
        // Don't trigger again while waiting for selection
        if (_pendingSelection)
            return false;

        // Already at max milestones?
        if (State.MilestoneCount >= SkillConfig.MilestoneThresholds.Length)
            return false;

        var nextThreshold = GetNextMilestoneThreshold();
        if (State.TotalEarned >= nextThreshold)
        {
            var pool = GenerateSelectionPool();
            if (pool.Count > 0)
            {
                _cachedPool = pool;
                _pendingSelection = true;
                OnMilestoneReached?.Invoke(pool);
                return true;
            }
        }

        return false;
    }

    public double GetNextMilestoneThreshold()
    {
        if (State.MilestoneCount >= SkillConfig.MilestoneThresholds.Length)
            return double.MaxValue;

        return SkillConfig.MilestoneThresholds[State.MilestoneCount];
    }

    public double GetMilestoneProgress()
    {
        if (State.MilestoneCount >= SkillConfig.MilestoneThresholds.Length)
            return 1.0;

        var threshold = GetNextMilestoneThreshold();
        var previousThreshold = State.MilestoneCount > 0
            ? SkillConfig.MilestoneThresholds[State.MilestoneCount - 1]
            : 0;

        var progress = State.TotalEarned - previousThreshold;
        var range = threshold - previousThreshold;

        return Math.Clamp(progress / range, 0, 1);
    }

    public List<SkillDefinition> GetSelectionPool()
    {
        // Return cached pool if available (during pending selection)
        return _cachedPool ?? new List<SkillDefinition>();
    }

    private List<SkillDefinition> GenerateSelectionPool()
    {
        var availableSkills = new List<SkillDefinition>();
        var playerHasMaxSkills = _skillService.GetActiveSkillCount() >= SkillConfig.MaxSkills;

        foreach (var skill in SkillConfig.Skills)
        {
            // Skip maxed skills
            if (_skillService.IsSkillMaxed(skill.Id))
                continue;

            // If player has 5 skills, only show existing skills (upgrades only)
            if (playerHasMaxSkills && !_skillService.HasSkill(skill.Id))
                continue;

            availableSkills.Add(skill);
        }

        // Shuffle and take up to 3 (using thread-safe Random.Shared)
        var shuffled = availableSkills.OrderBy(_ => Random.Shared.Next()).ToList();
        return shuffled.Take(SkillConfig.SkillChoicesPerMilestone).ToList();
    }

    public void CompleteMilestone(string selectedSkillId)
    {
        _skillService.AddOrUpgradeSkill(selectedSkillId);
        State.MilestoneCount++;

        // Clear pending state so next milestone can trigger
        _pendingSelection = false;
        _cachedPool = null;
    }

    public void Reset()
    {
        // Clear pending state (used on prestige)
        _pendingSelection = false;
        _cachedPool = null;
    }
}
