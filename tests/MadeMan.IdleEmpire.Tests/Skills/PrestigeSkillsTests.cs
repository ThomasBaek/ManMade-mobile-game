namespace MadeMan.IdleEmpire.Tests.Skills;

/// <summary>
/// Tests for OldConnections skill - starting cash after prestige.
/// v1.1: $75 per level (was $100)
/// </summary>
public class OldConnectionsTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 0)]     // No skill: $0
    [InlineData(1, 75)]    // Level 1: $75
    [InlineData(2, 150)]   // Level 2: $150
    [InlineData(3, 225)]   // Level 3: $225
    [InlineData(4, 300)]   // Level 4: $300
    [InlineData(5, 375)]   // Level 5: $375 max
    public void OldConnections_ReturnsCorrectStartingCash(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("old_connections", level);

        // Act
        var startCash = SkillService.GetStartingCashBonus();

        // Assert
        Assert.Equal(expected, startCash);
    }

    [Fact]
    public void OldConnections_NoEffectWithoutSkill()
    {
        // No skill added

        // Act
        var startCash = SkillService.GetStartingCashBonus();

        // Assert
        Assert.Equal(0.0, startCash);
    }

    [Fact]
    public void OldConnections_ValueCalculatedBeforeReset()
    {
        // Arrange
        AddSkill("old_connections", 5);

        // Act - Get starting cash before reset
        var startCashBefore = SkillService.GetStartingCashBonus();

        // Reset skills
        SkillService.ResetSkills();

        // Get starting cash after reset
        var startCashAfter = SkillService.GetStartingCashBonus();

        // Assert
        Assert.Equal(375, startCashBefore);  // Calculated before reset
        Assert.Equal(0, startCashAfter);     // No skill after reset
        Assert.Empty(GameState.Skills);       // Skills are cleared
    }
}

/// <summary>
/// Tests for Reputation skill - prestige bonus multiplier.
/// v1.1: 4% per level (was 6%)
/// Note: Returns multiplier (1.04 at level 1), not rate.
/// </summary>
public class ReputationTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 1.00)]  // No skill: 1x
    [InlineData(1, 1.04)]  // Level 1: +4%
    [InlineData(2, 1.08)]  // Level 2: +8%
    [InlineData(3, 1.12)]  // Level 3: +12%
    [InlineData(4, 1.16)]  // Level 4: +16%
    [InlineData(5, 1.20)]  // Level 5: +20% max
    public void Reputation_ReturnsCorrectMultiplier(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("reputation", level);

        // Act
        var multiplier = SkillService.GetPrestigeBonusMultiplier();

        // Assert
        AssertApproximately(expected, multiplier);
    }

    [Fact]
    public void Reputation_NoEffectWithoutSkill()
    {
        // No skill added

        // Act
        var multiplier = SkillService.GetPrestigeBonusMultiplier();

        // Assert
        Assert.Equal(1.0, multiplier);
    }
}

/// <summary>
/// Tests for FastLearner skill - upgrade cost reduction.
/// v1.1: -3% per level (was -4%)
/// </summary>
public class FastLearnerTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 1.00)]  // No skill: 100%
    [InlineData(1, 0.97)]  // Level 1: -3%
    [InlineData(2, 0.94)]  // Level 2: -6%
    [InlineData(3, 0.91)]  // Level 3: -9%
    [InlineData(4, 0.88)]  // Level 4: -12%
    [InlineData(5, 0.85)]  // Level 5: -15% max
    public void FastLearner_ReducesUpgradeCosts(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("fast_learner", level);

        // Act
        var multiplier = SkillService.GetUpgradeCostMultiplier();

        // Assert
        AssertApproximately(expected, multiplier);
    }

    [Fact]
    public void FastLearner_NoEffectWithoutSkill()
    {
        // No skill added

        // Act
        var multiplier = SkillService.GetUpgradeCostMultiplier();

        // Assert
        Assert.Equal(1.0, multiplier);
    }

    [Fact]
    public void FastLearner_MultiplierNeverNegative()
    {
        // Arrange - even at max level
        AddSkill("fast_learner", 5);

        // Act
        var multiplier = SkillService.GetUpgradeCostMultiplier();

        // Assert
        Assert.True(multiplier > 0);
        Assert.True(multiplier < 1.0);
    }
}

/// <summary>
/// Tests for EarlyBird skill - unlock cost reduction.
/// v1.1: -4% per level (was -5%)
/// </summary>
public class EarlyBirdTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 1.00)]  // No skill: 100%
    [InlineData(1, 0.96)]  // Level 1: -4%
    [InlineData(2, 0.92)]  // Level 2: -8%
    [InlineData(3, 0.88)]  // Level 3: -12%
    [InlineData(4, 0.84)]  // Level 4: -16%
    [InlineData(5, 0.80)]  // Level 5: -20% max
    public void EarlyBird_ReducesUnlockCosts(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("early_bird", level);

        // Act
        var multiplier = SkillService.GetUnlockCostMultiplier();

        // Assert
        AssertApproximately(expected, multiplier);
    }

    [Fact]
    public void EarlyBird_NoEffectWithoutSkill()
    {
        // No skill added

        // Act
        var multiplier = SkillService.GetUnlockCostMultiplier();

        // Assert
        Assert.Equal(1.0, multiplier);
    }

    [Fact]
    public void EarlyBird_MultiplierNeverNegative()
    {
        // Arrange - even at max level
        AddSkill("early_bird", 5);

        // Act
        var multiplier = SkillService.GetUnlockCostMultiplier();

        // Assert
        Assert.True(multiplier > 0);
        Assert.True(multiplier < 1.0);
    }
}
    