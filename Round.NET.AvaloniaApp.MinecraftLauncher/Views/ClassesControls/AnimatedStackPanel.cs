using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Threading.Tasks;
using Avalonia.Styling;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.CustomControls
{
    public class AnimatedStackPanel : StackPanel
    {
        // 定义可绑定的动画属性
        public static readonly StyledProperty<TimeSpan> AnimationDurationProperty =
            AvaloniaProperty.Register<AnimatedStackPanel, TimeSpan>(
                nameof(AnimationDuration), TimeSpan.FromMilliseconds(300));

        public static readonly StyledProperty<TimeSpan> ItemDelayProperty =
            AvaloniaProperty.Register<AnimatedStackPanel, TimeSpan>(
                nameof(ItemDelay), TimeSpan.FromMilliseconds(100));

        public static readonly StyledProperty<bool> IsAnimationEnabledProperty =
            AvaloniaProperty.Register<AnimatedStackPanel, bool>(
                nameof(IsAnimationEnabled), true);

        public TimeSpan AnimationDuration
        {
            get => GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }

        public TimeSpan ItemDelay
        {
            get => GetValue(ItemDelayProperty);
            set => SetValue(ItemDelayProperty, value);
        }

        public bool IsAnimationEnabled
        {
            get => GetValue(IsAnimationEnabledProperty);
            set => SetValue(IsAnimationEnabledProperty, value);
        }

        protected override async void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            
            if (IsAnimationEnabled && Children.Count > 0)
            {
                await AnimateChildrenEntrance();
            }
        }

        private async Task AnimateChildrenEntrance()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] is Control control)
                {
                    await AnimateControlEntrance(control, ItemDelay * i);
                }
            }
        }
        // 在 AnimatedStackPanel 类中添加
        protected override void ChildrenChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.ChildrenChanged(sender, e);
    
            if (IsAnimationEnabled && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is Control control)
                    {
                        _ = AnimateControlEntrance(control, ItemDelay * (Children.Count - 1));
                    }
                }
            }
        }
        private async Task AnimateControlEntrance(Control control, TimeSpan delay)
        {
            // 确保控件初始状态可见
            control.Opacity = 1;
            control.RenderTransform = new TranslateTransform { Y = 0 };
    
            // 设置动画初始状态
            control.Opacity = 0;
            control.RenderTransform = new TranslateTransform { Y = 50 };
    
            await Task.Delay(delay);
    
            var animation = new Animation
            {
                Duration = AnimationDuration,
                Easing = new CubicEaseOut(),
                FillMode = FillMode.Forward, // 关键设置：保持动画结束状态
                Children =
                {
                    new KeyFrame
                    {
                        Cue = new Cue(0),
                        Setters =
                        {
                            new Setter(OpacityProperty, 0.0),
                            new Setter(TranslateTransform.YProperty, 20.0)
                        }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters =
                        {
                            new Setter(OpacityProperty, 1.0),
                            new Setter(TranslateTransform.YProperty, 0.0)
                        }
                    }
                }
            };
    
            await animation.RunAsync(control);
    
            // 确保最终状态正确
            control.Opacity = 1;
            control.RenderTransform = new TranslateTransform { Y = 0 };
        }
    }
}