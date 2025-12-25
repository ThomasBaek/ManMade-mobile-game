using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public interface ISkillService
{
    // State queries
    int GetSkillLevel(string skillId);
    bool HasSkill(string skillId);
    bool CanAddNewSkill();
    bool IsSkillMaxed(string skillId);
    int GetActiveSkillCount();
    List<SkillState> GetActiveSkills();

    // Mutations
    void AddOrUpgradeSkill(string skillId);
    void ResetSkills();

    // Effect calculations
    double GetTotalIncomeMultiplier();
    double GetOperationMultiplier(string operationId);
    double GetUpgradeCostMultiplier();
    double GetUnlockCostMultiplier();
    double GetCashbackPercent();
    double GetPassiveIncomePerSecond();
    double GetOfflineEfficiencyBonus();
    double GetOfflineMaxHoursBonus();
    double GetOfflineEarningsMultiplier();
    double GetPrestigeBonusMultiplier();
    double GetStartingCashBonus();
    double GetCompoundInterestMultiplier(DateTime sessionStart);
    bool RollLuckyBreak(Random random);
}
