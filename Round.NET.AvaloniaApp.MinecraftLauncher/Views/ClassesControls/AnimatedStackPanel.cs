using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Threading.Tasks;
using Avalonia.Styling;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Enum;

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
                nameof(ItemDelay), TimeSpan.FromMilliseconds(10));

        public static readonly StyledProperty<bool> IsAnimationEnabledProperty =
            AvaloniaProperty.Register<AnimatedStackPanel, bool>(
                nameof(IsAnimationEnabled), true);
        
        public static readonly StyledProperty<AnimationDirectionEnum> AnimationDirectionProperty =
            AvaloniaProperty.Register<AnimatedStackPanel, AnimationDirectionEnum>(
                nameof(AnimationDirection),Modules.Enum.AnimationDirectionEnum.Right);

        public TimeSpan AnimationDuration
        {
            get => GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }
        public AnimationDirectionEnum AnimationDirection
        {
            get => GetValue(AnimationDirectionProperty);
            set => SetValue(AnimationDirectionProperty, value);
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

        private TranslateTransform GetTranslateTransform()
        {
            return AnimationDirection switch
            {
                AnimationDirectionEnum.Right => new TranslateTransform(){X = 50, Y = 0},
                AnimationDirectionEnum.Left => new TranslateTransform(){X = -50, Y = 0},
                AnimationDirectionEnum.Top => new TranslateTransform(){X = 0, Y = -50},
                AnimationDirectionEnum.Bottom => new TranslateTransform(){X = 0, Y = 50},
            };
        }
        private async Task AnimateControlEntrance(Control control, TimeSpan delay)
        {
            // 确保控件初始状态可见
            control.Opacity = 1;
            control.RenderTransform = new TranslateTransform { X = 0 };
    
            // 设置动画初始状态
            control.Opacity = 0;
            control.RenderTransform = GetTranslateTransform();
    
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
                            new Setter(TranslateTransform.XProperty, GetTranslateTransform().X),
                            new Setter(TranslateTransform.YProperty, GetTranslateTransform().Y)
                        }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1),
                        Setters =
                        {
                            new Setter(OpacityProperty, 1.0),
                            new Setter(TranslateTransform.XProperty, 0.0),
                            new Setter(TranslateTransform.YProperty, 0.0)
                        }
                    }
                }
            };
    
            await animation.RunAsync(control);
    
            // 确保最终状态正确
            control.Opacity = 1;
            control.RenderTransform = new TranslateTransform { X = 0 };
        }
    }
}