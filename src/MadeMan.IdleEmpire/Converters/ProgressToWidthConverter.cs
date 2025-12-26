using System.Globalization;

namespace MadeMan.IdleEmpire.Converters;

public class ProgressToWidthConverter : IValueConverter
{
    // Fallback max width if screen info unavailable
    private const double FallbackMaxWidth = 280;

    // Padding to account for screen margins (left + right padding from container)
    private const double HorizontalPadding = 96; // 16 * 2 (outer) + 16 * 2 (inner frame) + buffer

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double progress)
        {
            var maxWidth = CalculateMaxWidth();
            return Math.Max(0, Math.Min(progress, 1.0)) * maxWidth;
        }
        return 0;
    }

    private double CalculateMaxWidth()
    {
        try
        {
            // Get screen width and calculate available space for progress bar
            var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
            var screenWidthDp = displayInfo.Width / displayInfo.Density;

            // Calculate available width: screen width minus padding, then ~60% for progress bar
            // (The other ~40% is for the "Total Earned" label on the right)
            var availableWidth = (screenWidthDp - HorizontalPadding) * 0.6;

            return Math.Max(100, Math.Min(availableWidth, 400)); // Clamp to reasonable range
        }
        catch
        {
            return FallbackMaxWidth;
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
