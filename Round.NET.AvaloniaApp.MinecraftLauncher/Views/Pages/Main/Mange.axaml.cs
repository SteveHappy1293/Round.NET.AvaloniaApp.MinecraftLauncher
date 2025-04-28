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

public partial class Mange : UserControl,IParentPage
{
    public void Open()
    {
        ContentPagesPanel.ChangeSelectItemMenu();
    }


    public Mange()
    {
        InitializeComponent();
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = GameMange,
            Title = "游戏管理",
            Route = "GameMange",
            Icon = FluentIconSymbol.Games20Filled
        });    
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = UserMange,
            Title = "账户管理",
            Route = "UserMange",
            Icon = FluentIconSymbol.Person20Filled
        });     
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = ServerMange,
            Title = "服务器管理",
            Route = "ServerMange",
            Icon = FluentIconSymbol.Server20Filled
        });   
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = StarMange,
            Title = "收藏夹管理",
            Route = "StarMange",
            Icon = FluentIconSymbol.Star20Filled
        });    
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = PlugMange,
            Title = "启动器插件管理",
            Route = "PlugMange",
            Icon = FluentIconSymbol.PlugConnected20Filled
        });       
    }
    private GameMange GameMange { get; set; } = new();
    private UserMange UserMange { get; set; } = new();
    private JavaMange JavaMange { get; set; } = new();
    private PlugMange PlugMange { get; set; } = new();
    private StarMange StarMange { get; set; } = new();
    private ServerMange ServerMange { get; set; } = new();
}