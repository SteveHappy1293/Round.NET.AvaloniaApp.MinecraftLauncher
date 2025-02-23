using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Downloader;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;

namespace RMCLInstalledOnline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string repoUrl = "https://api.github.com/repos/Round-Studio/Round.NET.AvaloniaApp.MinecraftLauncher/releases/latest";

        public MainWindow()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                DownloadFileAsync(repoUrl);
            });
        }

        private async void DownloadFileAsync(string apiUrl)
        {
            string downloadUrl = "https://github.moeyy.xyz/"+GetDownloadUrl(apiUrl);

            if (!string.IsNullOrEmpty(downloadUrl))
            {
                await DownloadFile(downloadUrl);
            }
            else
            {
                Console.WriteLine("No suitable download URL found.");
            }
        }

        public static string GetInstallerName(string architecture)
        {
            switch (architecture)
            {
                case "arm64":
                    return "Round.NET.AvaloniaApp.MinecraftLauncher.Desktop.win.arm64.installer.exe";
                case "x64":
                    return "Round.NET.AvaloniaApp.MinecraftLauncher.Desktop.win.x64.installer.exe";
                case "x86":
                    return "Round.NET.AvaloniaApp.MinecraftLauncher.Desktop.win.x86.installer.exe";
                default:
                    return null;
            }
        }

        private string GetDownloadUrl(string apiUrl)
        {
            this.Dispatcher.Invoke(() => BZLabel.Content = $"当前进度：获取下载地址");
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36 Edg/133.0.0.0"); 
                    string response = client.GetStringAsync(apiUrl).Result;
                    var release = JsonSerializer.Deserialize<Release>(response);
                    this.Dispatcher.Invoke(() => BZLabel.Content = $"当前进度：解析地址");

                    if (release == null || release.assets == null)
                    {
                        return null;
                    }

                    // 获取当前系统架构信息
                    string arch = RuntimeInformation.ProcessArchitecture.ToString().ToLower();

                    // 遍历所有资产，寻找适用于Windows的下载链接
                    foreach (var asset in release.assets)
                    {
                        // Console.WriteLine(asset.name);
                        // 判断文件名是否包含 "win" 和对应的架构信息
                        if (asset.name.Contains("win") && asset.name.Contains(arch))
                        {
                            Console.WriteLine(asset.browser_download_url);
                            return asset.browser_download_url;
                        }
                    }
                }
                catch
                {
                    this.Dispatcher.Invoke(() => MessageBox.Show("无法获取更新地址！","错误",MessageBoxButton.OK,MessageBoxImage.Error));
                    Environment.Exit(0);
                }

                // 如果没有找到合适的下载链接，返回null
                return null;
            }
        }

        private async Task DownloadFile(string url)
        {
            string fileName = Path.GetFileName(url);
            string filePath = Path.Combine(Directory.GetCurrentDirectory()+"/RMCL/RMCL.Update", fileName);

            // 创建下载配置
            var downloadConfiguration = new DownloadConfiguration
            {
                // 设置下载配置（可选）
                Timeout = 10000, // 超时时间（毫秒）
                ParallelDownload = true, // 是否支持并行下载
            };

            // 创建下载服务
            var downloader = new DownloadService(downloadConfiguration);

            // 注册下载进度事件
            downloader.DownloadProgressChanged += (sender, args) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    JDBar.Value = args.ProgressPercentage;
                    BZLabel.Content = $"当前进度：下载更新文件 ({args.ProgressPercentage:0.00}%)";
                });
            };

            var apppath = "";
            // 开始下载
            await downloader.DownloadFileTaskAsync(url, filePath);
            // MessageBox.Show("文件下载完成！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Dispatcher.Invoke(() => BZLabel.Content = $"当前进度：启动安装");
            Dispatcher.Invoke(() =>
            {
                this.Visibility = Visibility.Hidden;
                TaskbarIcon.ShowBalloonTip("安装 RMCL", "RMCL 已开始后台安装，安装完毕将会自动开启", BalloonIcon.Info);
            });
            Thread.Sleep(100);
            var proc = Process.Start(filePath,new []{"/VERYSILENT"});
            string searchKey = "Round.NET.AvaloniaApp.MinecraftLauncher.Desktop";
            List<string> results = GetMatchingRegistryValues(searchKey);

            // 显示结果
            foreach (var result in results)
            {
                apppath = result.Replace(".ApplicationCompany", "").Replace(".FriendlyAppName", "");
            }

            proc.WaitForExit();
            Process.Start(apppath);
            Dispatcher.Invoke(() =>
            {
                TaskbarIcon.ShowBalloonTip("安装 RMCL", "RMCL 安装完毕", BalloonIcon.Info);
            });
            Thread.Sleep(100);
            Environment.Exit(0);
        }
        private List<string> GetMatchingRegistryValues(string searchKey)
        {
            List<string> results = new List<string>();
            try
            {
                using (RegistryKey baseKey = Registry.ClassesRoot.OpenSubKey(@"Local Settings\Software\Microsoft\Windows\Shell\MuiCache"))
                {
                    if (baseKey != null)
                    {
                        // 获取所有值的名称
                        string[] valueNames = baseKey.GetValueNames();
                        foreach (string valueName in valueNames)
                        {
                            // 检查值名称是否包含指定字符串
                            if (valueName.Contains(searchKey, StringComparison.OrdinalIgnoreCase))
                            {
                                object value = baseKey.GetValue(valueName);
                                if (value != null)
                                {
                                    results.Add(valueName);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("指定的注册表路径不存在");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }

            return results;
        }
    }
    public class Release
    {
        public Asset[] assets { get; set; }
    }

    public class Asset
    {
        public string browser_download_url { get; set; }
        public string name { get; set; }
    }
}