using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using MinecraftLaunch;
using MinecraftLaunch.Utilities;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class InternetSetting : UserControl,IPage
{
    public void Open()
    {
        Core.MainWindow.ChangeMenuItems(new List<MenuItem>{new MenuItem{Header = "添加用户"},new MenuItem{Header = "刷新"}});
    }
    public InternetSetting()
    {
        InitializeComponent();
        this.Loaded += (_,__) =>
            RefreshInternetStatus();
        DownloadThreadsCountSlider.Value = Config.MainConfig.DownloadThreads;
        DownloadThreadsCountBox.Description = $"下载器的下载线程数量 ({(int)DownloadThreadsCountSlider.Value})：";
        IsEdit = true;
    }
    public bool IsEdit { get; set; } = false;
    private void DownloadThreadsCountSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (IsEdit)
        {
            DownloadThreadsCountBox.Description = $"下载器的下载线程数量 ({(int)DownloadThreadsCountSlider.Value})：";
            Config.MainConfig.DownloadThreads = (int)DownloadThreadsCountSlider.Value;
            Config.SaveConfig();
            
            DownloadMirrorManager.MaxThread = Config.MainConfig.DownloadThreads;
            DownloadMirrorManager.IsEnableMirror = false;
            
            HttpUtil.Initialize();
        }
    }
    public void RefreshInternetStatus()
    {
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => ReButton.IsEnabled = false);
            Dispatcher.UIThread.Invoke(() => ReButton.Content = new ProgressRing()
            {
                Height = 16,
                Width = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
            });
            
            ControlChange.ChangeLabelText(InternetStautsLabel, "检测中...", Brushes.Orange);
            ControlChange.ChangeLabelText(Github, "检测中...", Brushes.Orange);
            ControlChange.ChangeLabelText(Gitee, "检测中...", Brushes.Orange);
            ControlChange.ChangeLabelText(Curseforge, "检测中...", Brushes.Orange);
            if (IsNetworkAvailable("8.8.8.8").IsSuccess)
            {
                ControlChange.ChangeLabelText(InternetStautsLabel, "已连接互联网", Brushes.Green);
                
                var ghms = IsNetworkAvailable("api.github.com");
                if (ghms.IsSuccess)
                {
                    ControlChange.ChangeLabelText(Github, $"已连接 ({ghms.Delay} ms)", Brushes.Green);
                }
                else
                {
                    ControlChange.ChangeLabelText(Github, $"无法连接", Brushes.Red);
                }
                
                var gems = IsNetworkAvailable("gitee.com");
                if (gems.IsSuccess)
                {
                    ControlChange.ChangeLabelText(Gitee, $"已连接 ({gems.Delay} ms)", Brushes.Green);
                }
                else
                {
                    ControlChange.ChangeLabelText(Gitee, $"无法连接", Brushes.Red);
                }
                
                var cums = IsNetworkAvailable("api.curseforge.com");
                if (cums.IsSuccess)
                {
                    ControlChange.ChangeLabelText(Curseforge, $"已连接 ({cums.Delay} ms)", Brushes.Green);
                }
                else
                {
                    ControlChange.ChangeLabelText(Curseforge, $"无法连接", Brushes.Red);
                }
            }
            else
            {
                ControlChange.ChangeLabelText(InternetStautsLabel, "未连接到互联网", Brushes.Red);
                ControlChange.ChangeLabelText(Github, "无互联网连接", Brushes.Red);
                ControlChange.ChangeLabelText(Gitee, "无互联网连接", Brushes.Red);
                ControlChange.ChangeLabelText(Curseforge, "无互联网连接", Brushes.Red);
            }

            Dispatcher.UIThread.Invoke(() => ReButton.IsEnabled = true);
            Dispatcher.UIThread.Invoke(() => ReButton.Content = "重新检测");
        });
    }
    public class pingcon
    {
        public bool IsSuccess { get; set; }
        public long Delay { get; set; } = -1;
    }
    public static pingcon IsNetworkAvailable(string url)
    {
        try
        {
            using (var ping = new Ping())
            {
                // Ping Google 的 DNS 服务器
                var reply = ping.SendPingAsync(url, 3000).Result; // 超时时间为 3000 毫秒
                /*return reply.Status == IPStatus.Success;*/
                return new pingcon()
                {
                    IsSuccess = reply.Status == IPStatus.Success,
                    Delay = reply.RoundtripTime
                };
            }
        }
        catch
        {
            // 如果发生异常（如无网络连接），返回 false
            return new pingcon()
            {
                IsSuccess = false,
                Delay = -1
            };
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        RefreshInternetStatus();
    }
}