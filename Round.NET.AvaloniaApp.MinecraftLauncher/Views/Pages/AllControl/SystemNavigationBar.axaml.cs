using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using HeroIconsAvalonia.Controls;
using HeroIconsAvalonia.Enums;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.AllControl;

public partial class SystemNavigationBar : UserControl
{
    private double hei = 64;
    private bool IsClosed = false;
    public SystemNavigationBar()
    {
        InitializeComponent();
        Core.NavigationBar = this;
        Core.API.RegisterNavigationRoute(new()
        {
            Icon = IconType.InboxArrowDown,
            Page = Core.DownloadPage,
            Route = "Download"
        });
        Core.API.RegisterNavigationRoute(new()
        {
            Icon = IconType.ListBullet,
            Page = new Mange(),
            Route = "Mange"
        });
        Core.API.RegisterNavigationRoute(new()
        {
            Icon = IconType.Cog6Tooth,
            Page = new Setting(),
            Route = "Setting"
        });
    }
    private Launcher Launcher { get; } = new();
    public List<Core.API.NavigationRouteConfig> RouteConfigs { get; } = new();
    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        NavTo(((NavigationViewItem)sender!).Tag.ToString());
    }
    public void Show()
    {
        if (IsClosed)
        {
            Margin = new(0);
            Opacity = 1;
            IsClosed = false;
        }
        else
        {
            Margin = new(-80);
            Opacity = 0;
            IsClosed = true;
        }
    }

    public void RegisterRoute(Core.API.NavigationRouteConfig routeConfig)
    {
        RouteConfigs.Add(routeConfig);
        var nav = new NavigationViewItem()
        {
            Height = 52,
            Tag = routeConfig.Route,
            Content = new HeroIcon()
            {
                Foreground = Brushes.White,
                Type = routeConfig.Icon,
                Min = true
            },
        };
        nav.PointerPressed += InputElement_OnPointerPressed;
        MainNavPanel.Children.Add(nav);
    }
    public void NavTo(string Tag)
    {
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 0);
            //Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(50,50,50,50));
            Thread.Sleep(380);
            Dispatcher.UIThread.Invoke(() => Show());
            //Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 1);
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(0,90,0,0));
            Thread.Sleep(100);
            Dispatcher.UIThread.Invoke(() =>
            {
                int ind = RouteConfigs.FindIndex((Core.API.NavigationRouteConfig nc) => { return nc.Route == Tag; });
                if (ind==-1)
                {
                    ((MainView)Core.MainWindow.Content).MainCortent.Content = Launcher;
                }
                else
                {
                    ((MainView)Core.MainWindow.Content).MainCortent.Content = RouteConfigs[ind].Page;
                }
            });
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 1);
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(0));
        });
    }
}