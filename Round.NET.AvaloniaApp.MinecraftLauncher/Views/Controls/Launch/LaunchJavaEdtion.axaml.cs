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
using MinecraftLaunch.Classes.Interfaces;
using MinecraftLaunch.Classes.Models.Game;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Install;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;

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

    private Process GameProcess;
    private StringBuilder LogOutput = new();
    public void Launch()
    {
        Message.Show("启动游戏",$"已将游戏 {Version} 添加到启动任务中。",InfoBarSeverity.Success);
        Task.Run(() =>
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
                InstallGame.DownloadGame(Version, (_, args) =>
                {
                    Dispatcher.UIThread.Invoke(() => BQAssetsJDBar.Value = (args.Progress * 100));
                });
            }

            Dispatcher.UIThread.Invoke(() =>
            {
                BQAssetsJDBar.Value = 100;
                JDLabel.Content = "当前进度：启动游戏";
                
                bool Launched = false;
                Modules.Game.JavaEdtion.Launch.LaunchJavaEdtion.LaunchGame(Version,((o, args) =>
                {
                    LogOutput.Append($"[{args.LogType}]{args.Log}");
                    if (!Launched)
                    {
                        if (args.LogType.ToString()=="Info")
                        {
                            Launched = true;
                            Message.Show("启动游戏",$"游戏 {Version} 已启动！",InfoBarSeverity.Success);
                            GameProcess = ((IGameProcessWatcher)o).Process;
                            Dispatcher.UIThread.Invoke(() =>
                            {
                                LoadBar.IsVisible = false;
                                JDLabel.Content = "当前进度：游戏已启动";
                                LaunJDBar.Value = 100;
                                MainGrid.Children.Remove(MainPanel);
                                KillGame.IsEnabled = true;
                                MinHeight = 110;
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
                                    Dispatcher.UIThread.Invoke(() => launicon.Type = IconType.Cube);
                                });
                            });
                        }
                    }
                }),(() =>
                {
                    if (Launched)
                    {
                        Message.Show("启动游戏",$"游戏 {Version} 已退出！",InfoBarSeverity.Informational);
                        Dispatcher.UIThread.Invoke(() => SystemMessageTaskMange.DeleteTask(Tuid));
                    }
                }));
            });
        });
    }

    private void KillGame_OnClick(object? sender, RoutedEventArgs e)
    {
        // Process process = Process.GetProcessById(GameProcess.Id);
        // process.Kill(true);
        
        GameProcess.Kill(true);
    }
}