using MadeMan.IdleEmpire.Helpers;
using MadeMan.IdleEmpire.Services;
using MadeMan.IdleEmpire.ViewModels;
using SkiaSharp.Extended.UI.Controls;

namespace MadeMan.IdleEmpire.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;
    private readonly ICelebrationService _celebrationService;
    private bool _prestigeModalShownThisSession;

    public MainPage(MainViewModel viewModel, ICelebrationService celebrationService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _celebrationService = celebrationService;
        BindingContext = _viewModel;

        // Subscribe to property changes to detect prestige availability
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;

        // Subscribe to prestige completion for celebration
        _viewModel.PrestigeCompleted += OnPrestigeCompleted;

        // Subscribe to TapCompleted events for animations
        SubscribeToOperationEvents();
    }

    private async void OnPrestigeCompleted(double bonusPercent, string newTitle, string titleDescription)
    {
        await _celebrationService.PlayPrestigeCelebrationAsync(
            CelebrationFlash,
            ConfettiAnimation,
            CelebrationTitle,
            CelebrationBonus,
            CelebrationTitleFrame,
            CelebrationNewTitle,
            CelebrationTitleDesc,
            bonusPercent,
            newTitle,
            titleDescription);
    }

    private void SubscribeToOperationEvents()
    {
        foreach (var op in _viewModel.Operations)
        {
            // Unsubscribe first to prevent double-subscription
            op.TapCompleted -= OnOperationTapCompleted;
            op.TapCompleted += OnOperationTapCompleted;
        }
    }

    private void UnsubscribeFromOperationEvents()
    {
        foreach (var op in _viewModel.Operations)
        {
            op.TapCompleted -= OnOperationTapCompleted;
        }
    }

    private void OnOperationTapCompleted(OperationViewModel.TapResult result)
    {
        // No longer used - animation handled directly in OnOperationButtonClicked
    }

    private async void OnOperationButtonClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button) return;
        if (button.BindingContext is not OperationViewModel opVm) return;

        // Check affordability BEFORE executing the tap
        bool canAfford = opVm.IsUnlocked
            ? opVm.ButtonColor == GetResourceColor("Success")
            : opVm.ButtonColor == GetResourceColor("Success");

        // Execute the tap command
        if (opVm.TapCommand.CanExecute(null))
        {
            opVm.TapCommand.Execute(null);
        }

        // Animate based on what we checked BEFORE the tap
        if (canAfford)
        {
            await AnimationHelper.BounceAsync(button);
        }
        else
        {
            await AnimationHelper.ShakeAsync(button);
        }
    }

    private static Color GetResourceColor(string key)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Color color)
            return color;
        return Colors.Gray;
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
            PrestigeModal.FadeToAsync(1, 400, Easing.CubicOut),
            PrestigeModal.ScaleToAsync(1, 400, Easing.CubicOut)
        );
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();

        // Resubscribe to events (in case we unsubscribed in OnDisappearing)
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged; // Prevent double-subscription
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        _viewModel.PrestigeCompleted -= OnPrestigeCompleted; // Prevent double-subscription
        _viewModel.PrestigeCompleted += OnPrestigeCompleted;
        SubscribeToOperationEvents();

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
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        _viewModel.PrestigeCompleted -= OnPrestigeCompleted;
        UnsubscribeFromOperationEvents();
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
