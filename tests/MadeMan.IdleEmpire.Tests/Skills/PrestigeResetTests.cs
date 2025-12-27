using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Tests.Skills;

/// <summary>
/// Tests for prestige reset behavior.
/// </summary>
public class PrestigeResetTests : SkillTestBase
{
    [Fact]
    public void ResetSkills_ClearsAllSkills()
    {
        // Arrange
        AddSkill("cash_flow", 3);
        AddSkill("street_smarts", 2);
        AddSkill("night_owl", 4);

        // Act
        SkillService.ResetSkills();

        // Assert
        Assert.Empty(GameState.Skills);
        Assert.Equal(0, SkillService.GetActiveSkillCount());
    }

    [Fact]
    public void ResetSkills_ResetsMilestoneCount()
    {
        // Arrange
        AddSkill("cash_flow", 3);
        GameState.MilestoneCount = 5;

        // Act
        SkillService.ResetSkills();

        // Assert
        Assert.Equal(0, GameState.MilestoneCount);
    }

    [Fact]
    public void PrestigeSkillValues_CalculatedBeforeReset()
    {
        // Arrange
        AddSkill("old_connections", 5);  // $375 starting cash
        AddSkill("reputation", 3);       // +12% prestige bonus

        // Act - Get values BEFORE reset
        var startingCash = SkillService.GetStartingCashBonus();
        var prestigeMult = SkillService.GetPrestigeBonusMultiplier();

        // Reset skills
        SkillService.ResetSkills();

        // Get values AFTER reset
        var startingCashAfter = SkillService.GetStartingCashBonus();
        var prestigeMultAfter = SkillService.GetPrestigeBonusMultiplier();

        // Assert - Values were calculated before reset
        Assert.Equal(375, startingCash);
        AssertApproximately(1.12, prestigeMult);

        // After reset, values are back to base
        Assert.Equal(0, startingCashAfter);
        Assert.Equal(1.0, prestigeMultAfter);
    }

    [Fact]
    public void ResetSkills_MultipliersBecomeNeutral()
    {
        // Arrange
        AddSkill("cash_flow", 5);
        AddSkill("fast_learner", 5);
        AddSkill("night_owl", 5);

        // Act
        SkillService.ResetSkills();

        // Assert - All multipliers back to neutral
        Assert.Equal(1.0, SkillService.GetTotalIncomeMultiplier());
        Assert.Equal(1.0, SkillService.GetUpgradeCostMultiplier());
        Assert.Equal(0.0, SkillService.GetOfflineEfficiencyBonus());
    }

    [Fact]
    public void ResetSkills_OperationMultipliersBecomeNeutral()
    {
        // Arrange
        AddSkill("quick_hands", 5);
        AddSkill("chop_shop", 5);
        AddSkill("street_smarts", 5);

        // Act
        SkillService.ResetSkills();

        // Assert - All operation multipliers back to 1.0
        Assert.Equal(1.0, SkillService.GetOperationMultiplier("pickpocket"));
        Assert.Equal(1.0, SkillService.GetOperationMultiplier("cartheft"));
        Assert.Equal(1.0, SkillService.GetOperationMultiplier("burglary"));
    }

    [Fact]
    public void AfterReset_CanAddNewSkills()
    {
        // Arrange - Fill all slots
        AddSkill("cash_flow", 1);
        AddSkill("street_smarts", 1);
        AddSkill("business_acumen", 1);
        AddSkill("quick_hands", 1);
        AddSkill("night_owl", 1);

        Assert.False(SkillService.CanAddNewSkill());

        // Act
        SkillService.ResetSkills();

        // Assert - Can add skills again
        Assert.True(SkillService.CanAddNewSkill());
    }

    [Fact]
    public void ResetSkills_UpdatesSessionStart()
    {
        // Arrange
        var oldSessionStart = GameState.SessionStartUtc;
        System.Threading.Thread.Sleep(10); // Ensure time passes

        // Act
        SkillService.ResetSkills();

        // Assert - Session start is updated
        Assert.True(GameState.SessionStartUtc >= oldSessionStart);
    }
}
