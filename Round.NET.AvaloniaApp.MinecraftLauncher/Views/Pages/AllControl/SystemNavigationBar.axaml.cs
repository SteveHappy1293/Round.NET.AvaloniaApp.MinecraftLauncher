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
        Circle.IsVisible = true;
        if (IsClosed)
        {
            Margin = new(0);
            Opacity = 1;
            IsClosed = false;
        }
        else
        {
            Margin = new(-80);
            // Opacity = 0;
            IsClosed = true;
        }
    }

    public void CircleShow()
    {
        
        Dispatcher.UIThread.Invoke(() => Circle.IsVisible = true);
        if (Circle.Width == 32)
        {
            var wid = 1000.00;
            if (Core.MainWindow.Bounds.Width * 2 > Core.MainWindow.Bounds.Height * 2)
            {
                wid = Core.MainWindow.Bounds.Width*2;
            }
            else
            {
                wid = Core.MainWindow.Bounds.Height*2;
            }
            Circle.Width = wid;
            Circle.Height = wid;
            Circle.Opacity = 0.6;
        }
        else
        {
            Circle.Width = 32;
            Circle.Height = 32;   
            Circle.Opacity = 0;
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
        int ind = RouteConfigs.FindIndex((Core.API.NavigationRouteConfig nc) => { return nc.Route == Tag; });
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 0);
            if(ind != -1) Dispatcher.UIThread.Invoke(() => CircleShow());
            else
            {
                Thread.Sleep(45);
                Dispatcher.UIThread.Invoke(() => CircleShow());
            }
            Thread.Sleep(380);
            Dispatcher.UIThread.Invoke(() => Show());
            // Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(0,50,0,0));
            Thread.Sleep(100);
            Dispatcher.UIThread.Invoke(() =>
            {
                if (ind==-1)
                {
                    ((MainView)Core.MainWindow.Content).MainCortent.Content = Launcher;
                    
                    ((MainView)Core.MainWindow.Content).MainCortent.Background = new SolidColorBrush()
                    {
                        Color = Colors.Black,
                        Opacity = 0.0
                    };
                }
                else
                {
                    ((MainView)Core.MainWindow.Content).MainCortent.Content = RouteConfigs[ind].Page;
                }
            });
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 1);
            // Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(0));
            Thread.Sleep(150);
            if (ind != -1)
            {
                Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Background =
                    new SolidColorBrush()
                    {
                        Color = Colors.Black,
                        Opacity = 0.6
                    });
            }
            
            Dispatcher.UIThread.Invoke(() => Circle.Opacity=0.3);
            Dispatcher.UIThread.Invoke(() => Circle.IsVisible = false);
        });
    }
}