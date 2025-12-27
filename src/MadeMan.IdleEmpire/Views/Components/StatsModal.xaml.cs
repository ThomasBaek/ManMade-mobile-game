using MadeMan.IdleEmpire.Services;
using MadeMan.IdleEmpire.Utilities;

namespace MadeMan.IdleEmpire.Views.Components;

public partial class StatsModal : ContentView
{
    private IGameEngine? _gameEngine;
    private IDispatcherTimer? _updateTimer;
    private bool _isAnimating;

    public static new readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
        nameof(IsVisible),
        typeof(bool),
        typeof(StatsModal),
        false,
        propertyChanged: OnIsVisibleChanged);

    public new bool IsVisible
    {
        get => (bool)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    public StatsModal()
    {
        InitializeComponent();
    }

    public void SetGameEngine(IGameEngine gameEngine)
    {
        _gameEngine = gameEngine;
    }

    private static async void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is StatsModal modal && newValue is bool isVisible)
        {
            if (isVisible)
            {
                await modal.ShowModal();
            }
            else
            {
                modal.HideModal();
            }
        }
    }

    private async Task ShowModal()
    {
        if (_isAnimating) return;
        _isAnimating = true;

        try
        {
            // Refresh stats immediately
            RefreshStats();

            // Start live updates
            StartLiveUpdates();

            // Set initial animation state
            Opacity = 0;
            ModalCard.Scale = 0.8;
            ModalCard.Opacity = 0;

            // Fade in overlay
            await this.FadeToAsync(1, 200);

            // Animate card in with bounce
            await Task.WhenAll(
                ModalCard.ScaleToAsync(1.05, 250, Easing.CubicOut),
                ModalCard.FadeToAsync(1, 200)
            );
            await ModalCard.ScaleToAsync(1, 150, Easing.BounceOut);
        }
        finally
        {
            _isAnimating = false;
        }
    }

    private void HideModal()
    {
        StopLiveUpdates();
    }

    private void StartLiveUpdates()
    {
        StopLiveUpdates();

        _updateTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_updateTimer != null)
        {
            _updateTimer.Interval = TimeSpan.FromSeconds(1);
            _updateTimer.Tick += OnUpdateTick;
            _updateTimer.Start();
        }
    }

    private void StopLiveUpdates()
    {
        if (_updateTimer != null)
        {
            _updateTimer.Tick -= OnUpdateTick;
            _updateTimer.Stop();
            _updateTimer = null;
        }
    }

    private void OnUpdateTick(object? sender, EventArgs e)
    {
        RefreshStats();
    }

    private void RefreshStats()
    {
        if (_gameEngine == null) return;

        var stats = _gameEngine.Stats;
        var state = _gameEngine.State;

        // Earnings
        LblTotalEarned.Text = NumberFormatter.FormatCurrency(stats.LifetimeEarned);
        LblTotalSpent.Text = NumberFormatter.FormatCurrency(stats.LifetimeSpent);
        LblHighestCash.Text = NumberFormatter.FormatCurrency(stats.HighestCash);
        LblOfflineEarnings.Text = NumberFormatter.FormatCurrency(stats.OfflineEarnings);

        // Progress
        LblPrestiges.Text = state.PrestigeCount.ToString();
        LblUpgrades.Text = stats.TotalUpgrades.ToString();
        LblUnlocks.Text = stats.TotalUnlocks.ToString();

        // Time
        LblTimePlayed.Text = FormatTimePlayed(stats.TimePlayed);
        LblFirstPlayed.Text = stats.FirstPlayedUtc.ToLocalTime().ToString("MMM d, yyyy");
    }

    private static string FormatTimePlayed(TimeSpan time)
    {
        if (time.TotalDays >= 1)
            return $"{(int)time.TotalDays}d {time.Hours}h {time.Minutes}m";
        if (time.TotalHours >= 1)
            return $"{(int)time.TotalHours}h {time.Minutes}m";
        if (time.TotalMinutes >= 1)
            return $"{(int)time.TotalMinutes}m {time.Seconds}s";
        return $"{time.Seconds}s";
    }

    private void OnOverlayTapped(object? sender, EventArgs e)
    {
        IsVisible = false;
    }

    private void OnCloseClicked(object? sender, EventArgs e)
    {
        IsVisible = false;
    }
}
