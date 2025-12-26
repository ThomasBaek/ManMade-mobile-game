using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public interface IGameEngine
{
    GameState State { get; }
    double IncomePerSecond { get; }

    // Offline earnings info (for Welcome Back modal)
    double LastOfflineEarnings { get; }
    TimeSpan LastOfflineTime { get; }
    double LastOfflineEfficiency { get; }

    void Initialize();
    void Tick(double deltaSeconds);
    bool CanUnlock(string operationId);
    bool CanUpgrade(string operationId);
    void UnlockOrUpgrade(string operationId);
    bool CanPrestige();
    void DoPrestige();
    void ClearOfflineEarningsDisplay();
}
