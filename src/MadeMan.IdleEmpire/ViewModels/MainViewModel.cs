using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Models;
using MadeMan.IdleEmpire.Services;
using MadeMan.IdleEmpire.Utilities;
using static MadeMan.IdleEmpire.Models.TitleConfig;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IGameEngine _engine;
    private readonly SaveManager _saveManager;
    private IDispatcherTimer? _gameTimer;
    private IDispatcherTimer? _saveTimer;
    private DateTime _lastTick;
    private DateTime _lastDisplayUpdate;
    private bool _isGameLoopRunning;

    // Display update throttling (4 updates per second for UI, game logic runs at full speed)
    private const int DisplayUpdatesPerSecond = 4;
    private static readonly TimeSpan DisplayUpdateInterval = TimeSpan.FromMilliseconds(1000.0 / DisplayUpdatesPerSecond);

    // Cached display values to avoid unnecessary binding updates
    private string _lastCashDisplay = "";
    private string _lastIncomeDisplay = "";
    private string _lastTotalEarnedDisplay = "";

    [ObservableProperty]
    private string _cashDisplay = "$0";

    [ObservableProperty]
    private double _cashValue;

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

    // Next title after prestige (for modal)
    public string NextTitle => TitleConfig.GetTitle(_engine.State.PrestigeCount + 1).Name;

    // Income bonus as percentage (for modal display)
    public string CurrentIncomeBonusDisplay => $"{(_engine.State.PrestigeBonus - 1.0) * 100:0}%";
    public string NextIncomeBonusDisplay => $"{(_engine.State.PrestigeBonus + GameConfig.PrestigeBonusPerReset - 1.0) * 100:0}%";

    [ObservableProperty]
    private bool _showTitleUnlockPopup;

    [ObservableProperty]
    private string _unlockedTitleName = "";

    [ObservableProperty]
    private string _unlockedTitleDescription = "";


    public ObservableCollection<OperationViewModel> Operations { get; } = new();

    public SkillViewModel SkillVM { get; }

    // Event fired when prestige is completed (for celebration)
    // Parameters: bonusPercent, newTitleName, newTitleDescription
    public event Action<double, string, string>? PrestigeCompleted;

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

        // Only build ViewModels once - reuse on subsequent tab switches
        if (Operations.Count == 0)
        {
            BuildOperationViewModels();
        }
        else
        {
            // Just refresh existing ViewModels instead of rebuilding
            foreach (var op in Operations)
            {
                op.Refresh();
            }
        }

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
        // Only save the game state and stats
        _saveManager.Save(_engine.State);
        _saveManager.SaveStats(_engine.Stats);
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
        _saveManager.SaveStats(_engine.Stats);
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

        // Game logic runs every tick (60fps)
        _engine.Tick(delta);

        // UI updates are throttled to reduce CPU usage (4fps is enough for display)
        if (now - _lastDisplayUpdate >= DisplayUpdateInterval)
        {
            _lastDisplayUpdate = now;
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        // Update raw cash value for AnimatedNumberLabel
        CashValue = _engine.State.Cash;

        // Use caching to avoid unnecessary binding updates
        // No $ prefix - TopBar has money icon
        var newCashDisplay = NumberFormatter.FormatNumber(_engine.State.Cash);
        if (newCashDisplay != _lastCashDisplay)
        {
            CashDisplay = newCashDisplay;
            _lastCashDisplay = newCashDisplay;
        }

        var newIncomeDisplay = $"+{NumberFormatter.FormatCurrency(_engine.IncomePerSecond)}/s";
        if (newIncomeDisplay != _lastIncomeDisplay)
        {
            IncomeDisplay = newIncomeDisplay;
            _lastIncomeDisplay = newIncomeDisplay;
        }

        CanPrestige = _engine.CanPrestige();

        // Prestige stats (these change rarely, so no caching needed)
        PrestigeCount = _engine.State.PrestigeCount;
        HasPrestiged = PrestigeCount > 0;
        var bonusPercent = (_engine.State.PrestigeBonus - 1.0) * 100;
        PrestigeBonusDisplay = $"+{bonusPercent:0}%";

        // Total earned + progress to prestige
        var newTotalEarned = NumberFormatter.FormatCurrency(_engine.State.TotalEarned);
        if (newTotalEarned != _lastTotalEarnedDisplay)
        {
            TotalEarnedDisplay = newTotalEarned;
            _lastTotalEarnedDisplay = newTotalEarned;
        }

        PrestigeProgress = Math.Min(_engine.State.TotalEarned / GameConfig.PrestigeThreshold, 1.0);
        PrestigeThresholdDisplay = $"/ {NumberFormatter.FormatCurrency(GameConfig.PrestigeThreshold)}";

        // Prestige badge color - use resource colors
        PrestigeBadgeColor = CanPrestige
            ? GetResourceColor("Primary")
            : GetResourceColor("SurfaceLight");

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

    private static Color GetResourceColor(string key)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Color color)
        {
            return color;
        }
        return Colors.Gray; // Fallback
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
        DoPrestigeInternal();
    }

    /// <summary>
    /// Debug command to test prestige without meeting requirements.
    /// </summary>
    [RelayCommand]
    private void DebugPrestige()
    {
        // Force prestige for testing
        _engine.State.TotalEarned = 25000; // Meet threshold
        DoPrestigeInternal();
    }

    private void DoPrestigeInternal()
    {
        _engine.DoPrestige();
        IsPrestigeModalVisible = false;
        BuildOperationViewModels();
        SkillVM.RefreshActiveSkills(); // Skills reset on prestige

        // Get current title after prestige
        var currentTitle = TitleConfig.GetTitle(_engine.State.PrestigeCount);

        // Update all properties including title
        OnPropertyChanged(nameof(CurrentTitle));
        OnPropertyChanged(nameof(CurrentTitleDescription));
        OnPropertyChanged(nameof(NextTitle));
        OnPropertyChanged(nameof(CurrentPrestigeBonusDisplay));
        OnPropertyChanged(nameof(NextPrestigeBonusDisplay));
        OnPropertyChanged(nameof(CurrentIncomeBonusDisplay));
        OnPropertyChanged(nameof(NextIncomeBonusDisplay));
        UpdateDisplay();

        // Fire celebration event with bonus percentage and title info
        var bonusPercent = (_engine.State.PrestigeBonus - 1.0) * 100;
        PrestigeCompleted?.Invoke(bonusPercent, currentTitle.Name, currentTitle.Description);
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
        _saveManager.SaveStats(_engine.Stats);
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
}
