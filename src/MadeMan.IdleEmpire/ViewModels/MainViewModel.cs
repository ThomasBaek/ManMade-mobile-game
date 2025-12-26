using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IGameEngine _engine;
    private readonly SaveManager _saveManager;
    private IDispatcherTimer? _gameTimer;
    private IDispatcherTimer? _saveTimer;
    private DateTime _lastTick;

    [ObservableProperty]
    private string _cashDisplay = "$0";

    [ObservableProperty]
    private string _incomeDisplay = "+$0/s";

    [ObservableProperty]
    private bool _canPrestige;

    [ObservableProperty]
    private string _prestigeButtonText = "BECOME A MADE MAN";

    // Prestige stats
    [ObservableProperty]
    private int _prestigeCount;

    [ObservableProperty]
    private string _prestigeBonusDisplay = "+0%";

    [ObservableProperty]
    private bool _hasPrestiged;

    // Total earned / progress
    [ObservableProperty]
    private string _totalEarnedDisplay = "$0";

    [ObservableProperty]
    private double _prestigeProgress;

    public ObservableCollection<OperationViewModel> Operations { get; } = new();

    public SkillViewModel SkillVM { get; }

    public MainViewModel(IGameEngine engine, SaveManager saveManager, SkillViewModel skillVM)
    {
        _engine = engine;
        _saveManager = saveManager;
        SkillVM = skillVM;
    }

    public void OnAppearing()
    {
        _engine.Initialize();
        BuildOperationViewModels();
        SkillVM.RefreshActiveSkills();
        SkillVM.UpdateProgress();
        StartGameLoop();
        StartAutoSave();
    }

    public void OnDisappearing()
    {
        StopTimers();
        _saveManager.Save(_engine.State);
    }

    private void BuildOperationViewModels()
    {
        Operations.Clear();
        foreach (var op in GameConfig.Operations)
        {
            Operations.Add(new OperationViewModel(op, _engine));
        }
    }

    private void StartGameLoop()
    {
        _lastTick = DateTime.UtcNow;
        _gameTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_gameTimer != null)
        {
            _gameTimer.Interval = TimeSpan.FromMilliseconds(1000.0 / GameConfig.TicksPerSecond);
            _gameTimer.Tick += OnGameTick;
            _gameTimer.Start();
        }
    }

    private void OnGameTick(object? sender, EventArgs e)
    {
        var now = DateTime.UtcNow;
        var delta = (now - _lastTick).TotalSeconds;
        _lastTick = now;

        _engine.Tick(delta);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        CashDisplay = FormatCash(_engine.State.Cash);
        IncomeDisplay = $"+{FormatCash(_engine.IncomePerSecond)}/s";
        CanPrestige = _engine.CanPrestige();

        // Prestige stats
        PrestigeCount = _engine.State.PrestigeCount;
        HasPrestiged = PrestigeCount > 0;
        var bonusPercent = (_engine.State.PrestigeBonus - 1.0) * 100;
        PrestigeBonusDisplay = $"+{bonusPercent:0}%";

        // Total earned + progress to prestige
        TotalEarnedDisplay = FormatCash(_engine.State.TotalEarned);
        PrestigeProgress = Math.Min(_engine.State.TotalEarned / GameConfig.PrestigeThreshold, 1.0);

        if (CanPrestige)
        {
            var bonus = GameConfig.PrestigeBonusPerReset * 100;
            PrestigeButtonText = $"RESET FOR +{bonus:0}% BONUS";
        }

        foreach (var op in Operations)
        {
            op.Refresh();
        }

        // Update skill milestone progress
        SkillVM.UpdateProgress();
    }

    [RelayCommand]
    private void Prestige()
    {
        if (!_engine.CanPrestige()) return;

        _engine.DoPrestige();
        BuildOperationViewModels();
        SkillVM.RefreshActiveSkills(); // Skills reset on prestige
        UpdateDisplay();
    }

    private void StartAutoSave()
    {
        _saveTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_saveTimer != null)
        {
            _saveTimer.Interval = TimeSpan.FromSeconds(GameConfig.AutoSaveIntervalSeconds);
            _saveTimer.Tick += (s, e) => _saveManager.Save(_engine.State);
            _saveTimer.Start();
        }
    }

    private void StopTimers()
    {
        _gameTimer?.Stop();
        _saveTimer?.Stop();
        _gameTimer = null;
        _saveTimer = null;
    }

    private static string FormatCash(double value)
    {
        if (value >= 1_000_000_000) return $"${value / 1_000_000_000:F2}B";
        if (value >= 1_000_000) return $"${value / 1_000_000:F2}M";
        if (value >= 1_000) return $"${value / 1_000:F2}K";
        return $"${value:F2}";
    }
}
