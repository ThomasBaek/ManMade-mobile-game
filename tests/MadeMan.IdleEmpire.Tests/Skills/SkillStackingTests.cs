using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Tests.Skills;

/// <summary>
/// Tests for income skill stacking behavior.
/// </summary>
public class IncomeStackingTests : SkillTestBase
{
    [Fact]
    public void MultipleIncomeSkills_StackMultiplicatively()
    {
        // Arrange
        AddSkill("cash_flow", 3);        // +9% global
        AddSkill("street_smarts", 2);    // +16% to tier 1
        AddSkill("quick_hands", 2);      // +24% to pickpocket

        // Act
        var globalMult = SkillService.GetTotalIncomeMultiplier();      // 1.09
        var operationMult = SkillService.GetOperationMultiplier("pickpocket"); // 1.24 * 1.16 = 1.4384

        // Assert - Each component works
        AssertApproximately(1.09, globalMult);
        Assert.True(operationMult > 1.3, $"Operation mult should be > 1.3 but was {operationMult}");
    }

    [Fact]
    public void TierSkills_DontAffectWrongTiers()
    {
        // Arrange
        AddSkill("street_smarts", 5);    // +40% to tier 1
        AddSkill("business_acumen", 5);  // +40% to tier 2

        // Act - Tier 1 operations
        var pickpocketMult = SkillService.GetOperationMultiplier("pickpocket");
        var carTheftMult = SkillService.GetOperationMultiplier("cartheft");
        var burglaryMult = SkillService.GetOperationMultiplier("burglary");

        // Act - Tier 2 operations
        var speakeasyMult = SkillService.GetOperationMultiplier("speakeasy");
        var casinoMult = SkillService.GetOperationMultiplier("casino");

        // Assert - Each tier gets only its own boost
        AssertApproximately(1.40, pickpocketMult);
        AssertApproximately(1.40, carTheftMult);
        AssertApproximately(1.40, burglaryMult);
        AssertApproximately(1.40, speakeasyMult);
        AssertApproximately(1.40, casinoMult);
    }

    [Fact]
    public void GlobalAndOperationSkills_StackCorrectly()
    {
        // Arrange
        AddSkill("cash_flow", 5);        // +15% global
        AddSkill("quick_hands", 5);      // +60% pickpocket only

        // Act
        var globalMult = SkillService.GetTotalIncomeMultiplier();
        var pickpocketOpMult = SkillService.GetOperationMultiplier("pickpocket");
        var carTheftOpMult = SkillService.GetOperationMultiplier("cartheft");

        // Assert
        AssertApproximately(1.15, globalMult);
        AssertApproximately(1.60, pickpocketOpMult);  // Has operation skill
        Assert.Equal(1.0, carTheftOpMult);             // No operation skill
    }

    [Fact]
    public void OperationAndTierSkills_CombineMultiplicatively()
    {
        // Arrange
        AddSkill("quick_hands", 3);      // +36% to pickpocket
        AddSkill("street_smarts", 3);    // +24% to tier 1

        // Act
        var pickpocketMult = SkillService.GetOperationMultiplier("pickpocket");

        // Assert - 1.36 * 1.24 = 1.6864
        AssertApproximately(1.6864, pickpocketMult, 0.01);
    }
}

/// <summary>
/// Tests for cost reduction skill stacking.
/// </summary>
public class CostStackingTests : SkillTestBase
{
    [Fact]
    public void CostReductionSkills_AreIndependent()
    {
        // Arrange
        AddSkill("fast_learner", 3);   // -9% upgrade cost
        AddSkill("early_bird", 3);     // -12% unlock cost

        // Act
        var upgradeMult = SkillService.GetUpgradeCostMultiplier();
        var unlockMult = SkillService.GetUnlockCostMultiplier();

        // Assert - Each affects only its type
        AssertApproximately(0.91, upgradeMult);
        AssertApproximately(0.88, unlockMult);
    }

