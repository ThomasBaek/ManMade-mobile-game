namespace MadeMan.IdleEmpire.Models;

public static class GameConfig
{
    // === OPERATIONS ===
    public static readonly Operation[] Operations = new[]
    {
        new Operation
        {
            Id = "pickpocket",
            Name = "Pickpocketing",
            Icon = "icon_pickpocket.png",
            Description = "Lift wallets from unsuspecting tourists",
            BaseIncome = 1.0,
            UnlockCost = 0,
            UpgradeMultiplier = 1.4
        },
        new Operation
        {
            Id = "cartheft",
            Name = "Car Theft",
            Icon = "icon_car.png",
            Description = "Steal cars, sell the parts",
            BaseIncome = 4.0,
            UnlockCost = 50,
            UpgradeMultiplier = 1.5
        },
        new Operation
        {
            Id = "burglary",
            Name = "Burglary",
            Icon = "icon_burglary.png",
            Description = "Break into the homes of the wealthy",
            BaseIncome = 15.0,
            UnlockCost = 250,
            UpgradeMultiplier = 1.6
        },
        new Operation
        {
            Id = "speakeasy",
            Name = "Speakeasy",
            Icon = "icon_speakeasy.png",
            Description = "Your first illegal watering hole",
            BaseIncome = 50.0,
            UnlockCost = 1000,
            UpgradeMultiplier = 1.8
        },
        new Operation
        {
            Id = "casino",
            Name = "Underground Casino",
            Icon = "icon_casino.png",
            Description = "The house always wins",
            BaseIncome = 200.0,
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
