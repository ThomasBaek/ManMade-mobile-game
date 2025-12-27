using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Tests.Skills;

/// <summary>
/// Tests for CashFlow skill - global income multiplier.
/// v1.1: 3% per level (was 4%)
/// </summary>
public class CashFlowTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 1.03)]  // Level 1: +3%
    [InlineData(2, 1.06)]  // Level 2: +6%
    [InlineData(3, 1.09)]  // Level 3: +9%
    [InlineData(4, 1.12)]  // Level 4: +12%
    [InlineData(5, 1.15)]  // Level 5: +15% (max)
    public void CashFlow_AppliesCorrectMultiplier(int level, double expected)
    {
        // Arrange
        AddSkill("cash_flow", level);

        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();

        // Assert
        AssertApproximately(expected, multiplier);
    }

    [Fact]
    public void CashFlow_NoEffectAtLevelZero()
    {
        // No skill added

        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();

        // Assert
        Assert.Equal(1.0, multiplier);
    }
}

/// <summary>
/// Tests for tier-specific skills (StreetSmarts, BusinessAcumen).
/// v1.1: 8% per level (was 10%/12%)
/// </summary>
public class TierSkillsTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 1.08)]  // Level 1: +8%
    [InlineData(2, 1.16)]  // Level 2: +16%
    [InlineData(3, 1.24)]  // Level 3: +24%
    [InlineData(5, 1.40)]  // Level 5: +40% (max)
    public void StreetSmarts_BoostsTier1Operations(int level, double expected)
    {
        // Arrange
        AddSkill("street_smarts", level);

        // Act - Tier 1 operations
        var pickpocketMult = SkillService.GetOperationMultiplier("pickpocket");
        var carTheftMult = SkillService.GetOperationMultiplier("cartheft");
        var burglaryMult = SkillService.GetOperationMultiplier("burglary");

        // Assert - All Tier 1 boosted
        AssertApproximately(expected, pickpocketMult);
        AssertApproximately(expected, carTheftMult);
        AssertApproximately(expected, burglaryMult);
    }

    [Fact]
    public void StreetSmarts_DoesNotAffectTier2()
    {
        // Arrange
        AddSkill("street_smarts", 5);

        // Act - Tier 2 operations
        var speakeasyMult = SkillService.GetOperationMultiplier("speakeasy");
        var casinoMult = SkillService.GetOperationMultiplier("casino");

        // Assert - Tier 2 unaffected
        Assert.Equal(1.0, speakeasyMult);
        Assert.Equal(1.0, casinoMult);
    }

    [Theory]
    [InlineData(1, 1.08)]  // Level 1: +8%
    [InlineData(3, 1.24)]  // Level 3: +24%
    [InlineData(5, 1.40)]  // Level 5: +40% (max)
    public void BusinessAcumen_BoostsTier2Operations(int level, double expected)
    {
        // Arrange
        AddSkill("business_acumen", level);

        // Act - Tier 2 operations
        var speakeasyMult = SkillService.GetOperationMultiplier("speakeasy");
        var casinoMult = SkillService.GetOperationMultiplier("casino");

        // Assert - Tier 2 boosted
        AssertApproximately(expected, speakeasyMult);
        AssertApproximately(expected, casinoMult);
    }

    [Fact]
    public void BusinessAcumen_DoesNotAffectTier1()
    {
        // Arrange
        AddSkill("business_acumen", 5);

        // Act - Tier 1 operations
        var pickpocketMult = SkillService.GetOperationMultiplier("pickpocket");
        var carTheftMult = SkillService.GetOperationMultiplier("cartheft");

        // Assert - Tier 1 unaffected
        Assert.Equal(1.0, pickpocketMult);
        Assert.Equal(1.0, carTheftMult);
    }
}

/// <summary>
/// Tests for LuckyBreak skill - critical hit chance.
/// Note: RollLuckyBreak uses hardcoded 4%/level in SkillCalculator.
/// </summary>
public class LuckyBreakTests : SkillTestBase
{
    [Fact]
    public void LuckyBreak_NoEffectWithoutSkill()
    {
        // Arrange - no skill added
        var random = new Random(42);

        // Act - try many times
        var triggered = false;
        for (int i = 0; i < 1000; i++)
        {
            if (SkillService.RollLuckyBreak(random))
            {
                triggered = true;
                break;
            }
        }

        // Assert - should never trigger
        Assert.False(triggered);
    }

    [Fact]
    public void LuckyBreak_CanTriggerWithSkill()
    {
        // Arrange
        AddSkill("lucky_break", 5); // Max level
        var random = new Random(42);

        // Act - try many times (15-20% chance at level 5)
        var triggeredCount = 0;
        var trials = 1000;
        for (int i = 0; i < trials; i++)
        {
            if (SkillService.RollLuckyBreak(random))
            {
                triggeredCount++;
            }
        }

        // Assert - should trigger sometimes (statistically)
        Assert.True(triggeredCount > 0, "LuckyBreak should trigger at least once in 1000 tries");
        Assert.True(triggeredCount < trials, "LuckyBreak should not trigger every time");
    }

