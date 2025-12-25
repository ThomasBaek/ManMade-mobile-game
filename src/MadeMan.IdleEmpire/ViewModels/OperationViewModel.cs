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
            ButtonText = $"${_operation.UnlockCost:F0}";
            ButtonColor = _engine.CanUnlock(_operation.Id)
                ? Color.FromArgb("#4ADE80")  // Success green
                : Color.FromArgb("#4A5568"); // Locked gray
        }
        else
        {
            LevelDisplay = $"Lvl {state.Level}";
            var income = _operation.GetIncome(state.Level, _engine.State.PrestigeBonus);
            IncomeDisplay = $"${income:F2}/s";

            var upgradeCost = _operation.GetUpgradeCost(state.Level);
            ButtonText = $"${upgradeCost:F0}";
            ButtonColor = _engine.CanUpgrade(_operation.Id)
                ? Color.FromArgb("#E94560")  // Primary red
                : Color.FromArgb("#4A5568"); // Locked gray
        }
    }

    [RelayCommand]
    private void Tap()
    {
        _engine.UnlockOrUpgrade(_operation.Id);
        Refresh();
    }
}
