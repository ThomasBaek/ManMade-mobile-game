namespace MadeMan.IdleEmpire.Models;

public class Operation
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Economics - Interval Based
    public double Interval { get; set; } = 1.0;   // Seconds between yields
    public double BaseYield { get; set; }          // Payout per cycle at level 1
    public double UnlockCost { get; set; }
    public double BaseUpgradeCost { get; set; }   // For free operations that still have upgrade costs
    public double UpgradeMultiplier { get; set; } = 1.5;

    // Calculate yield for given level
    public double GetYield(int level, double prestigeBonus)
    {
        if (level <= 0) return 0;
        return BaseYield * level * prestigeBonus;
    }

    // Calculate effective income/second for display
    public double GetIncomePerSecond(int level, double prestigeBonus)
    {
        if (level <= 0) return 0;
        return GetYield(level, prestigeBonus) / Interval;
    }

    // Upgrade cost calculation
    public double GetUpgradeCost(int currentLevel)
    {
        if (currentLevel <= 0) return UnlockCost;
        var baseCost = BaseUpgradeCost > 0 ? BaseUpgradeCost : UnlockCost;
        return baseCost * Math.Pow(UpgradeMultiplier, currentLevel);
    }
}
