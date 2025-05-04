using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Windowing;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.LaunchTasks;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Windows;

public partial class LogsWindow : AppWindow
{
    public StackPanel LogsStackPanel { get; set; } = new();
    public LaunchGame LaunchJavaEdtions { get; set; }
    public bool IsOpen { get; set; } = false;
    public CountConfig Count { get; set; } = new();
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

    public void Start()
    {
        this.MainView.Content = LogsStackPanel;
        IsOpen = true;
        RefreshCount(Count);
        
    }

    private void TopLevel_OnClosed(object? sender, EventArgs e)
    {
        MainView.Content = new Grid();
    }
    public class CountConfig
    {
        public int Error = 0;
        public int Warning = 0; 
        public int StackTrace = 0; 
        public int Debug = 0; 
        public int Fatal = 0; 
        public int Info = 0; 
    }
    public void RefreshCount(CountConfig config)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            Fatal.Content = $"致命：{config.Fatal}";
            Info.Content = $"信息：{config.Info}";
            Error.Content = $"错误：{config.Error}";
            Warning.Content = $"警告：{config.Warning}";
            Debug.Content = $"调试：{config.Debug}";
            StackTrace.Content = $"堆栈：{config.StackTrace}";
        });
        Count = config;
    }

    private void KillGame_OnClick(object? sender, RoutedEventArgs e)
    {
        //LaunchJavaEdtions.KillGame_OnClick(sender, e);
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