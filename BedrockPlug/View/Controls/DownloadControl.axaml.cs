using System.ComponentModel;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Downloader;
using FluentAvalonia.UI.Controls;
using Ionic.Zip;
using MCLauncher;
using MCLauncher.Versions;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Logs;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Message;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;

namespace BedrockPlug.View.Controls;

public partial class DownloadControl : UserControl
{
    public string Tuid { get; set; } = string.Empty;
    public VersionInfo Version { get; set; } = new ();
    public DownloadControl(VersionInfo versionInfo)
    {
        InitializeComponent();
        this.Version = versionInfo;
        TitleLabel.Content = $"下载基岩版 {versionInfo.Version}";
    }

    public void Download()
    {
        Task.Run(() =>
        {
            try
            {
                var url = Versions.GetDownloadUrl(Version.UUID, "1").Result;
                RLogs.WriteLog(url);
                Dispatcher.UIThread.Invoke(() => GETUrlJDBar.Value = 100);
                Dispatcher.UIThread.Invoke(() => JDLabel.Content = "当前进度：下载游戏文件");
                Directory.CreateDirectory(Path.GetFullPath("../RMCL/RMCL.Bedrock/Installer"));
                Directory.CreateDirectory(Path.GetFullPath("../RMCL/RMCL.Bedrock/Games"));
                var downloadOpt = new DownloadConfiguration()
                {
                    ChunkCount = 64,
                    MaxTryAgainOnFailover = 5,
                    ParallelDownload = true,
                    ParallelCount = 64
                };
                var downloader = new DownloadService(downloadOpt);
                
                var jd = 0;
                downloader.DownloadProgressChanged += (async (sender, args) =>
                {
                    jd = (int)args.ProgressPercentage;
                    RLogs.WriteLog($"Progress: {jd}");
                    Dispatcher.UIThread.InvokeAsync(() => DownloadFileJDBar.Value = jd);
                });
                downloader.DownloadFileCompleted += ((sender, args) =>
                {
                    if (jd >= 90)
                    {
                        Dispatcher.UIThread.InvokeAsync(() => DownloadFileJDBar.Value = 100);
                        Dispatcher.UIThread.Invoke(() => JDLabel.Content = "当前进度：解压原始数据");
                        Message.Show("基岩版下载插件", $"基岩版 {Version.Version} 下载完毕，即将安装...", InfoBarSeverity.Success);
                        //downloader.Dispose();
                        Task.Run(() => ExtractZipWithProgress(Path.GetFullPath($"../RMCL/RMCL.Bedrock/Installer/Minecraft_{Version.Version}.zip"), Path.GetFullPath($"../RMCL/RMCL.Bedrock/Games/Minecraft_{Version.Version}")));
                    }
                    else
                    {
                        Message.Show("基岩版下载插件", $"基岩版 {Version.Version} 下载失败！\n无法连接至服务器!", InfoBarSeverity.Error);
                        SystemMessageTaskMange.DeleteTask(this.Tuid);
                    }                   
                });
                downloader.DownloadFileTaskAsync(url, Path.GetFullPath($"../RMCL/RMCL.Bedrock/Installer/Minecraft_{Version.Version}.zip"));
            }
            catch (Exception ex)
            {
                Message.Show("基岩版下载插件", $"基岩版 {Version.Version} 下载失败！\n{ex.Message}", InfoBarSeverity.Error);
            }
        });
    }
    public void ExtractZipWithProgress(string zipPath, string extractPath)
    {
        Directory.CreateDirectory(extractPath);
        using (ZipFile zip = new ZipFile(zipPath))
        {
            zip.ExtractProgress += (sender, e) =>
            {
                if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
                {
                    // 更新进度条
                    Dispatcher.UIThread.Invoke(() => JYFileJDBar.Value=(int)((e.EntriesExtracted / (float)e.EntriesTotal) * 100));
                }
            };

            zip.ExtractAll(extractPath, ExtractExistingFileAction.OverwriteSilently);
        }
    }
}