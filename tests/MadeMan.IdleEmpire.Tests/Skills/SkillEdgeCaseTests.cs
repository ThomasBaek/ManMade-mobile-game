using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Tests.Skills;

/// <summary>
/// Tests for edge cases and error conditions.
/// </summary>
public class SkillEdgeCaseTests : SkillTestBase
{
    [Fact]
    public void NoSkills_ReturnsBaseValues()
    {
        // No skills added

        // Act
        var incomeMultiplier = SkillService.GetTotalIncomeMultiplier();
        var upgradeCostMultiplier = SkillService.GetUpgradeCostMultiplier();
        var unlockCostMultiplier = SkillService.GetUnlockCostMultiplier();
        var offlineBonus = SkillService.GetOfflineEfficiencyBonus();
        var passiveIncome = SkillService.GetPassiveIncomePerSecond();

        // Assert - All base values
        Assert.Equal(1.0, incomeMultiplier);
        Assert.Equal(1.0, upgradeCostMultiplier);
        Assert.Equal(1.0, unlockCostMultiplier);
        Assert.Equal(0.0, offlineBonus);
        Assert.Equal(0.0, passiveIncome);
    }

    [Fact]
    public void InvalidOperationId_ReturnsNeutralMultiplier()
    {
        // Arrange
        AddSkill("quick_hands", 5);

        // Act
        var multiplier = SkillService.GetOperationMultiplier("nonexistent_operation");

        // Assert
        Assert.Equal(1.0, multiplier);
    }

    [Fact]
    public void ZeroLevel_TreatedAsNoSkill()
    {
        // Arrange - Skill with level 0 (shouldn't happen but be safe)
        GameState.Skills.Add(new SkillState { Id = "cash_flow", Level = 0 });

        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();
        var hasSkill = SkillService.HasSkill("cash_flow");

        // Assert
        Assert.Equal(1.0, multiplier);
        Assert.True(hasSkill);  // It exists but has no effect
    }

    [Fact]
    public void NegativeLevel_TreatedAsNoSkill()
    {
        // Arrange - Negative level (shouldn't happen)
        GameState.Skills.Add(new SkillState { Id = "cash_flow", Level = -1 });

        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();

        // Assert
        Assert.Equal(1.0, multiplier);
    }

    [Fact]
    public void GetSkillLevel_ReturnsZeroForMissingSkill()
    {
        // No skills added

        // Act
        var level = SkillService.GetSkillLevel("cash_flow");

        // Assert
        Assert.Equal(0, level);
    }

    [Fact]
    public void HasSkill_ReturnsFalseForMissingSkill()
    {
        // No skills added

        // Act
        var hasSkill = SkillService.HasSkill("cash_flow");

        // Assert
        Assert.False(hasSkill);
    }

    [Fact]
    public void IsSkillMaxed_ReturnsFalseForMissingSkill()
    {
        // No skills added

        // Act
        var isMaxed = SkillService.IsSkillMaxed("cash_flow");

        // Assert
        Assert.False(isMaxed);
    }

    [Fact]
    public void OfflineBonus_CombinedWithBaseStaysUnderOne()
    {
        // Arrange - Max Night Owl (+30%)
        AddSkill("night_owl", 5);

        // Act - Base 40% + 30% = 70%
        var bonus = SkillService.GetOfflineEfficiencyBonus();

        // Assert - Bonus is 0.30, total efficiency would be 0.70
        AssertApproximately(0.30, bonus);
        Assert.True(0.40 + bonus <= 1.0);  // Combined still <= 100%
    }

    [Fact]
    public void AllSkillsAtMaxLevel_StillWorks()
    {
        // Arrange - Max out 5 skills
        AddSkill("cash_flow", 5);
        AddSkill("street_smarts", 5);
        AddSkill("quick_hands", 5);
        AddSkill("night_owl", 5);
        AddSkill("fast_learner", 5);

        // Act
        var income = SkillService.GetTotalIncomeMultiplier();
        var pickpocket = SkillService.GetOperationMultiplier("pickpocket");
        var upgrade = SkillService.GetUpgradeCostMultiplier();

        // Assert - All provide max bonuses
        AssertApproximately(1.15, income);       // +15% from cash_flow
        Assert.True(pickpocket > 2.0);           // +40% tier + 60% operation = 2.24
        AssertApproximately(0.85, upgrade);      // -15% from fast_learner
    }

    [Fact]
    public void MultipleSkillsOfSameId_FirstOneUsed()
    {
        // Arrange - Same skill added twice (shouldn't happen in normal use)
        GameState.Skills.Add(new SkillState { Id = "cash_flow", Level = 2 });
        GameState.Skills.Add(new SkillState { Id = "cash_flow", Level = 4 });

        // Act
        var level = SkillService.GetSkillLevel("cash_flow");

        // Assert - FirstOrDefault returns first one
        Assert.Equal(2, level);
    }

    [Fact]
    public void SkillConfig_Has18Skills()
    {
        // Assert
        Assert.Equal(18, SkillConfig.Skills.Length);
    }

    [Fact]
    public void SkillConfig_Has10Milestones()
    {
        // Assert
        Assert.Equal(10, SkillConfig.MilestoneThresholds.Length);
    }

    [Fact]
    public void SkillConfig_MaxSkillsIs5()
    {
        // Assert
        Assert.Equal(5, SkillConfig.MaxSkills);
    }

    [Fact]
    public void SkillConfig_MaxLevelIs5()
    {
        // Assert
        Assert.Equal(5, SkillConfig.MaxSkillLevel);
    }

    [Fact]
    public void CompoundInterest_RecentSessionStart_ReturnsNeutral()
    {
        // Arrange - Session just started (no intervals passed yet)
        AddSkill("compound_interest", 5);
        var recentStart = DateTime.UtcNow;

        // Act
        var multiplier = SkillService.GetCompoundInterestMultiplier(recentStart);

        // Assert - No intervals passed = no bonus
        Assert.Equal(1.0, multiplier);
    }

    [Fact]
    public void LuckyBreak_WithoutSkill_NeverTriggers()
    {
        // No skill added
        var random = new Random(12345);

        // Act - Try many times
        var triggered = false;
        for (int i = 0; i < 10000; i++)
        {
            if (SkillService.RollLuckyBreak(random))
            {
                triggered = true;
                break;
            }
        }

        // Assert
        Assert.False(triggered);
    }
}
