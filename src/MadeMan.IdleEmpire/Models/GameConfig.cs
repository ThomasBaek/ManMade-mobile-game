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
            Description = "Snup lommerne på turisterne",
            BaseIncome = 1.0,
            UnlockCost = 0,
            UpgradeMultiplier = 1.4
        },
        new Operation
        {
            Id = "cartheft",
            Name = "Car Theft",
            Icon = "icon_car.png",
            Description = "Stjæl biler, sælg dele",
            BaseIncome = 4.0,
            UnlockCost = 50,
            UpgradeMultiplier = 1.5
        },
        new Operation
        {
            Id = "burglary",
            Name = "Burglary",
            Icon = "icon_burglary.png",
            Description = "Bryd ind i de riges hjem",
            BaseIncome = 15.0,
            UnlockCost = 250,
            UpgradeMultiplier = 1.6
        },
        new Operation
        {
            Id = "speakeasy",
            Name = "Speakeasy",
            Icon = "icon_speakeasy.png",
            Description = "Din første illegale bar",
            BaseIncome = 50.0,
            UnlockCost = 1000,
            UpgradeMultiplier = 1.8
        },
        new Operation
        {
            Id = "casino",
            Name = "Underground Casino",
            Icon = "icon_casino.png",
            Description = "Huset vinder altid",
            BaseIncome = 200.0,
            UnlockCost = 5000,
            UpgradeMultiplier = 2.0
        }
    };

    // === PRESTIGE ===
    public const double PrestigeThreshold = 10_000;
    public const double PrestigeBonusPerReset = 0.25;

    // === OFFLINE ===
    public const double MaxOfflineHours = 4;
    public const double OfflineEfficiency = 0.5;

    // === TIMING ===
    public const int TicksPerSecond = 10;
    public const int AutoSaveIntervalSeconds = 30;
}
