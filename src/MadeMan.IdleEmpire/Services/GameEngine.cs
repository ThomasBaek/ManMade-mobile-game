using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Services;

public class GameEngine : IGameEngine
{
    private readonly GameStateHolder _stateHolder;
    private readonly SaveManager _saveManager;
    private readonly ISkillService _skillService;
    private readonly IMilestoneService _milestoneService;

    // Cached lookups for O(1) access instead of O(n) LINQ queries
    private readonly Dictionary<string, Operation> _operationLookup;

    // Income cache for display (invalidated on state changes)
    private double _cachedBaseIncome;
    private bool _incomeCacheDirty = true;
    private bool _isInitialized;

    public GameState State => _stateHolder.State;

    public double IncomePerSecond
    {
        get
        {
            // Use cached value if available (for display purposes)
            if (!_incomeCacheDirty)
            {
                return _cachedBaseIncome;
            }

            // Recalculate base income
            double total = 0;
            foreach (var op in GameConfig.Operations)
            {
                var opState = GetOperationState(op.Id);
                if (opState != null && opState.Level > 0)
                {
                    total += CalculateBaseOperationIncome(opState, op);
                }
            }
            // Add passive income from skills
            total += _skillService.GetPassiveIncomePerSecond();

            _cachedBaseIncome = total;
            _incomeCacheDirty = false;
            return total;
        }
    }

    /// <summary>
    /// Calculates actual income for a tick, including Lucky Break chance.
    /// </summary>
    private double CalculateActualIncomeForTick(double deltaSeconds)
    {
        double total = 0;
        foreach (var op in GameConfig.Operations)
        {
            var opState = GetOperationState(op.Id);
            if (opState != null && opState.Level > 0)
            {
                var income = CalculateBaseOperationIncome(opState, op);

                // Lucky Break check (2x income chance) - applied per tick
                if (_skillService.RollLuckyBreak(Random.Shared))
                {
                    income *= 2.0;
                }

                total += income;
            }
        }
        total += _skillService.GetPassiveIncomePerSecond();
        return total * deltaSeconds;
    }

    public GameEngine(
        GameStateHolder stateHolder,
        SaveManager saveManager,
        ISkillService skillService,
        IMilestoneService milestoneService)
    {
        _stateHolder = stateHolder;
        _saveManager = saveManager;
        _skillService = skillService;
        _milestoneService = milestoneService;

        // Build O(1) lookup dictionary for operations
        _operationLookup = GameConfig.Operations.ToDictionary(o => o.Id);
    }

    public void Initialize()
    {
        // Only initialize once - don't reload from disk on tab switches
        if (_isInitialized) return;

        _stateHolder.State = _saveManager.Load() ?? CreateNewGame();
        CalculateOfflineEarnings();
        _isInitialized = true;
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
        // Use actual income calculation (includes Lucky Break chance)
        double earned = CalculateActualIncomeForTick(deltaSeconds);
        State.Cash += earned;
        State.TotalEarned += earned;

        // Check for milestone
        _milestoneService.CheckForMilestone();
    }

    /// <summary>
    /// Calculates base income for an operation (without Lucky Break).
    /// Used for display and as base for actual income calculation.
    /// </summary>
    private double CalculateBaseOperationIncome(OperationState opState, Operation config)
    {
        if (opState.Level == 0) return 0;

        // Base income with prestige bonus
        var baseIncome = config.GetIncome(opState.Level, State.PrestigeBonus);

        // Apply skill multipliers
        var skillMultiplier = _skillService.GetTotalIncomeMultiplier();
        var operationMultiplier = _skillService.GetOperationMultiplier(opState.Id);
        var compoundMultiplier = _skillService.GetCompoundInterestMultiplier(State.SessionStartUtc);

        return baseIncome * skillMultiplier * operationMultiplier * compoundMultiplier;
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
        return op.UnlockCost * _skillService.GetUnlockCostMultiplier();
    }

    private double GetUpgradeCost(Operation op, int currentLevel)
    {
        var baseCost = op.GetUpgradeCost(currentLevel);
        // Apply skill reduction (Fast Learner)
        return baseCost * _skillService.GetUpgradeCostMultiplier();
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
            InvalidateIncomeCache(); // Operation level changed

            // Cashback from The Skim skill
            var cashbackPercent = _skillService.GetCashbackPercent();
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
        var prestigeMultiplier = _skillService.GetPrestigeBonusMultiplier();
        State.PrestigeBonus += GameConfig.PrestigeBonusPerReset * prestigeMultiplier;

        // Reset skills (they don't persist through prestige)
        _skillService.ResetSkills();
        _milestoneService.Reset();

        // Starting cash from Old Connections skill
        State.Cash = _skillService.GetStartingCashBonus();
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

        InvalidateIncomeCache(); // Skills and operations reset
    }

    private void CalculateOfflineEarnings()
    {
        var offlineTime = DateTime.UtcNow - State.LastPlayedUtc;

        // Max hours with skill bonus (Extended Shift)
        var maxHours = GameConfig.MaxOfflineHours + _skillService.GetOfflineMaxHoursBonus();
        var hours = Math.Min(offlineTime.TotalHours, maxHours);

        if (hours > 0.01)
        {
            // Base efficiency + skill bonus (Night Owl)
            var efficiency = GameConfig.OfflineEfficiency + _skillService.GetOfflineEfficiencyBonus();
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
            earnings *= _skillService.GetOfflineEarningsMultiplier();

            State.Cash += earnings;
            State.TotalEarned += earnings;
        }

        State.LastPlayedUtc = DateTime.UtcNow;
    }

    private Operation? GetOperation(string id)
    {
        return _operationLookup.TryGetValue(id, out var op) ? op : null;
    }

    private OperationState? GetOperationState(string id)
    {
        // Still O(n) but n is small (5 operations)
        // Could be optimized with a dictionary if needed
        return State.Operations.FirstOrDefault(o => o.Id == id);
    }

    /// <summary>
    /// Invalidates the income cache. Call when operations or skills change.
    /// </summary>
    private void InvalidateIncomeCache()
    {
        _incomeCacheDirty = true;
    }
}
