using MadeMan.IdleEmpire.ViewModels;

namespace MadeMan.IdleEmpire.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();

        // Check for offline earnings to display
        ShowWelcomeBackIfNeeded();
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
