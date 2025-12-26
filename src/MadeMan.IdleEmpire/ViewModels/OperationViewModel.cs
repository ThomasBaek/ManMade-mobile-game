using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;
using MadeMan.IdleEmpire.Utilities;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class OperationViewModel : ObservableObject
{
    private readonly Operation _operation;
    private readonly IGameEngine _engine;

    // Cached values to avoid unnecessary binding updates
    private string _lastButtonText = "";
    private string _lastYieldDisplay = "";
    private string _lastIncomeDisplay = "";
    private string _lastTimeRemainingDisplay = "";
    private double _lastProgress = -1;
    private double _lastUnlockProgress = -1;
    private Color _lastButtonColor = Colors.Transparent;

    public string Name => _operation.Name;
    public string Icon => _operation.Icon;

    // Emoji icons for MVP (real icons in polish phase)
    public string IconEmoji => _operation.Id switch
    {
        "pickpocket" => "\U0001F91A",  // Raised back of hand
        "cartheft" => "\U0001F697",     // Car
        "burglary" => "\U0001F3E0",     // House
        "speakeasy" => "\U0001F37A",    // Beer mug
        "casino" => "\U0001F3B0",       // Slot machine
        _ => "\U0001F4B0"               // Money bag
    };

    [ObservableProperty]
    private string _levelDisplay = string.Empty;

    [ObservableProperty]
    private string _incomeDisplay = string.Empty;

    [ObservableProperty]
    private string _buttonText = string.Empty;

    [ObservableProperty]
    private Color _buttonColor = Colors.Gray;

    [ObservableProperty]
    private bool _isUnlocked;

    // Visibility support (TASK-036)
    [ObservableProperty]
    private bool _shouldShow = true;

    [ObservableProperty]
    private bool _isNextToUnlock;

    [ObservableProperty]
    private double _unlockProgress; // 0.0 - 1.0 for locked operations

    // Progress bar support
    [ObservableProperty]
    private double _progress; // 0.0 - 1.0

    [ObservableProperty]
    private string _yieldDisplay = string.Empty; // "$20"

    [ObservableProperty]
    private string _timeRemainingDisplay = string.Empty; // "3.2s"

    public OperationViewModel(Operation operation, IGameEngine engine)
    {
        _operation = operation;
        _engine = engine;
        Refresh();
    }

    public void Refresh()
    {
        var state = _engine.State.Operations.FirstOrDefault(o => o.Id == _operation.Id);
        if (state == null) return;

        IsUnlocked = state.Level > 0;

        if (!IsUnlocked)
        {
            LevelDisplay = "LOCKED";

            // Only update if changed
            var newButtonText = $"${NumberFormatter.FormatNumber(_operation.UnlockCost)}";
            if (newButtonText != _lastButtonText)
            {
                ButtonText = newButtonText;
                _lastButtonText = newButtonText;
            }

            var canUnlock = _engine.CanUnlock(_operation.Id);
            var newButtonColor = canUnlock
                ? GetResourceColor("Success")
                : GetResourceColor("Smoke");
            if (newButtonColor != _lastButtonColor)
            {
                ButtonColor = newButtonColor;
                _lastButtonColor = newButtonColor;
            }

            // Calculate unlock progress for visibility
            var newUnlockProgress = _operation.UnlockCost > 0
                ? Math.Min(1.0, _engine.State.Cash / _operation.UnlockCost)
                : 1.0;
            if (Math.Abs(newUnlockProgress - _lastUnlockProgress) > 0.01)
            {
                UnlockProgress = newUnlockProgress;
                _lastUnlockProgress = newUnlockProgress;
            }

            // Show if: 50%+ progress OR is next to unlock
            ShouldShow = UnlockProgress >= 0.5 || IsNextToUnlock;

            // Reset progress display for locked operations
            if (_lastProgress != 0) { Progress = 0; _lastProgress = 0; }
            if (_lastYieldDisplay != "") { YieldDisplay = ""; _lastYieldDisplay = ""; }
            if (_lastIncomeDisplay != "") { IncomeDisplay = ""; _lastIncomeDisplay = ""; }
            if (_lastTimeRemainingDisplay != "") { TimeRemainingDisplay = ""; _lastTimeRemainingDisplay = ""; }
        }
        else
        {
            ShouldShow = true; // Always show unlocked operations
            UnlockProgress = 1.0;
            LevelDisplay = $"Lvl {state.Level}";

            // Calculate yield for this cycle
            var yield = _operation.GetYield(state.Level, _engine.State.PrestigeBonus);
            var newYieldDisplay = $"${NumberFormatter.FormatNumber(yield)}";
            if (newYieldDisplay != _lastYieldDisplay)
            {
                YieldDisplay = newYieldDisplay;
                _lastYieldDisplay = newYieldDisplay;
            }

            // Effective income per second for reference
            var incomePerSec = _operation.GetIncomePerSecond(state.Level, _engine.State.PrestigeBonus);
            var newIncomeDisplay = $"${NumberFormatter.FormatNumber(incomePerSec)}/s";
            if (newIncomeDisplay != _lastIncomeDisplay)
            {
                IncomeDisplay = newIncomeDisplay;
                _lastIncomeDisplay = newIncomeDisplay;
            }

            // Progress calculation
            var newProgress = Math.Min(state.AccumulatedTime / _operation.Interval, 1.0);
            if (Math.Abs(newProgress - _lastProgress) > 0.01)
            {
                Progress = newProgress;
                _lastProgress = newProgress;
            }

            var timeRemaining = Math.Max(_operation.Interval - state.AccumulatedTime, 0);
            var newTimeDisplay = $"{timeRemaining:F1}s";
            if (newTimeDisplay != _lastTimeRemainingDisplay)
            {
                TimeRemainingDisplay = newTimeDisplay;
                _lastTimeRemainingDisplay = newTimeDisplay;
            }

            // Upgrade button
            var upgradeCost = _operation.GetUpgradeCost(state.Level);
            var newButtonText = $"${NumberFormatter.FormatNumber(upgradeCost)}";
            if (newButtonText != _lastButtonText)
            {
                ButtonText = newButtonText;
                _lastButtonText = newButtonText;
            }

            var canUpgrade = _engine.CanUpgrade(_operation.Id);
            var newButtonColor = canUpgrade
                ? GetResourceColor("Primary")
                : GetResourceColor("Smoke");
            if (newButtonColor != _lastButtonColor)
            {
                ButtonColor = newButtonColor;
                _lastButtonColor = newButtonColor;
            }
        }
    }

    private static Color GetResourceColor(string key)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Color color)
        {
            return color;
        }
        return Colors.Gray;
    }

    [RelayCommand]
    private void Tap()
    {
        _engine.UnlockOrUpgrade(_operation.Id);
        Refresh();
    }
}
