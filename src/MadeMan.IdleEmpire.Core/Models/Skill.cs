namespace MadeMan.IdleEmpire.Models;

public enum SkillCategory
{
    Income,
    Operations,
    Offline,
    Prestige
}

public enum SkillEffectType
{
    Multiplier,      // Multiply value (1.0 + bonus)
    Reduction,       // Reduce value (1.0 - bonus)
    FlatBonus,       // Add flat value
    Chance,          // Percentage chance
    Duration         // Time in hours/minutes
}

public class SkillDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public SkillCategory Category { get; set; }
    public SkillEffectType EffectType { get; set; }

    // Effect values
    public double EffectPerLevel { get; set; }
    public int MaxLevel { get; set; } = 5;

    // For operation-specific skills
    public string? TargetOperationId { get; set; }

    // For tier-specific skills (1 = Pickpocket/CarTheft/Burglary, 2 = Speakeasy/Casino)
    public int? TargetTier { get; set; }
}

public class SkillState
{
    public string Id { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
}
