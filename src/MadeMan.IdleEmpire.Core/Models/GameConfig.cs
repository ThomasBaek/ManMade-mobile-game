namespace MadeMan.IdleEmpire.Models;

public static class GameConfig
{
    // === OPERATIONS (Interval-Based Yields) ===
    // v1.1 Rebalance: Halved income, increased unlock costs, standardized upgrade multiplier
    public static readonly Operation[] Operations = new[]
    {
        new Operation
        {
            Id = "pickpocket",
            Name = "Pickpocketing",
            Icon = "icon_pickpocket.png",
            Description = "Lift wallets from unsuspecting tourists",
            Interval = 1.0,       // 1 second
            BaseYield = 0.5,      // $0.5 per cycle = $0.5/s effective (v1.1: was $1/s)
            UnlockCost = 0,       // FREE starter operation
            BaseUpgradeCost = 5,  // Upgrades start at $5
            UpgradeMultiplier = 1.6  // v1.1: standardized
        },
        new Operation
        {
            Id = "cartheft",
            Name = "Car Theft",
            Icon = "icon_car.png",
            Description = "Steal cars, sell the parts",
            Interval = 5.0,       // 5 seconds
            BaseYield = 10.0,     // $10 per cycle = $2/s effective (v1.1: was $4/s)
            UnlockCost = 75,      // v1.1: was $50
            UpgradeMultiplier = 1.6  // v1.1: standardized
        },
        new Operation
        {
            Id = "burglary",
            Name = "Burglary",
            Icon = "icon_burglary.png",
            Description = "Break into the homes of the wealthy",
            Interval = 20.0,      // 20 seconds
            BaseYield = 160.0,    // $160 per cycle = $8/s effective (v1.1: was $15/s)
            UnlockCost = 400,     // v1.1: was $250
            UpgradeMultiplier = 1.6
        },
        new Operation
        {
            Id = "speakeasy",
            Name = "Speakeasy",
            Icon = "icon_speakeasy.png",
            Description = "Your first illegal watering hole",
            Interval = 60.0,      // 60 seconds
            BaseYield = 1800.0,   // $1800 per cycle = $30/s effective (v1.1: was $50/s)
            UnlockCost = 2000,    // v1.1: was $1000
            UpgradeMultiplier = 1.6  // v1.1: standardized
        },
        new Operation
        {
            Id = "casino",
            Name = "Underground Casino",
            Icon = "icon_casino.png",
            Description = "The house always wins",
            Interval = 120.0,     // 120 seconds
            BaseYield = 12000.0,  // $12000 per cycle = $100/s effective (v1.1: was $200/s)
            UnlockCost = 10000,   // v1.1: was $5000
            UpgradeMultiplier = 1.6  // v1.1: standardized
        }
    };

    // === PRESTIGE ===
    // DEBUG: Set to 250 for testing, production should be 25_000
    public const double PrestigeThreshold = 250;
    public const double PrestigeBonusPerReset = 0.20;  // v1.1: was 0.25

    // === OFFLINE ===
    public const double MaxOfflineHours = 4;
    public const double OfflineEfficiency = 0.4;  // v1.1: was 0.5 (50% -> 40%)

    // === TIMING ===
    public const int TicksPerSecond = 2;  // Reduced from 10 - smoother, less frantic
    public const int AutoSaveIntervalSeconds = 30;
}
