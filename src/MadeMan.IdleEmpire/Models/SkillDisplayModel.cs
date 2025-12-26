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
}
