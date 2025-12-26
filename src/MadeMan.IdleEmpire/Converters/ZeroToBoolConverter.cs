using System.Globalization;

namespace MadeMan.IdleEmpire.Converters;

public class ZeroToBoolConverter : IValueConverter
{
    private const double Epsilon = 0.0001;

    // Returns true if value is 0 (for "empty state" visibility)
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue == 0;
        }
        if (value is double doubleValue)
        {
            // Use epsilon comparison for floating-point precision
            return Math.Abs(doubleValue) < Epsilon;
        }
        return true;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