    [Fact]
    public void LuckyBreak_HigherLevelTriggersMoreOften()
    {
        // Arrange
        var random1 = new Random(42);
        var random5 = new Random(42);
        var trials = 10000;

        // Act - Level 1
        AddSkill("lucky_break", 1);
        var level1Count = 0;
        for (int i = 0; i < trials; i++)
        {
            if (SkillService.RollLuckyBreak(random1))
                level1Count++;
        }

        // Reset and test Level 5
        ClearSkills();
        AddSkill("lucky_break", 5);
        var level5Count = 0;
        for (int i = 0; i < trials; i++)
        {
            if (SkillService.RollLuckyBreak(random5))
                level5Count++;
        }

        // Assert - Level 5 should trigger more often
        Assert.True(level5Count > level1Count,
            $"Level 5 ({level5Count}) should trigger more than level 1 ({level1Count})");
    }
}

/// <summary>
/// Tests for TheSkim skill - cashback on purchases.
/// v1.1: 4% per level (was 5%)
/// </summary>
public class TheSkimTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 4.0)]   // Level 1: 4%
    [InlineData(2, 8.0)]   // Level 2: 8%
    [InlineData(3, 12.0)]  // Level 3: 12%
    [InlineData(5, 20.0)]  // Level 5: 20% (max)
    public void TheSkim_ReturnsCorrectCashbackPercent(int level, double expectedPercent)
    {
        // Arrange
        AddSkill("the_skim", level);

        // Act
        var cashbackPercent = SkillService.GetCashbackPercent();

        // Assert
        AssertApproximately(expectedPercent, cashbackPercent);
    }

    [Fact]
    public void TheSkim_NoEffectWithoutSkill()
    {
        // No skill added

        // Act
        var cashbackPercent = SkillService.GetCashbackPercent();

        // Assert
        Assert.Equal(0.0, cashbackPercent);
    }
}

/// <summary>
/// Tests for CompoundInterest skill - income grows over session time.
/// v1.1: 2% per level per 5-min interval (was 3%)
/// Note: SkillCalculator may have hardcoded value.
/// </summary>
public class CompoundInterestTests : SkillTestBase
{
    [Fact]
    public void CompoundInterest_NoEffectWithoutSkill()
    {
        // No skill added
        var sessionStart = DateTime.UtcNow.AddMinutes(-30);

        // Act
        var multiplier = SkillService.GetCompoundInterestMultiplier(sessionStart);

        // Assert
        Assert.Equal(1.0, multiplier);
    }

    [Fact]
    public void CompoundInterest_NoEffectAtSessionStart()
    {
        // Arrange
        AddSkill("compound_interest", 5);
        var sessionStart = DateTime.UtcNow; // Just started

        // Act
        var multiplier = SkillService.GetCompoundInterestMultiplier(sessionStart);

        // Assert - No intervals passed yet
        Assert.Equal(1.0, multiplier);
    }

    [Fact]
    public void CompoundInterest_IncreasesOverTime()
    {
        // Arrange
        AddSkill("compound_interest", 3);
        var sessionStart = DateTime.UtcNow.AddMinutes(-15); // 3 intervals

        // Act
        var multiplier = SkillService.GetCompoundInterestMultiplier(sessionStart);

        // Assert - Should be > 1.0 after time passes
        Assert.True(multiplier > 1.0, $"Multiplier should be > 1.0 but was {multiplier}");
    }

    [Fact]
    public void CompoundInterest_CapsAtMaxIntervals()
    {
        // Arrange
        AddSkill("compound_interest", 5);
        var shortSession = DateTime.UtcNow.AddMinutes(-30);  // 6 intervals (max)
        var longSession = DateTime.UtcNow.AddMinutes(-60);   // Would be 12 intervals

        // Act
        var shortMultiplier = SkillService.GetCompoundInterestMultiplier(shortSession);
        var longMultiplier = SkillService.GetCompoundInterestMultiplier(longSession);

        // Assert - Both should cap at same value
        Assert.Equal(shortMultiplier, longMultiplier);
    }

    [Fact]
    public void CompoundInterest_HigherLevelGivesMoreBonus()
    {
        // Arrange
        var sessionStart = DateTime.UtcNow.AddMinutes(-15);

        // Level 1
        AddSkill("compound_interest", 1);
        var level1Mult = SkillService.GetCompoundInterestMultiplier(sessionStart);

        // Level 5
        ClearSkills();
        AddSkill("compound_interest", 5);
        var level5Mult = SkillService.GetCompoundInterestMultiplier(sessionStart);

        // Assert
        Assert.True(level5Mult > level1Mult,
            $"Level 5 ({level5Mult}) should be higher than level 1 ({level1Mult})");
    }
}
