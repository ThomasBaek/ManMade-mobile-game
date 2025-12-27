namespace MadeMan.IdleEmpire.Tests;

/// <summary>
/// Basic smoke tests to verify test infrastructure is working.
/// </summary>
public class SmokeTests : SkillTestBase
{
    [Fact]
    public void GameState_InitializesCorrectly()
    {
        Assert.NotNull(GameState);
        Assert.Equal(0, GameState.Cash);
        Assert.Empty(GameState.Skills);
    }

    [Fact]
    public void SkillService_InitializesCorrectly()
    {
        Assert.NotNull(SkillService);
        Assert.Equal(0, SkillService.GetActiveSkillCount());
    }

    [Fact]
    public void AddSkill_AddsToGameState()
    {
        AddSkill("cash_flow", 1);

        Assert.Single(GameState.Skills);
        Assert.Equal("cash_flow", GameState.Skills[0].Id);
        Assert.Equal(1, GameState.Skills[0].Level);
    }

    [Fact]
    public void AddSkill_UpdatesExistingLevel()
    {
        AddSkill("cash_flow", 1);
        AddSkill("cash_flow", 3);

        Assert.Single(GameState.Skills);
        Assert.Equal(3, GameState.Skills[0].Level);
    }

    [Fact]
    public void ClearSkills_RemovesAllSkills()
    {
        AddSkill("cash_flow", 1);
        AddSkill("street_smarts", 2);

        ClearSkills();

        Assert.Empty(GameState.Skills);
    }

    [Fact]
    public void SkillConfig_HasExpectedSkillCount()
    {
        // Should have 18 skills total
        Assert.Equal(18, MadeMan.IdleEmpire.Models.SkillConfig.Skills.Length);
    }

    [Fact]
    public void SkillConfig_HasExpectedMilestoneCount()
    {
        // Should have 10 milestones
        Assert.Equal(10, MadeMan.IdleEmpire.Models.SkillConfig.MilestoneThresholds.Length);
    }
}
