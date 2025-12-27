namespace MadeMan.IdleEmpire.Views.Components;

public partial class PrestigeModal : ContentView
{
    private bool _isAnimating;

    public PrestigeModal()
    {
        InitializeComponent();

        // Subscribe to visibility changes for animation
        Overlay.PropertyChanged += OnOverlayPropertyChanged;
    }

    private async void OnOverlayPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsVisible) && Overlay.IsVisible && !_isAnimating)
        {
            await AnimateIn();
        }
    }

    private async Task AnimateIn()
    {
        _isAnimating = true;

        try
        {
            // Set initial state
            Overlay.Opacity = 0;
            ModalCard.Scale = 0.8;
            ModalCard.Opacity = 0;

            // Fade in overlay
            await Overlay.FadeToAsync(1, 200);

            // Animate card in with bounce
            await Task.WhenAll(
                ModalCard.ScaleToAsync(1.05, 250, Easing.CubicOut),
                ModalCard.FadeToAsync(1, 200)
            );
            await ModalCard.ScaleToAsync(1, 150, Easing.BounceOut);
        }
        finally
        {
            _isAnimating = false;
        }
    }
}
