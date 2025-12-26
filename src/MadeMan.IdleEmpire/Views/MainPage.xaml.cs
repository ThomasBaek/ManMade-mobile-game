using MadeMan.IdleEmpire.ViewModels;

namespace MadeMan.IdleEmpire.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;
    private bool _prestigeModalShownThisSession;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Subscribe to property changes to detect prestige availability
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private async void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.CanPrestige))
        {
            if (_viewModel.CanPrestige && !_prestigeModalShownThisSession && !_viewModel.IsPrestigeModalVisible)
            {
                await ShowPrestigeModalWithAnimation();
            }
        }

        // When prestige happens (CanPrestige goes false), reset so modal can show again next cycle
        if (e.PropertyName == nameof(MainViewModel.CanPrestige) && !_viewModel.CanPrestige)
        {
            _prestigeModalShownThisSession = false;
        }
    }

    private async Task ShowPrestigeModalWithAnimation()
    {
        _prestigeModalShownThisSession = true;

        // Set initial state for animation
        PrestigeModal.Opacity = 0;
        PrestigeModal.Scale = 0.8;
        _viewModel.IsPrestigeModalVisible = true;

        // Fade in and scale up
        await Task.WhenAll(
            PrestigeModal.FadeTo(1, 400, Easing.CubicOut),
            PrestigeModal.ScaleTo(1, 400, Easing.CubicOut)
        );
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();

        // Check for offline earnings to display
        ShowWelcomeBackIfNeeded();

        // Check if prestige is already available on appear (and not already shown)
        if (_viewModel.CanPrestige && !_prestigeModalShownThisSession && !_viewModel.IsPrestigeModalVisible)
        {
            _ = ShowPrestigeModalWithAnimation();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnDisappearing();
    }

    private void ShowWelcomeBackIfNeeded()
    {
        var engine = _viewModel.GameEngine;

        // Only show if away for more than 1 minute and earned something
        if (engine.LastOfflineTime.TotalMinutes >= 1 && engine.LastOfflineEarnings > 0)
        {
            WelcomeBackModal.Show(
                engine.LastOfflineEarnings,
                engine.LastOfflineTime,
                engine.LastOfflineEfficiency);

            // Clear so it doesn't show again on tab switch
            engine.ClearOfflineEarningsDisplay();
        }
    }
}
