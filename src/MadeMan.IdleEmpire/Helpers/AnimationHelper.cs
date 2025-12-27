namespace MadeMan.IdleEmpire.Helpers;

/// <summary>
/// Simple animation helper for UI feedback.
/// </summary>
public static class AnimationHelper
{
    /// <summary>
    /// Bounce animation for successful button press.
    /// </summary>
    public static async Task BounceAsync(VisualElement element)
    {
        if (element == null) return;

        try
        {
            await element.ScaleToAsync(0.95, 50, Easing.CubicOut);
            await element.ScaleToAsync(1.0, 100, Easing.BounceOut);
        }
        catch
        {
            // Reset scale if animation fails
            element.Scale = 1.0;
        }
    }

    /// <summary>
    /// Shake animation for error/can't afford.
    /// </summary>
    public static async Task ShakeAsync(VisualElement element)
    {
        if (element == null) return;

        try
        {
            await element.TranslateToAsync(-5, 0, 30);
            await element.TranslateToAsync(5, 0, 30);
            await element.TranslateToAsync(-3, 0, 30);
            await element.TranslateToAsync(3, 0, 30);
            await element.TranslateToAsync(0, 0, 30);
        }
        catch
        {
            // Reset position if animation fails
            element.TranslationX = 0;
        }
    }

    /// <summary>
    /// Pop animation for unlock/prestige.
    /// </summary>
    public static async Task PopAsync(VisualElement element)
    {
        if (element == null) return;

        try
        {
            await element.ScaleToAsync(1.1, 80, Easing.CubicOut);
            await element.ScaleToAsync(1.0, 120, Easing.BounceOut);
        }
        catch
        {
            // Reset scale if animation fails
            element.Scale = 1.0;
        }
    }
}
