using System.Globalization;

namespace MadeMan.IdleEmpire.Converters;

public class ProgressToWidthConverter : IValueConverter
{
    // Max width of progress bar container (will be set based on layout)
    public double MaxWidth { get; set; } = 280;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double progress)
        {
            return Math.Max(0, Math.Min(progress, 1.0)) * MaxWidth;
        }
        return 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
