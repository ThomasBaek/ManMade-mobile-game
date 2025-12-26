namespace MadeMan.IdleEmpire.Models;

public static class GameConfig
{
    // === OPERATIONS (Interval-Based Yields) ===
    public static readonly Operation[] Operations = new[]
    {
        new Operation
        {
            Id = "pickpocket",
            Name = "Pickpocketing",
            Icon = "icon_pickpocket.png",
            Description = "Lift wallets from unsuspecting tourists",
            Interval = 1.0,       // 1 second
            BaseYield = 1.0,      // $1 per cycle = $1/s effective
            UnlockCost = 0,       // FREE starter operation
            BaseUpgradeCost = 5,  // Upgrades start at $5
            UpgradeMultiplier = 1.4
        },
        new Operation
        {
            Id = "cartheft",
            Name = "Car Theft",
            Icon = "icon_car.png",
            Description = "Steal cars, sell the parts",
            Interval = 5.0,       // 5 seconds
            BaseYield = 20.0,     // $20 per cycle = $4/s effective
            UnlockCost = 50,
            UpgradeMultiplier = 1.5
        },
        new Operation
        {
            Id = "burglary",
            Name = "Burglary",
            Icon = "icon_burglary.png",
            Description = "Break into the homes of the wealthy",
            Interval = 20.0,      // 20 seconds
            BaseYield = 300.0,    // $300 per cycle = $15/s effective
            UnlockCost = 250,
            UpgradeMultiplier = 1.6
        },
        new Operation
        {
            Id = "speakeasy",
            Name = "Speakeasy",
            Icon = "icon_speakeasy.png",
            Description = "Your first illegal watering hole",
            Interval = 60.0,      // 60 seconds
            BaseYield = 3000.0,   // $3000 per cycle = $50/s effective
            UnlockCost = 1000,
            UpgradeMultiplier = 1.8
        },
        new Operation
        {
            Id = "casino",
            Name = "Underground Casino",
            Icon = "icon_casino.png",
            Description = "The house always wins",
            Interval = 120.0,     // 120 seconds
            BaseYield = 24000.0,  // $24000 per cycle = $200/s effective
            UnlockCost = 5000,
            UpgradeMultiplier = 2.0
        }
    };

    // === PRESTIGE ===
#if DEBUG
    public const double PrestigeThreshold = 100;  // Lower for testing
#else
    public const double PrestigeThreshold = 10_000;
#endif
    public const double PrestigeBonusPerReset = 0.25;

    // === OFFLINE ===
    public const double MaxOfflineHours = 4;
    public const double OfflineEfficiency = 0.5;

    // === TIMING ===
    public const int TicksPerSecond = 2;  // Reduced from 10 - smoother, less frantic
    public const int AutoSaveIntervalSeconds = 30;
}
