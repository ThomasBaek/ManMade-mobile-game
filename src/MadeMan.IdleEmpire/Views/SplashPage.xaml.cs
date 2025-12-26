namespace MadeMan.IdleEmpire.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Wait for splash duration
        await Task.Delay(2000);

        // Fade out
        await this.FadeTo(0, 500);

        // Navigate to main app (get AppShell from DI to ensure proper dependency injection)
        if (Application.Current != null && Handler?.MauiContext?.Services != null)
        {
            var shell = Handler.MauiContext.Services.GetService<AppShell>();
            if (shell != null)
            {
                Application.Current.Windows[0].Page = shell;
            }
        }
    }
}
