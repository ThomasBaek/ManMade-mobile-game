using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public class SkillService : ISkillService
{
    private readonly IGameStateProvider _stateProvider;

    public SkillService(IGameStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }

    private GameState State => _stateProvider.State;

    // === STATE QUERIES ===

    public int GetSkillLevel(string skillId)
    {
        return State.Skills.FirstOrDefault(s => s.Id == skillId)?.Level ?? 0;
    }

    public bool HasSkill(string skillId)
    {
        return State.Skills.Any(s => s.Id == skillId);
    }

    public bool CanAddNewSkill()
    {
        return State.Skills.Count < SkillConfig.MaxSkills;
    }

    public bool IsSkillMaxed(string skillId)
    {
        return GetSkillLevel(skillId) >= SkillConfig.MaxSkillLevel;
    }

    public int GetActiveSkillCount()
    {
        return State.Skills.Count;
    }

    public List<SkillState> GetActiveSkills()
    {
        return State.Skills.ToList();
    }

    // === MUTATIONS ===

    public void AddOrUpgradeSkill(string skillId)
    {
        var existing = State.Skills.FirstOrDefault(s => s.Id == skillId);

        if (existing != null)
        {
            // Upgrade existing
            if (existing.Level < SkillConfig.MaxSkillLevel)
            {
                existing.Level++;
            }
        }
        else
        {
            // Add new (if room)
            if (CanAddNewSkill())
            {
                State.Skills.Add(new SkillState { Id = skillId, Level = 1 });
            }
        }
    }

    public void ResetSkills()
    {
        State.Skills.Clear();
        State.MilestoneCount = 0;
        State.SessionStartUtc = DateTime.UtcNow;
    }

    // === EFFECT CALCULATIONS ===

    private SkillDefinition? GetDefinition(string skillId)
    {
        return SkillConfig.Skills.FirstOrDefault(s => s.Id == skillId);
    }

    public double GetTotalIncomeMultiplier()
    {
        // Cash Flow: +4% per level
        var level = GetSkillLevel("cash_flow");
        var def = GetDefinition("cash_flow");
        return def != null ? SkillCalculator.GetMultiplier(def, level) : 1.0;
    }

    public double GetOperationMultiplier(string operationId)
    {
        double multiplier = 1.0;

        // Check operation-specific skills
        foreach (var skill in SkillConfig.Skills.Where(s => s.TargetOperationId == operationId))
        {
            var level = GetSkillLevel(skill.Id);
            multiplier *= SkillCalculator.GetMultiplier(skill, level);
        }

        // Check tier-specific skills
        var tier = SkillConfig.GetOperationTier(operationId);
        foreach (var skill in SkillConfig.Skills.Where(s => s.TargetTier == tier))
        {
            var level = GetSkillLevel(skill.Id);
            multiplier *= SkillCalculator.GetMultiplier(skill, level);
        }

        return multiplier;
    }

    public double GetUpgradeCostMultiplier()
    {
        // Fast Learner: -4% per level
        var level = GetSkillLevel("fast_learner");
        var def = GetDefinition("fast_learner");
        return def != null ? SkillCalculator.GetReduction(def, level) : 1.0;
    }

    public double GetUnlockCostMultiplier()
    {
        // Early Bird: -5% per level
        var level = GetSkillLevel("early_bird");
        var def = GetDefinition("early_bird");
        return def != null ? SkillCalculator.GetReduction(def, level) : 1.0;
    }

    public double GetCashbackPercent()
    {
        // The Skim: +5% per level
        var level = GetSkillLevel("the_skim");
        var def = GetDefinition("the_skim");
        return def != null ? SkillCalculator.GetChancePercent(def, level) : 0;
    }

    public double GetPassiveIncomePerSecond()
    {
        // Passive Income: +$1/s per level
        var level = GetSkillLevel("passive_income");
        var def = GetDefinition("passive_income");
        return def != null ? SkillCalculator.GetFlatBonus(def, level) : 0;
    }

    public double GetOfflineEfficiencyBonus()
    {
        // Night Owl: +10% per level (additive to base 50%)
        var level = GetSkillLevel("night_owl");
        var def = GetDefinition("night_owl");
        return def != null ? level * def.EffectPerLevel / 100.0 : 0;
    }

    public double GetOfflineMaxHoursBonus()
    {
        // Extended Shift: +1 hour per level
        var level = GetSkillLevel("extended_shift");
        var def = GetDefinition("extended_shift");
        return def != null ? SkillCalculator.GetDurationHours(def, level) : 0;
    }

    public double GetOfflineEarningsMultiplier()
    {
        // Godfather's Cut: +8% per level
        var level = GetSkillLevel("godfathers_cut");
        var def = GetDefinition("godfathers_cut");
        return def != null ? SkillCalculator.GetMultiplier(def, level) : 1.0;
    }

    public double GetPrestigeBonusMultiplier()
    {
        // Reputation: +6% per level (additive to base 25%)
        var level = GetSkillLevel("reputation");
        var def = GetDefinition("reputation");
        return def != null ? SkillCalculator.GetMultiplier(def, level) : 1.0;
    }

    public double GetStartingCashBonus()
    {
        // Old Connections: +$100 per level
        var level = GetSkillLevel("old_connections");
        var def = GetDefinition("old_connections");
        return def != null ? SkillCalculator.GetFlatBonus(def, level) : 0;
    }

    public double GetCompoundInterestMultiplier(DateTime sessionStart)
    {
        var level = GetSkillLevel("compound_interest");
        return SkillCalculator.GetCompoundInterestMultiplier(level, sessionStart);
    }

    public bool RollLuckyBreak(Random random)
    {
        var level = GetSkillLevel("lucky_break");
        return SkillCalculator.RollLuckyBreak(level, random);
    }
}
