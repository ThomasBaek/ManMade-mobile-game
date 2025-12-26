using System.Globalization;

namespace MadeMan.IdleEmpire.Converters;

public class ZeroToBoolConverter : IValueConverter
{
    // Returns true if value is 0 (for "empty state" visibility)
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            return intValue == 0;
        }
        if (value is double doubleValue)
        {
            return doubleValue == 0;
        }
        return true;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
