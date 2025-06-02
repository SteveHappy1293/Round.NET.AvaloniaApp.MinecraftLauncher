using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FluentAvalonia.FluentIcons;
using RMCL.Controls.ControlHelper;
using RMCL.Views.Pages.Main.DownloadPages;

namespace RMCL.Views.Pages.Main;

public partial class Download : UserControl
{
    public Download()
    {
        InitializeComponent();
        NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Route = "DownloadGame",
            Title = "游戏本体",
            Page = new DownloadGame(),
            Icon = FluentIconSymbol.Games20Regular,
        });
        NavigationPage.RegisterRoute(new NavigationRouteConfig()
        {
            Route = "DownloadCurseForgeAssets",
            Title = "CurseForge 资源",
            Page = new DownloadCurseForgeAssets(),
            Icon = FluentIconSymbol.WebAsset20Regular,
        });
    }
}