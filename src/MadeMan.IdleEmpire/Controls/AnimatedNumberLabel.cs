using MadeMan.IdleEmpire.Utilities;

namespace MadeMan.IdleEmpire.Controls;

/// <summary>
/// A Label that animates smoothly between numeric values.
/// Core component for idle game "number go up" satisfaction.
/// </summary>
public class AnimatedNumberLabel : Label
{
    private CancellationTokenSource? _cts;
    private double _displayValue;

    #region Bindable Properties

    public static readonly BindableProperty TargetValueProperty = BindableProperty.Create(
        nameof(TargetValue),
        typeof(double),
        typeof(AnimatedNumberLabel),
        0.0,
        propertyChanged: OnTargetValueChanged);

    public static readonly BindableProperty AnimationEnabledProperty = BindableProperty.Create(
        nameof(AnimationEnabled),
        typeof(bool),
        typeof(AnimatedNumberLabel),
        true);

    public static readonly BindableProperty PrefixProperty = BindableProperty.Create(
        nameof(Prefix),
        typeof(string),
        typeof(AnimatedNumberLabel),
        "");

    public double TargetValue
    {
        get => (double)GetValue(TargetValueProperty);
        set => SetValue(TargetValueProperty, value);
    }

    public bool AnimationEnabled
    {
        get => (bool)GetValue(AnimationEnabledProperty);
        set => SetValue(AnimationEnabledProperty, value);
    }

    public string Prefix
    {
        get => (string)GetValue(PrefixProperty);
        set => SetValue(PrefixProperty, value);
    }

    #endregion

    public AnimatedNumberLabel()
    {
        _displayValue = 0;
        UpdateText();
    }

    private static void OnTargetValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AnimatedNumberLabel label)
        {
            label.AnimateToValue((double)newValue);
        }
    }

    private async void AnimateToValue(double target)
    {
        // Cancel any running animation
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        var startValue = _displayValue;
        var delta = Math.Abs(target - startValue);

        // Skip animation for tiny changes or if disabled
        if (!AnimationEnabled || delta < 0.01)
        {
            _displayValue = target;
            UpdateText();
            return;
        }

        // Smart duration based on delta size
        var duration = GetDurationForDelta(delta);
        var startTime = DateTime.UtcNow;

        try
        {
            while (!token.IsCancellationRequested)
            {
                var elapsed = (DateTime.UtcNow - startTime).TotalMilliseconds;
                var progress = Math.Min(elapsed / duration, 1.0);

                // EaseOutQuad for smooth deceleration
                var easedProgress = 1 - (1 - progress) * (1 - progress);

                _displayValue = startValue + (target - startValue) * easedProgress;

                // Update on main thread
                MainThread.BeginInvokeOnMainThread(UpdateText);

                if (progress >= 1.0)
                    break;

                // ~60fps
                await Task.Delay(16, token);
            }
        }
        catch (TaskCanceledException)
        {
            // Animation was cancelled - that's fine
        }
        finally
        {
            // Ensure final value is exact
            _displayValue = target;
            MainThread.BeginInvokeOnMainThread(UpdateText);
        }
    }

    private static int GetDurationForDelta(double delta)
    {
        // Smart duration: bigger changes = longer animation for dramatic effect
        return delta switch
        {
            < 10 => 100,      // Tiny: very fast
            < 100 => 150,     // Small: fast
            < 1000 => 200,    // Medium: normal
            < 10000 => 300,   // Large: slower
            _ => 500          // Huge (offline earnings): dramatic
        };
    }

    private void UpdateText()
    {
        Text = $"{Prefix}{NumberFormatter.FormatNumber(_displayValue)}";
    }
}
