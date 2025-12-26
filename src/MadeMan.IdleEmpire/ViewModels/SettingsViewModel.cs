using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Services;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private const string SoundKey = "settings_sound";
    private const string MusicKey = "settings_music";
    private const string NotificationsKey = "settings_notifications";

    private readonly SaveManager _saveManager;
    private readonly IGameEngine _gameEngine;
    private readonly IServiceProvider _serviceProvider;

    public string Version => "1.1.0";

    [ObservableProperty]
    private bool _soundEnabled;

    [ObservableProperty]
    private bool _musicEnabled;

    [ObservableProperty]
    private bool _notificationsEnabled;

    public SettingsViewModel(SaveManager saveManager, IGameEngine gameEngine, IServiceProvider serviceProvider)
    {
        _saveManager = saveManager;
        _gameEngine = gameEngine;
        _serviceProvider = serviceProvider;

        // Load settings from Preferences
        SoundEnabled = Preferences.Default.Get(SoundKey, true);
        MusicEnabled = Preferences.Default.Get(MusicKey, true);
        NotificationsEnabled = Preferences.Default.Get(NotificationsKey, true);
    }

    partial void OnSoundEnabledChanged(bool value)
    {
        Preferences.Default.Set(SoundKey, value);
    }

    partial void OnMusicEnabledChanged(bool value)
    {
        Preferences.Default.Set(MusicKey, value);
    }

    partial void OnNotificationsEnabledChanged(bool value)
    {
        Preferences.Default.Set(NotificationsKey, value);
    }

    [RelayCommand]
    private async Task ShowCredits()
    {
        var page = GetCurrentPage();
        if (page != null)
        {
            await page.DisplayAlert(
                "Credits",
                "Made Man: Idle Empire\n\n" +
                "Version 1.1.0\n\n" +
                "Developed with .NET MAUI\n\n" +
                "© 2024 Made Man Studios",
                "OK");
        }
    }

    [RelayCommand]
    private async Task ResetGame()
    {
        var page = GetCurrentPage();
        if (page == null) return;

        // Step 1: First warning
        var firstConfirm = await page.DisplayAlert(
            "⚠️ WARNING",
            "Are you sure you want to delete ALL progress?\n\n" +
            "This CANNOT be undone!\n\n" +
            $"Prestige: {_gameEngine.State.PrestigeCount} → 0\n" +
            $"Total Earned: ${FormatNumber(_gameEngine.State.TotalEarned)} → $0",
            "CONTINUE",
            "Cancel");

        if (!firstConfirm) return;

        // Step 2: Type DELETE confirmation
        var deleteConfirm = await page.DisplayPromptAsync(
            "🚨 LAST CHANCE",
            "Type \"DELETE\" to confirm:",
            "DELETE ALL",
            "Cancel",
            placeholder: "DELETE",
            maxLength: 10);

        if (deleteConfirm?.ToUpper() != "DELETE") return;

        // Step 3: Perform reset
        await PerformReset();
    }

    private async Task PerformReset()
    {
        try
        {
            // Clear all game data
            _saveManager.Delete();

            // Clear settings
            Preferences.Default.Clear();

            var page = GetCurrentPage();
            if (page != null)
            {
                // Show confirmation
                await page.DisplayAlert(
                    "✓ Reset Complete",
                    "Game has been reset. Please restart the app.",
                    "OK");
            }

            // Quit app - user needs to restart manually
            // (Can't create new AppShell without full app restart for DI)
            Application.Current?.Quit();
        }
        catch (Exception ex)
        {
            var page = GetCurrentPage();
            if (page != null)
            {
                await page.DisplayAlert(
                    "Error",
                    $"Reset failed: {ex.Message}",
                    "OK");
            }
        }
    }

    private static Page? GetCurrentPage()
    {
        if (Application.Current?.Windows.Count > 0)
        {
            return Application.Current.Windows[0].Page;
        }
        return null;
    }

    private static string FormatNumber(double value)
    {
        if (value >= 1_000_000_000) return $"{value / 1_000_000_000:F2}B";
        if (value >= 1_000_000) return $"{value / 1_000_000:F2}M";
        if (value >= 1_000) return $"{value / 1_000:F2}K";
        return $"{value:F0}";
    }
}
