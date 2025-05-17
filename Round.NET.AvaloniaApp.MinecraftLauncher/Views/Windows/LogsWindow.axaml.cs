using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Windowing;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Windows;

public partial class LogsWindow : AppWindow
{
    public StackPanel LogsStackPanel { get; set; } = new();
    public LaunchJavaEdtion LaunchJavaEdtions { get; set; }
    public bool IsOpen { get; set; } = false;
    public LogsWindow()
    {
        InitializeComponent();
        var num = 0;
        Task.Run(() =>
        {
            while (true)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    if (LogsStackPanel.Children.Count != num)
                    {
                        MainView.ScrollToEnd();
                        num = LogsStackPanel.Children.Count;
                    }
                });
                Thread.Sleep(10);
            }
        });
    }

    public void Start(StringBuilder logs)
    {
        this.MainView.Content = LogsStackPanel;
        IsOpen = true;
        RefreshCount(logs);
        
    }

    private void TopLevel_OnClosed(object? sender, EventArgs e)
    {
        MainView.Content = new Grid();
    }
    public void RefreshCount(StringBuilder logs)
    {
        
    }

    private void KillGame_OnClick(object? sender, RoutedEventArgs e)
    {
        LaunchJavaEdtions.KillGame_OnClick(sender, e);
    }
    public void TheCallBackIsInvalid()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            InfoBox.Content = "此会话已失效";
            KillGame.IsEnabled = false;
        });
    }
}