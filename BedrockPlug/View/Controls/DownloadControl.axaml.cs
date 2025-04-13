using System.ComponentModel;
using System.Diagnostics;
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
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        if (jd != DownloadFileJDBar.Value)
                        {
                            RLogs.WriteLog($"Progress: {jd}");
                            Dispatcher.UIThread.InvokeAsync(() => DownloadFileJDBar.Value = jd);
                        }
                    });
                });
                downloader.DownloadFileCompleted += ((sender, args) =>
                {
                    if (jd >= 90)
                    {
                        Dispatcher.UIThread.InvokeAsync(() => DownloadFileJDBar.Value = 100);
                        Dispatcher.UIThread.Invoke(() => JDLabel.Content = "当前进度：解压原始数据");
                        Message.Show("基岩版下载插件", $"基岩版 {Version.Version} 下载完毕，即将安装...", InfoBarSeverity.Success);
                        //downloader.Dispose();
                        Task.Run(() =>
                        {
                            ExtractZipWithProgress(
                                Path.GetFullPath($"../RMCL/RMCL.Bedrock/Installer/Minecraft_{Version.Version}.zip"),
                                Path.GetFullPath($"../RMCL/RMCL.Bedrock/Games/Minecraft_{Version.Version}"));
                            
                            Dispatcher.UIThread.Invoke(() => JDLabel.Content = "当前进度：安装基岩版");
                            install(Path.GetFullPath($"../RMCL/RMCL.Bedrock/Games/Minecraft_{Version.Version}"));
                            Dispatcher.UIThread.Invoke(() => SystemMessageTaskMange.DeleteTask(this.Tuid));
                        });
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
                SystemMessageTaskMange.DeleteTask(this.Tuid);
            }
        });
    }

    public void install(string path)
    {
        string appNameToRemove = "Microsoft.Minecraft";
        string newAppManifestPath = $@"{path}\AppxManifest.xml";

        try
        {
            // 1. 删除现有的 Minecraft UWP 应用
            RemoveAppPackage(appNameToRemove);
            Dispatcher.UIThread.Invoke(() => InstallJDBar.Value = 40);

            // 2. 安装新的应用
            if(File.Exists($"{path}/AppxSignature.p7x")) File.Delete($"{path}/AppxSignature.p7x");
            Dispatcher.UIThread.Invoke(() => InstallJDBar.Value = 50);
            InstallNewAppPackage(Path.GetFullPath($"{path}/AppxSignature.p7x"));
            Dispatcher.UIThread.Invoke(() => InstallJDBar.Value = 100);

            RLogs.WriteLog("应用替换操作完成！");
        }
        catch (Exception ex)
        {
            RLogs.WriteLog($"操作过程中发生错误: {ex.Message}");
        }
    }
    public void ExtractZipWithProgress(string zipPath, string extractPath)
    {
        Directory.CreateDirectory(extractPath);
        using (ZipFile zip = new ZipFile(zipPath))
        {
            try
            {
                zip.ExtractProgress += (sender, e) =>
                {
                    if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
                    {
                        // 更新进度条
                        Dispatcher.UIThread.Invoke(() => JYFileJDBar.Value=(int)((e.EntriesExtracted / (float)e.EntriesTotal) * 100));
                        RLogs.WriteLog($"解压进度：{(int)((e.EntriesExtracted / (float)e.EntriesTotal) * 100)}");
                        if ((int)((e.EntriesExtracted / (float)e.EntriesTotal) * 100) >= 95)
                        {
                        
                        }
                    }
                };

                zip.ExtractAll(extractPath, ExtractExistingFileAction.OverwriteSilently);
            }catch
            {
                Message.Show("基岩版插件","解压元数据出错！正在重试...",InfoBarSeverity.Error);
                ExtractZipWithProgress(zipPath, extractPath);
            }
        }
    }
    
    static void RemoveAppPackage(string appName)
    {
        RLogs.WriteLog($"正在查找并删除 {appName} 相关的软件包...");

        // 使用 PowerShell 命令获取所有匹配的包
        string command = $"Get-AppxPackage *{appName}* | Remove-AppxPackage";

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -ExecutionPolicy unrestricted -Command \"{command}\"",
            Verb = "runas",
            UseShellExecute = false,
            CreateNoWindow = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using (Process process = new Process { StartInfo = psi })
        {
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"删除软件包失败: {error}");
            }

            RLogs.WriteLog(output);
        }
    }

    static void InstallNewAppPackage(string manifestPath)
    {
        RLogs.WriteLog($"正在安装新的应用包: {manifestPath}");
        string command = $"Add-AppxPackage -register \"{manifestPath}\"";

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -ExecutionPolicy unrestricted -Command \"{command}\"",
            Verb = "runas",
            UseShellExecute = false,
            CreateNoWindow = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using (Process process = new Process { StartInfo = psi })
        {
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"安装新软件包失败: {error}");
            }

            RLogs.WriteLog(output);
        }
    }
}