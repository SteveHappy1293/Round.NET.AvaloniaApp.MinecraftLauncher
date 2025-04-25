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
using FluentAvalonia.FluentIcons;
using FluentAvalonia.UI.Controls;
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
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = DownloadGamePage,
            Title = "下载游戏",
            Route = "DownloadGame",
            Icon = FluentIconSymbol.Games20Filled
        });
        ContentPagesPanel.RegisterRoute(new Core.API.NavigationRouteConfig()
        {
            Page = DownloadAssetsPage,
            Title = "下载资源",
            Route = "DownloadAssets",
            Icon = FluentIconSymbol.ArrowDownload20Filled
        });
    }
    private DownloadGamePage DownloadGamePage { get; set; } = new();
    private DownloadAssetsPage DownloadAssetsPage { get; set; } = new();
}