namespace MadeMan.IdleEmpire.Controls;

/// <summary>
/// A ProgressBar that smoothly animates to its target value instead of jumping.
/// </summary>
public class AnimatedProgressBar : ProgressBar
{
    public static readonly BindableProperty TargetProgressProperty =
        BindableProperty.Create(
            nameof(TargetProgress),
            typeof(double),
            typeof(AnimatedProgressBar),
            0.0,
            propertyChanged: OnTargetProgressChanged);

    public static readonly BindableProperty AnimationDurationProperty =
        BindableProperty.Create(
            nameof(AnimationDuration),
            typeof(uint),
            typeof(AnimatedProgressBar),
            (uint)400);

    public double TargetProgress
    {
        get => (double)GetValue(TargetProgressProperty);
        set => SetValue(TargetProgressProperty, value);
    }

    public uint AnimationDuration
    {
        get => (uint)GetValue(AnimationDurationProperty);
        set => SetValue(AnimationDurationProperty, value);
    }

    private static void OnTargetProgressChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AnimatedProgressBar bar && newValue is double target)
        {
            // Cancel any ongoing animation and start new one
            bar.AbortAnimation("ProgressAnimation");

            // If progress resets (cycle complete), don't animate - just jump
            if (target < bar.Progress && target < 0.1)
            {
                bar.Progress = target;
                return;
            }

            // Animate smoothly to target
            bar.ProgressTo(target, bar.AnimationDuration, Easing.Linear);
        }
    }
}
