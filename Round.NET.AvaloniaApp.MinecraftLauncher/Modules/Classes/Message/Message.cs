using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Info;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;

public class Message
{
    public class MessageEntry
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public Control Control { get; set; }
        public InfoBarSeverity Type { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }

    public static int MaxInfoCount { get; set; } = 3;
    public static List<MessageEntry> Messages { get; private set; } = new();
    private static List<MessageInfoBox> CloseMessages = new();
    public static void Show(string title, string message, InfoBarSeverity type,Control control = null)
    {
        Messages.Add(new MessageEntry { Message = message, Title = title, Type = type });
        Dispatcher.UIThread.Invoke(() =>
        {
            var messagebox = new MessageInfoBox(message,title)
            {
                Transitions = new Transitions
                {
                    // 添加时的动画
                    new ThicknessTransition
                    {
                        Property = InfoBar.MarginProperty,
                        Duration = TimeSpan.FromSeconds(0.5),
                        Easing = new ExponentialEaseOut()
                    },
                    // 添加时的透明度动画
                    new DoubleTransition
                    {
                        Property = InfoBar.OpacityProperty,
                        Duration = TimeSpan.FromSeconds(0),
                        Easing = new ExponentialEaseOut()
                    }
                },
            };

            void close(MessageInfoBox infbox)
            {
                CloseMessages.Add(infbox);
                Task.Run(() =>
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        // infbox.Opacity = 0;
                        infbox.Margin = new Thickness(285, 5, -280, 5);
                    });
                    Thread.Sleep(400);
                    Dispatcher.UIThread.Invoke(() =>
                        Core.SystemMessage.MessageBox.Children.Remove(infbox));
                    CloseMessages.Remove(infbox);
                });
            }

            /*messagebox.Closed += (_, _) =>
            {
                messagebox.IsOpen = true;
                close();
            };*/

            // 设置初始状态
            messagebox.Opacity = 0;
            // messagebox.Margin = new Thickness(-200, 5, 200, 5); // 初始位置在上方隐藏
            messagebox.Margin = new Thickness(285, 5, -280, 5);

            // 添加到 StackPanel
            // Core.SystemMessage.MessageBox.Children.Insert(0, messagebox);
            Core.SystemMessage.MessageBox.Children.Add(messagebox);
            if (Core.SystemMessage.MessageBox.Children.Count >= MaxInfoCount)
            {
                // 找到第一个未被标记为关闭的消息框
                var oldestMessage = Core.SystemMessage.MessageBox.Children
                    .OfType<MessageInfoBox>()
                    .FirstOrDefault(msg => !CloseMessages.Contains(msg));
    
                if (oldestMessage != null)
                {
                    close(oldestMessage);
                }
            }

            // 动画：显示
            messagebox.Opacity = 1;
            messagebox.Margin = new Thickness(5);

            // 延迟 5 秒后删除
            Task.Run(() =>
            {
                Thread.Sleep(Config.Config.MainConfig.MessageLiveTimeMs);
                close(messagebox);
            });
        });
    }
}