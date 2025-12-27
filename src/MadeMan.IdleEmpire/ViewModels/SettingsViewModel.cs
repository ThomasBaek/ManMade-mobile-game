using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MadeMan.IdleEmpire.Helpers;
using MadeMan.IdleEmpire.Services;
using MadeMan.IdleEmpire.Utilities;

namespace MadeMan.IdleEmpire.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private const string SoundKey = "settings_sound";
    private const string MusicKey = "settings_music";
    private const string NotificationsKey = "settings_notifications";
    private const string VibrationKey = "settings_vibration";
    private const string CelebrationsKey = "settings_celebrations";

    private readonly SaveManager _saveManager;
    private readonly IGameEngine _gameEngine;

    public string Version => "1.2.0";

    [ObservableProperty]
    private bool _soundEnabled;

    [ObservableProperty]
    private bool _musicEnabled;

    [ObservableProperty]
    private bool _notificationsEnabled;

    [ObservableProperty]
    private bool _vibrationEnabled;

    [ObservableProperty]
    private bool _celebrationsEnabled;

    public SettingsViewModel(SaveManager saveManager, IGameEngine gameEngine)
    {
        _saveManager = saveManager;
        _gameEngine = gameEngine;

        // Load settings from Preferences
        SoundEnabled = Preferences.Default.Get(SoundKey, true);
        MusicEnabled = Preferences.Default.Get(MusicKey, true);
        NotificationsEnabled = Preferences.Default.Get(NotificationsKey, true);
        VibrationEnabled = Preferences.Default.Get(VibrationKey, true);
        CelebrationsEnabled = Preferences.Default.Get(CelebrationsKey, true);

        // Apply vibration setting to helper
        HapticHelper.IsEnabled = VibrationEnabled;
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

    partial void OnVibrationEnabledChanged(bool value)
    {
        Preferences.Default.Set(VibrationKey, value);
        HapticHelper.IsEnabled = value;
    }

    partial void OnCelebrationsEnabledChanged(bool value)
    {
        Preferences.Default.Set(CelebrationsKey, value);
    }

    [RelayCommand]
    private async Task ShowCredits()
    {
        var page = GetCurrentPage();
        if (page != null)
        {
            await page.DisplayAlertAsync(
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
        var firstConfirm = await page.DisplayAlertAsync(
            "⚠️ WARNING",
            "Are you sure you want to delete ALL progress?\n\n" +
            "This CANNOT be undone!\n\n" +
            $"Prestige: {_gameEngine.State.PrestigeCount} → 0\n" +
            $"Total Earned: ${NumberFormatter.FormatNumber(_gameEngine.State.TotalEarned)} → $0",
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
                await page.DisplayAlertAsync(
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
                await page.DisplayAlertAsync(
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
}
