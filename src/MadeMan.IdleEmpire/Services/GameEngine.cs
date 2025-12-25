using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public class GameEngine : IGameEngine
{
    private readonly SaveManager _saveManager;

    public GameState State { get; private set; } = new();

    public double IncomePerSecond
    {
        get
        {
            double total = 0;
            foreach (var op in GameConfig.Operations)
            {
                var state = GetOperationState(op.Id);
                if (state != null)
                {
                    total += op.GetIncome(state.Level, State.PrestigeBonus);
                }
            }
            return total;
        }
    }

    public GameEngine(SaveManager saveManager)
    {
        _saveManager = saveManager;
    }

    public void Initialize()
    {
        State = _saveManager.Load() ?? CreateNewGame();
        CalculateOfflineEarnings();
    }

    private GameState CreateNewGame()
    {
        var state = new GameState();

        foreach (var op in GameConfig.Operations)
        {
            state.Operations.Add(new OperationState
            {
                Id = op.Id,
                Level = op.UnlockCost == 0 ? 1 : 0
            });
        }

        return state;
    }

    public void Tick(double deltaSeconds)
    {
        double earned = IncomePerSecond * deltaSeconds;
        State.Cash += earned;
        State.TotalEarned += earned;
    }

    public bool CanUnlock(string operationId)
    {
        var op = GetOperation(operationId);
        var state = GetOperationState(operationId);
        if (op == null || state == null) return false;
        return state.Level == 0 && State.Cash >= op.UnlockCost;
    }

    public bool CanUpgrade(string operationId)
    {
        var op = GetOperation(operationId);
        var state = GetOperationState(operationId);
        if (op == null || state == null) return false;
        if (state.Level == 0) return false;
        return State.Cash >= op.GetUpgradeCost(state.Level);
    }

    public void UnlockOrUpgrade(string operationId)
    {
        var op = GetOperation(operationId);
        var state = GetOperationState(operationId);
        if (op == null || state == null) return;

        double cost;
        if (state.Level == 0)
        {
            cost = op.UnlockCost;
        }
        else
        {
            cost = op.GetUpgradeCost(state.Level);
        }

        if (State.Cash >= cost)
        {
            State.Cash -= cost;
            state.Level++;
        }
    }

    public bool CanPrestige()
    {
        return State.TotalEarned >= GameConfig.PrestigeThreshold;
    }

    public void DoPrestige()
    {
        if (!CanPrestige()) return;

        State.PrestigeCount++;
        State.PrestigeBonus += GameConfig.PrestigeBonusPerReset;

        State.Cash = 0;
        State.TotalEarned = 0;

        State.Operations.Clear();
        foreach (var op in GameConfig.Operations)
        {
            State.Operations.Add(new OperationState
            {
                Id = op.Id,
                Level = op.UnlockCost == 0 ? 1 : 0
            });
        }
    }

    private void CalculateOfflineEarnings()
    {
        var offlineTime = DateTime.UtcNow - State.LastPlayedUtc;
        var hours = Math.Min(offlineTime.TotalHours, GameConfig.MaxOfflineHours);

        if (hours > 0.01)
        {
            double earnings = IncomePerSecond * hours * 3600 * GameConfig.OfflineEfficiency;
            State.Cash += earnings;
            State.TotalEarned += earnings;
        }

        State.LastPlayedUtc = DateTime.UtcNow;
    }

    private Operation? GetOperation(string id)
    {
        return GameConfig.Operations.FirstOrDefault(o => o.Id == id);
    }

    private OperationState? GetOperationState(string id)
    {
        return State.Operations.FirstOrDefault(o => o.Id == id);
    }
}
