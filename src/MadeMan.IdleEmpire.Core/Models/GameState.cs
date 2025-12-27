namespace MadeMan.IdleEmpire.Models;

public class GameState
{
    // Core resources
    public double Cash { get; set; } = 0;
    public double TotalEarned { get; set; } = 0;

    // Prestige
    public int PrestigeCount { get; set; } = 0;
    public double PrestigeBonus { get; set; } = 1.0;

    // Operations
    public List<OperationState> Operations { get; set; } = new();

    // Skills
    public List<SkillState> Skills { get; set; } = new();
    public int MilestoneCount { get; set; } = 0;

    // Session tracking (for Compound Interest skill)
    public DateTime SessionStartUtc { get; set; } = DateTime.UtcNow;

    // Meta
    public DateTime LastPlayedUtc { get; set; } = DateTime.UtcNow;
}

public class OperationState
{
    public string Id { get; set; } = string.Empty;
    public int Level { get; set; } = 0; // 0 = locked
    public double AccumulatedTime { get; set; } = 0; // Progress toward next yield
}
