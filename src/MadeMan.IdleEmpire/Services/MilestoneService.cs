using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public class MilestoneService : IMilestoneService
{
    private readonly Func<GameState> _getState;
    private readonly ISkillService _skillService;
    private readonly Random _random = new();

    public event Action<List<SkillDefinition>>? OnMilestoneReached;

    public MilestoneService(Func<GameState> getState, ISkillService skillService)
    {
        _getState = getState;
        _skillService = skillService;
    }

    private GameState State => _getState();

    public bool CheckForMilestone()
    {
        // Already at max milestones?
        if (State.MilestoneCount >= SkillConfig.MilestoneThresholds.Length)
            return false;

        var nextThreshold = GetNextMilestoneThreshold();
        if (State.TotalEarned >= nextThreshold)
        {
            var pool = GetSelectionPool();
            if (pool.Count > 0)
            {
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
        var availableSkills = new List<SkillDefinition>();
        var playerHasMaxSkills = _skillService.GetActiveSkillCount() >= SkillConfig.MaxSkills;

        foreach (var skill in SkillConfig.Skills)
        {
            // Skip maxed skills
            if (_skillService.IsSkillMaxed(skill.Id))
                continue;

            // If player has 5 skills, only show existing skills
            if (playerHasMaxSkills && !_skillService.HasSkill(skill.Id))
                continue;

            availableSkills.Add(skill);
        }

        // Shuffle and take up to 3
        var shuffled = availableSkills.OrderBy(_ => _random.Next()).ToList();
        return shuffled.Take(SkillConfig.SkillChoicesPerMilestone).ToList();
    }

    public void CompleteMilestone(string selectedSkillId)
    {
        _skillService.AddOrUpgradeSkill(selectedSkillId);
        State.MilestoneCount++;
    }
}
