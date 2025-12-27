namespace MadeMan.IdleEmpire.Tests.Skills;

/// <summary>
/// Tests for NightOwl skill - increases offline efficiency.
/// v1.1: 6% per level (was 8%)
/// Note: GetOfflineEfficiencyBonus returns the BONUS (not total efficiency).
/// </summary>
public class NightOwlTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 0.00)]  // No skill: 0% bonus
    [InlineData(1, 0.06)]  // Level 1: +6%
    [InlineData(2, 0.12)]  // Level 2: +12%
    [InlineData(3, 0.18)]  // Level 3: +18%
    [InlineData(4, 0.24)]  // Level 4: +24%
    [InlineData(5, 0.30)]  // Level 5: +30% (max)
    public void NightOwl_ReturnsCorrectEfficiencyBonus(int level, double expectedBonus)
    {
        // Arrange
        if (level > 0) AddSkill("night_owl", level);

        // Act
        var bonus = SkillService.GetOfflineEfficiencyBonus();

        // Assert
        AssertApproximately(expectedBonus, bonus);
    }

    [Fact]
    public void NightOwl_NoEffectWithoutSkill()
    {
        // No skill added

        // Act
        var bonus = SkillService.GetOfflineEfficiencyBonus();

        // Assert
        Assert.Equal(0.0, bonus);
    }

    [Fact]
    public void NightOwl_MaxBonusAt30Percent()
    {
        // Arrange
        AddSkill("night_owl", 5);

        // Act
        var bonus = SkillService.GetOfflineEfficiencyBonus();

        // Assert - Max is 30%
        AssertApproximately(0.30, bonus);
    }
}

/// <summary>
/// Tests for ExtendedShift skill - extends max offline time.
/// v1.1: 0.5h per level (was 1h)
/// Note: GetOfflineMaxHoursBonus returns the BONUS hours (not total).
/// </summary>
public class ExtendedShiftTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 0.0)]   // No skill: 0h bonus
    [InlineData(1, 0.5)]   // Level 1: +0.5h
    [InlineData(2, 1.0)]   // Level 2: +1.0h
    [InlineData(3, 1.5)]   // Level 3: +1.5h
    [InlineData(4, 2.0)]   // Level 4: +2.0h
    [InlineData(5, 2.5)]   // Level 5: +2.5h max
    public void ExtendedShift_ReturnsCorrectHoursBonus(int level, double expectedBonus)
    {
        // Arrange
        if (level > 0) AddSkill("extended_shift", level);

        // Act
        var bonus = SkillService.GetOfflineMaxHoursBonus();

        // Assert
        AssertApproximately(expectedBonus, bonus, 0.1);
    }

    [Fact]
    public void ExtendedShift_NoEffectWithoutSkill()
    {
        // No skill added

        // Act
        var bonus = SkillService.GetOfflineMaxHoursBonus();

        // Assert
        Assert.Equal(0.0, bonus);
    }
}

/// <summary>
/// Tests for PassiveIncome skill - baseline income.
/// v1.1: $0.5/s per level (was $1/s)
/// </summary>
public class PassiveIncomeTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 0.0)]   // No skill: $0/s
    [InlineData(1, 0.5)]   // Level 1: $0.5/s
    [InlineData(2, 1.0)]   // Level 2: $1.0/s
    [InlineData(3, 1.5)]   // Level 3: $1.5/s
    [InlineData(4, 2.0)]   // Level 4: $2.0/s
    [InlineData(5, 2.5)]   // Level 5: $2.5/s max
    public void PassiveIncome_AddsBaselineIncome(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("passive_income", level);

        // Act
        var passive = SkillService.GetPassiveIncomePerSecond();

        // Assert
        AssertApproximately(expected, passive);
    }

    [Fact]
    public void PassiveIncome_NoEffectWithoutSkill()
    {
        // No skill added

        // Act
        var passive = SkillService.GetPassiveIncomePerSecond();

        // Assert
        Assert.Equal(0.0, passive);
    }
}

/// <summary>
/// Tests for GodfathersCut skill - offline earnings multiplier.
/// v1.1: 6% per level (was 8%)
/// </summary>
public class GodfathersCutTests : SkillTestBase
{
    [Theory]
    [InlineData(0, 1.00)]  // No skill: 1x
    [InlineData(1, 1.06)]  // Level 1: +6%
    [InlineData(2, 1.12)]  // Level 2: +12%
    [InlineData(3, 1.18)]  // Level 3: +18%
    [InlineData(4, 1.24)]  // Level 4: +24%
    [InlineData(5, 1.30)]  // Level 5: +30% max
    public void GodfathersCut_ReturnsCorrectMultiplier(int level, double expected)
    {
        // Arrange
        if (level > 0) AddSkill("godfathers_cut", level);

        // Act
        var multiplier = SkillService.GetOfflineEarningsMultiplier();

        // Assert
        AssertApproximately(expected, multiplier);
    }

    [Fact]
    public void GodfathersCut_NoEffectWithoutSkill()
    {
        // No skill added

        // Act
        var multiplier = SkillService.GetOfflineEarningsMultiplier();

        // Assert
        Assert.Equal(1.0, multiplier);
    }
}
