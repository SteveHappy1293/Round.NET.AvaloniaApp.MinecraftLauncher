using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using Flurl.Util;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Java;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Settings;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Setting : UserControl
{
    public Setting()
    {
        InitializeComponent();
        Core.SettingPage = this;
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = MySelfSetting,
            Title = "本体设置",
            Route = "MySelfSetting"
        });    
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = GameSetting,
            Title = "游戏设置",
            Route = "GameSetting"
        });  
    }
    private GameSetting GameSetting { get; set; } = new();
    private MySelfSetting MySelfSetting { get; set; } = new();
    public List<Core.API.NavigationRouteConfig> RouteConfigs { get; set; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        Task.Run(() => // Margin="10,50,10,10"
        {
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 0);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(30,70,30,-10));
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() =>
            {
                MangeFrame.Content = RouteConfigs.Find((config =>
                {
                    return config.Route == ((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Tag;
                })).Page;
            });
            Thread.Sleep(180);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 1);
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(10,50,10,10));
        });
    }

    public void RegisterRoute(Core.API.NavigationRouteConfig config)
    {
        RouteConfigs.Add(config);
        var isthis = false;
        if (config.Route == "MySelfSetting")
        {
            isthis = true;
        }
        View.MenuItems.Add(new NavigationViewItem()
        {
            Tag = config.Route,
            Content = config.Title,
            IsSelected = isthis
        });
    }
}