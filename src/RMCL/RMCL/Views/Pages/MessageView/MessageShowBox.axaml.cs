using Avalonia.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;

namespace RMCL.Views.Pages.Main.MessageView;
public partial class MessageShowBox : UserControl
{
    private int _currentInfoBarCount = 0;
    private const int AutoHideDelay = 5000; // 5秒
    
    public MessageShowBox()
    {
        InitializeComponent();
    }
    
    public async void AddInfoBar(string title, string message, InfoBarSeverity severity)
    {
        var infoBar = new InfoBar
        {
            Title = title,
            Message = message,
            Severity = severity,
            IsOpen = true,
            Margin = new Thickness(0, 0, 0, 10),
            IsClosable = false
        };
        
        MessageStackPanel.Children.Add(infoBar);
        _currentInfoBarCount++;
        
        // 触发过渡动画
        infoBar.Opacity = 1;
        infoBar.RenderTransform = new TransformGroup
        {
            Children = 
            {
                new TranslateTransform { Y = 0 },
                new ScaleTransform { ScaleX = 1, ScaleY = 1 }
            }
        };

        Task.Run(() =>
        {
            Thread.Sleep(AutoHideDelay);

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                MessageStackPanel.Children.Remove(infoBar);
                _currentInfoBarCount--;
            });
        });
        
        // 如果消息太多，移除最早的消息
        if (_currentInfoBarCount > 5)
        {
            var firstItem = MessageStackPanel.Children[0];
            firstItem.Opacity = 0;
            
            Task.Delay(200).ContinueWith(_ => 
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    MessageStackPanel.Children.Remove(firstItem);
                    _currentInfoBarCount--;
                });
            });
        }
    }
}