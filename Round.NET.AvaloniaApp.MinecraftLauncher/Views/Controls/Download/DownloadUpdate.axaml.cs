using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Downloader;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Path = System.IO.Path;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download;

public partial class DownloadUpdate : UserControl
{
    public string URL { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Tuid { get; set; } = string.Empty;
    public DownloadUpdate()
    {
        InitializeComponent();
    }

    public void Download()
    {
        TitleLabel.Content = $"正在下载更新 {Version}";
        DownloadConfiguration downloadOpt = new()
        {
            
        };
        var downloader = new DownloadService(downloadOpt);
        downloader.DownloadFileCompleted += ((sender, args) =>
        {
            SystemMessageTaskMange.DeleteTask(Tuid);
            Process.Start(Path.GetFullPath($"../RMCL/RMCL.Update/Installer/{Version}.exe"));
            Thread.Sleep(100);
            Environment.Exit(0);
        });
        downloader.DownloadProgressChanged += ((sender, args) =>
        {
            Dispatcher.UIThread.Invoke(() => JDBar.Value = (int)args.ProgressPercentage);
            Dispatcher.UIThread.Invoke(() => JDLabel.Content = $"当前进度：{args.ProgressPercentage:0.00}%");
        });
        Directory.CreateDirectory(Path.GetFullPath($"../RMCL/RMCL.Update/Installer"));
        downloader.DownloadFileTaskAsync($"https://github.moeyy.xyz/{URL}",Path.GetFullPath($"../RMCL/RMCL.Update/Installer/{Version}.exe"));
    }
}