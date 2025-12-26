using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class OperationViewModel : ObservableObject
{
    private readonly Operation _operation;
    private readonly IGameEngine _engine;

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
            IncomeDisplay = string.Empty;
            ButtonText = $"${FormatNumber(_operation.UnlockCost)}";
            ButtonColor = _engine.CanUnlock(_operation.Id)
                ? Color.FromArgb("#4ADE80")  // Success green
                : Color.FromArgb("#4A5568"); // Locked gray

            // Calculate unlock progress for visibility
            UnlockProgress = _operation.UnlockCost > 0
                ? Math.Min(1.0, _engine.State.Cash / _operation.UnlockCost)
                : 1.0;

            // Show if: 50%+ progress OR is next to unlock
            ShouldShow = UnlockProgress >= 0.5 || IsNextToUnlock;

            // Reset progress display for locked operations
            Progress = 0;
            YieldDisplay = string.Empty;
            TimeRemainingDisplay = string.Empty;
        }
        else
        {
            ShouldShow = true; // Always show unlocked operations
            UnlockProgress = 1.0;
            LevelDisplay = $"Lvl {state.Level}";

            // Calculate yield for this cycle
            var yield = _operation.GetYield(state.Level, _engine.State.PrestigeBonus);
            YieldDisplay = $"${FormatNumber(yield)}";

            // Effective income per second for reference
            var incomePerSec = _operation.GetIncomePerSecond(state.Level, _engine.State.PrestigeBonus);
            IncomeDisplay = $"${FormatNumber(incomePerSec)}/s";

            // Progress calculation
            Progress = Math.Min(state.AccumulatedTime / _operation.Interval, 1.0);
            var timeRemaining = Math.Max(_operation.Interval - state.AccumulatedTime, 0);
            TimeRemainingDisplay = $"{timeRemaining:F1}s";

            // Upgrade button
            var upgradeCost = _operation.GetUpgradeCost(state.Level);
            ButtonText = $"${FormatNumber(upgradeCost)}";
            ButtonColor = _engine.CanUpgrade(_operation.Id)
                ? Color.FromArgb("#E94560")  // Primary red
                : Color.FromArgb("#4A5568"); // Locked gray
        }
    }

    private static string FormatNumber(double value)
    {
        if (value >= 1_000_000) return $"{value / 1_000_000:F1}M";
        if (value >= 1_000) return $"{value / 1_000:F1}K";
        return $"{value:F0}";
    }

    [RelayCommand]
    private void Tap()
    {
        _engine.UnlockOrUpgrade(_operation.Id);
        Refresh();
    }
}