    [Fact]
    public void MaxCostReductions_StillPositive()
    {
        // Arrange - Max both cost reduction skills
        AddSkill("fast_learner", 5);   // -15% upgrade cost
        AddSkill("early_bird", 5);     // -20% unlock cost

        // Act
        var upgradeMult = SkillService.GetUpgradeCostMultiplier();
        var unlockMult = SkillService.GetUnlockCostMultiplier();

        // Assert - Multipliers should be > 0
        Assert.True(upgradeMult > 0);
        Assert.True(unlockMult > 0);
        AssertApproximately(0.85, upgradeMult);
        AssertApproximately(0.80, unlockMult);
    }
}

/// <summary>
/// Tests for skill limit enforcement.
/// </summary>
public class SkillLimitTests : SkillTestBase
{
    [Fact]
    public void MaxSkillLimit_EnforcedAtFive()
    {
        // Arrange - Add 5 different skills
        AddSkill("cash_flow", 1);
        AddSkill("street_smarts", 1);
        AddSkill("business_acumen", 1);
        AddSkill("quick_hands", 1);
        AddSkill("night_owl", 1);

        // Act
        var canAddMore = SkillService.CanAddNewSkill();

        // Assert
        Assert.False(canAddMore);
        Assert.Equal(5, SkillService.GetActiveSkillCount());
    }

    [Fact]
    public void UnderLimit_CanAddNewSkill()
    {
        // Arrange - Add 3 skills
        AddSkill("cash_flow", 1);
        AddSkill("street_smarts", 1);
        AddSkill("business_acumen", 1);

        // Act
        var canAddMore = SkillService.CanAddNewSkill();

        // Assert
        Assert.True(canAddMore);
        Assert.Equal(3, SkillService.GetActiveSkillCount());
    }

    [Fact]
    public void MaxLevelSkill_IsMaxed()
    {
        // Arrange
        AddSkill("cash_flow", 5);  // Max level

        // Act
        var isMaxed = SkillService.IsSkillMaxed("cash_flow");

        // Assert
        Assert.True(isMaxed);
    }

    [Fact]
    public void NonMaxLevelSkill_NotMaxed()
    {
        // Arrange
        AddSkill("cash_flow", 3);

        // Act
        var isMaxed = SkillService.IsSkillMaxed("cash_flow");

        // Assert
        Assert.False(isMaxed);
    }

    [Fact]
    public void AddOrUpgradeSkill_AddsNewSkill()
    {
        // Arrange - No skills

        // Act
        SkillService.AddOrUpgradeSkill("cash_flow");

        // Assert
        Assert.Equal(1, SkillService.GetSkillLevel("cash_flow"));
        Assert.Single(GameState.Skills);
    }

    [Fact]
    public void AddOrUpgradeSkill_UpgradesExisting()
    {
        // Arrange
        AddSkill("cash_flow", 2);

        // Act
        SkillService.AddOrUpgradeSkill("cash_flow");

        // Assert
        Assert.Equal(3, SkillService.GetSkillLevel("cash_flow"));
        Assert.Single(GameState.Skills);
    }

    [Fact]
    public void AddOrUpgradeSkill_CapsAtMaxLevel()
    {
        // Arrange
        AddSkill("cash_flow", 5);  // Already max

        // Act
        SkillService.AddOrUpgradeSkill("cash_flow");

        // Assert - Still max
        Assert.Equal(5, SkillService.GetSkillLevel("cash_flow"));
    }

    [Fact]
    public void AddOrUpgradeSkill_RespectsSkillLimit()
    {
        // Arrange - Fill all 5 slots
        AddSkill("cash_flow", 1);
        AddSkill("street_smarts", 1);
        AddSkill("business_acumen", 1);
        AddSkill("quick_hands", 1);
        AddSkill("night_owl", 1);

        // Act - Try to add a 6th skill
        SkillService.AddOrUpgradeSkill("chop_shop");

        // Assert - Should not add
        Assert.Equal(5, SkillService.GetActiveSkillCount());
        Assert.Equal(0, SkillService.GetSkillLevel("chop_shop"));
    }
}
