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
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;
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
            Icon = FluentIconSymbol.ArrowDownload20Regular,
            Page = Core.DownloadPage,
            Route = "Download",
            Title = "下载",
        });
        Core.API.RegisterNavigationRoute(new()
        {
            Icon = FluentIconSymbol.List20Regular,
            Page = new Mange(),
            Route = "Mange",
            Title = "管理",
        });
        Core.API.RegisterNavigationRoute(new()
        {
            Icon = FluentIconSymbol.Settings20Regular,
            Page = new Setting(),
            Route = "Setting",
            Title = "设置",
        });
    }
    private Launcher Launcher { get; } = new();
    public List<Core.API.NavigationRouteConfig> RouteConfigs { get; } = new();
    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        NavTo(((NavigationViewItem)sender!).Tag.ToString());
        IsEnabled = false;
    }
    public void Show()
    {
        Circle.IsVisible = true;
        if (IsClosed)
        {
            Margin = new(0);
            Opacity = 1;
            IsEnabled = true;
            IsClosed = false;
        }
        else
        {
            Margin = new(-80);
            // Opacity = 0;
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
            Content = new FluentIcon()
            {
                Icon = routeConfig.Icon,
                Margin = new Thickness(-6),
            },
        };
        nav.PointerPressed += InputElement_OnPointerPressed;
        MainNavPanel.Children.Add(nav);
    }
    public void NavTo(string Tag,bool isearch = false)
    {
        Console.WriteLine(Tag);
        int ind = RouteConfigs.FindIndex((Core.API.NavigationRouteConfig nc) => { return nc.Route == Tag; });
        if (Tag == "Clear") ind = -2;
        if (Tag == "BackSearch") ind = -3;
        Task.Run(() =>
        {
            Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Opacity = 0);
            if (ind != -1 && ind != -2 && ind != -3)
            {
                Dispatcher.UIThread.Invoke(() => Circle.CircleShow(0.6));
            }
            else if (ind == -1)
            {
                Thread.Sleep(45);
                Dispatcher.UIThread.Invoke(() => Circle.CircleShow(0.6));
            }
            Thread.Sleep(380);
            if(ind!=-2 || !IsClosed)Dispatcher.UIThread.Invoke(() => Show());
            // Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(0,50,0,0));
            Thread.Sleep(100);
            Dispatcher.UIThread.Invoke(() =>
            {
                if (ind==-1||ind==-3)
                {
                    ((MainView)Core.MainWindow.Content).MainCortent.Content = Launcher;
                    
                    ((MainView)Core.MainWindow.Content).MainCortent.Background = new SolidColorBrush()
                    {
                        Color = Colors.Black,
                        Opacity = 0.0
                    };
                }else if (ind == -2)
                {
                    ((MainView)Core.MainWindow.Content).MainCortent.Content = new Grid();
                    if(!Circle.IsOpen) Dispatcher.UIThread.Invoke(() => Circle.CircleShow(0.6));
                    
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
            Dispatcher.UIThread.Invoke(() => Circle.Opacity=0.38);
            if (ind != -1&&ind!=-2&&ind!=-3)
            {
                Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Background =
                    new SolidColorBrush()
                    {
                        Color = Colors.Black,
                        Opacity = 0.3
                    });
            }
            
            Dispatcher.UIThread.Invoke(() => Circle.IsVisible = false);
            
            // 右一下后恢复
            if (ind >= 0 & false)
            {
                Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(10,0,-10,0));
                Thread.Sleep(150);
            
                Dispatcher.UIThread.Invoke(() => ((MainView)Core.MainWindow.Content).MainCortent.Margin = new Thickness(0,0,0,0));
            }
        });
    }
}