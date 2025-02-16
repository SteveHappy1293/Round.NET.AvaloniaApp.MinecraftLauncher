using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData;
using FluentAvalonia.UI.Controls;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
using Microsoft.VisualBasic.CompilerServices;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Install;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Windows;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch;

public partial class LaunchJavaEdtion : UserControl
{
    private string _version;

    public string Version
    {
        get => _version;
        set
        {
            _version = value;
            this.TitleLabel.Content = TitleLabel.Content.ToString().Replace("{Version}",_version);
        }
    }
    public string Tuid { get; set; } = string.Empty;
    public LaunchJavaEdtion()
    {
        InitializeComponent();
    }

    private int Error = 0;
    private int Warning = 0; 
    private int StackTrace = 0; 
    private int Debug = 0; 
    private int Fatal = 0; 
    private int Info = 0; 

    private Process GameProcess;
    private StringBuilder LogOutput = new();
    private Thread GameThread;
    private bool UserExit = false;
    private StackPanel LogPanel = new();
    
    LogsWindow window = new LogsWindow();
    public void Launch()
    {
        Message.Show("启动游戏",$"已将游戏 {Version} 添加到启动任务中。",InfoBarSeverity.Success);
        GameThread = new Thread(() =>
        {
            try
            {
                bool assets = Modules.Game.JavaEdtion.Launch.LaunchJavaEdtion.ResourceCompletion(Version);
                Dispatcher.UIThread.Invoke(() => JCAssetsJDBar.Value = 100);
                if (!assets)
                {

                    Dispatcher.UIThread.Invoke(() =>
                    {
                        JCAssetsJDBar.Value = 100;
                        JDLabel.Content = "当前进度：补全资源文件";
                    });
                }
            }
            catch (Exception ex)
            {
                Dispatcher.UIThread.Invoke(() => SystemMessageTaskMange.DeleteTask(Tuid));
                Message.Show("启动游戏",$"游戏 {Version} 启动失败：{ex.Message}",InfoBarSeverity.Error);
                return;
            }

            Dispatcher.UIThread.Invoke(() =>
            {
                BQAssetsJDBar.Value = 100;
                JDLabel.Content = "当前进度：启动游戏";
                
                bool Launched = false;
                Task.Run(() =>
                {
                    Modules.Game.JavaEdtion.Launch.LaunchJavaEdtion.LaunchGame(Version, (sender) =>
                    {
                        GameProcess = sender;
                    }, ((o, args) =>
                    {
                        LogOutput.Append($"[{args.Data.LogLevel}]{args.Data.Log}");
                        Task.Run(() =>
                        {
                            Dispatcher.UIThread.Invoke(() =>
                            {
                                var label = GetLogLabel(args.Data.LogLevel, args.Data.Log);
                                label.Margin = new Thickness(50, 0);
                                LogPanel.Children.Add(label);
                                label.Opacity = 1;
                                label.Margin = new Thickness(0, 0);
                            });
                        });
                        if (!Launched)
                        {
                            if (args.Data.LogLevel == MinecraftLogLevel.Info)
                            {
                                Launched = true;
                                Message.Show("启动游戏",
                                    $"游戏 {Version} 已启动！\n账户名称：{Modules.Game.User.User.GetAccount(Modules.Game.User.User.Users[Modules.Config.Config.MainConfig.SelectedUser].UUID).Name}\n账户模式：{Modules.Game.User.User.GetAccount(Modules.Game.User.User.Users[Modules.Config.Config.MainConfig.SelectedUser].UUID).Type}",
                                    InfoBarSeverity.Success);
                                GameProcess = ((MinecraftProcess)o).Process;
                                Dispatcher.UIThread.Invoke(() => LaunJDBar.Value = 100);
                                Thread.Sleep(300);
                                Dispatcher.UIThread.Invoke(() =>
                                {
                                    LoadBar.IsVisible = false;
                                    JDLabel.Content = "当前进度：游戏已启动";
                                    MainGrid.Children.Remove(MainPanel);
                                    KillGame.Width = 120;
                                    Task.Run(() =>
                                    {
                                        Thread.Sleep(300);
                                        Dispatcher.UIThread.Invoke(() =>
                                        {
                                            MinHeight = 110;
                                            KillGame.Content = "结束游戏进程";
                                            LogButton.IsEnabled = true;
                                            var launicon = new HeroIcon()
                                            {
                                                Foreground = Brushes.White,
                                                Type = IconType.RocketLaunch,
                                                Margin = new Thickness(18),
                                                HorizontalAlignment = HorizontalAlignment.Right,
                                                VerticalAlignment = VerticalAlignment.Top,
                                                Min = true,
                                                Opacity = 1
                                            };
                                            MainGrid.Children.Add(launicon);
                                            Task.Run(() =>
                                            {
                                                Thread.Sleep(1000);
                                                Dispatcher.UIThread.Invoke(() => launicon.Opacity = 0);
                                                Thread.Sleep(300);
                                                Dispatcher.UIThread.Invoke(() => launicon.Opacity = 1);
                                                Dispatcher.UIThread.Invoke(() => launicon.Type = IconType.Check);
                                                Thread.Sleep(1000);
                                                Dispatcher.UIThread.Invoke(() => launicon.Opacity = 0);
                                                Thread.Sleep(300);
                                                Dispatcher.UIThread.Invoke(() => launicon.Opacity = 1);
                                                Dispatcher.UIThread.Invoke(() => launicon.Type = IconType.Eye);
                                                Dispatcher.UIThread.Invoke(() => JDLabel.Content = "监控游戏中...");
                                            });
                                        });
                                    });
                                });
                            }
                        }
                    }), (() =>
                    {
                        if (Launched)
                        {
                            Message.Show("启动游戏", $"游戏 {Version} 已退出！", InfoBarSeverity.Informational);
                            Dispatcher.UIThread.Invoke(() => SystemMessageTaskMange.DeleteTask(Tuid));
                            window.TheCallBackIsInvalid();
                        }
                        else
                        {
                            if (!UserExit)
                            {
                                Message.Show("启动游戏", $"游戏 {Version} 启动失败！日志：\n{LogOutput.ToString()}",
                                    InfoBarSeverity.Error);
                                Dispatcher.UIThread.Invoke(() => SystemMessageTaskMange.DeleteTask(Tuid));
                                window.TheCallBackIsInvalid();
                            }
                        }
                    }));
                });
            });
        });
        
        GameThread.Start();
    }

