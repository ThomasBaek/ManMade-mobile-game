namespace MadeMan.IdleEmpire.Models;

/// <summary>
/// Display model for showing active skills in the UI.
/// </summary>
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

    /// <summary>
    /// Formatted effect description (e.g., "+15% Pickpocket income")
    /// </summary>
    public string EffectDescription => $"+{EffectPerLevel * Level:0}% {Description}";

    /// <summary>
    /// Progress toward max level (0.0 to 1.0)
    /// </summary>
    public double LevelProgress => MaxLevel > 0 ? (double)Level / MaxLevel : 0;
}
