using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public static class SkillCalculator
{
    /// <summary>
    /// Calculate multiplier for a skill (1.0 + bonus)
    /// </summary>
    public static double GetMultiplier(SkillDefinition skill, int level)
    {
        if (level <= 0) return 1.0;
        return 1.0 + (level * skill.EffectPerLevel / 100.0);
    }

    /// <summary>
    /// Calculate reduction for a skill (1.0 - reduction)
    /// </summary>
    public static double GetReduction(SkillDefinition skill, int level)
    {
        if (level <= 0) return 1.0;
        return 1.0 - (level * skill.EffectPerLevel / 100.0);
    }

    /// <summary>
    /// Calculate flat bonus for a skill
    /// </summary>
    public static double GetFlatBonus(SkillDefinition skill, int level)
    {
        if (level <= 0) return 0;
        return level * skill.EffectPerLevel;
    }

    /// <summary>
    /// Calculate chance percent for a skill (0-100)
    /// </summary>
    public static double GetChancePercent(SkillDefinition skill, int level)
    {
        if (level <= 0) return 0;
        return level * skill.EffectPerLevel;
    }

    /// <summary>
    /// Calculate duration bonus in hours
    /// </summary>
    public static double GetDurationHours(SkillDefinition skill, int level)
    {
        if (level <= 0) return 0;
        return level * skill.EffectPerLevel;
    }

    /// <summary>
    /// Calculate Compound Interest multiplier based on session time
    /// </summary>
    public static double GetCompoundInterestMultiplier(int skillLevel, DateTime sessionStart)
    {
        if (skillLevel <= 0) return 1.0;

        var sessionMinutes = (DateTime.UtcNow - sessionStart).TotalMinutes;
        var intervals = Math.Min((int)(sessionMinutes / 5), 6); // Max 6 intervals (30 min)

        // +3% per level per 5-minute interval
        var bonusPercent = skillLevel * 3.0 * intervals;
        return 1.0 + (bonusPercent / 100.0);
    }

    /// <summary>
    /// Check for Lucky Break critical hit
    /// </summary>
    public static bool RollLuckyBreak(int skillLevel, Random random)
    {
        if (skillLevel <= 0) return false;

        var chance = skillLevel * 4.0; // 4% per level
        return random.NextDouble() * 100 < chance;
    }
}
