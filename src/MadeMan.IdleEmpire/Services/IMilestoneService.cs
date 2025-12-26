using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public interface IMilestoneService
{
    event Action<List<SkillDefinition>>? OnMilestoneReached;

    bool CheckForMilestone();
    double GetNextMilestoneThreshold();
    double GetMilestoneProgress();
    List<SkillDefinition> GetSelectionPool();
    void CompleteMilestone(string selectedSkillId);
    void Reset();
}
