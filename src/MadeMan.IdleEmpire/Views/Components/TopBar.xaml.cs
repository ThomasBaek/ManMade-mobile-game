namespace MadeMan.IdleEmpire.Views.Components;

public partial class TopBar : ContentView
{
    public static readonly BindableProperty ShowPrestigeProgressProperty =
        BindableProperty.Create(
            nameof(ShowPrestigeProgress),
            typeof(bool),
            typeof(TopBar),
            true,
            propertyChanged: OnShowPrestigeProgressChanged);

    public bool ShowPrestigeProgress
    {
        get => (bool)GetValue(ShowPrestigeProgressProperty);
        set => SetValue(ShowPrestigeProgressProperty, value);
    }

    public TopBar()
    {
        InitializeComponent();
    }

    private static void OnShowPrestigeProgressChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TopBar topBar && newValue is bool isVisible)
        {
            topBar.PrestigeProgressSection.IsVisible = isVisible;
        }
    }
}
