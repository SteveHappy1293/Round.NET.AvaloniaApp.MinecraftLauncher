using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Styling;

namespace RMCL.Controls.Panel;

public class SmoothScrollViewer : ScrollViewer
{
    private Task? _currentAnimationTask;
    private CancellationTokenSource? _currentAnimationCts;

    /// <summary>
    /// Defines the <see cref="IsSmoothScrollingEnabled"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsSmoothScrollingEnabledProperty =
        AvaloniaProperty.Register<SmoothScrollViewer, bool>(nameof(IsSmoothScrollingEnabled), true);

    /// <summary>
    /// Gets or sets whether smooth scrolling is enabled.
    /// </summary>
    public bool IsSmoothScrollingEnabled
    {
        get => GetValue(IsSmoothScrollingEnabledProperty);
        set => SetValue(IsSmoothScrollingEnabledProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="SmoothScrollDuration"/> property.
    /// </summary>
    public static readonly StyledProperty<TimeSpan> SmoothScrollDurationProperty =
        AvaloniaProperty.Register<SmoothScrollViewer, TimeSpan>(nameof(SmoothScrollDuration),
            TimeSpan.FromMilliseconds(200));

    /// <summary>
    /// Gets or sets the duration of smooth scrolling.
    /// </summary>
    public TimeSpan SmoothScrollDuration
    {
        get => GetValue(SmoothScrollDurationProperty);
        set => SetValue(SmoothScrollDurationProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="ScrollStepMultiplier"/> property.
    /// </summary>
    public static readonly StyledProperty<double> ScrollStepMultiplierProperty =
        AvaloniaProperty.Register<SmoothScrollViewer, double>(nameof(ScrollStepMultiplier), 1.0);

    /// <summary>
    /// Gets or sets the multiplier for scroll steps (affects mouse wheel scrolling).
    /// </summary>
    public double ScrollStepMultiplier
    {
        get => GetValue(ScrollStepMultiplierProperty);
        set => SetValue(ScrollStepMultiplierProperty, value);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        if (Extent.Height > Viewport.Height || Extent.Width > Viewport.Width)
        {
            var delta = e.Delta;
            var multiplier = ScrollStepMultiplier * 3; // Base multiplier for wheel

            var y = Offset.Y - (delta.Y * ScrollStepMultiplier * multiplier);
            var x = Offset.X - (delta.X * ScrollStepMultiplier * multiplier);

            if (IsSmoothScrollingEnabled)
            {
                _ = SmoothScrollTo(new Vector(x, y), SmoothScrollDuration);
            }
            else
            {
                Offset = new Vector(x, y);
            }

            e.Handled = true;
        }
    }

    public new void LineUp()
    {
        if (IsSmoothScrollingEnabled)
        {
            _ = SmoothScrollTo(new Vector(Offset.X, Offset.Y - ScrollStepMultiplier), SmoothScrollDuration);
        }
        else
        {
            base.LineUp();
        }
    }

    public new void LineDown()
    {
        if (IsSmoothScrollingEnabled)
        {
            _ = SmoothScrollTo(new Vector(Offset.X, Offset.Y + ScrollStepMultiplier), SmoothScrollDuration);
        }
        else
        {
            base.LineDown();
        }
    }

    public new void PageUp()
    {
        if (IsSmoothScrollingEnabled)
        {
            _ = SmoothScrollTo(new Vector(Offset.X, Offset.Y - Viewport.Height), SmoothScrollDuration);
        }
        else
        {
            base.PageUp();
        }
    }

    public new void PageDown()
    {
        if (IsSmoothScrollingEnabled)
        {
            _ = SmoothScrollTo(new Vector(Offset.X, Offset.Y + Viewport.Height), SmoothScrollDuration);
        }
        else
        {
            base.PageDown();
        }
    }

    public async Task ScrollToHomeAsync()
    {
        if (IsSmoothScrollingEnabled)
        {
            await SmoothScrollTo(new Vector(0, 0), SmoothScrollDuration);
        }
        else
        {
            ScrollToHome();
        }
    }

    public async Task ScrollToEndAsync()
    {
        if (IsSmoothScrollingEnabled)
        {
            await SmoothScrollTo(new Vector(0, Extent.Height - Viewport.Height), SmoothScrollDuration);
        }
        else
        {
            ScrollToEnd();
        }
    }

    private async Task SmoothScrollTo(Vector targetOffset, TimeSpan duration)
    {
        // Cancel any ongoing animation
        _currentAnimationCts?.Cancel();
        _currentAnimationCts?.Dispose();
        _currentAnimationCts = new CancellationTokenSource();

        try
        {
            var animation = new Animation
            {
                Duration = duration,
                Easing = new CubicEaseInOut(),
                Children =
                {
                    new KeyFrame
                    {
                        Setters =
                        {
                            new Setter(OffsetProperty, targetOffset)
                        },
                        KeyTime = TimeSpan.FromSeconds(1)
                    }
                }
            };

            _currentAnimationTask = animation.RunAsync(this, _currentAnimationCts.Token);
            await _currentAnimationTask;
        }
        catch (TaskCanceledException)
        {
            // Animation was canceled, ignore
        }
        finally
        {
            _currentAnimationTask = null;
            _currentAnimationCts?.Dispose();
            _currentAnimationCts = null;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == OffsetProperty &&
            IsSmoothScrollingEnabled &&
            (_currentAnimationTask?.IsCompleted ?? true))
        {
            // If offset is changed externally while smooth scrolling is enabled,
            // and no animation is running, we should animate to the new position
            var newOffset = change.GetNewValue<Vector>();
            _ = SmoothScrollTo(newOffset, SmoothScrollDuration);
        }
    }
}