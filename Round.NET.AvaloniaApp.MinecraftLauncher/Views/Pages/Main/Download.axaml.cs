using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Download.AddNewGame;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Downloads;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Download : UserControl
{
    public Download()
    {
        InitializeComponent();
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = DownloadGamePage,
            Title = "下载游戏",
            Route = "DownloadGame"
        });
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = DownloadAssetsPage,
            Title = "下载资源",
            Route = "DownloadAssets"
        });
    }
    private DownloadGamePage DownloadGamePage { get; set; } = new();
    private DownloadAssetsPage DownloadAssetsPage { get; set; } = new();
    public List<Core.API.NavigationRouteConfig> RouteConfigs { get; set; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        
        ControlChange.ChangeLabelText(PageTitleLabel,((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Content.ToString());
        Task.Run(() => // Margin="10,50,10,10"
        {
            Dispatcher.UIThread.Invoke(() => MangeFrame.Opacity = 0);
            // Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(220,100,30,-10));
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(190,100,10,-10));
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
            Dispatcher.UIThread.Invoke(() => MangeFrame.Margin = new Thickness(190,80,10,10));
        });
    }

    public void RegisterRoute(Core.API.NavigationRouteConfig config)
    {
        RouteConfigs.Add(config);
        var isthis = false;
        if (config.Route == "DownloadGame")
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