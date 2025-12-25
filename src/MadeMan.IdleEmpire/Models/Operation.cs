namespace MadeMan.IdleEmpire.Models;

public class Operation
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Economics
    public double BaseIncome { get; set; }
    public double UnlockCost { get; set; }
    public double UpgradeMultiplier { get; set; } = 1.5;

    // Calculated
    public double GetUpgradeCost(int currentLevel)
    {
        if (currentLevel <= 0) return UnlockCost;
        return UnlockCost * Math.Pow(UpgradeMultiplier, currentLevel);
    }

    public double GetIncome(int level, double prestigeBonus)
    {
        if (level <= 0) return 0;
        return BaseIncome * level * prestigeBonus;
    }
}
