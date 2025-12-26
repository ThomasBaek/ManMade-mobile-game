namespace MadeMan.IdleEmpire.Models;

/// <summary>
/// Model for skill selection in the milestone modal.
/// </summary>
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
