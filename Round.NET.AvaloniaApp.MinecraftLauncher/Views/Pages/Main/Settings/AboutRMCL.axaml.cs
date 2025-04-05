using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class AboutRMCL : UserControl
{
    public AboutRMCL()
    {
        InitializeComponent();

        this.Loaded += (sender, args) =>
        {
            ConfigPath.Text = Path.GetFullPath("../RMCL/RMCL.Config");
            Assembly assembly = Assembly.GetExecutingAssembly();

            // 获取程序集的版本信息
            Version version = assembly.GetName().Version;
            RMCLVersion.Content = $"RMCL v{version.ToString()}";
            ConfigSize.Text = $"{GetDirectorySize(Path.GetFullPath("../RMCL/RMCL.Config")):0.01} Kb";
        };
    }
    public static long GetDirectorySize(string folderPath)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
        return (long)(dirInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length)*0.001);
    }
    public static void OpenFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"目录不存在: {folderPath}");
        }

        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("explorer.exe", folderPath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", folderPath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", folderPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"无法打开文件夹: {ex.Message}");
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenFolder(Path.GetFullPath("../RMCL/RMCL.Config"));
    }

    private void Button_OnClick1(object? sender, RoutedEventArgs e)
    {
        ((Button)sender).Content = new ProgressRing()
        {
            Width = 20,
            Height = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        ((Button)sender).IsEnabled = false;
        Task.Run(() =>
            {
                var downloader = new Updater((v, s) =>
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();

                    // 获取程序集的版本信息
                    Version version = assembly.GetName().Version;
                    if (v.Replace("v", "").Replace("0", "").Replace(".", "") !=
                        version.ToString().Replace(".", "").Replace("0", ""))
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            var con = new ContentDialog()
                            {
                                PrimaryButtonText = "取消",
                                CloseButtonText = "现在更新",
                                Title = $"更新 RMCL3 - {v.Replace("0", "")}",
                                DefaultButton = ContentDialogButton.Close,
                                Content = new StackPanel()
                                {
                                    Children =
                                    {
                                        new Label()
                                        {
                                            Content = "你好！打扰一下~\nRMCL当前有个更新，需要花费您一些时间，请问您是否更新？"
                                        },
                                        new Label()
                                        {
                                            Content = $"当前版本：v{version.ToString().Replace(".", "").Replace("0", "")}"
                                        },
                                        new Label()
                                        {
                                            Content = $"更新版本：{v.Replace(".", "").Replace("0", "")}"
                                        }
                                    }
                                }
                            };
                            con.CloseButtonClick += (_, __) =>
                            {
                                var dow = new DownloadUpdate();
                                dow.Tuid = SystemMessageTaskMange.AddTask(dow);
                                dow.URL = s;
                                dow.Version = v.Replace(".", "").Replace("0", "");
                                dow.Download();
                            };
                            con.ShowAsync(Core.MainWindow);
                            Dispatcher.UIThread.InvokeAsync(async () =>
                            {
                                ((Button)sender).Content = "检查更新";
                                ((Button)sender).IsEnabled = true;
                            });
                        });
                    }
                    
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        ((Button)sender).Content = "检查更新";
                        ((Button)sender).IsEnabled = true;
                    });
                });
                
                downloader.GetDownloadUrlAsync(
                    "https://api.github.com/repos/Round-Studio/Round.NET.AvaloniaApp.MinecraftLauncher/releases");
            });
    }
}