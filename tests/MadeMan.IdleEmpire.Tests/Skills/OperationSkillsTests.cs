namespace MadeMan.IdleEmpire.Tests.Skills;

/// <summary>
/// Tests for operation-specific skills (QuickHands, ChopShop, InsideMan, HappyHour).
/// v1.1: All 12% per level (was 15-25%)
/// </summary>
public class OperationSkillsTests : SkillTestBase
{
    [Theory]
    [InlineData("quick_hands", "pickpocket", 1, 1.12)]
    [InlineData("quick_hands", "pickpocket", 3, 1.36)]
    [InlineData("quick_hands", "pickpocket", 5, 1.60)]
    [InlineData("chop_shop", "cartheft", 1, 1.12)]
    [InlineData("chop_shop", "cartheft", 5, 1.60)]
    [InlineData("inside_man", "burglary", 1, 1.12)]
    [InlineData("inside_man", "burglary", 5, 1.60)]
    [InlineData("happy_hour", "speakeasy", 1, 1.12)]
    [InlineData("happy_hour", "speakeasy", 5, 1.60)]
    public void OperationSkill_BoostsCorrectOperation(
        string skillId, string operationId, int level, double expected)
    {
        // Arrange
        AddSkill(skillId, level);

        // Act
        var multiplier = SkillService.GetOperationMultiplier(operationId);

        // Assert
        AssertApproximately(expected, multiplier);
    }

    [Fact]
    public void QuickHands_OnlyAffectsPickpocket()
    {
        // Arrange
        AddSkill("quick_hands", 5);

        // Act
        var pickpocket = SkillService.GetOperationMultiplier("pickpocket");
        var carTheft = SkillService.GetOperationMultiplier("cartheft");
        var burglary = SkillService.GetOperationMultiplier("burglary");
        var speakeasy = SkillService.GetOperationMultiplier("speakeasy");
        var casino = SkillService.GetOperationMultiplier("casino");

        // Assert
        AssertApproximately(1.60, pickpocket);
        Assert.Equal(1.0, carTheft);
        Assert.Equal(1.0, burglary);
        Assert.Equal(1.0, speakeasy);
        Assert.Equal(1.0, casino);
    }

    [Fact]
    public void ChopShop_OnlyAffectsCarTheft()
    {
        // Arrange
        AddSkill("chop_shop", 5);

        // Act
        var pickpocket = SkillService.GetOperationMultiplier("pickpocket");
        var carTheft = SkillService.GetOperationMultiplier("cartheft");
        var burglary = SkillService.GetOperationMultiplier("burglary");

        // Assert
        Assert.Equal(1.0, pickpocket);
        AssertApproximately(1.60, carTheft);
        Assert.Equal(1.0, burglary);
    }

    [Fact]
    public void InsideMan_OnlyAffectsBurglary()
    {
        // Arrange
        AddSkill("inside_man", 5);

        // Act
        var pickpocket = SkillService.GetOperationMultiplier("pickpocket");
        var burglary = SkillService.GetOperationMultiplier("burglary");
        var speakeasy = SkillService.GetOperationMultiplier("speakeasy");

        // Assert
        Assert.Equal(1.0, pickpocket);
        AssertApproximately(1.60, burglary);
        Assert.Equal(1.0, speakeasy);
    }

    [Fact]
    public void HappyHour_OnlyAffectsSpeakeasy()
    {
        // Arrange
        AddSkill("happy_hour", 5);

        // Act
        var burglary = SkillService.GetOperationMultiplier("burglary");
        var speakeasy = SkillService.GetOperationMultiplier("speakeasy");
        var casino = SkillService.GetOperationMultiplier("casino");

        // Assert
        Assert.Equal(1.0, burglary);
        AssertApproximately(1.60, speakeasy);
        Assert.Equal(1.0, casino);
    }

    [Fact]
    public void AllOperationSkills_Independent()
    {
        // Arrange - add all operation skills at different levels
        AddSkill("quick_hands", 3);   // +36%
        AddSkill("chop_shop", 2);     // +24%
        AddSkill("inside_man", 1);    // +12%
        AddSkill("happy_hour", 4);    // +48%

        // Act & Assert - each only affects its operation
        AssertApproximately(1.36, SkillService.GetOperationMultiplier("pickpocket"));
        AssertApproximately(1.24, SkillService.GetOperationMultiplier("cartheft"));
        AssertApproximately(1.12, SkillService.GetOperationMultiplier("burglary"));
        AssertApproximately(1.48, SkillService.GetOperationMultiplier("speakeasy"));
        Assert.Equal(1.0, SkillService.GetOperationMultiplier("casino"));
    }

    [Fact]
    public void Casino_HasNoSpecificSkill()
    {
        // Arrange - add all available operation skills
        AddSkill("quick_hands", 5);
        AddSkill("chop_shop", 5);
        AddSkill("inside_man", 5);
        AddSkill("happy_hour", 5);

        // Act
        var casinoMult = SkillService.GetOperationMultiplier("casino");

        // Assert - Casino has no specific skill in v1.1
        Assert.Equal(1.0, casinoMult);
    }

    [Fact]
    public void OperationSkill_NoEffectWithoutSkill()
    {
        // No skills added

        // Act
        var pickpocket = SkillService.GetOperationMultiplier("pickpocket");
        var carTheft = SkillService.GetOperationMultiplier("cartheft");

        // Assert
        Assert.Equal(1.0, pickpocket);
        Assert.Equal(1.0, carTheft);
    }

    [Fact]
    public void OperationSkill_CombinesWithTierSkill()
    {
        // Arrange
        AddSkill("quick_hands", 2);    // +24% to pickpocket
        AddSkill("street_smarts", 2);  // +16% to tier 1

        // Act
        var pickpocketMult = SkillService.GetOperationMultiplier("pickpocket");

        // Assert - Multipliers should combine: 1.24 * 1.16 = 1.4384
        AssertApproximately(1.4384, pickpocketMult, 0.01);
    }
}
