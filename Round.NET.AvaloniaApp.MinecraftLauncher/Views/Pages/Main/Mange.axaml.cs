using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Config;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Game.JavaEdtion.Launch;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.TaskMange.SystemMessage;
using Round.NET.AvaloniaApp.MinecraftLauncher.Modules.UIControls;
using Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main.Manges;
using LaunchJavaEdtion = Round.NET.AvaloniaApp.MinecraftLauncher.Views.Controls.Launch.LaunchJavaEdtion;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Views.Pages.Main;

public partial class Mange : UserControl
{
    public Mange()
    {
        InitializeComponent();
        Core.MangePage = this; 
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = GameMange,
            Title = "游戏管理",
            Route = "GameMange",
            Icon = FluentIconSymbol.Games20Filled
        });    
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = UserMange,
            Title = "账户管理",
            Route = "UserMange",
            Icon = FluentIconSymbol.Person20Filled
        });     
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = ServerMange,
            Title = "服务器管理",
            Route = "ServerMange",
            Icon = FluentIconSymbol.Server20Filled
        });   
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = StarMange,
            Title = "收藏夹管理",
            Route = "StarMange",
            Icon = FluentIconSymbol.Star20Filled
        });    
        RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = PlugMange,
            Title = "启动器插件管理",
            Route = "PlugMange",
            Icon = FluentIconSymbol.PlugConnected20Filled
        });       
    }
    public List<Core.API.NavigationRouteConfig> RouteConfigs { get; set; } = new();
    private GameMange GameMange { get; set; } = new();
    private UserMange UserMange { get; set; } = new();
    private JavaMange JavaMange { get; set; } = new();
    private PlugMange PlugMange { get; set; } = new();
    private StarMange StarMange { get; set; } = new();
    private ServerMange ServerMange { get; set; } = new();
    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        var item = ((DockPanel)((NavigationViewItem)((NavigationView)sender!).SelectedItem!).Content);
        var text = ((TextBlock)(item.Children[1])).Text;
        ControlChange.ChangeLabelText(PageTitleLabel,text);
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
        if (config.Route == "GameMange")
        {
            isthis = true;
        }
        
        var doc = new DockPanel()
        {
            Children =
            {
                new FluentIcon() { Margin = new Thickness(-5,0,0,0),Icon = config.Icon,Width = 15,Height = 15 },
                new TextBlock() { Margin = new Thickness(10,0,0,0),Text = config.Title,Name = "TitleLabel"}
            }
        };
        var item = new NavigationViewItem()
        {
            Tag = config.Route,
            Content = doc,
            IsSelected = isthis
        };
        if (config.IsFoot)
        {
            View.FooterMenuItems.Add(item);
        }
        else
        {
            View.MenuItems.Add(item);
        }
    }
}