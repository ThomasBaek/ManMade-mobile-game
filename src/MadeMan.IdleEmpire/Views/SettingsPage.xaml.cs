using MadeMan.IdleEmpire.Services;
using MadeMan.IdleEmpire.ViewModels;

namespace MadeMan.IdleEmpire.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel viewModel, IGameEngine gameEngine)
    {
        InitializeComponent();
        BindingContext = viewModel;

        // Wire up stats modal with game engine
        StatsModalComponent.SetGameEngine(gameEngine);
    }

    private void OnStatisticsTapped(object? sender, EventArgs e)
    {
        StatsModalComponent.IsVisible = true;
    }

    private async void OnSkillsGuideTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("skillsguide");
    }

    private async void OnHelpTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("help");
    }
}
