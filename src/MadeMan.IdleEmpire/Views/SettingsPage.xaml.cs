using MadeMan.IdleEmpire.ViewModels;

namespace MadeMan.IdleEmpire.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
