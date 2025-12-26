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

    // Offline earnings info (for Welcome Back modal)
    public double LastOfflineEarnings { get; private set; }
    public TimeSpan LastOfflineTime { get; private set; }
    public double LastOfflineEfficiency { get; private set; }

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

            // Recalculate effective income per second (yield/interval with multipliers)
            double total = 0;
            foreach (var op in GameConfig.Operations)
            {
                var opState = GetOperationState(op.Id);
                if (opState != null && opState.Level > 0)
                {
                    total += CalculateEffectiveIncomePerSecond(opState, op);
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
    /// Calculates yield for a single operation cycle, including all multipliers and Lucky Break.
    /// </summary>
    private double CalculateOperationYield(OperationState opState, Operation config)
    {
        var baseYield = config.GetYield(opState.Level, State.PrestigeBonus);

        // Apply skill multipliers
        var skillMultiplier = _skillService.GetTotalIncomeMultiplier();
        var operationMultiplier = _skillService.GetOperationMultiplier(opState.Id);
        var compoundMultiplier = _skillService.GetCompoundInterestMultiplier(State.SessionStartUtc);

        var finalYield = baseYield * skillMultiplier * operationMultiplier * compoundMultiplier;

        // Lucky Break check (2x yield chance) - per yield, not per tick
        if (_skillService.RollLuckyBreak(Random.Shared))
        {
            finalYield *= 2.0;
        }

        return finalYield;
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
                Level = op.UnlockCost == 0 ? 1 : 0,
                AccumulatedTime = 0
            });
        }

        return state;
    }

    public void Tick(double deltaSeconds)
    {
        // Interval-based tick: accumulate time and yield when interval is reached
        foreach (var op in GameConfig.Operations)
        {
            var opState = GetOperationState(op.Id);
            if (opState == null || opState.Level <= 0) continue;

            // Accumulate time
            opState.AccumulatedTime += deltaSeconds;

            // Check for yield(s) - handle multiple yields if delta is large
            while (opState.AccumulatedTime >= op.Interval)
            {
                opState.AccumulatedTime -= op.Interval;

                // Calculate and apply yield
                var yield = CalculateOperationYield(opState, op);
                State.Cash += yield;
                State.TotalEarned += yield;
            }
        }

        // Check for milestone
        _milestoneService.CheckForMilestone();
    }

    /// <summary>
    /// Calculates effective income per second for display (yield/interval with multipliers).
    /// </summary>
    private double CalculateEffectiveIncomePerSecond(OperationState opState, Operation config)
    {
        if (opState.Level == 0) return 0;

        // Base yield per cycle with prestige bonus
        var yieldPerCycle = config.GetYield(opState.Level, State.PrestigeBonus);

        // Apply skill multipliers
        var skillMultiplier = _skillService.GetTotalIncomeMultiplier();
        var operationMultiplier = _skillService.GetOperationMultiplier(opState.Id);
        var compoundMultiplier = _skillService.GetCompoundInterestMultiplier(State.SessionStartUtc);

        // Calculate income per second (yield / interval)
        return (yieldPerCycle * skillMultiplier * operationMultiplier * compoundMultiplier) / config.Interval;
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
                Level = op.UnlockCost == 0 ? 1 : 0,
                AccumulatedTime = 0 // Reset progress
            });
        }

        InvalidateIncomeCache(); // Skills and operations reset
    }

    private void CalculateOfflineEarnings()
    {
        var offlineTime = DateTime.UtcNow - State.LastPlayedUtc;
        LastOfflineTime = offlineTime;

        // Max hours with skill bonus (Extended Shift)
        var maxHours = GameConfig.MaxOfflineHours + _skillService.GetOfflineMaxHoursBonus();
        var offlineSeconds = Math.Min(offlineTime.TotalSeconds, maxHours * 3600);

        if (offlineSeconds < 60) // Less than 1 minute - no offline earnings
        {
            LastOfflineEarnings = 0;
            LastOfflineEfficiency = 0;
            State.LastPlayedUtc = DateTime.UtcNow;
            return;
        }

        // Base efficiency + skill bonus (Night Owl)
        var efficiency = GameConfig.OfflineEfficiency + _skillService.GetOfflineEfficiencyBonus();
        efficiency = Math.Min(efficiency, 1.0); // Cap at 100%
        LastOfflineEfficiency = efficiency;

        // Calculate earnings per operation using interval-based logic
        double totalEarnings = 0;

        foreach (var op in GameConfig.Operations)
        {
            var opState = GetOperationState(op.Id);
            if (opState == null || opState.Level <= 0) continue;

            // How many complete cycles in offline time?
            var completeCycles = Math.Floor(offlineSeconds / op.Interval);

            // Base yield per cycle
            var yieldPerCycle = op.GetYield(opState.Level, State.PrestigeBonus);

            // Total for this operation (with efficiency)
            var operationEarnings = completeCycles * yieldPerCycle * efficiency;

            totalEarnings += operationEarnings;
        }

        // Apply offline earnings multiplier (Godfather's Cut skill)
        totalEarnings *= _skillService.GetOfflineEarningsMultiplier();

        LastOfflineEarnings = totalEarnings;
        State.Cash += totalEarnings;
        State.TotalEarned += totalEarnings;

        // Note: AccumulatedTime is NOT modified - progress continues from where player left
        State.LastPlayedUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Clear offline earnings display so modal doesn't show again on tab switch
    /// </summary>
    public void ClearOfflineEarningsDisplay()
    {
        LastOfflineEarnings = 0;
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
