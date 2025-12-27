namespace MadeMan.IdleEmpire.Helpers;

/// <summary>
/// Simple static helper for haptic feedback.
/// Uses static access to avoid DI complexity.
/// </summary>
public static class HapticHelper
{
    private static bool _isEnabled = true;

    public static bool IsEnabled
    {
        get => _isEnabled;
        set => _isEnabled = value;
    }

    /// <summary>
    /// Light click feedback for button taps.
    /// </summary>
    public static void Click()
    {
        if (!_isEnabled) return;

        try
        {
            if (Vibration.Default.IsSupported)
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(10));
            }
        }
        catch
        {
            // Silently ignore - haptics are non-critical
        }
    }

    /// <summary>
    /// Success feedback for successful actions (unlock, upgrade).
    /// </summary>
    public static void Success()
    {
        if (!_isEnabled) return;

        try
        {
            if (Vibration.Default.IsSupported)
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(30));
            }
        }
        catch
        {
            // Silently ignore
        }
    }

    /// <summary>
    /// Error feedback for failed actions (can't afford).
    /// </summary>
    public static void Error()
    {
        if (!_isEnabled) return;

        try
        {
            if (Vibration.Default.IsSupported)
            {
                // Double pulse for error
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(50));
            }
        }
        catch
        {
            // Silently ignore
        }
    }
}
