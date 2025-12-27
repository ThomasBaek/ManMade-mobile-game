using System.Text.Json.Serialization;

namespace MadeMan.IdleEmpire.Models;

/// <summary>
/// Lifetime statistics that persist across prestige resets.
/// Saved separately from GameState.
/// </summary>
public class GameStats
{
    // Earnings
    public double LifetimeEarned { get; set; }
    public double LifetimeSpent { get; set; }
    public double HighestCash { get; set; }

    // Gameplay
    public int TotalUpgrades { get; set; }
    public int TotalUnlocks { get; set; }
    public int TotalTaps { get; set; }

    // Offline
    public double OfflineEarnings { get; set; }

    // Time - stored as ticks for JSON serialization
    public long TimePlayedTicks { get; set; }

    [JsonIgnore]
    public TimeSpan TimePlayed
    {
        get => TimeSpan.FromTicks(TimePlayedTicks);
        set => TimePlayedTicks = value.Ticks;
    }

    public DateTime FirstPlayedUtc { get; set; } = DateTime.UtcNow;
}
