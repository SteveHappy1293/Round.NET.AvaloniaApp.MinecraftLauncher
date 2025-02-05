using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch.Classes.Models.Download;
using MinecraftLaunch.Components.Installer;
using MinecraftLaunch.Components.Resolver;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Install;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.DownloadGame;

public partial class DownloadGame : UserControl
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
    public DownloadGame()
    {
        InitializeComponent();
    }

    public void StartDownload()
    {
        Message.Show("下载任务","下载任务已添加至后台。",InfoBarSeverity.Success);
        Task.Run(() =>
        {
            var result = InstallGame.DownloadGame(Version, (_, args) =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    JDBar.Value = (args.Progress * 100);
                    JDLabel.Content = $"当前进度：{args.Progress * 100:0.00} %";
                });
            });
            Dispatcher.UIThread.Invoke(() => SystemMessageTaskMange.DeleteTask(Tuid));

            if (result)
            {
                Message.Show("下载任务",$"游戏核心 {Version} 安装成功。",InfoBarSeverity.Success);
            }
            else
            {
                Message.Show("下载任务",$"游戏核心 {Version} 安装失败。",InfoBarSeverity.Error);
            }
        });
    }
}