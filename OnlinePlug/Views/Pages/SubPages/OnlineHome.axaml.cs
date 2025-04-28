using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;

namespace OnlinePlug.Views.Pages.SubPages;

public partial class OnlineHome : UserControl
{
    public OnlineHome()
    {
        InitializeComponent();
    }
    public List<MinecraftServerInfo> Servers { get; set; } = new();
    private async void RefreshTheInstance_OnClick(object? sender, RoutedEventArgs e)
    {
        InstanceBox.Items.Clear();
        InstanceBox.PlaceholderText = "扫描中...";
        RefreshTheInstance.Content = new ProgressRing()
        {
            Width = 16,
            Height = 16
        };
        RefreshTheInstance.IsEnabled = false;
        try
        {
            using var client = new MinecraftClient();
            var ls = await client.PerformSingleScanAsync();
            Servers = ls;
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (ls.Count == 0) InstanceBox.PlaceholderText = "当前无实例，请确保游戏以打开局域网。";
                else
                {
                    InstanceBox.PlaceholderText = "请选择一个实例";
                    foreach (var c in ls)
                    {
                        InstanceBox.Items.Add(new TextBlock(){Text = $"{c.Motd} 端口：{c.Port}",Foreground = Brushes.White,HorizontalAlignment = HorizontalAlignment.Left});
                    }
                }
            });
        }catch{ }
        RefreshTheInstance.IsEnabled = true;
        RefreshTheInstance.Content = "刷新";
    }

    private void CreatRoom_OnClick(object? sender, RoutedEventArgs e)
    {
        var cret = new CreateRoom();
        cret.Port = Servers[InstanceBox.SelectedIndex].Port;
        cret.RoomName = Servers[InstanceBox.SelectedIndex].Motd;
        Main.MainPage.MainFrame.Content = cret;
        cret.Start();
    }
}