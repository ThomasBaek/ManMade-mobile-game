namespace MadeMan.IdleEmpire.Utilities;

/// <summary>
/// Centralized number formatting utility to ensure consistent display across the app.
/// </summary>
public static class NumberFormatter
{
    /// <summary>
    /// Formats a currency value with $ prefix and appropriate suffix (K, M, B).
    /// Example: 1234567.89 -> "$1.23M"
    /// </summary>
    public static string FormatCurrency(double value)
    {
        if (value >= 1_000_000_000) return $"${value / 1_000_000_000:F2}B";
        if (value >= 1_000_000) return $"${value / 1_000_000:F2}M";
        if (value >= 1_000) return $"${value / 1_000:F2}K";
        return $"${value:F2}";
    }

    /// <summary>
    /// Formats a number without $ prefix, suitable for costs and yields.
    /// Example: 1234567 -> "1.23M"
    /// </summary>
    public static string FormatNumber(double value)
    {
        if (value >= 1_000_000_000) return $"{value / 1_000_000_000:F2}B";
        if (value >= 1_000_000) return $"{value / 1_000_000:F2}M";
        if (value >= 1_000) return $"{value / 1_000:F2}K";
        return $"{value:F0}";
    }

    /// <summary>
    /// Formats a number with $ prefix but shorter precision for compact displays.
    /// Example: 1234 -> "$1.2K"
    /// </summary>
    public static string FormatCompact(double value)
    {
        if (value >= 1_000_000) return $"${value / 1_000_000:F1}M";
        if (value >= 1_000) return $"${value / 1_000:F1}K";
        return $"${value:F0}";
    }
}
