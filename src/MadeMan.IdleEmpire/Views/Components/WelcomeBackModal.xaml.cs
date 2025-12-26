namespace MadeMan.IdleEmpire.Views.Components;

public partial class WelcomeBackModal : ContentView
{
    public static readonly BindableProperty IsModalVisibleProperty =
        BindableProperty.Create(nameof(IsModalVisible), typeof(bool), typeof(WelcomeBackModal), false);

    public bool IsModalVisible
    {
        get => (bool)GetValue(IsModalVisibleProperty);
        set => SetValue(IsModalVisibleProperty, value);
    }

    public WelcomeBackModal()
    {
        InitializeComponent();
    }

    public void Show(double earnings, TimeSpan timeAway, double efficiency)
    {
        // Format time away
        var hours = (int)timeAway.TotalHours;
        var minutes = timeAway.Minutes;
        var timeText = hours > 0
            ? $"Mens du var væk i {hours}t {minutes}m"
            : $"Mens du var væk i {minutes} minutter";
        TimeAwayLabel.Text = timeText;

        // Format earnings
        EarningsLabel.Text = $"${FormatNumber(earnings)}";

        // Stats
        TimeStatLabel.Text = hours > 0 ? $"{hours}t {minutes}m" : $"{minutes}m";
        EfficiencyLabel.Text = $"{(int)(efficiency * 100)}%";

        IsModalVisible = true;
    }

    private void OnCollectClicked(object? sender, EventArgs e)
    {
        IsModalVisible = false;
    }

    private static string FormatNumber(double value)
    {
        if (value >= 1_000_000_000) return $"{value / 1_000_000_000:F2}B";
        if (value >= 1_000_000) return $"{value / 1_000_000:F2}M";
        if (value >= 1_000) return $"{value / 1_000:F2}K";
        return $"{value:F2}";
    }
}
