using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

public partial class InternetSetting : UserControl
{
    public InternetSetting()
    {
        InitializeComponent();
        this.Loaded += (_,__) =>
            RefreshInternetStatus();
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
            ControlChange.ChangeLabelText(Github, "Github API：检测中...", Brushes.Orange);
            ControlChange.ChangeLabelText(Gitee, "Gitee API：检测中...", Brushes.Orange);
            ControlChange.ChangeLabelText(Curseforge, "Curseforge API：检测中...", Brushes.Orange);
            if (IsNetworkAvailable("8.8.8.8").IsSuccess)
            {
                ControlChange.ChangeLabelText(InternetStautsLabel, "已连接互联网", Brushes.Green);
                
                var ghms = IsNetworkAvailable("api.github.com");
                if (ghms.IsSuccess)
                {
                    ControlChange.ChangeLabelText(Github, $"Github API：已连接 ({ghms.Delay} ms)", Brushes.Green);
                }
                else
                {
                    ControlChange.ChangeLabelText(Github, $"Github API：无法连接", Brushes.Red);
                }
                
                var gems = IsNetworkAvailable("gitee.com");
                if (gems.IsSuccess)
                {
                    ControlChange.ChangeLabelText(Gitee, $"Gitee API：已连接 ({gems.Delay} ms)", Brushes.Green);
                }
                else
                {
                    ControlChange.ChangeLabelText(Gitee, $"Gitee API：无法连接", Brushes.Red);
                }
                
                var cums = IsNetworkAvailable("api.curseforge.com");
                if (cums.IsSuccess)
                {
                    ControlChange.ChangeLabelText(Curseforge, $"Curseforge API：已连接 ({cums.Delay} ms)", Brushes.Green);
                }
                else
                {
                    ControlChange.ChangeLabelText(Curseforge, $"Curseforge API：无法连接", Brushes.Red);
                }
            }
            else
            {
                ControlChange.ChangeLabelText(InternetStautsLabel, "未连接到互联网", Brushes.Red);
                ControlChange.ChangeLabelText(Github, "Github API：无互联网连接", Brushes.Red);
                ControlChange.ChangeLabelText(Gitee, "Gitee API：无互联网连接", Brushes.Red);
                ControlChange.ChangeLabelText(Curseforge, "Curseforge API：无互联网连接", Brushes.Red);
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