using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;
using static MadeMan.IdleEmpire.Models.TitleConfig;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IGameEngine _engine;
    private readonly SaveManager _saveManager;
    private IDispatcherTimer? _gameTimer;
    private IDispatcherTimer? _saveTimer;
    private DateTime _lastTick;
    private bool _isGameLoopRunning;

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

    [ObservableProperty]
    private string _prestigeThresholdDisplay = "/ $10K";

    [ObservableProperty]
    private Color _prestigeBadgeColor = Color.FromArgb("#252540");

    // Prestige Modal (TASK-038)
    [ObservableProperty]
    private bool _isPrestigeModalVisible;

    // Prestige bonus display
    public string CurrentPrestigeBonusDisplay => $"{_engine.State.PrestigeBonus:F2}x";
    public string NextPrestigeBonusDisplay => $"{_engine.State.PrestigeBonus + GameConfig.PrestigeBonusPerReset:F2}x";

    // Title System (TASK-039)
    public string CurrentTitle => TitleConfig.GetTitle(_engine.State.PrestigeCount).Name;
    public string CurrentTitleDescription => TitleConfig.GetTitle(_engine.State.PrestigeCount).Description;

    [ObservableProperty]
    private bool _showTitleUnlockPopup;

    [ObservableProperty]
    private string _unlockedTitleName = "";

    [ObservableProperty]
    private string _unlockedTitleDescription = "";

    public ObservableCollection<OperationViewModel> Operations { get; } = new();

    public SkillViewModel SkillVM { get; }

    // Expose game engine for Welcome Back modal
    public IGameEngine GameEngine => _engine;

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

        // Only start game loop once - keep it running across tab switches
        if (!_isGameLoopRunning)
        {
            StartGameLoop();
            StartAutoSave();
            _isGameLoopRunning = true;
        }
    }

    public void OnDisappearing()
    {
        // Don't stop timers when switching tabs - game should keep running
        // Only save the game state
        _saveManager.Save(_engine.State);
    }

    /// <summary>
    /// Called when the app is going to background or closing.
    /// This is the only time we should stop the game loop.
    /// </summary>
    public void OnAppSleep()
    {
        StopTimers();
        _isGameLoopRunning = false;
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
        PrestigeThresholdDisplay = $"/ {FormatCash(GameConfig.PrestigeThreshold)}";

        // Prestige badge color (glows when available)
        PrestigeBadgeColor = CanPrestige
            ? Color.FromArgb("#8B0000")  // Primary/blood red when available
            : Color.FromArgb("#252540"); // SurfaceLight when not

        if (CanPrestige)
        {
            var bonus = GameConfig.PrestigeBonusPerReset * 100;
            PrestigeButtonText = $"RESET FOR +{bonus:0}% BONUS";
        }

        // Mark first locked operation as "next to unlock"
        bool foundFirstLocked = false;
        foreach (var op in Operations)
        {
            if (!op.IsUnlocked && !foundFirstLocked)
            {
                op.IsNextToUnlock = true;
                foundFirstLocked = true;
            }
            else
            {
                op.IsNextToUnlock = false;
            }
            op.Refresh();
        }

        // Update skill milestone progress
        SkillVM.UpdateProgress();
    }

    [RelayCommand]
    private void ShowPrestigeModal()
    {
        if (CanPrestige)
        {
            IsPrestigeModalVisible = true;
        }
    }

    [RelayCommand]
    private void DismissPrestigeModal()
    {
        IsPrestigeModalVisible = false;
    }

    [RelayCommand]
    private void DismissTitlePopup()
    {
        ShowTitleUnlockPopup = false;
    }

    [RelayCommand]
    private void Prestige()
    {
        if (!_engine.CanPrestige()) return;

        // Remember current title before prestige
        var oldTitle = TitleConfig.GetTitle(_engine.State.PrestigeCount);

        _engine.DoPrestige();
        IsPrestigeModalVisible = false;
        BuildOperationViewModels();
        SkillVM.RefreshActiveSkills(); // Skills reset on prestige

        // Check for new title
        var newTitle = TitleConfig.GetTitle(_engine.State.PrestigeCount);
        if (newTitle != oldTitle)
        {
            UnlockedTitleName = newTitle.Name;
            UnlockedTitleDescription = newTitle.Description;
            ShowTitleUnlockPopup = true;
        }

        // Update all properties including title
        OnPropertyChanged(nameof(CurrentTitle));
        OnPropertyChanged(nameof(CurrentTitleDescription));
        OnPropertyChanged(nameof(CurrentPrestigeBonusDisplay));
        OnPropertyChanged(nameof(NextPrestigeBonusDisplay));
        UpdateDisplay();
    }

    private void StartAutoSave()
    {
        _saveTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_saveTimer != null)
        {
            _saveTimer.Interval = TimeSpan.FromSeconds(GameConfig.AutoSaveIntervalSeconds);
            _saveTimer.Tick += OnAutoSaveTick;
            _saveTimer.Start();
        }
    }

    private async void OnAutoSaveTick(object? sender, EventArgs e)
    {
        await _saveManager.SaveAsync(_engine.State);
    }

    private void StopTimers()
    {
        // Unsubscribe event handlers before stopping to prevent memory leaks
        if (_gameTimer != null)
        {
            _gameTimer.Tick -= OnGameTick;
            _gameTimer.Stop();
            _gameTimer = null;
        }

        if (_saveTimer != null)
        {
            _saveTimer.Tick -= OnAutoSaveTick;
            _saveTimer.Stop();
            _saveTimer = null;
        }
    }

    private static string FormatCash(double value)
    {
        if (value >= 1_000_000_000) return $"${value / 1_000_000_000:F2}B";
        if (value >= 1_000_000) return $"${value / 1_000_000:F2}M";
        if (value >= 1_000) return $"${value / 1_000:F2}K";
        return $"${value:F2}";
    }
}
