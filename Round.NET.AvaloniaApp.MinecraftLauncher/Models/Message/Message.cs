using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Models.Message;

public class Message
{
    public static void Show(string title, string message, InfoBarSeverity type)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            var messagebox = new InfoBar()
            {
                Title = title,
                Message = message,
                IsOpen = true,
                Severity = type,
                Margin = new Thickness(5),
                IsClosable = true,
                Transitions = new Transitions
                {
                    // 添加时的动画
                    new ThicknessTransition
                    {
                        Property = InfoBar.MarginProperty,
                        Duration = TimeSpan.FromSeconds(0.5),
                        Easing = new ExponentialEaseInOut()
                    },
                    // 添加时的透明度动画
                    new DoubleTransition
                    {
                        Property = InfoBar.OpacityProperty,
                        Duration = TimeSpan.FromSeconds(0.5),
                        Easing = new QuarticEaseInOut()
                    }
                }
            };

            void close()
            {
                Task.Run(() =>
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        messagebox.Opacity = 0;
                        messagebox.Margin = new Thickness(5, -55, 5, 5);
                    });
                    Thread.Sleep(400);
                    Dispatcher.UIThread.Invoke(() =>
                        Core.SystemMessage.MessageBox.Children.Remove(messagebox));
                });
            }

            messagebox.Closed += (_, _) =>
            {
                messagebox.IsOpen = true;
                close();
            };

            // 设置初始状态
            messagebox.Opacity = 0;
            messagebox.Margin = new Thickness(5, -50, 5, 5); // 初始位置在上方隐藏

            // 添加到 StackPanel
            Core.SystemMessage.MessageBox.Children.Add(messagebox);

            // 动画：显示
            messagebox.Opacity = 1;
            messagebox.Margin = new Thickness(5);

            // 延迟 5 秒后删除
            Task.Run(() =>
            {
                Thread.Sleep(5000);
                close();
            });
        });
    }
}