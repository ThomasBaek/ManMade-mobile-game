using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;

namespace MadeMan.IdleEmpire.Tests;

/// <summary>
/// Base class for skill tests providing common setup and helper methods.
/// </summary>
public abstract class SkillTestBase
{
    protected GameState GameState { get; }
    protected GameStateHolder StateHolder { get; }
    protected ISkillService SkillService { get; }

    protected SkillTestBase()
    {
        GameState = new GameState();
        StateHolder = new GameStateHolder { State = GameState };
        SkillService = new SkillService(StateHolder);
    }

    /// <summary>
    /// Adds or upgrades a skill to the specified level.
    /// </summary>
    protected void AddSkill(string skillId, int level = 1)
    {
        var existing = GameState.Skills.FirstOrDefault(s => s.Id == skillId);
        if (existing != null)
        {
            existing.Level = level;
        }
        else
        {
            GameState.Skills.Add(new SkillState { Id = skillId, Level = level });
        }
    }

    /// <summary>
    /// Clears all skills from the game state.
    /// </summary>
    protected void ClearSkills()
    {
        GameState.Skills.Clear();
    }

    /// <summary>
    /// Gets the skill definition for a given skill ID.
    /// </summary>
    protected SkillDefinition? GetSkillDefinition(string skillId)
    {
        return SkillConfig.Skills.FirstOrDefault(s => s.Id == skillId);
    }

    /// <summary>
    /// Asserts that a double value equals expected within tolerance.
    /// </summary>
    protected static void AssertApproximately(double expected, double actual, double tolerance = 0.01)
    {
        Assert.True(
            Math.Abs(expected - actual) <= tolerance,
            $"Expected {expected} but got {actual} (tolerance: {tolerance})");
    }
}
