using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public class GameEngine : IGameEngine
{
    private readonly SaveManager _saveManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly Random _random = new();

    // Lazy resolution to break circular dependency
    private ISkillService? _cachedSkillService;
    private IMilestoneService? _cachedMilestoneService;
    private ISkillService SkillService =>
        _cachedSkillService ??= _serviceProvider.GetRequiredService<ISkillService>();
    private IMilestoneService MilestoneService =>
        _cachedMilestoneService ??= _serviceProvider.GetRequiredService<IMilestoneService>();

    public GameState State { get; private set; } = new();

    public double IncomePerSecond
    {
        get
        {
            double total = 0;
            foreach (var op in GameConfig.Operations)
            {
                var opState = GetOperationState(op.Id);
                if (opState != null && opState.Level > 0)
                {
                    total += CalculateOperationIncome(opState, op);
                }
            }
            // Add passive income from skills
            total += SkillService.GetPassiveIncomePerSecond();
            return total;
        }
    }

    public GameEngine(SaveManager saveManager, IServiceProvider serviceProvider)
    {
        _saveManager = saveManager;
        _serviceProvider = serviceProvider;
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

        // Check for milestone
        MilestoneService.CheckForMilestone();
    }

    private double CalculateOperationIncome(OperationState opState, Operation config)
    {
        if (opState.Level == 0) return 0;

        // Base income with prestige bonus
        var baseIncome = config.GetIncome(opState.Level, State.PrestigeBonus);

        // Apply skill multipliers
        var skillMultiplier = SkillService.GetTotalIncomeMultiplier();
        var operationMultiplier = SkillService.GetOperationMultiplier(opState.Id);
        var compoundMultiplier = SkillService.GetCompoundInterestMultiplier(State.SessionStartUtc);

        var totalIncome = baseIncome * skillMultiplier * operationMultiplier * compoundMultiplier;

        // Lucky Break check (2x income chance)
        if (SkillService.RollLuckyBreak(_random))
        {
            totalIncome *= 2.0;
        }

        return totalIncome;
    }

    public bool CanUnlock(string operationId)
    {
        var op = GetOperation(operationId);
        var opState = GetOperationState(operationId);
        if (op == null || opState == null) return false;
        return opState.Level == 0 && State.Cash >= GetUnlockCost(op);
    }

    public bool CanUpgrade(string operationId)
    {
        var op = GetOperation(operationId);
        var opState = GetOperationState(operationId);
        if (op == null || opState == null) return false;
        if (opState.Level == 0) return false;
        return State.Cash >= GetUpgradeCost(op, opState.Level);
    }

    private double GetUnlockCost(Operation op)
    {
        // Apply skill reduction (Early Bird)
        return op.UnlockCost * SkillService.GetUnlockCostMultiplier();
    }

    private double GetUpgradeCost(Operation op, int currentLevel)
    {
        var baseCost = op.GetUpgradeCost(currentLevel);
        // Apply skill reduction (Fast Learner)
        return baseCost * SkillService.GetUpgradeCostMultiplier();
    }

    public void UnlockOrUpgrade(string operationId)
    {
        var op = GetOperation(operationId);
        var opState = GetOperationState(operationId);
        if (op == null || opState == null) return;

        double cost;
        if (opState.Level == 0)
        {
            cost = GetUnlockCost(op);
        }
        else
        {
            cost = GetUpgradeCost(op, opState.Level);
        }

        if (State.Cash >= cost)
        {
            State.Cash -= cost;
            opState.Level++;

            // Cashback from The Skim skill
            var cashbackPercent = SkillService.GetCashbackPercent();
            if (cashbackPercent > 0)
            {
                var cashback = cost * (cashbackPercent / 100.0);
                State.Cash += cashback;
            }
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

        // Apply Reputation skill bonus to prestige multiplier
        var prestigeMultiplier = SkillService.GetPrestigeBonusMultiplier();
        State.PrestigeBonus += GameConfig.PrestigeBonusPerReset * prestigeMultiplier;

        // Reset skills (they don't persist through prestige)
        SkillService.ResetSkills();
        MilestoneService.Reset();

        // Starting cash from Old Connections skill
        State.Cash = SkillService.GetStartingCashBonus();
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

        // Max hours with skill bonus (Extended Shift)
        var maxHours = GameConfig.MaxOfflineHours + SkillService.GetOfflineMaxHoursBonus();
        var hours = Math.Min(offlineTime.TotalHours, maxHours);

        if (hours > 0.01)
        {
            // Base efficiency + skill bonus (Night Owl)
            var efficiency = GameConfig.OfflineEfficiency + SkillService.GetOfflineEfficiencyBonus();
            efficiency = Math.Min(efficiency, 1.0); // Cap at 100%

            // Calculate base offline income (use base income without skill effects to avoid double-dipping)
            double baseIncomePerSecond = 0;
            foreach (var op in GameConfig.Operations)
            {
                var opState = GetOperationState(op.Id);
                if (opState != null && opState.Level > 0)
                {
                    baseIncomePerSecond += op.GetIncome(opState.Level, State.PrestigeBonus);
                }
            }

            double earnings = baseIncomePerSecond * hours * 3600 * efficiency;

            // Apply offline earnings multiplier (Godfather's Cut)
            earnings *= SkillService.GetOfflineEarningsMultiplier();

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