    public void KillGame_OnClick(object? sender, RoutedEventArgs e)
    {
        UserExit = true;
        if (KillGame.Content == "取消启动")
        {
            try
            {
                GameProcess.Kill(true);
            }catch{ }
            Dispatcher.UIThread.Invoke(() => SystemMessageTaskMange.DeleteTask(Tuid));
            Message.Show("启动游戏",$"游戏 {Version} 的启动任务已取消！",InfoBarSeverity.Warning);
            
            window.TheCallBackIsInvalid();
        }
        else
        {
            GameProcess.Kill(true);
            window.TheCallBackIsInvalid();
        }
    }
    private TextBlock GetLogLabel(MinecraftLogLevel logtype, string log)
    {
        var label = new TextBlock()
        {
            Text = $"[{logtype}]{log}",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            Opacity = 0
        };
        switch (logtype)
        {
            case MinecraftLogLevel.Error:
                label.Foreground = Brushes.Orange;
                Error++;
                break;
            case MinecraftLogLevel.Warning:
                label.Foreground = Brushes.Yellow;
                Warning++;
                break;
            case MinecraftLogLevel.StackTrace:
                label.Foreground = Brushes.HotPink;
                StackTrace++;
                break;
            case MinecraftLogLevel.Debug:
                label.Foreground = Brushes.Green;
                Debug++;
                break;
            case MinecraftLogLevel.Fatal:
                label.Foreground = Brushes.Red;
                Fatal++;
                break;
            default:
                label.Foreground = Brushes.White;
                Info++;
                break;
        }
        window.RefreshCount(new LogsWindow.CountConfig()
        {
            Debug = Debug,
            Error = Error,
            Info = Info,
            Warning = Warning,
            StackTrace = StackTrace,
            Fatal = Fatal
        });
        return label;
    }
    private void LogButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (window.IsOpen)
        {
            window.Close();
        }
        window = new LogsWindow();
        window.LogsStackPanel = LogPanel;
        window.LaunchJavaEdtions = this;
        window.Show();
        window.Start();window.RefreshCount(new LogsWindow.CountConfig()
        {
            Debug = Debug,
            Error = Error,
            Info = Info,
            Warning = Warning,
            StackTrace = StackTrace,
            Fatal = Fatal
        });
    }
}