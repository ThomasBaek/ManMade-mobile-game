using MadeMan.IdleEmpire.Helpers;
using SkiaSharp.Extended.UI.Controls;

namespace MadeMan.IdleEmpire.Services;

/// <summary>
/// Orchestrates celebration sequences for major game events.
/// </summary>
public interface ICelebrationService
{
    bool IsEnabled { get; set; }
    Task PlayPrestigeCelebrationAsync(
        View flashOverlay,
        SKLottieView confettiView,
        Label titleLabel,
        Label bonusLabel,
        View titleFrame,
        Label newTitleLabel,
        Label titleDescLabel,
        double bonusPercent,
        string newTitle,
        string titleDescription);
}

public class CelebrationService : ICelebrationService
{
    private const string CelebrationsEnabledKey = "settings_celebrations";

    public bool IsEnabled
    {
        get => Preferences.Default.Get(CelebrationsEnabledKey, true);
        set => Preferences.Default.Set(CelebrationsEnabledKey, value);
    }

    public async Task PlayPrestigeCelebrationAsync(
        View flashOverlay,
        SKLottieView confettiView,
        Label titleLabel,
        Label bonusLabel,
        View titleFrame,
        Label newTitleLabel,
        Label titleDescLabel,
        double bonusPercent,
        string newTitle,
        string titleDescription)
    {
        if (!IsEnabled) return;

        try
        {
            // Reset initial states
            flashOverlay.Opacity = 0;
            flashOverlay.IsVisible = true;
            confettiView.Opacity = 0;
            confettiView.IsVisible = true;
            confettiView.Progress = TimeSpan.Zero;
            confettiView.IsAnimationEnabled = true;
            titleLabel.Scale = 0;
            titleLabel.Opacity = 0;
            titleLabel.IsVisible = true;
            bonusLabel.Scale = 0;
            bonusLabel.Opacity = 0;
            bonusLabel.Text = $"+{bonusPercent:0}% INCOME BONUS!";
            titleFrame.Scale = 0;
            titleFrame.Opacity = 0;
            newTitleLabel.Text = newTitle;
            titleDescLabel.Text = titleDescription;

            // T+0ms: Screen flash
            HapticHelper.Success();
            await flashOverlay.FadeToAsync(0.7, 50, Easing.CubicIn);
            await flashOverlay.FadeToAsync(0, 200, Easing.CubicOut);
            flashOverlay.IsVisible = false;

            // T+100ms: Confetti starts
            await confettiView.FadeToAsync(1, 100);

            // T+100ms: "PRESTIGE COMPLETE!" animation
            await Task.WhenAll(
                titleLabel.ScaleToAsync(1.2, 300, Easing.CubicOut),
                titleLabel.FadeToAsync(1, 200)
            );
            await titleLabel.ScaleToAsync(1, 150, Easing.BounceOut);

            // T+500ms: Bonus text
            await Task.Delay(200);
            bonusLabel.IsVisible = true;
            await Task.WhenAll(
                bonusLabel.ScaleToAsync(1.1, 250, Easing.CubicOut),
                bonusLabel.FadeToAsync(1, 200)
            );
            await bonusLabel.ScaleToAsync(1, 100, Easing.CubicIn);

            // T+800ms: Title frame slides in
            await Task.Delay(300);
            titleFrame.IsVisible = true;
            await Task.WhenAll(
                titleFrame.ScaleToAsync(1.05, 300, Easing.CubicOut),
                titleFrame.FadeToAsync(1, 250)
            );
            await titleFrame.ScaleToAsync(1, 150, Easing.BounceOut);

            // Hold for impact
            await Task.Delay(2000);

            // Fade out everything
            await Task.WhenAll(
                confettiView.FadeToAsync(0, 500),
                titleLabel.FadeToAsync(0, 400),
                bonusLabel.FadeToAsync(0, 400),
                titleFrame.FadeToAsync(0, 400)
            );

            // Cleanup
            confettiView.IsAnimationEnabled = false;
            confettiView.IsVisible = false;
            titleLabel.IsVisible = false;
            bonusLabel.IsVisible = false;
            titleFrame.IsVisible = false;
        }
        catch (Exception)
        {
            // Ensure cleanup on error
            flashOverlay.IsVisible = false;
            confettiView.IsAnimationEnabled = false;
            confettiView.IsVisible = false;
            titleLabel.IsVisible = false;
            bonusLabel.IsVisible = false;
            titleFrame.IsVisible = false;
        }
    }
}
